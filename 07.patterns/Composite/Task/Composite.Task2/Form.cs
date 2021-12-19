using System;
using System.Collections.Generic;
using System.Text;

namespace Composite.Task2
{
    public class Form : IComponent
    {
        protected readonly List<IComponent> children = new List<IComponent>();
        String name;
        const String elementName = "form";

        public Form(String name)
        {
            this.name = name;
        }

        public void AddComponent(IComponent component)
        {
            children.Add(component);
        }

        public String ConvertToString(int depth = 0)
        {
            var tab = new String(' ', depth);
            var content = GetInnerContent(depth);

            return $"{tab}<{elementName} name='{name}'>{Environment.NewLine + content}{tab}</{elementName}>";
        }

        private String GetInnerContent(int depth)
        {
            var builder = new StringBuilder();
            children.ForEach(element => builder.Append(element.ConvertToString(depth + 1) + Environment.NewLine));

            return builder.ToString();
        }
    }
}