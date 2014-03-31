import glob
import Image

template = """using CadEditor;
using System;
using System.Drawing;

public class Data 
{ 
  public GameType getGameType()        { return GameType.Generic; }
  public OffsetRec getScreensOffset()  { return new OffsetRec(4096, 1 , 0x20*0x40);   }
  public OffsetRec getScreensOffset2() { return new OffsetRec(0   , 1 , 0x20*0x40);   }
  public int getScreenWidth()          { return 0x20; }
  public int getScreenHeight()         { return 0x40; }
  public int getWordLen()              { return 2;}
  public int getLayersCount()          { return 2;}
  public bool isLittleEndian()         { return true; }
  public int    getPictureBlocksWidth(){ return 64; }
  public int getBigBlocksCount()       { return %d; }
  public string getBlocksFilename()    { return "settings_gba_final_fantasy_tactics_advance/tiles_%s.png"; }
  
  public bool isBigBlockEditorEnabled() { return false; }
  public bool isBlockEditorEnabled()    { return false; }
  public bool isLayoutEditorEnabled()   { return false; }
  public bool isEnemyEditorEnabled()    { return false; }
  public bool isVideoEditorEnabled()    { return false; }
}"""

templFname = 'Settings_GbaFfta_%s.cs'
listTiles  = glob.glob("tiles_*.png")
listBin    = glob.glob("map_*.bin")


def makeSettingsFile(fileNameTempl):
  for tileName in listTiles:
    im = Image.open(tileName)
    tileCount = im.size[0]/16
    fname = tileName[tileName.index("tiles_")+6:tileName.index(".png")]
    s = template%(tileCount, fname)
    f = open(fileNameTempl%fname, "wt")
    f.write(s)
    f.close()
    
def resizeImagesInplace():
  for tileName in listTiles:
    im = Image.open(tileName)
    im64 = im.resize((im.size[0]*8, im.size[1]*8))
    im64.save(tileName)
    
def splitBinFiles():
  for binFile in listBin:
    f = open(binFile, "rb")
    d = f.read()
    f.close()
    f = open(binFile[:len(binFile)-4] + "_layerA.bin", "wb")
    f.write(d[4096:])
    f.close()
    f = open(binFile[:len(binFile)-4] + "_layerB.bin", "wb")
    f.write(d[:4096])
    f.close()
    
makeSettingsFile(templFname)
resizeImagesInplace()
splitBinFiles()