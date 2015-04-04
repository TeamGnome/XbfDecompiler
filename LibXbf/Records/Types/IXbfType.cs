using System;
using System.IO;

namespace LibXbf.Records.Types
{
    public interface IXbfType<T>
    {
        T ReadValue(BinaryReader br, Version fv);
    }
}
