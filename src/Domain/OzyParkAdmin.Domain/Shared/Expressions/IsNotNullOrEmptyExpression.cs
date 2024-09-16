using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared.Expressions;
internal sealed class IsNotNullOrEmptyExpression<T> : IsNullOrEmptyExpression<T>
{
    public IsNotNullOrEmptyExpression(Expression<Func<T, string?>> memberExpression) 
        : base(memberExpression)
    {
    }

    protected override Expression GenerateBody()
    {
        return Expression.Not(base.GenerateBody());
    }
}
