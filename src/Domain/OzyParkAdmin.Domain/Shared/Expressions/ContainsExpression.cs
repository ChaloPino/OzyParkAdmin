using System.Linq.Expressions;
using System.Reflection;

namespace OzyParkAdmin.Domain.Shared.Expressions;
internal class ContainsExpression<T> : FilterOperationExpression<T>
{
    private static readonly MethodInfo ContainsMethodInfo = typeof(string).GetMethod(nameof(string.Contains), BindingFlags.Public | BindingFlags.Instance, [typeof(string)])!;
    private readonly ConstantExpression _value;

    public ContainsExpression(Expression<Func<T, string?>> memberExpression, string? value)
        : base(memberExpression)
    {
        _value = Expression.Constant(value, typeof(string));
    }

    /// <inheritdoc/>
    protected override Expression GenerateBody()
    {
        return Expression.Call(Member, ContainsMethodInfo, _value);
    }
}
