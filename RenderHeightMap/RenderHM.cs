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

namespace WindowsFormsApplication2
{
    public partial class RenderHM : Form
    {
        public RenderHM()
        {
            InitializeComponent();
        }

        //hardcode
        const int WIDTH = 16;
        const int HEIGHT = 8;
        const int MAP_WIDTH = 16;
        const int MAP_HEIGHT = 14;
        const string FILENAME = "gameData/h_08577984.bin";

        static Random rnd = new Random();

        private void drawPile(Graphics g, Point pos, int height)
        {
            var brushes = new Brush[] { Brushes.Red, Brushes.Green, Brushes.Blue, Brushes.BlueViolet, Brushes.DeepPink, Brushes.DarkCyan, Brushes.Crimson, Brushes.DarkRed};
            var brush = brushes[rnd.Next(brushes.Length)];
            var redPen = new Pen(Brushes.Black, 1.0f);
            var pointsArray1 = new Point[] { new Point(pos.X - WIDTH, pos.Y ), new Point(pos.X, pos.Y - HEIGHT), new Point(pos.X + WIDTH, pos.Y), new Point(pos.X , pos.Y + HEIGHT) };
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

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            for (int i = 0; i < MAP_WIDTH; i++)
            {
                for (int j = MAP_HEIGHT - 1; j >= 0; j--)
                {
                    //hardcode. don't render first 2 lines of dump. it's addresses, not values.
                    if (i < 2)
                        continue;
                    int no = (MAP_HEIGHT - j - 1) * MAP_WIDTH + i;
                    int x = (j * WIDTH) + (i * WIDTH) + 80;
                    int y = (i * HEIGHT) - (j * HEIGHT) +250;
                    drawPile(g, new Point(x, y), 8 * data[no*2]);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (FileStream f = File.OpenRead(FILENAME))
            {
                int size = (int)f.Length;
                data = new byte[size];
                f.Read(data, 0, size);
            }
        }

        byte[] data = null;
    }
}
