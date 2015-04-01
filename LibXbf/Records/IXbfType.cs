using System.IO;

namespace LibXbf.Records
{
    public interface IXbfType<T>
    {
        T ReadValue(BinaryReader br);
    }

    public class XbfString : IXbfType<string>
    {
        public string ReadValue(BinaryReader br)
        {
            uint sizeOfVal = br.ReadUInt32();
            char[] val = br.ReadChars((int)sizeOfVal);

            return new string(val);
        }
    }

    public class XbfUInt32 : IXbfType<uint>
    {
        public uint ReadValue(BinaryReader br)
        {
            return br.ReadUInt32();
        }
    }
}
