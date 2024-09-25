using System.Linq.Expressions;
using System.Reflection;

namespace OzyParkAdmin.Domain.Shared.Utils;
internal static class PropertyPath
{
    public static PropertyHolder Visit<TSource, TResult>(Expression<Func<TSource, TResult>> expression) =>
        Visit((LambdaExpression)expression);

    public static PropertyHolder Visit(LambdaExpression expression)
    {
        var body = expression.Body;
        var visitor = new PropertyVisitor(body is MemberExpression);
        visitor.Visit(body);

        return visitor.PropertyHolder;
    }

    public sealed class PropertyHolder
    {
        private readonly List<MemberInfo> _members;

        public PropertyHolder(bool isBodyMemberExpression)
        {
            IsBodyMemberExpression = isBodyMemberExpression;
            _members = [];
        }

        public bool IsBodyMemberExpression { get; }

        public void AddMember(MemberInfo member) =>
            _members.Insert(0, member);

        public IReadOnlyList<MemberInfo> GetMembers =>
            _members;

        public string GetPath() =>
            string.Join(".", _members.Select(x => x.Name));

        public string GetLastMemberName()
        {
            string? lastMemberNamne = _members
                .Select(x => x.Name)
                .LastOrDefault();
            return string.IsNullOrEmpty(lastMemberNamne) ? string.Empty : lastMemberNamne;
        }

        public override string ToString() =>
            GetPath();
    }

    public sealed class PropertyVisitor : ExpressionVisitor
    {
        public PropertyVisitor(bool isBodyMemberExpression)
        {
            PropertyHolder = new PropertyHolder(isBodyMemberExpression);
        }

        public PropertyHolder PropertyHolder { get; }

        protected override Expression VisitMember(MemberExpression node)
        {
            PropertyHolder.AddMember(node.Member);
            return base.VisitMember(node);
        }
    }
}
