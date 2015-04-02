using System.IO;

namespace LibXbf.Records.Types
{
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
}
