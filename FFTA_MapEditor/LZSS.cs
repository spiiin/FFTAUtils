using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFTA_MapEditor
{
    class LZSS
    {
        public int Decompress(byte[] source, ref byte[] dest)
        {
            int retlen = source[3] + (source[2] * 0x100) + (source[1] * 0x10000) + (source[0] * 0x1000000);
            int xIn = 4;
            int xOut = 0;
            int tmp = 0;
            int i = 0;
            int j = 0;

            while (xOut < retlen)
            {
                if ((source[xIn] & 0x80) == 0x80)
                {
                    tmp = (xOut - ((source[xIn] & 0x07) << 8)) - source[xIn + 1] - 1;

                    for (i = ((source[xIn] >> 3) & 0x0F) + 3; i > 0; i--)
                    {
                        dest[xOut] = dest[tmp];
                        xOut++;
                        tmp++;
                    }

                    xIn++;
                }
                else if ((source[xIn] & 0x40) == 0x40)
                {
                    for (i = (source[xIn] & 0x3F) + 1; i > 0; i--)
                    {
                        xIn++;
                        dest[xOut] = source[xIn];
                        xOut++;
                    }
                }
                else if ((source[xIn] & 0x20) == 0x20)
                {
                    for (i = (source[xIn] & 0x1F) + 2; i > 0; i--)
                    {
                        dest[xOut] = 0;
                        xOut++;
                    }
                }
                else if ((source[xIn] & 0x10) == 0x10)
                {
                    j = ((source[xIn + 1] & 0x3F) << 8) | source[xIn + 2];

                    tmp = (xOut - j) - 1;
                    if (tmp < 0) tmp = 0;

                    for (i = (((source[xIn + 1] >> 2) & 0x30) | (source[xIn] & 0x0F)) + 4; i > 0; i--)
                    {
                        dest[xOut] = dest[tmp];
                        tmp++;
                        xOut++;
                    }

                    xIn += 2;
                }
                else if (source[xIn] == 0x01)
                {
                    for (i = source[xIn + 1] + 3; i > 0; i--)
                    {
                        dest[xOut] = 0xFF;
                        xOut++;
                    }

                    xIn += 1;
                }
                else if (source[xIn] == 0x02)
                {
                    for (i = source[xIn + 1] + 3; i > 0; i--)
                    {
                        dest[xOut] = 0x00;
                        xOut++;
                    }

                    xIn += 1;
                }
                else if (source[xIn] == 0x00)
                {
                    j = (source[xIn + 2] << 0x08) | source[xIn + 3];
                    tmp = xOut - j - 1;
                    if (tmp < 0) tmp = 0;

                    for (i = source[xIn + 1] + 5; i > 0; i--)
                    {
                        dest[xOut] = dest[tmp];
                        xOut++;
                        tmp++;
                    }

                    xIn += 3;
                }

                xIn++;
            }

            return retlen;
        }
    }
}
