using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace AnimatedGifGlobalPalette
{
    class AnimatedGifGlobalPalette
    {
        private byte[] gifHeader = new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }; //GIF89a
        private byte[] loopHeader = new byte[] { 0x21, 0xFF, 0x0B, 0x4E, 0x45, 0x54, 0x53, 0x43, 0x41, 0x50, 0x45, 0x32, 0x2E, 0x30, 0x03, 0x01, 0x00, 0x00, 0x00 }; //http://odur.let.rug.nl/~kleiweg/gif/netscape.html

        private List<Image> frames = new List<Image>();
        private List<UInt16> delays = new List<UInt16>();
        private Color[] palette;
        private Size size;
        private bool transparent = false;

        public AnimatedGifGlobalPalette(UInt16 width, UInt16 height, Color[] pal, Color color0, bool color0IsTransparent)
        {
            size = new Size(width < 1 ? 1 : width, height < 1 ? 1 : height);
            palette = pal;
            if (color0 != Color.Empty && color0IsTransparent) palette[0] = color0;
            transparent = color0IsTransparent;
        }

        public AnimatedGifGlobalPalette(Image img, Color[] pal, Color color0, bool color0IsTransparent)
        {
            frames.Add(img);
            delays.Add(0);
            size = new Size(img.Width < 1 ? 1 : img.Width, img.Height < 1 ? 1 : img.Height);
            palette = pal;
            if (color0 != Color.Empty && color0IsTransparent) palette[0] = color0;
            transparent = color0IsTransparent;
        }

        public void AddFrame(Image img, UInt16 delay)
        {
            frames.Add(img);
            delays.Add(delay);
        }

        public void WriteToFile(string filename)
        {
            if (frames.Count < 1) return;
            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                fs.Position = 0;
                fs.Write(gifHeader, 0, gifHeader.Length);               //Main Header "GIF89a"
                fs.Write(CreateScreenDescriptor(), 0, 7);               //Width, Height, etc
                fs.Write(CreateColorTable(), 0, 0x300);                 //Global Color Table
                if (frames.Count > 1) fs.Write(loopHeader, 0, loopHeader.Length);             //"NETSCAPE2.0" Header for infinite looping.

                for (int i = 0; i < frames.Count; i++)
                {
                    fs.Write(CreateGraphicsControlExtension(i), 0, 8);
                    fs.Write(CreateImageDescriptor(i), 0, 10);
                    LZWEncoder encoder = new LZWEncoder(frames[i].Width, frames[i].Height, GetPixels(i), 8);
                    encoder.Encode(fs);
                }

                fs.WriteByte(0x3B);
            }
        }

        private byte[] GetPixels(int index)
        {
            byte[] ret = new byte[frames[index].Width * frames[index].Height];

            Bitmap img = new Bitmap(frames[index]);

            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    ret[x + y * img.Width] = FindColorInPalette(img.GetPixel(x, y));
                }
            }

            return ret;
        }

        private byte FindColorInPalette(Color color)
        {
            for (int i = 0; i < palette.Length; i++)
            {
                if (color == palette[i]) return (byte)i;
            }
            return 0;
        }

        private byte[] CreateScreenDescriptor()
        {
            byte[] ret = new byte[7];

            ret[0] = (byte)(size.Width & 0xFF);
            ret[1] = (byte)((size.Width & 0xFF00) >> 8);
            ret[2] = (byte)(size.Height & 0xFF);
            ret[3] = (byte)((size.Height & 0xFF00) >> 8);
            ret[4] = 0xF7;
            ret[5] = 0;
            ret[6] = 0;

            return ret;
        }

        private byte[] CreateImageDescriptor(int index)
        {
            byte[] ret = new byte[10];

            ret[0] = 0x2C;
            ret[1] = 0; //Image left hword
            ret[2] = 0;
            ret[3] = 0; //Image top hword
            ret[4] = 0;
            ret[5] = (byte)(frames[index].Width & 0xFF);
            ret[6] = (byte)((frames[index].Width & 0xFF00) >> 8);
            ret[7] = (byte)(frames[index].Height & 0xFF);
            ret[8] = (byte)((frames[index].Height & 0xFF00) >> 8);
            ret[9] = 0;


            return ret;
        }

        private byte[] CreateColorTable()
        {
            byte[] ret = new byte[0x100 * 3];

            for (int i = 0; i < (palette.Length < 0x100 ? palette.Length : 0x100); i++)
            {
                ret[i * 3 + 0] = palette[i].R;
                ret[i * 3 + 1] = palette[i].G;
                ret[i * 3 + 2] = palette[i].B;
            }
            for (int i = palette.Length; i < 0x100; i++)
            {
                ret[i * 3 + 0] = 0;
                ret[i * 3 + 1] = 0;
                ret[i * 3 + 2] = 0;
            }

            return ret;
        }

        private byte[] CreateGraphicsControlExtension(int index)
        {
            byte[] ret = new byte[8];

            ret[0] = 0x21;
            ret[1] = 0xF9;
            ret[2] = 0x04; //Block Size
            ret[3] = (byte)(0x00 + (transparent ? 1 : 0)); //Contains Transparent Color;
            UInt16 thisDelay = delays.ToArray()[index];
            ret[4] = (byte)(thisDelay & 0xFF);
            ret[5] = (byte)((thisDelay & 0xFF00) >> 8);
            ret[6] = 0x00;
            ret[7] = 0x00;

            return ret;
        }
    }
}
