using System;
using System.Reflection;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency;

public static class MemberFactory
{
    public static IMember Create(MemberInfo memberInfo, INode parent)
    {
        return memberInfo.MemberType switch
        {
            MemberTypes.Field => new Field((FieldInfo)memberInfo, parent),
            MemberTypes.Property => new Property((PropertyInfo)memberInfo, parent),
            _ => throw new NotSupportedException($"Don't know how to deal with a {memberInfo.MemberType}")
        };
    }

    internal static IMember Find(object target, string memberName, INode parent)
    {
        PropertyInfo property = target.GetType().FindProperty(memberName);

        if (property is not null && !property.IsIndexer())
        {
            return new Property(property, parent);
        }

        FieldInfo field = target.GetType().FindField(memberName);
        return field is not null ? new Field(field, parent) : null;
    }
}
