using OzyParkAdmin.Domain.Plantillas;
using OzyParkAdmin.Infrastructure.Shared;
using RazorEngineCore;
using System.Collections.Concurrent;
using System.Dynamic;
using System.Reflection;
using System.Text;
using System.Xml;

namespace OzyParkAdmin.Infrastructure.Plantillas;

/// <summary>
/// Genera el contenido de la plantilla en html.
/// </summary>
public sealed class HtmlGenerator : IInfrastructure
{
    private readonly ConcurrentDictionary<int, object> TemplateCache = new();

    private readonly IRazorEngine _razorEngine;
    private readonly AssemblyProvider _assemblyProvider;

    /// <summary>
    /// Crea una nueva instancia de <see cref="HtmlGenerator"/>.
    /// </summary>
    /// <param name="razorEngine">El <see cref="IRazorEngine"/>.</param>
    /// <param name="assemblyProvider">El <see cref="AssemblyProvider"/></param>
    public HtmlGenerator(IRazorEngine razorEngine, AssemblyProvider assemblyProvider)
    {
        ArgumentNullException.ThrowIfNull(razorEngine);
        ArgumentNullException.ThrowIfNull(assemblyProvider);
        _razorEngine = razorEngine;
        _assemblyProvider = assemblyProvider;
    }

    /// <summary>
    /// Genera el html.
    /// </summary>
    /// <typeparam name="TModel">El tipo del modelo.</typeparam>
    /// <param name="model">El modelo usado como dato.</param>
    /// <param name="plantilla">La plantilla.</param>
    /// <param name="viewBag">Los datos adicionales.</param>
    /// <returns>El html.</returns>
    public async Task<string> GenerateHtmlAsync<TModel>(TModel model, Plantilla plantilla, ExpandoObject viewBag)
    {
        int hashCode = plantilla.Contenido.GetHashCode();
        IRazorEngineCompiledTemplate<RazorTemplate<TModel>> compiledTemplate
            = (IRazorEngineCompiledTemplate<RazorTemplate<TModel>>)TemplateCache.GetOrAdd(hashCode, (_) => CreateTemplate<TModel>(plantilla.Contenido));

        string html = await compiledTemplate.RunAsync(instance =>
        {
            instance.Model = model;
            instance.ViewBag = viewBag;
        });
        return IncludeStyle(html, plantilla.Estilo);
    }

    private static string IncludeStyle(string html, string? style)
    {
        if (style is null)
        {
            return html;
        }

        XmlDocument document = new();
        document.LoadXml(html);
        var headNode = document.DocumentElement!.ChildNodes[0];

        if (headNode is not null)
        {
            var styleNode = document.CreateElement("style");
            styleNode.InnerText = style;
            headNode.AppendChild(styleNode);
        }

        return document.OuterXml;
    }

    private object CreateTemplate<TModel>(string templateContent)
    {
        StringBuilder stringBuilder = new();
        stringBuilder.AppendLine("@using OzyParkAdmin.Infrastructure.BarCodes");
        stringBuilder.AppendLine("@using OzyParkAdmin.Infrastructure.Plantillas");
        stringBuilder.Append(templateContent);
        return _razorEngine.Compile<RazorTemplate<TModel>>(stringBuilder.ToString(), builder =>
        {
            builder.AddAssemblyReference(typeof(TModel).Assembly);

            foreach (Assembly assembly in _assemblyProvider.GetAssemblies())
            {
                builder.AddAssemblyReference(assembly);
            }
        });
    }
}