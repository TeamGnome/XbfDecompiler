using System;
using System.IO;

namespace LibXbf.Records.Types
{
    public class XbfUInt32 : IXbfType<uint>
    {
        public uint ReadValue(BinaryReader br, Version fv)
        {
            return br.ReadUInt32();
        }
    }
}
