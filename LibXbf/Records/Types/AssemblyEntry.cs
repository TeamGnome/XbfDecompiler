using System.IO;

namespace LibXbf.Records.Types
{
    public struct AssemblyEntry
    {
        public AssemblyEntryType ProviderKind { get; set; }
        public uint StringId { get; set; }
    }

    public enum AssemblyEntryType
    {
        Unknown = 0,
        Native = 1,
        Managed = 2,
        System = 3,
        Parser = 4,
        Alternate = 5
    }
}
