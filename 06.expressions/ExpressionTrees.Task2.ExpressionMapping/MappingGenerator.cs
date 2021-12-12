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
        GeneratorParameters _parameters;

        public MappingGenerator() { }

        public MappingGenerator(GeneratorParameters parameters)
        {
            _parameters = parameters;
        }

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
                var destinationMemberInfo = destinationMemberInfos.FirstOrDefault(p => MemberInfoMatch(p, sourceMemberInfo));

                if (destinationMemberInfo != null)
                {
                    var converter = GetConverter(sourceMemberInfo);
                    if (converter != null || TypesAreEqual(destinationMemberInfo, sourceMemberInfo))
                    {
                        Expression memberExpression = Expression.PropertyOrField(sourceParam, sourceMemberInfo.Name);
                        if (converter != null)
                        {
                            memberExpression = Expression.Call(Expression.Constant(converter), 
                                typeof(IMappingConverter).GetMethod("Convert"),
                                memberExpression);

                            var destinationType = GetMemberInfoType(destinationMemberInfo);
                            memberExpression = Expression.ConvertChecked(memberExpression, destinationType);
                        }

                        var memberAssignment = Expression.Bind(destinationMemberInfo, memberExpression);
                        bindings.Add(memberAssignment);
                    }
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
            return GetMemberInfoType(info1) == GetMemberInfoType(info2);
        }

        private Type GetMemberInfoType(MemberInfo memberInfo)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return (memberInfo as FieldInfo).FieldType;
                case MemberTypes.Property:
                    return (memberInfo as PropertyInfo).PropertyType;
                default:
                    throw new ArgumentException("Member type is not suppoerted.");
            }
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

            var destinationAttribute = source.GetCustomAttribute<DestinationMemberAttribute>();
            if (destinationAttribute != null)
            {
                return destination.Name == destinationAttribute.Name;
            }

            var sourceAttribute = destination.GetCustomAttribute<SourceMemberAttribute>();
            if (sourceAttribute != null)
            {
                return source.Name == sourceAttribute.Name;
            }

            return destination.Name == source.Name;
        }

        private IMappingConverter GetConverter(MemberInfo memberInfo)
        {
            if (_parameters == null)
            {
                return null;
            }

            IMappingConverter converter = null;
            if (_parameters.DestinationConverters != null && _parameters.DestinationConverters.ContainsKey(memberInfo.Name))
            {
                converter = _parameters.DestinationConverters[memberInfo.Name];
            }
            else if (_parameters.SourceConverters != null && _parameters.SourceConverters.ContainsKey(memberInfo.Name))
            {
                converter = _parameters.SourceConverters[memberInfo.Name];
            }

            return converter;
        }
    }
}
