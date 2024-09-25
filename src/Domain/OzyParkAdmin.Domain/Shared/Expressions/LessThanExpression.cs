using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared.Expressions;
internal sealed class LessThanExpression<T, TProperty> : FilterOperationExpression<T>
    where TProperty : struct
{
    private readonly ConstantExpression _value;

    public LessThanExpression(Expression<Func<T, TProperty>> memberExpression, TProperty value)
        : base(memberExpression)
    {
        _value = Expression.Constant(value, typeof(TProperty));
    }

    public LessThanExpression(Expression<Func<T, TProperty?>> memberExpression, TProperty? value)
        : base(memberExpression)
    {
        _value = Expression.Constant(value, typeof(TProperty));
    }

    protected override Expression GenerateBody()
    {
        return Expression.LessThan(Member, _value);
    }
}
