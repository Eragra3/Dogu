using System;
using System.Linq;
using System.Text;
using Dogu.Backend.Structures.Parameters;

namespace Dogu.Backend.Structures
{
    public class Interface : TopLevelType
    {
        public Method[]   Methods    { get; }
        public Property[] Properties { get; }
        public Indexer[]  Indexers   { get; }

        public Interface(InterfaceParameters parameters) :
            base(parameters)
        {
            Methods    = parameters.Methods;
            Properties = parameters.Properties;
            Indexers   = parameters.Indexers;
        }
    }
}
