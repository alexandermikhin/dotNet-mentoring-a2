using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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
            var propertiesBindings = GetPropertiesBindings<TSource, TDestination>(sourceParam);
            var fieldsBindings = GetFieldsBindings<TSource, TDestination>(sourceParam);

            return propertiesBindings.Concat(fieldsBindings);
        }

        private IEnumerable<MemberBinding> GetPropertiesBindings<TSource, TDestination>(ParameterExpression sourceParam)
        {
            var bindings = new List<MemberBinding>();
            var sourceProperties = GetProperties<TSource>(p => p.CanRead);
            var destinationProperties = GetProperties<TDestination>(p => p.CanWrite);

            foreach (var sourceProperty in sourceProperties)
            {
                var destinationProperty = destinationProperties.FirstOrDefault(p =>
                    p.Name == sourceProperty.Name && p.PropertyType == sourceProperty.PropertyType);

                if (destinationProperty != null)
                {
                    var memberExpression = Expression.PropertyOrField(sourceParam, sourceProperty.Name);
                    var memberAssignment = Expression.Bind(destinationProperty, memberExpression);
                    bindings.Add(memberAssignment);
                }
            }

            return bindings;
        }

        private IEnumerable<MemberBinding> GetFieldsBindings<TSource, TDestination>(ParameterExpression sourceParam)
        {
            var bindings = new List<MemberBinding>();
            var sourceFields = GetFields<TSource>();
            var destinationFields = GetFields<TDestination>();

            foreach (var sourceField in sourceFields)
            {
                var destinationField = destinationFields.FirstOrDefault(p =>
                    p.Name == sourceField.Name && p.FieldType == sourceField.FieldType);

                if (destinationField != null)
                {
                    var memberExpression = Expression.PropertyOrField(sourceParam, sourceField.Name);
                    var memberAssignment = Expression.Bind(destinationField, memberExpression);
                    bindings.Add(memberAssignment);
                }
            }

            return bindings;
        }

        private IEnumerable<PropertyInfo> GetProperties<T>(Func<PropertyInfo, bool> propertySelector)
        {
            return typeof(T).GetProperties().Where(propertySelector);
        }

        private IEnumerable<FieldInfo> GetFields<T>()
        {
            return typeof(T).GetFields();
        }
    }
}
