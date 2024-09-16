using System.Linq.Expressions;
using System.Reflection;

namespace OzyParkAdmin.Domain.Shared.Expressions;
internal sealed class StartsWithExpression<T> : FilterOperationExpression<T>
{
    private static readonly MethodInfo StartsWithMethodInfo = typeof(string).GetMethod(nameof(string.StartsWith), BindingFlags.Public | BindingFlags.Instance, [typeof(string)])!;
    private readonly ConstantExpression _value;

    public StartsWithExpression(Expression<Func<T, string?>> memberExpression, string? value)
        : base(memberExpression)
    {
        _value = Expression.Constant(value, typeof(string));
    }

    protected override Expression GenerateBody()
    {
        return Expression.Call(Member, StartsWithMethodInfo, _value);
    }
}
