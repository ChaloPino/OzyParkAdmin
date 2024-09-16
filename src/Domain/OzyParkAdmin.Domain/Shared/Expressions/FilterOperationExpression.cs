using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared.Expressions;

internal abstract class FilterOperationExpression<T>
{
    protected FilterOperationExpression(LambdaExpression memberExpression)
    {
        Parameter = memberExpression.Parameters[0];
        Member = (MemberExpression)memberExpression.Body;
    }

    public ParameterExpression Parameter { get; }
    public MemberExpression Member { get; }


    public Expression<Func<T, bool>> Reduce()
    {
        Expression body = GenerateBody();
        return Expression.Lambda<Func<T, bool>>(body, Parameter);
    }

    protected abstract Expression GenerateBody();
}