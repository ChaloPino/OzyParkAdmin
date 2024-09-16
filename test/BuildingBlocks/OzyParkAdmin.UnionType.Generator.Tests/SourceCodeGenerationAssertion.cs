using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;

namespace OzyParkAdmin.UnionType.Generator.Tests;
internal static class SourceCodeGenerationAssertion
{
    public static void AssertCorrectSourceCodeIsGeneratedWithNoDiasnostics(
        string inputSource,
        (string ExpectedCode, string GeneratedFileName)[] expected,
        int expectedCompilationFileCount = 34)
    {
        var parsedAttributes = new SyntaxTree[expected.Length];

        for (int i = 0; i < expected.Length; i++)
        {
            parsedAttributes[i] = CSharpSyntaxTree.ParseText(expected[i].ExpectedCode);
        }

        var inputCompilation = CreateCompilation(inputSource);

        GeneratorDriver driver = CSharpGeneratorDriver.Create(new TypeUnionSourceGenerator());

        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);

        Assert.True(diagnostics.IsEmpty);

        Assert.Equal(expectedCompilationFileCount, outputCompilation.SyntaxTrees.Count());

        var compilationDiagnostics = outputCompilation.GetDiagnostics();

        Assert.Empty(compilationDiagnostics);

        for (int i = 0; i < expected.Length; i++)
        {
            var compiledAttribute = outputCompilation.SyntaxTrees.Single(e => e.FilePath.Contains(expected[i].GeneratedFileName));
            Assert.True(parsedAttributes[i].IsEquivalentTo(compiledAttribute));
        }

        Assert.True(outputCompilation.GetDiagnostics().IsEmpty);
    }

    public static void AssertDiagnosticErrorIsReturned(string inputSource, string diagnosticId)
    {
        var inputCompilation = CreateCompilation(inputSource);

        GeneratorDriver driver = CSharpGeneratorDriver.Create(new TypeUnionSourceGenerator());

        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out _, out var diagnostics);

        Assert.Contains(diagnostics, d => d.Id == diagnosticId && d.Severity == DiagnosticSeverity.Error);
    }

    public static Compilation CreateCompilation(string source)
    {
        var references = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location),
            MetadataReference.CreateFromFile(AppDomain.CurrentDomain.GetAssemblies().Single(a => a.GetName().Name == "netstandard").Location),
        };

        foreach (var assembly in Assembly.GetEntryAssembly()!.GetReferencedAssemblies())
        {
            references.Add(MetadataReference.CreateFromFile(Assembly.Load(assembly).Location));
        }

        return CSharpCompilation.Create("compilation", [CSharpSyntaxTree.ParseText(source)], references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
    }
}

