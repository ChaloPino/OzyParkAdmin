using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared.Expressions;
internal sealed class LessThanOrEqualsExpression<T, TProperty> : FilterOperationExpression<T>
    where TProperty : struct
{
    private readonly ConstantExpression _value;

    public LessThanOrEqualsExpression(Expression<Func<T, TProperty>> memberExpression, TProperty value)
        : base(memberExpression)
    {
        _value = Expression.Constant(value, typeof(TProperty));
    }

    public LessThanOrEqualsExpression(Expression<Func<T, TProperty?>> memberExpression, TProperty value)
        : base(memberExpression)
    {
        _value = Expression.Constant(value, typeof(TProperty));
    }

    protected override Expression GenerateBody()
    {
        return Expression.LessThanOrEqual(Member, _value);
    }
}
