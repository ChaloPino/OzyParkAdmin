using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared.Expressions;
internal sealed class IsEmptyExpression<T, TProperty> : FilterOperationExpression<T>
    where TProperty : struct
{
    private readonly ConstantExpression _nullValue;

    public IsEmptyExpression(Expression<Func<T, TProperty>> memberExpression)
        : base(memberExpression)
    {
        _nullValue = Expression.Constant(null, typeof(TProperty?));
    }

    public IsEmptyExpression(Expression<Func<T, TProperty?>> memberExpression)
        : base(memberExpression)
    {
        _nullValue = Expression.Constant(null, typeof(TProperty?));
    }

    protected override Expression GenerateBody()
    {
        return Expression.Equal(Member, _nullValue);
    }
}
