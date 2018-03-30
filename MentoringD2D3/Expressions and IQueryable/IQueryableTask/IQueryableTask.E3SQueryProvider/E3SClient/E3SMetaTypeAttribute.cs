using System;

namespace IQueryableTask.E3SQueryProvider.E3SClient
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    class E3SMetaTypeAttribute : Attribute
    {
        public string Name { get; }

        public E3SMetaTypeAttribute(string name)
        {
            Name = name;
        }
    }
}
