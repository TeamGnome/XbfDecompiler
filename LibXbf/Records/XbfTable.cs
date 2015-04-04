using LibXbf.Records.Types;
using System;
using System.Collections.Generic;
using System.IO;

namespace LibXbf.Records
{
    public class XbfTable<T, C> where C : IXbfType<T>, new()
    {
        public uint Size { get; set; }
        public T[] Values { get; set; }

        public XbfTable(BinaryReader br, Version fv)
        {
            Size = br.ReadUInt32();

            List<T> _values = new List<T>((int)Size);

            for (int i = 0; i < Size; i++)
            {
                _values.Insert(i, new C().ReadValue(br, fv));
            }

            Values = _values.ToArray();
        }
    }
}
