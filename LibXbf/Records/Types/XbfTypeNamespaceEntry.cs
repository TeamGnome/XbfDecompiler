﻿using System.IO;

namespace LibXbf.Records.Types
{
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
