using System;

namespace MesGamification.Logger.Api.Repositories
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class CollectionNameAttribute : Attribute
    {
        public CollectionNameAttribute(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Empty collection name is not allowed", nameof(value));
            Name = value;
        }

        public virtual string Name { get; private set; }
    }
}