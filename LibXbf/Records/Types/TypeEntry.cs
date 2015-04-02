using System;
using System.IO;

namespace LibXbf.Records.Types
{
    public struct TypeEntry
    {
        public TypeFlags Flags { get; set; }
        public uint NamespaceId { get; set; }
        public uint StringId { get; set; }
    }

    [Flags]
    public enum TypeFlags
    {
        None = 0,
        IsMarkupDirective = 1
    }
}
