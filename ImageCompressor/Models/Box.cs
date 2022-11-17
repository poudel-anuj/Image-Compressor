using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageCompressor.Models
{
    public struct Box
    {
        public byte AlphaMinimum;
        public byte AlphaMaximum;
        public byte RedMinimum;
        public byte RedMaximum;
        public byte GreenMinimum;
        public byte GreenMaximum;
        public byte BlueMinimum;
        public byte BlueMaximum;
        public int Size;
    }
}