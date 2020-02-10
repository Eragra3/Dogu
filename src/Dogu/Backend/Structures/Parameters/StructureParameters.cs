namespace Dogu.Backend.Structures.Parameters
{
    public class StructureParameters : TopLevelTypeParameters
    {
        public Method[]   Methods    { get; set; }
        public Property[] Properties { get; set; }
        public Indexer[]  Indexers   { get; set; }
    }
}
