using System.IO;

namespace LibXbf.Records.Types
{
    public interface IXbfType<T>
    {
        T ReadValue(BinaryReader br);
    }

    public class XbfUInt32 : IXbfType<uint>
    {
        public uint ReadValue(BinaryReader br)
        {
            return br.ReadUInt32();
        }
    }
}
