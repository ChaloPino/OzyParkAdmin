using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared.Expressions;
internal sealed class EqualsExpression<T, TProperty> : FilterOperationExpression<T>
{
    private readonly ConstantExpression _value;

    public EqualsExpression(Expression<Func<T, TProperty?>> memberExpression, TProperty? value)
        : base(memberExpression)
    {
        _value = Expression.Constant(value, typeof(TProperty));
    }

    protected override Expression GenerateBody()
    {
        return Expression.Equal(Member, _value);
    }
}
