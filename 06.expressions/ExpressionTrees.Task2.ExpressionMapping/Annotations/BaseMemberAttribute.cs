using System;

namespace ExpressionTrees.Task2.ExpressionMapping.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class BaseMemberAttribute: Attribute
    {
        public string Name { get; }

        public BaseMemberAttribute(string name)
        {
            this.Name = name;
        }
    }
}
