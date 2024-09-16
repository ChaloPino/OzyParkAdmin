using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared.Expressions;
internal sealed class IsNotEmptyExpression<T, TProperty> : FilterOperationExpression<T>
    where TProperty : struct
{
    private readonly ConstantExpression _nullValue;

    public IsNotEmptyExpression(Expression<Func<T, TProperty>> memberExpression)
        : base(memberExpression)
    {
        _nullValue = Expression.Constant(null, typeof(TProperty?));
    }

    public IsNotEmptyExpression(Expression<Func<T, TProperty?>> memberExpression)
        : base(memberExpression)
    {
        _nullValue = Expression.Constant(null, typeof(TProperty?));
    }

    protected override Expression GenerateBody()
    {
        return Expression.NotEqual(Member, _nullValue);
    }
}
