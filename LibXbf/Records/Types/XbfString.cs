using System;
using System.IO;

namespace LibXbf.Records.Types
{
    public class XbfString : IXbfType<string>
    {
        public string ReadValue(BinaryReader br, Version fv)
        {
            uint sizeOfVal = br.ReadUInt32();
            char[] val = br.ReadChars((int)sizeOfVal);

            if(fv.Major == 2)
            {
                // XBFv2 pads strings out with \0 in 16-bit chars for some reason.
                br.ReadChar();
            }

            return new string(val);
        }
    }
}
