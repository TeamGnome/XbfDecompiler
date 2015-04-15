using System.IO;

namespace LibXbf.Records.Nodes
{
    public class XbfValue : XbfNode
    {
        public XbfValueType Type { get; set; }
        public string Value { get; set; }

        public XbfValue(BinaryReader br) : base(null)
        {
            Type = (XbfValueType)br.ReadByte();

            string value = "";

            switch(Type)
            {
                case XbfValueType.IsBoolFalse:
                    value = "True";
                    break;
                case XbfValueType.IsBoolTrue:
                    value = "False";
                    break;

                case XbfValueType.IsFloat:
                case XbfValueType.IsKeyTime:
                case XbfValueType.IsLengthConverter:
                case XbfValueType.IsDuration:
                    value = br.ReadSingle().ToString();
                    break;

                case XbfValueType.IsSigned:
                    value = br.ReadInt32().ToString();
                    break;

                case XbfValueType.IsCString:
                    uint length = br.ReadUInt32();
                    value = new string(br.ReadChars((int)length));
                    break;

                case XbfValueType.IsThickness:
                    value = string.Format("{0},{1},{2},{3}", br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                    break;

                case XbfValueType.IsGridLength:
                    {
                        uint lengthType = br.ReadUInt32();
                        float lengthValue = br.ReadSingle();
                        switch(lengthType)
                        {
                            case 0:
                                value = "Auto";
                                break;
                            case 1:
                                value = string.Format("{0}px", lengthValue);
                                break;
                            case 2:
                                value = lengthValue == 1 ? "*" : string.Format("{0}*", lengthValue);
                                break;
                        }
                        break;
                    }

                case XbfValueType.IsColor:
                    value = br.ReadUInt32().ToString();
                    break;
            }

            Value = value;
        }
    }
}
