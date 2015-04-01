using System;
using System.IO;

namespace LibXbf.Records
{
    public struct PropertyEntry
    {
        public PropertyFlags Flags { get; set; }
        public uint TypeId { get; set; }
        public uint StringId { get; set; }
    }

    public class XbfPropertyEntry : IXbfType<PropertyEntry>
    {
        public PropertyEntry ReadValue(BinaryReader br)
        {
            PropertyEntry te = new PropertyEntry();
            te.Flags = (PropertyFlags)br.ReadUInt32();
            te.TypeId = br.ReadUInt32();
            te.StringId = br.ReadUInt32();
            return te;
        }
    }

    [Flags]
    public enum PropertyFlags
    {
        None = 0,
        IsXmlProperty = 1,
        IsMarkupDirective = 2,
        IsImplicitProperty = 4
    }
}
