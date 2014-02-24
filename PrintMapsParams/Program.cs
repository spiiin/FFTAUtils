using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PrintMapsParams
{
    class Program
    {
        static int readIntGba(byte[] data, int addr)
        {
            return (data[addr + 3] << 24) | (data[addr + 2] << 16) | (data[addr + 1]) << 8 | (data[addr + 0]);
        }

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Call format: \"PrintMapsParams.exe FFTA_FILENAME.gba");
                return;
            }
            var Filename = args[0];
            try
            {
                byte[] romdata;
                using (FileStream f = File.OpenRead(Filename))
                {
                    int size = (int)f.Length;
                    romdata = new byte[size];
                    f.Read(romdata, 0, size);
                }
                const int MAP_COUNT = 163;
                const int MAP_PTRS_BASE_ADDR = 0x569104;
                const int ROM_PREFIX = 0x8000000;
                const int MAP_REC_SIZE = 88;
                MapRec[] mapRecs = new MapRec[MAP_COUNT];
                //---------------------------------------------------------------------
                Console.WriteLine("Array pointers:");
                for (int mapNo = 0; mapNo < MAP_COUNT; mapNo++)
                {
                    int mapRecAddr = MAP_PTRS_BASE_ADDR + mapNo * MAP_REC_SIZE;
                    int graphicsDataAddr = readIntGba(romdata, mapRecAddr + 0) + MAP_PTRS_BASE_ADDR + ROM_PREFIX;
                    int arrangementDataAddr = readIntGba(romdata, mapRecAddr + 4) + MAP_PTRS_BASE_ADDR + ROM_PREFIX;
                    int heightDataAddr = readIntGba(romdata, mapRecAddr + 16) + MAP_PTRS_BASE_ADDR + ROM_PREFIX;
                    Console.WriteLine(
                        String.Format("No:{0,2:X2} G:{1,8:X8} A:{2,8:X8}, H:{3,8:X8}",
                            mapNo,
                            graphicsDataAddr,
                            arrangementDataAddr,
                            heightDataAddr
                        )
                    );
                    mapRecs[mapNo] = new MapRec();
                    mapRecs[mapNo].graphics_data = graphicsDataAddr;
                    mapRecs[mapNo].arrangement_data = arrangementDataAddr;
                    mapRecs[mapNo].height_data = heightDataAddr;
                }
                //---------------------------------------------------------------------
                var graphicsDataCount = new Dictionary<int, int>();
                var arrangmentDataCount = new Dictionary<int, int>();
                var heightDataCount = new Dictionary<int, int>();
                for (int i = 0; i < mapRecs.Length; i++)
                {
                    if (!graphicsDataCount.ContainsKey(mapRecs[i].graphics_data))
                      graphicsDataCount.Add(mapRecs[i].graphics_data, mapRecs.Count( mr => mr.graphics_data == mapRecs[i].graphics_data));
                    if (!arrangmentDataCount.ContainsKey(mapRecs[i].arrangement_data))
                        arrangmentDataCount.Add(mapRecs[i].arrangement_data, mapRecs.Count(mr => mr.arrangement_data == mapRecs[i].arrangement_data));
                    if (!heightDataCount.ContainsKey(mapRecs[i].height_data))
                        heightDataCount.Add(mapRecs[i].height_data, mapRecs.Count(mr => mr.height_data == mapRecs[i].height_data));
                }
                Console.WriteLine();
                Console.WriteLine("Press any key");
                Console.ReadLine();
                Console.WriteLine("Count of array:");
                Console.WriteLine("Graphics data count: {0}", graphicsDataCount.Keys.Count);
                foreach (var gdc in graphicsDataCount)
                    Console.WriteLine("G[{0,8:X8}] : {1}", gdc.Key, gdc.Value);

                Console.WriteLine("Arrangment data count: {0}", arrangmentDataCount.Keys.Count);
                foreach (var gdc in arrangmentDataCount)
                    Console.WriteLine("A[{0,8:X8}] : {1}", gdc.Key, gdc.Value);

                Console.WriteLine("Heights data count: {0}", heightDataCount.Keys.Count);
                foreach (var gdc in heightDataCount)
                    Console.WriteLine("H[{0,8:X8}] : {1}", gdc.Key, gdc.Value);
                Console.WriteLine();
                Console.WriteLine("Press any key");
                Console.ReadLine();
                //---------------------------------------------------------------------
                Console.WriteLine("Generate configAddrs.py file");
                Console.WriteLine("#Addresses of compressed array for Final Fantasy Tactics Advance");
                Console.WriteLine("#Generated file. Do not edit manually");
                Console.WriteLine("g = [");
                foreach (var gdc in graphicsDataCount)
                    Console.WriteLine("  0x{0,8:X8},", gdc.Key);
                Console.WriteLine("]");
                Console.WriteLine("a = [");
                foreach (var gdc in arrangmentDataCount)
                    Console.WriteLine("  0x{0,8:X8},", gdc.Key);
                Console.WriteLine("]");
                Console.WriteLine("h = [");
                foreach (var gdc in heightDataCount)
                    Console.WriteLine("  0x{0,8:X8},", gdc.Key);
                Console.WriteLine("]");
                Console.WriteLine();
                Console.WriteLine("Press any key");
                Console.ReadLine();
                //---------------------------------------------------------------------
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
            }
        }

        class MapRec
        {
            public int graphics_data = -1;
            public int arrangement_data = -1;
            public int clipping_data = -1 ;
            public int palette_data = -1;
            public int height_data = -1;
            public int[] unknown = null;
        }
    }
}