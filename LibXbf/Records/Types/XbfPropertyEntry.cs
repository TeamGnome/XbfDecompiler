using System;
using System.IO;

namespace LibXbf.Records.Types
{
    public class XbfPropertyEntry : IXbfType<PropertyEntry>
    {
        public PropertyEntry ReadValue(BinaryReader br, Version fv)
        {
            PropertyEntry te = new PropertyEntry();
            te.Flags = (PropertyFlags)br.ReadUInt32();
            te.TypeId = br.ReadUInt32();
            te.StringId = br.ReadUInt32();
            return te;
        }
    }
}
