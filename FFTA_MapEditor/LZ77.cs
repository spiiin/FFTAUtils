using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFTA_MapEditor
{
    class LZ77
    {
        private int Inc(ref int variable, int optionalValue = 1)
        {
            int ret = variable;
            variable += optionalValue;
            return ret;
        }

        private int Dec(ref int source)
        {
            int ret = source;
            source--;
            return ret;
        }

        public int Decompress(byte[] source, ref byte[] dest)
        {
            int header = source[0] | (source[1] << 8) | (source[2] << 16) | (source[3] << 24);
            int i = 0;
            int j = 0;
            int xIn = 4;
            int xOut = 0;
            int length = 0;
            int offset = 0;
            int windowOffset = 0;
            int xLen = header / 256;
            int retLen = xLen;
            int data = 0;
            byte d = 0;

            while (xLen > 0)
            {
                d = source[xIn];
                xIn++;
                for (i = 0; i <= 7; i++)
                {
                    if ((d & 0x80) != 0)
                    {
                        data = ((source[xIn] * 0x100) | source[xIn + 1]);

                        xIn += 2;
                        length = (data / 4096) + 3;
                        offset = (data & 0xFFF);
                        windowOffset = xOut - offset - 1;
                        for (j = 0; j < length; j++)
                        {
                            if (windowOffset >= 0) dest[xOut] = dest[windowOffset];
                            xOut++;
                            windowOffset++;

                            xLen--;
                            if (xLen == 0)
                            {
                                return retLen;
                            }
                        }
                    }
                    else
                    {
                        dest[xOut] = source[xIn];
                        xOut++;
                        xIn++;

                        xLen--;
                        if (xLen == 0)
                        {
                            return retLen;
                        }
                    }
                    d = (byte)((int)(d * 2) % 0x100);
                }
            }
            return retLen;
        }

        public int Compress(int sourceSize, byte[] source, ref byte[] dest)
        {
            int i = 0;
            int j = 0;
            int xIn = 0;
            int xOut = 0;

            int length = 0;
            int offset = 0;
            int tmplen = 0;
            int tmpoff = 0;
            int tmpxin = 0;
            int tmpxout = 0;
            int bufxout = 0;
            byte ctrl = 0;
            byte[,] xdata = new byte[8, 2];

            dest[0] = 0x10;
            dest[1] = (byte)(sourceSize % 0x100);
            dest[2] = (byte)((sourceSize / 0x100) % 0x100);
            dest[3] = (byte)((sourceSize / 0x10000) % 0x100);

            while (sourceSize > tmpxin)
            {
                ctrl = 0;
                for (i = 7; i >= 0; i--)
                {
                    if (xIn < 0x1000)
                    {
                        j = xIn;
                    }
                    else
                    {
                        j = 0x1000;
                    }
                    length = 0;
                    offset = 0;
                    while (j > 1)
                    {
                        tmpxin = xIn;
                        tmpxout = (xIn - j);
                        while (source[Inc(ref tmpxin)] == source[Inc(ref tmpxout)])
                        {
                            if (tmpxin >= sourceSize) break;
                        }
                        tmplen = (tmpxin - xIn - 1);
                        tmpoff = (tmpxin - tmpxout - 1);
                        if (tmplen > length)
                        {
                            length = tmplen;
                            offset = tmpoff;
                        }
                        if (length > 0x12) break;
                        Inc(ref j, -1);
                    }
                    if (length >= 3)
                    {
                        ctrl = (byte)(ctrl | (1 * 2 ^ i));
                        if (length >= 0x12) { length = 0x12; }
                        xdata[i, 0] = (byte)(((length - 3) * (2 ^ 4)) | (offset / 0x100));
                        xdata[i, 1] = (byte)(offset % 0x100);
                        Inc(ref xIn, length);
                        Inc(ref bufxout, 2);
                    }
                    else
                    {
                        xdata[i, 0] = source[Inc(ref(xIn))];
                        Inc(ref bufxout);
                    }
                }
                dest[Inc(ref xOut) + 4] = ctrl;
                for (i = 7; i >= 0; i--)
                {
                    dest[Inc(ref xOut) + 4] = xdata[i, 0];
                    if ((ctrl & 0x80) != 0) { dest[Inc(ref xOut) + 4] = xdata[i, 1]; }
                    ctrl = (byte)((ctrl * 2) % 0x100);
                    if (sourceSize < tmpxin) break;
                }
            }
            return xOut + 4;
        }
    }
}
