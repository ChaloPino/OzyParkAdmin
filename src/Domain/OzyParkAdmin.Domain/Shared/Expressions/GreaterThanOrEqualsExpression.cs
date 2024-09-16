using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared.Expressions;
internal sealed class GreaterThanOrEqualsExpression<T, TProperty> : FilterOperationExpression<T>
    where TProperty : struct
{
    private readonly ConstantExpression _value;

    public GreaterThanOrEqualsExpression(Expression<Func<T, TProperty>> memberExpression, TProperty value)
        : base(memberExpression)
    {
        _value = Expression.Constant(value, typeof(TProperty));
    }

    public GreaterThanOrEqualsExpression(Expression<Func<T, TProperty?>> memberExpression, TProperty value)
        : base(memberExpression)
    {
        _value = Expression.Constant(value, typeof(TProperty));
    }

    protected override Expression GenerateBody()
    {
        return Expression.GreaterThanOrEqual(Member, _value);
    }
}
