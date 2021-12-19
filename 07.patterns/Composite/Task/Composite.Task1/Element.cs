namespace Composite.Task1
{
    public abstract class Element
    {
        protected string elementName;

        protected Element(string name)
        {
            elementName = name;
        }

        public abstract string ConvertToString();
    }
}
