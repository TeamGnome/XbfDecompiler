using System;
using System.IO;

namespace LibXbf.Records.Types
{
    public class XbfTypeEntry : IXbfType<TypeEntry>
    {
        public TypeEntry ReadValue(BinaryReader br, Version fv)
        {
            TypeEntry te = new TypeEntry();
            te.Flags = (TypeFlags)br.ReadUInt32();
            te.NamespaceId = br.ReadUInt32();
            te.StringId = br.ReadUInt32();
            return te;
        }
    }
}
