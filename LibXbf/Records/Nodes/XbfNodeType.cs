namespace LibXbf.Records.Nodes
{
    public enum XbfNodeType : byte
    {
        None = 0,
        StartObject = 1,
        EndObject = 2,
        StartProperty = 3,
        EndProperty = 4,
        Text = 5,
        Value = 6,
        Namespace = 7,
        EndOfAttributes = 8,
        EndOfStream = 9,
        LineInfo = 10,
        LineInfoAbsolute = 11
    }
}
