using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibXbf.Records.Nodes
{
    public class XbfProperty : XbfNode
    {
        public List<XbfNode> Values { get; set; }

        public XbfProperty(BinaryReader br) : base(br)
        {
            Values = new List<XbfNode>();
        }
    }
}
