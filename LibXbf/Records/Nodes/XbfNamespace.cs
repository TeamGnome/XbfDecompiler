using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibXbf.Records.Nodes
{
    public class XbfNamespace : XbfNode
    {
        public string Namespace { get; set; }

        public XbfNamespace(BinaryReader br) : base(br)
        {
            uint length = br.ReadUInt32();

            Namespace = new string(br.ReadChars((int)length));
        }
    }
}
