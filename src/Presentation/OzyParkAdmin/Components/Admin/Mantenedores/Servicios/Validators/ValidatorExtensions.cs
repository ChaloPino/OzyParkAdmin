using FluentValidation;
using System.Linq.Expressions;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Validators;

internal static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, TProperty> NotNullWhen<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, Expression<Func<T, bool>> canEvaluate)
    {
        var func = canEvaluate.Compile();
        return ruleBuilder.SetValidator(new NotNullWhenValidator<T, TProperty>(func));
    }
}
