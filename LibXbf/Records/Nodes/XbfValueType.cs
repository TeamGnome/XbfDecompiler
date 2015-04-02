using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibXbf.Records.Nodes
{
    public enum XbfValueType : byte
    {
        None = 0,
        IsBoolFalse = 1,
        IsBoolTrue = 2,
        IsFloat = 3,
        IsSigned = 4,
        IsCString = 5,
        IsKeyTime = 6,
        IsThickness = 7,
        IsLengthConverter = 8,
        IsGridLength = 9,
        IsColor = 10,
        IsDuration = 11
    }
}
