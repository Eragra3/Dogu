using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using Dogu.Backend.Structures.Parameters;

namespace Dogu.Backend.Structures
{
    public class Structure : TopLevelType
    {
        public Method[]   Methods    { get; }
        public Property[] Properties { get; }
        public Indexer[]  Indexers   { get; }

        public Structure(StructureParameters parameters) : base(parameters)
        {
            Methods    = parameters.Methods;
            Properties = parameters.Properties;
            Indexers   = parameters.Indexers;
        }
    }
}
