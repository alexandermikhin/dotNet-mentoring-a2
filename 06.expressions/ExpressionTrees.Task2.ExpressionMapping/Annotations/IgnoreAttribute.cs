using System;

namespace ExpressionTrees.Task2.ExpressionMapping.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IgnoreAttribute: Attribute
    {
    }
}
