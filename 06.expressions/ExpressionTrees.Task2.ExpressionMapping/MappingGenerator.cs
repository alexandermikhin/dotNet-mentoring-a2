using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionTrees.Task2.ExpressionMapping.Annotations;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class MappingGenerator
    {
        public Mapper<TSource, TDestination> Generate<TSource, TDestination>()
        {
            var sourceParam = Expression.Parameter(typeof(TSource));
            var ctor = Expression.New(typeof(TDestination));
            var memberBindings = GetMemberBindings<TSource, TDestination>(sourceParam);
            var memberInitExpression = Expression.MemberInit(ctor, memberBindings);
            var mapFunction =
                Expression.Lambda<Func<TSource, TDestination>>(
                    memberInitExpression,
                    sourceParam
                );

            return new Mapper<TSource, TDestination>(mapFunction.Compile());
        }

        private IEnumerable<MemberBinding> GetMemberBindings<TSource, TDestination>(ParameterExpression sourceParam)
        {
            var bindings = new List<MemberBinding>();
            var sourceMemberInfos = GetMemberInfos<TSource>(p => p.CanRead);
            var destinationMemberInfos = GetMemberInfos<TDestination>(p => p.CanWrite);

            foreach (var sourceMemberInfo in sourceMemberInfos)
            {
                var destinationMemberInfo = destinationMemberInfos.FirstOrDefault(p =>
                    MemberInfoMatch(p, sourceMemberInfo));

                if (destinationMemberInfo != null && TypesAreEqual(destinationMemberInfo, sourceMemberInfo))
                {
                    var memberExpression = Expression.PropertyOrField(sourceParam, sourceMemberInfo.Name);
                    var memberAssignment = Expression.Bind(destinationMemberInfo, memberExpression);
                    bindings.Add(memberAssignment);
                }
            }

            return bindings;
        }

        private IEnumerable<MemberInfo> GetMemberInfos<T>(Func<PropertyInfo, bool> propertySelector)
        {
            var fields = GetFields<T>();
            var properties = GetProperties<T>(propertySelector);

            return fields.Cast<MemberInfo>().Concat(properties);
        }

        private bool TypesAreEqual(MemberInfo info1, MemberInfo info2)
        {
            if (info1.MemberType == MemberTypes.Field && info2.MemberType == MemberTypes.Field)
            {
                return (info1 as FieldInfo).FieldType == (info2 as FieldInfo).FieldType;
            }

            if (info1.MemberType == MemberTypes.Property && info2.MemberType == MemberTypes.Property)
            {
                return (info1 as PropertyInfo).PropertyType == (info2 as PropertyInfo).PropertyType;
            }

            return false;
        }

        private IEnumerable<PropertyInfo> GetProperties<T>(Func<PropertyInfo, bool> propertySelector)
        {
            return typeof(T).GetProperties().Where(propertySelector);
        }

        private IEnumerable<FieldInfo> GetFields<T>()
        {
            return typeof(T).GetFields();
        }

        private bool MemberInfoMatch(MemberInfo destination, MemberInfo source)
        {
            if (destination.GetCustomAttribute<IgnoreAttribute>() != null ||
                source.GetCustomAttribute<IgnoreAttribute>() != null)
            {
                return false;
            }

            return destination.Name == source.Name;
        }
    }
}
