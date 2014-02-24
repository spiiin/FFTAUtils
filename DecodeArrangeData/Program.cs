using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DecodeArrangeData
{
    class Program
    {
        static int readIntGba(byte[] data, int addr)
        {
            return (data[addr + 3] << 24) | (data[addr + 2] << 16) | (data[addr + 1]) << 8 | (data[addr + 0]);
        }

        static int readWordGba(byte[] data, int addr)
        {
            return (data[addr + 1]) << 8 | (data[addr + 0]);
        }

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Call format: \"DecodeArrangeData.exe ArrangeArray.bin");
                return;
            }
            var Filename = args[0];
            try
            {
                byte[] data;
                using (FileStream f = File.OpenRead(Filename))
                {
                    int size = (int)f.Length;
                    data = new byte[size];
                    f.Read(data, 0, size);
                }
                Console.WriteLine("Decode file: " + Filename);
                int headerAddr = readIntGba(data,0);
                int maxTileNo = 0;
                int maxAddr = 0;
                Console.WriteLine("Header: {0,8:X8}", headerAddr);
                int ptr = 4;
                while (ptr < data.Length)
                {
                    int nextAddr = readWordGba(data, ptr);
                    if (nextAddr == 0)
                        break;
                    int tileCount = data[ptr+2];
                    int newMaxAddr = nextAddr + tileCount - 1;
                    if (newMaxAddr > maxAddr)
                        maxAddr = newMaxAddr;
                    Console.Write("{0,4:X4} - {1,2:D2} - ", nextAddr, tileCount);
                    for (int i = 0; i < tileCount; i++)
                    {
                        int tileNo = readWordGba(data, ptr + 3 + i*2);
                        if (tileNo > maxTileNo)
                            maxTileNo = tileNo;
                        Console.Write("{0,4:X4} ", tileNo);
                    }
                    Console.WriteLine();
                    ptr += tileCount*2 + 3;
                }
                int addDataCount = 0;
                while (ptr < data.Length - 1 )
                {
                    int d = readWordGba(data, ptr);
                    Console.Write("{0,4:X4} ", d);
                    ptr += 2;
                    addDataCount++;
                }
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Max tileNo : 0x{0,4:X4} ({1})", maxTileNo, maxTileNo);
                Console.WriteLine("Max addr   : 0x{0,4:X4}", maxAddr);
                Console.WriteLine("Add bytes  : {0}", addDataCount);
                Console.WriteLine();
                Console.WriteLine("Press any key");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
            }
        }
    }
}
