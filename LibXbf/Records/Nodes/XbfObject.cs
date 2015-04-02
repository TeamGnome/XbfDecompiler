using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibXbf.Records.Nodes
{
    public class XbfObject : XbfNode
    {
        public List<XbfProperty> Properties { get; set; }

        public XbfObject(BinaryReader br) : base (br)
        {
            Properties = new List<XbfProperty>();
        }
    }
}
