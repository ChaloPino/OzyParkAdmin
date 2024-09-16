using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared.Expressions;
internal sealed class NotContainsExpression<T> : ContainsExpression<T>
{
    public NotContainsExpression(Expression<Func<T, string?>> memberExpression, string? value)
        : base(memberExpression, value)
    {
    }

    protected override Expression GenerateBody()
    {
        return Expression.Not(base.GenerateBody());
    }
}
