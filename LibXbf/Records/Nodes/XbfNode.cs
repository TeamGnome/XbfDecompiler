﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibXbf.Records.Nodes
{
    public class XbfNode
    {
        public uint Id { get; set; }
        public uint Flags { get; set; }

        public XbfLineInfo LineInfo { get; set; }

        public XbfNode(BinaryReader br)
        {
            Id = br.ReadUInt32();
            Flags = br.ReadUInt32();
        }
    }
}
