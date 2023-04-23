using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class FoldoutAttributeAttribute : MetaAttribute, IGroupAttribute
    {
        public string Name { get; private set; }

        public FoldoutAttributeAttribute(string name = "")
        {
            Name = name;
        }
    }
}
