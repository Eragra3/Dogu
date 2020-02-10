namespace Dogu.Backend.Structures.Parameters
{
    public abstract class SerializableParameters
    {
        public override string ToString() => DebuggingUtility.Serialize(this);
    }
}
