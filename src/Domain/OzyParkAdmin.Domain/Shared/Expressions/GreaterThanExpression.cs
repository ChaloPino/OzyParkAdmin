using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared.Expressions;
internal sealed class GreaterThanExpression<T, TProperty> : FilterOperationExpression<T>
    where TProperty : struct
{
    private readonly ConstantExpression _value;

    public GreaterThanExpression(Expression<Func<T, TProperty>> memberExpression, TProperty value)
        : base(memberExpression)
    {
        _value = Expression.Constant(value, typeof(TProperty));
    }

    public GreaterThanExpression(Expression<Func<T, TProperty?>> memberExpression, TProperty value)
        : base(memberExpression)
    {
        _value = Expression.Constant(value, typeof(TProperty));
    }

    protected override Expression GenerateBody()
    {
        return Expression.GreaterThan(Member, _value);
    }
}
