using System.IO;

namespace LibXbf.Records
{
    public class XbfHeader
    {
        public byte[] MagicNumber { get; set; }

        public uint MetadataSize { get; set; }

        public uint NodeSize { get; set; }

        public uint MajorFileVersion { get; set; }

        public uint MinorFileVersion { get; set; }

        public ulong StringTableOffset { get; set; }

        public ulong AssemblyTableOffset { get; set; }

        public ulong TypeNamespaceTableOffset { get; set; }

        public ulong TypeTableOffset { get; set; }

        public ulong PropertyTableOffset { get; set; }

        public ulong XmlNamespaceTableOffset { get; set; }

        public char[] Hash { get; set; }

        public XbfHeader(BinaryReader br)
        {
            MagicNumber = br.ReadBytes(4);
            MetadataSize = br.ReadUInt32();
            NodeSize = br.ReadUInt32();
            MajorFileVersion = br.ReadUInt32();
            MinorFileVersion = br.ReadUInt32();
            StringTableOffset = br.ReadUInt64();
            AssemblyTableOffset = br.ReadUInt64();
            TypeNamespaceTableOffset = br.ReadUInt64();
            TypeTableOffset = br.ReadUInt64();
            PropertyTableOffset = br.ReadUInt64();
            XmlNamespaceTableOffset = br.ReadUInt64();
            Hash = br.ReadChars(0x20);
        }
    }
}
