using System.IO;

namespace LibXbf.Records
{
    public struct AssemblyEntry
    {
        public AssemblyEntryType ProviderKind { get; set; }
        public uint StringId { get; set; }
    }

    public class XbfAssemblyEntry : IXbfType<AssemblyEntry>
    {
        public AssemblyEntry ReadValue(BinaryReader br)
        {
            AssemblyEntry ae = new AssemblyEntry();
            ae.ProviderKind = (AssemblyEntryType)br.ReadUInt32();
            ae.StringId = br.ReadUInt32();
            return ae;
        }
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
