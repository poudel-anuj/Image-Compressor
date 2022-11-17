using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageCompressor.Models
{
    public struct CubeCut
    {
        public readonly byte? Position;
        public readonly float Value;

        public CubeCut(byte? cutPoint, float result)
        {
            Position = cutPoint;
            Value = result;
        }
    }
}