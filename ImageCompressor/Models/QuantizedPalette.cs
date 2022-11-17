using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ImageCompressor.Models
{
    public class QuantizedPalette
    {
        public QuantizedPalette(int size)
        {
            Colors = new List<Color>();
            PixelIndex = new int[size];
        }
        public IList<Color> Colors { get; private set; }
        public int[] PixelIndex { get; private set; }
    }
}