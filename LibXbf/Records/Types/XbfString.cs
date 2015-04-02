using System.IO;

namespace LibXbf.Records.Types
{
    public class XbfString : IXbfType<string>
    {
        public string ReadValue(BinaryReader br)
        {
            uint sizeOfVal = br.ReadUInt32();
            char[] val = br.ReadChars((int)sizeOfVal);

            return new string(val);
        }
    }
}
