using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared.Expressions;
internal sealed class NotEqualsExpression<T, TProperty> : FilterOperationExpression<T>
{
    private readonly ConstantExpression _value;

    public NotEqualsExpression(Expression<Func<T, TProperty?>> memberExpression, TProperty? value)
        : base(memberExpression)
    {
        _value = Expression.Constant(value, typeof(TProperty));
    }

    protected override Expression GenerateBody()
    {
        return Expression.NotEqual(Member, _value);
    }
}
