#nullable enable
using System;
using System.Linq;
using System.Text;
using Dogu.Backend.Structures.Parameters;

namespace Dogu.Backend.Structures
{
    public class Class : TopLevelType
    {
        public Method[]   Methods    { get; }
        public Property[] Properties { get; }
        public Indexer[]  Indexers   { get; }

        public Class(ClassParameters parameters) : base(parameters)
        {
            Methods    = parameters.Methods;
            Properties = parameters.Properties;
            Indexers   = parameters.Indexers;
        }
    }
}
