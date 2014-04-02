using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace FFTA_MapEditor
{
    public partial class Form1 : Form
    {
        const string ROMlocation = ".\\rom\\ffta.gba";
        const int PILE_WIDTH = 16;
        const int PILE_HEIGHT = 8;
        const int A_WIDTH = 0x80;
        const int A_HEIGHT = 0x40;
        int MAP_WIDTH = 16;
        int MAP_DEPTH = 16;

        Image imgTileBase;
        Image imgTiles;
        Image imgPalette;
        Image[] imgMap0;
        Image[] imgMap1;
        Image imgMapHeight;

        const int mapDataBase = 0x569104;

        const int mapHeightSize = 0x400;
        const int mapArrangeSize = 0x10000;
        const int mapClippingSize = 0x2000;
        const int mapPaletteSize = 0x800;
        const int mapTileSize = 0x8000;

        byte[] mapHeightData;
        byte[] mapArrangeData;
        byte[] mapClippingData;
        byte[][] mapAnimationTiles;
        Color[] paletteData;
        int paletteLength;
        int tileCount = 0;
        int animationCount = 0;
        int animationFrame = 0;
        bool animationOnTopOfTiles = false;
        int selectedTile = 0;
        int hoveredTile = -1;

        int mapGFXBase = 0;

        int[] arrangeDataForCad;
        public Form1()
        {
            InitializeComponent();
        }

        private void cboMapIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            LZ77 lz77 = new LZ77();
            LZSS lzss = new LZSS();
            FileStream fs = new FileStream(ROMlocation, FileMode.Open, FileAccess.ReadWrite);
            BinaryReader br = new BinaryReader(fs);

            mapAnimationTiles = null;
            animationFrame = 0;

            bool success = false;
            int mapIndex = cboMapIndex.SelectedIndex;

            fs.Position = mapDataBase + (0x58 * mapIndex) + 0x14;
            animationCount = br.ReadInt32();

            if (animationCount > 0)
            {
                fs.Position = mapDataBase + animationCount;
                Int16 temp = br.ReadInt16();
                Int16 frameCount = br.ReadInt16();
                byte[] frameData = br.ReadBytes(frameCount * 4);
                mapAnimationTiles = new byte[frameCount][];
                imgMap0 = new Image[frameCount];
                imgMap1 = new Image[frameCount];
                animationCount = (temp >> 5) + 1;

                fs.Position = mapDataBase + (0x58 * mapIndex) + 0x1C;
                int animDrawBase = br.ReadInt32();

                for (int i = 0; i < frameCount; i++)
                {
                    fs.Position = mapDataBase + (0x58 * mapIndex) + 0x18;
                    fs.Position = mapDataBase + br.ReadInt32();
                    fs.Position += (frameData[(i * 4 + 2)] | (frameData[(i * 4 + 3)] << 8)) * 0x20 - (animDrawBase & 0xFFFFFF);
                    mapAnimationTiles[i] = br.ReadBytes((animationCount * 0x20) + (animDrawBase & 0xFFFFFF));
                    for (int o = 0; o < (animDrawBase & 0xFFFFFF); o++) mapAnimationTiles[i][o] = 0;
                }

                btnPlay.Visible = true;
            }
            else
            {
                btnPlay.Checked = false;
                btnPlay.Visible = false;
                btnPlay.Image = FFTA_MapEditor.Properties.Resources.play;
                tmrAnimate.Enabled = false;
                imgMap0 = new Image[1];
                imgMap1 = new Image[1];

            }

            fs.Position = mapDataBase + (0x58 * mapIndex) + 0x1C;
            mapGFXBase = br.ReadInt32();

            mapClippingData = new byte[mapClippingSize];

            //Load Clipping Data.
            while (!success)
            {
                fs.Position = mapDataBase + (0x58 * mapIndex) + 0x08;
                fs.Position = mapDataBase + br.ReadInt32();

                byte bMapClippingType = br.ReadByte();

                if (bMapClippingType == 0x10)
                {
                    success = true;
                    fs.Position--;
                    byte[] source = br.ReadBytes(mapClippingSize);
                    int length = lz77.Decompress(source, ref mapClippingData);
                }
                else if (bMapClippingType == 1)
                {
                    int trueMap = br.ReadByte() | br.ReadByte() * 0x100 + br.ReadByte() * 0x10000;
                    if (trueMap == 0xFFFFFF) success = true; else { mapIndex = trueMap; continue; }
                    mapClippingData = br.ReadBytes(mapClippingSize);
                }
                else if (bMapClippingType == 0x11)
                {
                    int trueMap = br.ReadByte() | br.ReadByte() * 0x100 + br.ReadByte() * 0x10000;
                    if (trueMap == 0xFFFFFF) success = true; else { mapIndex = trueMap; continue; }
                    byte[] source = br.ReadBytes(mapClippingSize);
                    int length = lz77.Decompress(source, ref mapClippingData);
                }
                else
                {
                    success = true;
                    fs.Position += 3;
                    mapClippingData = br.ReadBytes(mapClippingSize);
                }
            }

            //Load Palette Data.
            fs.Position = mapDataBase + (0x58 * cboMapIndex.SelectedIndex) + 0x54;
            byte palType = br.ReadByte();
            if ((palType & 0x03) != 1 && (palType & 0x03) != 2)
            {
                fs.Position = mapDataBase + (0x58 * cboMapIndex.SelectedIndex) + 0x0C;
                fs.Position = mapDataBase + br.ReadInt32();
                if (br.ReadByte() == 0x10) //LZ77 Compressed
                {
                    chkPalCompressed.Checked = true;
                    fs.Position -= 1;
                    byte[] source = br.ReadBytes(mapPaletteSize);
                    byte[] dest = new byte[mapPaletteSize];
                    paletteLength = lz77.Decompress(source, ref dest) / 2;
                    imgPalette = createImageFromPaletteData(dest, paletteLength);
                }
                else //Uncompressed
                {
                    chkPalCompressed.Checked = false;
                    paletteLength = (((br.ReadByte() | (br.ReadByte() << 0x08) | (br.ReadByte() << 0x10)) & 0x3FFFFF) >> 1) / 2;
                    byte[] source = br.ReadBytes(mapPaletteSize);
                    imgPalette = createImageFromPaletteData(source, paletteLength);
                }
            }
            else
            {
                fs.Position = mapDataBase + (0x58 * cboMapIndex.SelectedIndex) + 0x56;
                int palIndex = br.ReadByte() << 1;
                int mapPalettes;
                if ((palType & 0x03) == 1)
                {
                    fs.Position = 0x01A4F8;
                    mapPalettes = br.ReadInt32() & 0x1FFFFFF;
                }
                else
                {
                    fs.Position = 0x01A514;
                    mapPalettes = br.ReadInt32() & 0x1FFFFFF;
                }
                fs.Position = mapPalettes + palIndex;
                fs.Position = mapPalettes + br.ReadInt16();
                byte[] source = br.ReadBytes(mapPaletteSize);
                byte[] dest = new byte[mapPaletteSize];
                paletteLength = lzss.Decompress(source, ref dest) / 2;
                imgPalette = createImageFromPaletteData(dest, paletteLength);
            }

            //Load Tiles.
            fs.Position = mapDataBase + (0x58 * cboMapIndex.SelectedIndex);
            fs.Position = mapDataBase + br.ReadInt32();
            byte bTileCompressionType = br.ReadByte();

            animationOnTopOfTiles = (((br.ReadByte() | (br.ReadByte() << 8) | (br.ReadByte() << 0x10)) & 0x4000) == 0x4000) ? true : false;

            if (bTileCompressionType == 0x10)
            {
                cboTileCompression.SelectedIndex = 1;
            }
            else if (bTileCompressionType == 0x12)
            {
                cboTileCompression.SelectedIndex = 1;
            }
            else if (bTileCompressionType == 0x20)
            {
                cboTileCompression.SelectedIndex = 2;
                byte[] source = br.ReadBytes(mapTileSize);
                byte[] dest = new byte[mapTileSize];
                int length = lzss.Decompress(source, ref dest);

                imgTileBase = createImageFromTileData(dest, length / 32);
            }
            else if (bTileCompressionType == 0x22)
            {
                cboTileCompression.SelectedIndex = 2;
                fs.Position += 4;
                byte[] source = br.ReadBytes(mapTileSize);
                byte[] dest = new byte[mapTileSize];
                int length = lzss.Decompress(source, ref dest);

                imgTileBase = createImageFromTileData(dest, length / 32);
            }

            success = false;
            mapIndex = cboMapIndex.SelectedIndex;

            //Load Heightmap.
            while (!success)
            {
                fs.Position = mapDataBase + (0x58 * mapIndex) + 0x10;
                fs.Position = mapDataBase + br.ReadInt32();

                byte bMapHeightType = br.ReadByte();

                if (bMapHeightType == 1) //Uncompressed. Packed.
                {
                    int trueMap = br.ReadByte() | br.ReadByte() * 0x100 | br.ReadByte() * 0x10000;
                    if (trueMap == 0xFFFFFF) success = true; else { mapIndex = trueMap; continue; }
                    chkHeightCompressed.Checked = false;
                    chkHeightPacked.Checked = true;
                    byte[] source = br.ReadBytes(mapHeightSize);
                    mapHeightData = unpackMapHeightData(source);
                }
                else if (bMapHeightType == 0x10) //Compressed. Unpacked.
                {
                    chkHeightCompressed.Checked = true;
                    chkHeightPacked.Checked = false;
                    fs.Position--;
                    byte[] source = br.ReadBytes(mapHeightSize);
                    int length = lz77.Decompress(source, ref mapHeightData);
                    MAP_WIDTH = 16;
                    MAP_DEPTH = 16;
                    success = true;
                }
                else if (bMapHeightType == 0x11) //Compressed. Packed.
                {
                    int trueMap = br.ReadByte() | br.ReadByte() * 0x100 | br.ReadByte() * 0x10000;
                    if (trueMap == 0xFFFFFF) success = true; else { mapIndex = trueMap; continue; }
                    chkHeightCompressed.Checked = true;
                    chkHeightPacked.Checked = true;
                    byte[] source = br.ReadBytes(mapHeightSize);
                    byte[] dest = new byte[mapHeightSize];
                    int length = lz77.Decompress(source, ref dest);

                    /*StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < dest.Length; i++)
                    {
                        sb.Append((i % 16 == 0 ? i.ToString("X5") + ": " : "") + dest[i].ToString("X2") + ((i + 1) % 16 == 0 ? '\n' : ' '));
                    }
                    Clipboard.SetText(sb.ToString());//*/

                    mapHeightData = unpackMapHeightData(dest);
                }
                else //Uncompressed. Unpacked.
                {
                    chkHeightCompressed.Checked = false;
                    chkHeightPacked.Checked = false;
                    mapHeightData = br.ReadBytes(mapHeightSize);
                    MAP_WIDTH = 16;
                    MAP_DEPTH = 16;
                    success = true;
                }
            }

            success = false;
            mapIndex = cboMapIndex.SelectedIndex;

            //Load Tile Arrangement Data.
            while (!success)
            {
                fs.Position = mapDataBase + (0x58 * mapIndex) + 0x04;
                fs.Position = mapDataBase + br.ReadInt32();

                byte bMapArrangementType = br.ReadByte();
                int length = 0;

                mapArrangeData = new byte[mapArrangeSize];
                if (bMapArrangementType == 0x10)
                {
                    fs.Position--;
                    byte[] source = br.ReadBytes(mapArrangeSize);
                    length = lz77.Decompress(source, ref mapArrangeData);
                    success = true;
                }
                else if (bMapArrangementType == 1)
                {
                    int trueMap = br.ReadByte() | br.ReadByte() * 0x100 | br.ReadByte() * 0x10000;
                    if (trueMap == 0xFFFFFF) success = true; else { mapIndex = trueMap; continue; }
                    mapArrangeData = br.ReadBytes(mapArrangeSize);
                    length = 0x2000;
                }
                else if (bMapArrangementType == 0x11)
                {
                    int trueMap = br.ReadByte() | br.ReadByte() * 0x100 | br.ReadByte() * 0x10000;
                    if (trueMap == 0xFFFFFF) success = true; else { mapIndex = trueMap; continue; }
                    byte[] source = br.ReadBytes(mapArrangeSize);
                    length = lz77.Decompress(source, ref mapArrangeData);
                    success = true;
                }
            }

            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < mapArrangeData.Length; i++)
            {
                sb.Append((i % 16 == 0 ? i.ToString("X5") + ": " : "") + mapArrangeData[i].ToString("X2") + ((i + 1) % 16 == 0 ? '\n' : ' '));
            }
            Clipboard.SetText(sb.ToString());//*/

            if (animationCount > 0) LoadMap(0, 0);

            imgTiles = createImageFromTileFormatData(mapArrangeData);
            Image[] imgMap = createImageFromArrangeData(mapArrangeData, out arrangeDataForCad);;

            imgMap0[0] = imgMap[0];
            imgMap1[0] = imgMap[1];

            Image imgTemp = new Bitmap(imgTiles.Width * 3, imgTiles.Height * 3);
            Graphics gTemp = Graphics.FromImage(imgTemp);
            gTemp.InterpolationMode = InterpolationMode.NearestNeighbor;
            gTemp.PixelOffsetMode = PixelOffsetMode.Half;
            gTemp.DrawImage(imgTiles, new Rectangle(0, 0, imgTemp.Width, imgTemp.Height), new Rectangle(0, 0, imgTiles.Width, imgTiles.Height), GraphicsUnit.Pixel);
            picTiles.Image = imgTemp;

            imgMapHeight = createImageFromMapHeights(200);

            DrawMapFromLoadedImages();
            picPalette.Image = imgPalette;

            //Clipboard.SetImage(imgTileBase);

            picHmapEdit.Invalidate();
        }

        private void DrawMapFromLoadedImages()
        {
            Image img = new Bitmap(512, 512);
            Graphics g = Graphics.FromImage(img);

            Image temp = LoadMap(1, animationFrame);
            if (chkShowBottom.Checked) g.DrawImage(temp, new Point(0, 0));
            if (chkShowTop.Checked) g.DrawImage(LoadMap(0, animationFrame), new Point(0, 0));
            if (chkShowHeight.Checked) g.DrawImage(imgMapHeight, new Point(0, 0));

            picMain.Image = img;
        }

        private Image[] createImageFromArrangeData(byte[] source, out int[] planarArray)
        {
            bool complete = false;
            Image img = new Bitmap(512, 1024);
            Graphics g = Graphics.FromImage(img);
            planarArray = new int[64 * 64];
            planarArray = Enumerable.Repeat(-1, planarArray.Length).ToArray();
            int ptr = 4;
            while (!complete)
            {
                int addr = mapArrangeData[ptr] | (mapArrangeData[ptr + 1] << 0x8);
                if (addr == 0) { complete = true; break; }
                int iCount = mapArrangeData[ptr + 2];
                ptr += 3;

                //Halfword tiles
                if ((mapArrangeData[3] & 0x40) == 0x40)
                {
                    for (int i = 0; i < iCount; i++)
                    {
                       int tileNo = mapArrangeData[ptr] | (mapArrangeData[ptr + 1] << 0x8);
                        int planarAddr = ((addr % 0x80) +(addr / 0x80)*128)/4;
                        planarArray[planarAddr] = tileNo;
                        g.DrawImage(imgTiles, new Rectangle((addr % 0x80) * 4, (addr / 0x80) * 8, 16, 8), new Rectangle((BitConverter.ToInt16(mapArrangeData, ptr) * 16), 0, 16, 8), GraphicsUnit.Pixel);
                        ptr += 2;
                        addr += 0x4;
                    }
                }
                //Single byte tiles
                else 
                {
                    for (int i = 0; i < iCount; i++)
                    {
                        int tileNo = mapArrangeData[ptr];
                        g.DrawImage(imgTiles, new Rectangle((addr % 0x80) * 4, (addr / 0x80) * 8, 16, 8), new Rectangle((tileNo * 16), 0, 16, 8), GraphicsUnit.Pixel);
                        ptr += 1;
                        addr += 0x4;
                    }
                }
            }

            Image[] ret = new Bitmap[2];
            ret[0] = new Bitmap(512, 512);
            g = Graphics.FromImage(ret[0]);
            g.DrawImage(img, new Rectangle(0, 0, 512, 512), new Rectangle(0, 0, 512, 512), GraphicsUnit.Pixel);

            ret[1] = new Bitmap(512, 512);
            g = Graphics.FromImage(ret[1]);
            g.DrawImage(img, new Rectangle(0, 0, 512, 512), new Rectangle(0, 512, 512, 512), GraphicsUnit.Pixel);

            return ret;
        }

        private Image createImageFromTileFormatData(byte[] source)
        {
            Image img = new Bitmap(0x10000, 8);
            Graphics g = Graphics.FromImage(img);
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.SmoothingMode = SmoothingMode.None;
            tileCount = 0;
            int xIn = source[0] + (source[1] << 0x08);
            int tile1 = 0;
            int tile2 = 0;

            while (true)
            {
                if ((source[xIn] == 0) && (source[xIn + 1] == 0) && (source[xIn + 2] == 0) && (source[xIn + 3] == 0)) break;
                tile1 = source[xIn] + (source[xIn + 1] << 0x08);
                if ((tile1 & 0x8000) == 0x8000)
                {
                    tile2 = source[xIn + 2] + (source[xIn + 3] << 0x08);
                }
                else
                {
                    tile2 = tile1 + 1;
                }
                g.DrawImage(imgTileBase, new Rectangle((tileCount * 16), 0, 8, 8), new Rectangle(((tile1) & 0xFFF) * 8, ((tile1 & 0x7000) / 0x1000) * 8, 8, 8), GraphicsUnit.Pixel);
                g.DrawImage(imgTileBase, new Rectangle((tileCount * 16) + 8, 0, 8, 8), new Rectangle(((tile2) & 0xFFF) * 8, ((tile2 & 0x7000) / 0x1000) * 8, 8, 8), GraphicsUnit.Pixel);
                tileCount++;
                xIn += 2;
            }

            Image ret = new Bitmap(tileCount * 16, 8);
            g = Graphics.FromImage(ret);
            g.DrawImage(img, 0, 0);

            return ret;
        }

        private Image createImageFromTileData(byte[] source, int count)
        {
            Bitmap img = new Bitmap((count + animationCount) * 8, 8 * paletteLength / 16);

            BitmapData data = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int stride = data.Stride;

            int animationDrawBase = animationOnTopOfTiles ? 0 : animationCount;
            int palIndex = 0;
            int ptrIndex = 0;

            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                for (int tile = (animationCount == 0 ? 1 : 0); tile < count; tile++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        for (int x = 0; x < 8; x += 2)
                        {
                            for (int pal = 0; pal < paletteLength / 16; pal++)
                            {
                                palIndex = (source[(tile * 32) + y * 4 + (x / 2)] & 0x0F) + 0x10 * pal;
                                ptrIndex = (((tile + animationDrawBase) * 8 + x + 0) * 4) + (pal * 8 + y) * stride;
                                ptr[ptrIndex + 0] = paletteData[palIndex].B;
                                ptr[ptrIndex + 1] = paletteData[palIndex].G;
                                ptr[ptrIndex + 2] = paletteData[palIndex].R;
                                ptr[ptrIndex + 3] = palIndex % 16 == 0 ? (byte)0 : paletteData[palIndex].A;
                                palIndex = ((source[(tile * 32) + y * 4 + (x / 2)] & 0xF0) / 0x10) + 0x10 * pal;
                                ptrIndex = (((tile + animationDrawBase) * 8 + x + 1) * 4) + (pal * 8 + y) * stride;
                                ptr[ptrIndex + 0] = paletteData[palIndex].B;
                                ptr[ptrIndex + 1] = paletteData[palIndex].G;
                                ptr[ptrIndex + 2] = paletteData[palIndex].R;
                                ptr[ptrIndex + 3] = palIndex % 16 == 0 ? (byte)0 : paletteData[palIndex].A;
                            }
                        }
                    }
                }
            }

            img.UnlockBits(data);

            Image ret = new Bitmap((count + animationDrawBase) * 24, 24 * paletteLength / 16);
            Graphics g = Graphics.FromImage(ret);
            g.SmoothingMode = SmoothingMode.None;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;

            g.DrawImage(img, new Rectangle(0, 0, (count + animationDrawBase) * 24, 24 * paletteLength / 16), new Rectangle(0, 0, (count + animationDrawBase) * 8, 8 * paletteLength / 16), GraphicsUnit.Pixel);

            return img;
        }

        private void updateTileSetWithNewAnimationFrames(int frame)
        {
            if (frame >= mapAnimationTiles.Length) frame = 0;

            Bitmap img = new Bitmap(imgTileBase);

            BitmapData data = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int stride = data.Stride;

            int palIndex = 0;
            int ptrIndex = 0;

            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                for (int tile = 0; tile < animationCount; tile++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        for (int x = 0; x < 8; x += 2)
                        {
                            for (int pal = 0; pal < paletteLength / 16; pal++)
                            {
                                palIndex = (mapAnimationTiles[frame][(tile * 32) + y * 4 + (x / 2)] & 0x0F) + 0x10 * pal;
                                ptrIndex = ((tile * 8 + x + 0) * 4) + (pal * 8 + y) * stride;
                                ptr[ptrIndex + 0] = paletteData[palIndex].B;
                                ptr[ptrIndex + 1] = paletteData[palIndex].G;
                                ptr[ptrIndex + 2] = paletteData[palIndex].R;
                                ptr[ptrIndex + 3] = palIndex % 16 == 0 ? (byte)0 : paletteData[palIndex].A;
                                palIndex = ((mapAnimationTiles[frame][(tile * 32) + y * 4 + (x / 2)] & 0xF0) / 0x10) + 0x10 * pal;
                                ptrIndex = ((tile * 8 + x + 1) * 4) + (pal * 8 + y) * stride;
                                ptr[ptrIndex + 0] = paletteData[palIndex].B;
                                ptr[ptrIndex + 1] = paletteData[palIndex].G;
                                ptr[ptrIndex + 2] = paletteData[palIndex].R;
                                ptr[ptrIndex + 3] = palIndex % 16 == 0 ? (byte)0 : paletteData[palIndex].A;
                            }
                        }
                    }
                }
            }

            img.UnlockBits(data);

            imgTileBase = img;
        }

        private Image createImageFromPaletteData(byte[] source, int count)
        {
            Image img = new Bitmap(128, (count / 16) * 8);
            Graphics g = Graphics.FromImage(img);

            paletteData = new Color[count];

            for (int y = 0; y < count / 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    paletteData[(y * 16 + x)] = Get32BitColor((Int16)(source[(y * 16 + x) * 2] | ((Int16)source[(y * 16 + x) * 2 + 1] << 0x08)));
                    g.FillRectangle(new SolidBrush(paletteData[(y * 16 + x)]), new Rectangle(x * 8, y * 8, 8, 8));
                }
            }

            return img;
        }

        private Color Get32BitColor(Int16 color)
	    {
            return Color.FromArgb(255, (color & 0x001F) << 0x03, (color & 0x03E0) >> 0x02, (color & 0x7C00) >> 0x07);
	    }

        private byte[] unpackMapHeightData(byte[] source)
        {
            MAP_WIDTH = 1;
            MAP_DEPTH = 1;

            bool complete = false;
            byte[] ret = new byte[mapHeightSize];
            int xIn = 0;
            int xOut;
            int x0;

            while (!complete)
            {
                xOut = source[xIn] + (source[xIn + 1] * 0x100);
                x0 = source[xIn + 2] + (source[xIn + 3] * 0x100);
                xIn += 4;

                if (xOut == 0 && x0 == 0) { complete = true; break; }

                while (x0 > 0)
                {
                    ret[xOut] = source[xIn];
                    ret[xOut + 1] = source[xIn + 1];
                    xOut += 2;
                    xIn += 2;
                    x0 -= 2;

                    if ((xOut % 0x20) / 2 > MAP_WIDTH) MAP_WIDTH = (xOut % 0x20) / 2;
                    if (xOut / 0x20 + 1 > MAP_DEPTH) MAP_DEPTH = xOut / 0x20 + 1;
                }
            }

            return ret;
        }

        private Image createImageFromMapHeights(int alpha)
        {
            Image img = new Bitmap(1024, 1024);

            int longestY = 0;

            var g = Graphics.FromImage(img);

            Color[] colorsNormal = new Color[] { Color.FromArgb(255, 28, 220, 28), Color.FromArgb(255, 64, 128, 64) };              //Green
            Color[] colorsImpassable = new Color[] { Color.FromArgb(255, 220, 28, 28), Color.FromArgb(255, 128, 12, 12) };          //Red
            Color[] colorsWater = new Color[] { Color.FromArgb(255, 28, 28, 220), Color.FromArgb(255, 12, 12, 128) };               //Blue
            Color[] colorsUnselectable = new Color[] { Color.FromArgb(255, 220, 220, 220), Color.FromArgb(255, 196, 196, 196) };    //Grey

            for (int i = 0; i < MAP_WIDTH; i++)
            {
                for (int j = MAP_DEPTH - 1; j >= 0; j--)
                {
                    int no = (MAP_DEPTH - j - 1) * 16 + i;
                    int x = (j * PILE_WIDTH) + (i * PILE_WIDTH) + PILE_WIDTH;
                    int y = (i * PILE_HEIGHT) - (j * PILE_HEIGHT) + 352;

                    if (y > longestY) longestY = y;

                    if (mapHeightData[no * 2] != 0)
                    {
                        switch (mapHeightData[no * 2 + 1])
                        {
                            case 0:
                                drawPile(g, new Point(x, y), 8 * mapHeightData[no * 2], colorsNormal[(no + j) % 2]);
                                break;
                            case 1:
                                drawPile(g, new Point(x, y), 8 * mapHeightData[no * 2], colorsImpassable[(no + j) % 2]);
                                break;
                            case 2:
                                drawPile(g, new Point(x, y), 8 * mapHeightData[no * 2], colorsWater[(no + j) % 2]);
                                break;
                            default:
                                drawPile(g, new Point(x, y), 8 * mapHeightData[no * 2], colorsUnselectable[(no + j) % 2]);
                                break;
                        }         
                    }
                    else drawPile(g, new Point(x, y), 8 * mapHeightData[no * 2], colorsUnselectable[(no + j) % 2]);
                }
            }

            //Image ret = new Bitmap((int)((double)(MAP_WIDTH + MAP_DEPTH) / 2 * PILE_WIDTH * 2) + 2, longestY + PILE_HEIGHT + 2);
            Image ret = new Bitmap(1024, 1024);
            Graphics g2 = Graphics.FromImage(ret);
            
            ColorMatrix cm = new ColorMatrix();
            cm.Matrix00 = cm.Matrix11 = cm.Matrix22 = cm.Matrix44 = 1;
            cm.Matrix33 = (float)alpha / 255;

            ImageAttributes ia = new ImageAttributes();
            ia.SetColorMatrix(cm);

            g2.DrawImage(img, new Rectangle(16 * (16 - MAP_DEPTH), 32 - (8 * (16 - MAP_DEPTH)), img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
            return ret;
        }

        private void drawPile(Graphics g, Point pos, int height, Color brushColor)
        {

            var pen = new Pen(Brushes.Black, 1.0f);
            var pointsArray1 = new Point[] { new Point(pos.X - PILE_WIDTH, pos.Y), new Point(pos.X, pos.Y - PILE_HEIGHT), new Point(pos.X + PILE_WIDTH, pos.Y), new Point(pos.X, pos.Y + PILE_HEIGHT) };
            var pointsArray2 = new Point[4];
            for (int i = 0; i < 4; i++) pointsArray2[i] = new Point(pointsArray1[i].X, pointsArray1[i].Y - height);

            var brush = new SolidBrush(brushColor);
            for (int i = 0; i < 4; i++)
            {
                if (i == 1)
                {
                    brush = new SolidBrush(ControlPaint.Dark(brushColor, 0.10f));
                }
                else if (i == 3)
                {
                    brush = new SolidBrush(ControlPaint.Dark(brushColor, 0.35f));
                }
                g.FillPolygon(brush, new Point[] { pointsArray1[i], pointsArray2[i], pointsArray2[(i + 1) % 4], pointsArray1[(i + 1) % 4] });
            }

            //Draw lines seperately to polygon so they are not hidden.
            for (int i = 0; i < 4; i++)
            {
                g.DrawLine(pen, pointsArray1[i], pointsArray2[i]);
            }

            //Redraw the SW base line.
            g.DrawLine(pen, pointsArray1[0], pointsArray1[3]);
            //Redraw the SE base line.
            g.DrawLine(pen, pointsArray1[2], pointsArray1[3]);

            brush = new SolidBrush(brushColor);
            g.FillPolygon(brush, pointsArray2);
            g.DrawPolygon(pen, pointsArray2);
        }

        private void picTiles_Paint(object sender, PaintEventArgs e)
        {
            if (hoveredTile > -1)
            {
                e.Graphics.DrawRectangle(Pens.Gray, new Rectangle(hoveredTile * 24, 0, 47, 23));
                e.Graphics.DrawRectangle(Pens.LightGray, new Rectangle(hoveredTile * 24 + 1, 1, 45, 21));
            }
            if (tileCount > 0)
            {
                e.Graphics.DrawRectangle(SystemPens.Highlight, new Rectangle(selectedTile * 24, 0, 47, 23));
                e.Graphics.DrawRectangle(Pens.LightGray, new Rectangle(selectedTile * 24 + 1, 1, 45, 21));
            }
        }

        private void picTiles_MouseUp(object sender, MouseEventArgs e)
        {
            selectedTile = (e.X - 12) / 24;
            if (selectedTile < 0) selectedTile = 0;
            if (selectedTile > (tileCount - 1) * 2) selectedTile = (tileCount - 1) * 2;
            this.Text = selectedTile.ToString("X4");
            picTiles.Invalidate();
        }

        private void picTiles_MouseMove(object sender, MouseEventArgs e)
        {
            hoveredTile = (e.X - 12) / 24;
            if (hoveredTile < 0) hoveredTile = 0;
            if (hoveredTile > (tileCount - 1) * 2) hoveredTile = (tileCount - 1) * 2;
            picTiles.Invalidate();
        }

        private void picTiles_MouseLeave(object sender, EventArgs e)
        {
            hoveredTile = -1;
            picTiles.Invalidate();
        }

        private void chkShowLayer_CheckedChanged(object sender, EventArgs e)
        {
            DrawMapFromLoadedImages();
        }

        private void tmrAnimate_Tick(object sender, EventArgs e)
        {
            animationFrame++;
            if (animationFrame >= mapAnimationTiles.Length) animationFrame = 0;
            LoadMap(0, animationFrame);
            DrawMapFromLoadedImages();
        }

        private Image LoadMap(int map, int frame)
        {
            Image ret = new Bitmap(1, 1);
            if (frame < imgMap0.Length)
            {
                if (imgMap0[frame] == null)
                {
                    updateTileSetWithNewAnimationFrames(frame);
                    imgTiles = createImageFromTileFormatData(mapArrangeData);
                    Image[] imgMap = createImageFromArrangeData(mapArrangeData, out arrangeDataForCad);

                    imgMap0[frame] = imgMap[0];
                    imgMap1[frame] = imgMap[1];
                }

                switch (map)
                {
                    case 0:
                        ret = imgMap0[frame];
                        break;
                    case 1:
                        ret = imgMap1[frame];
                        break;
                }
            }

            return ret;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlgSave = new SaveFileDialog();
            dlgSave.Filter = "Bitmap Image (*.BMP)|*.bmp|Portable Network Graphic (*.PNG)|*.png|Animated GIF (*.GIF)|*.gif";
            dlgSave.FilterIndex = 2;

            if (dlgSave.ShowDialog(this) == DialogResult.OK)
            {
                switch (dlgSave.FilterIndex)
                {
                    case 1:
                        using (FileStream fs = new FileStream(dlgSave.FileName, FileMode.Create, FileAccess.Write))
                        {
                            Image img = new Bitmap(512, 512);
                            Graphics g = Graphics.FromImage(img);
                            g.FillRectangle(new SolidBrush(paletteData[0]), 0, 0, 512, 512);
                            g.DrawImage(picMain.Image, 0, 0);
                            img.Save(fs, ImageFormat.Bmp);
                        }
                        break;
                    case 2:
                        using (FileStream fs = new FileStream(dlgSave.FileName, FileMode.Create, FileAccess.Write))
                        {
                            picMain.Image.Save(fs, ImageFormat.Png);
                        }
                        break;
                    case 3:
                        Color transparent = Color.FromArgb(255, 255, 0, 255);
                        AnimatedGifGlobalPalette.AnimatedGifGlobalPalette newGIF = new AnimatedGifGlobalPalette.AnimatedGifGlobalPalette(512, 512, paletteData, transparent, true);

                        for (int i = 0; i < imgMap0.Length; i++)
                        {
                            Image img = new Bitmap(512, 512);
                            Graphics g = Graphics.FromImage(img);

                            g.FillRectangle(new SolidBrush(transparent), new Rectangle(0, 0, 512, 512));
                            if (chkShowBottom.Checked) g.DrawImage(LoadMap(1, i), 0, 0);
                            if (chkShowTop.Checked) g.DrawImage(LoadMap(0, i), 0, 0);

                            newGIF.AddFrame(img, 14);
                        }

                        newGIF.WriteToFile(dlgSave.FileName);
                        break;
                }
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            tmrAnimate.Enabled = !tmrAnimate.Enabled;
            if (tmrAnimate.Enabled)
            {
                btnPlay.Image = FFTA_MapEditor.Properties.Resources.pause;
            }
            else
            {
                btnPlay.Image = FFTA_MapEditor.Properties.Resources.play;
            }
        }

        static void writeWordGba(byte[] data, int addr, int word)
        {
            data[addr + 1] = (byte)(word >> 8);
            data[addr] = (byte)(word & 0xFF);
    }
        private void button1_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[64 * 32 * 4];
            string mapName = cboMapIndex.Text;
            mapName = mapName.Replace(" ", "_");
            mapName = mapName.Replace("/", "");
            string fn = "map_" + mapName + ".bin";
            imgTiles.Save("tiles_" + mapName + ".png");
            for (int i = 0; i < arrangeDataForCad.Length; i++)
                writeWordGba(data, i * 2, arrangeDataForCad[i]);
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

        private void picHmapEdit_Paint(object sender, PaintEventArgs e)
        {
            int SIZE = 16;
            if (mapHeightData == null)
                return;
            var g = e.Graphics;
            var f = new Font("System", 12.0f, FontStyle.Regular, GraphicsUnit.Pixel);
            for (int i = 0; i < 32; i+=2)
            {
                for (int j = 0; j < 32; j+=2)
                {
                    int val = mapHeightData[i * 16 + j];
                    int rgbVal = (int)(64 + (192/24.0*val));
                    int typeVal = mapHeightData[i * 16 + j + 1];
                    Color color;
                    if (typeVal == 0)
                        color = Color.FromArgb(0, rgbVal, 0);
                    else if (typeVal == 1)
                        color = Color.FromArgb(rgbVal, 0, 0);
                    else if (typeVal == 2)
                        color = Color.FromArgb(0, 0, rgbVal);
                    else
                        color = Color.FromArgb(rgbVal, rgbVal, rgbVal);
                    g.FillRectangle(new SolidBrush(color), new Rectangle(i*SIZE, j*SIZE, 32,32));
                    g.DrawRectangle(new Pen(Brushes.Black, 1.0f), new Rectangle(i*SIZE, j*SIZE, 32,32));
                    g.DrawString(val.ToString(), f, Brushes.Black, new Point (SIZE*i, SIZE*j)); 
                }
            }

            //
            imgMapHeight = createImageFromMapHeights(200);
            DrawMapFromLoadedImages();
        }

        private void picMain_Paint(object sender, PaintEventArgs e)
        {
            /*if (mapHeightData != null)
              imgMapHeight = createImageFromMapHeights(200);*/
        }

        private void picHmapEdit_MouseClick(object sender, MouseEventArgs e)
        {
            int SIZE = 32;
            if (mapHeightData == null)
                return;
            int x = e.X / SIZE;
            int y = e.Y / SIZE;
            if (x < 0 || x >= 16 || y < 0 || y >= 16)
                return;

            int shift = Control.ModifierKeys.HasFlag(Keys.Shift) == true ? 1 : 0;
            int index = (x * 16 + y) * 2 + shift;
            if (e.Button == MouseButtons.Left)
            {
                int maxVal = shift == 0 ? 25 : 3;
                if (mapHeightData[index] < maxVal)
                  mapHeightData[index]++;
            }
            else
            {
                if (mapHeightData[index] > 0)
                  mapHeightData[index]--;
            }
            picHmapEdit.Invalidate();
        }
    }
}
