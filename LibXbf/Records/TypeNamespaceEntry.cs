using System.IO;

namespace LibXbf.Records
{
    public struct TypeNamespaceEntry
    {
        public uint AssemblyId { get; set; }
        public uint StringId { get; set; }
    }

    public class XbfTypeNamespaceEntry : IXbfType<TypeNamespaceEntry>
    {
        public TypeNamespaceEntry ReadValue(BinaryReader br)
        {
            TypeNamespaceEntry tne = new TypeNamespaceEntry();
            tne.AssemblyId = br.ReadUInt32();
            tne.StringId = br.ReadUInt32();
            return tne;
        }
    }
}
