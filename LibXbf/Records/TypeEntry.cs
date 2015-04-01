using System;
using System.IO;

namespace LibXbf.Records
{
    public struct TypeEntry
    {
        public TypeFlags Flags { get; set; }
        public uint NamespaceId { get; set; }
        public uint StringId { get; set; }
    }

    public class XbfTypeEntry : IXbfType<TypeEntry>
    {
        public TypeEntry ReadValue(BinaryReader br)
        {
            TypeEntry te = new TypeEntry();
            te.Flags = (TypeFlags)br.ReadUInt32();
            te.NamespaceId = br.ReadUInt32();
            te.StringId = br.ReadUInt32();
            return te;
        }
    }

    [Flags]
    public enum TypeFlags
    {
        None = 0,
        IsMarkupDirective = 1
    }
}
