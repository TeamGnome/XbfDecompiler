using System;
using System.IO;

namespace LibXbf.Records.Types
{
    public struct PropertyEntry
    {
        public PropertyFlags Flags { get; set; }
        public uint TypeId { get; set; }
        public uint StringId { get; set; }
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
