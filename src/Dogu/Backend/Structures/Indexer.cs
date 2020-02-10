using System;
using System.Reflection;

namespace Dogu.Backend.Structures
{
    public class Indexer : Property
    {
        public Indexer(PropertyInfo propertyInfo) : base(propertyInfo)
        {
        }
    }
}
