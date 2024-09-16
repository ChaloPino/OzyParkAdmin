using System.Linq.Expressions;
using System.Reflection;

namespace OzyParkAdmin.Domain.Shared.Expressions;
internal class IsNullOrEmptyExpression<T> : FilterOperationExpression<T>
{
    private static readonly MethodInfo IsNullOrWhiteSpaceMethodInfo = typeof(string).GetMethod(nameof(string.IsNullOrWhiteSpace), BindingFlags.Public | BindingFlags.Static, [typeof(string)])!;

    public IsNullOrEmptyExpression(Expression<Func<T, string?>> memberExpression) 
        : base(memberExpression)
    {
    }

    protected override Expression GenerateBody()
    {
        return Expression.Call(IsNullOrWhiteSpaceMethodInfo, Member);
    }
}
