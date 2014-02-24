using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace RenderArrangeMap
{
    public partial class FormRenderMap : Form
    {
        public FormRenderMap()
        {
            InitializeComponent();
        }

        const int PILE_WIDTH = 16;
        const int PILE_HEIGHT = 8;
        const int MAP_WIDTH = 16;
        const int MAP_HEIGHT = 14;
        const int A_WIDTH  = 0x80;
        const int A_HEIGHT = 0x40;
        const string FILENAME_HEIGHTS = "gameData/h_085704C8.bin";
        const string FILENAME_ARRANGE = "gameData/a_0856F528.bin";
        const string FILENAME_PIC = "pic/map0.png";

        static Random rnd = new Random();

        private void drawPile(Graphics g, Point pos, int height, Color brushColor)
        {
            var brush = new SolidBrush(brushColor);

            var redPen = new Pen(Brushes.Black, 1.0f);
            var pointsArray1 = new Point[] { new Point(pos.X - PILE_WIDTH, pos.Y), new Point(pos.X, pos.Y - PILE_HEIGHT), new Point(pos.X + PILE_WIDTH, pos.Y), new Point(pos.X, pos.Y + PILE_HEIGHT) };
            var pointsArray2 = new Point[4];
            for (int i = 0; i < 4; i++)
                pointsArray2[i] = new Point(pointsArray1[i].X, pointsArray1[i].Y - height);

            g.DrawPolygon(redPen, pointsArray1);
            for (int i = 0; i < 4; i++)
            {
                g.FillPolygon(brush, new Point[] { pointsArray1[i], pointsArray2[i], pointsArray2[(i + 1) % 4], pointsArray1[(i + 1) % 4] });
                g.DrawLine(redPen, pointsArray1[i], pointsArray2[i]);
            }
            g.FillPolygon(brush, pointsArray2);
            g.DrawPolygon(redPen, pointsArray2);
        }

        static int readIntGba(byte[] data, int addr)
        {
            return (data[addr + 3] << 24) | (data[addr + 2] << 16) | (data[addr + 1]) << 8 | (data[addr + 0]);
        }

        static int readWordGba(byte[] data, int addr)
        {
            return (data[addr + 1]) << 8 | (data[addr + 0]);
        }

        static void writeWordGba(byte[] data, int addr, int word)
        {
            data[addr+1] = (byte)(word >> 8);
            data[addr] = (byte)(word & 0xFF);
        }

        private void FormRenderMap_Load(object sender, EventArgs e)
        {
            
            ilPictures.Images.AddStrip(Image.FromFile(FILENAME_PIC));
            tileRecs.Clear();
            try
            {
                byte[] data;
                using (FileStream f = File.OpenRead(FILENAME_ARRANGE))
                {
                    int size = (int)f.Length;
                    data = new byte[size];
                    f.Read(data, 0, size);
                }
                int headerAddr = readIntGba(data, 0);
                int maxTileNo = 0;
                int ptr = 4;
                while (ptr < data.Length)
                {
                    int nextAddr = readWordGba(data, ptr);
                    if (nextAddr == 0)
                        break;
                    int tileCount = data[ptr + 2];
                    for (int i = 0; i < tileCount; i++)
                    {
                        int tileNo = readWordGba(data, ptr + 3 + i * 2);
                        if (tileNo > maxTileNo)
                            maxTileNo = tileNo;
                        tileRecs.Add(new TileRec(nextAddr+i*4, tileNo));
                    }
                    ptr += tileCount * 2 + 3;
                }
                while (ptr < data.Length - 1)
                {
                    int d = readWordGba(data, ptr);
                    ptr += 2;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }

            using (FileStream f = File.OpenRead(FILENAME_HEIGHTS))
            {
                int size = (int)f.Length;
                hdata = new byte[size];
                f.Read(hdata, 0, size);
            }

            convertToPlanarArray(tileRecs, out layer1Data, out layer2Data);

            pbMap.Invalidate();
        }

        List<TileRec> tileRecs = new List<TileRec>();
        int[] layer1Data, layer2Data;
        byte[] hdata;
        
        private void pbMap_Paint(object sender, PaintEventArgs e)
        {
            bool showLayer1 = cbLayer1.Checked;
            bool showLayer2 = cbLayer2.Checked;
            bool showLayer3 = cbLayer3.Checked;
            var g = e.Graphics;
            if (showLayer1)
            {
                /*for (int i = 0; i < tileRecs.Count; i++)
                {
                    var tileRec = tileRecs[i];
                    int yCoord = tileRec.addr / A_WIDTH;
                    int xCoord = tileRec.addr % A_WIDTH;
                    if (yCoord > A_HEIGHT)
                        g.DrawImage(ilPictures.Images[tileRec.tileNo], new Point(xCoord * 4, (yCoord % A_HEIGHT) * 8));
                }*/
                for (int i = 0; i < layer1Data.Length; i++)
                {
                    if (layer1Data[i] != -1)
                    {
                        int yCoord = i / A_WIDTH;
                        int xCoord = i % A_WIDTH;
                        g.DrawImage(ilPictures.Images[layer1Data[i]], new Point(xCoord * 4, yCoord * 8));
                    }
                }
            }
            if (showLayer2)
            {
                /*for (int i = 0; i < tileRecs.Count; i++)
                {
                    var tileRec = tileRecs[i];
                    int yCoord = tileRec.addr / A_WIDTH;
                    int xCoord = tileRec.addr % A_WIDTH;
                    if (yCoord < A_HEIGHT)
                        g.DrawImage(ilPictures.Images[tileRec.tileNo], new Point(xCoord * 4, yCoord * 8));
                }*/
                for (int i = 0; i < layer2Data.Length; i++)
                {
                    if (layer2Data[i] != -1)
                    {
                        int yCoord = i / A_WIDTH;
                        int xCoord = i % A_WIDTH;
                        g.DrawImage(ilPictures.Images[layer2Data[i]], new Point(xCoord * 4, yCoord * 8));
                    }
                }
            }
            if (showLayer3)
            {
                Color[] colors = new Color[] { Color.FromArgb(255, 0, 255, 0), Color.FromArgb(255, 64, 128, 64) };
                for (int i = 0; i < MAP_WIDTH; i++)
                {
                    for (int j = MAP_HEIGHT - 1; j >= 0; j--)
                    {
                        //hardcode. don't render first 2 lines of dump. it's addresses, not values.
                        if (i < 2)
                            continue;
                        int no = (MAP_HEIGHT - j - 1) * MAP_WIDTH + i;
                        int x = (j * PILE_WIDTH) + (i * PILE_WIDTH) + PILE_WIDTH;
                        int y = (i * PILE_HEIGHT) - (j * PILE_HEIGHT) + 352;
                        drawPile(g, new Point(x, y), 8 * hdata[no * 2], colors[(no+j)%2]);
                    }
                }
            }
        }

        private void cbLayer1_CheckedChanged(object sender, EventArgs e)
        {
            pbMap.Invalidate();
        }

        private void convertToPlanarArray(List<TileRec> tileRecs, out int[] layer1Data, out int[] layer2Data)
        {
            layer1Data = new int[A_WIDTH * A_HEIGHT];
            layer1Data = Enumerable.Repeat(-1, layer1Data.Length).ToArray();
            layer2Data = new int[A_WIDTH * A_HEIGHT];
            layer2Data = Enumerable.Repeat(-1, layer2Data.Length).ToArray();
            foreach (var tileRec in tileRecs)
            {
                int yCoord = tileRec.addr / A_WIDTH;
                int xCoord = tileRec.addr % A_WIDTH;
                if (yCoord < A_HEIGHT)
                    layer2Data[yCoord * A_WIDTH + xCoord] = tileRec.tileNo;
                else
                    layer1Data[(yCoord % A_HEIGHT) * A_WIDTH + xCoord] = tileRec.tileNo;
            }
        }

        private void btExport_Click(object sender, EventArgs e)
        {
            string fn = "map0_layer1.bin";
            byte[] data = new byte[A_WIDTH*A_HEIGHT*2/4];
            for (int i = 0; i < A_WIDTH*A_HEIGHT; i+=4)
                writeWordGba(data, i/2, layer1Data[i]);
            try
            {
                using (FileStream f = File.Open(fn, FileMode.Create))
                {
                    f.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            fn = "map0_layer2.bin";
            for (int i = 0; i < A_WIDTH * A_HEIGHT; i+=4)
                writeWordGba(data, i/2, layer2Data[i]);
            try
            {
                using (FileStream f = File.Open(fn, FileMode.Create))
                {
                    f.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    struct TileRec
    {
        public TileRec(int addr, int tileNo)
        {
            this.addr = addr;
            this.tileNo = tileNo;
        }
        public int addr;
        public int tileNo;
    }
}
