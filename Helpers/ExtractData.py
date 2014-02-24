from configAddrs import a,h
from subprocess import Popen

DECOMPRESS_TEMPLATE = "../Compressors/GBAmdc.exe -e ../BIN/rom/ffta.gba %s %s"

for ar in a:
  print "Decode arrange: ", hex(ar)[2:].upper(), ":     "
  p = Popen(DECOMPRESS_TEMPLATE % ("../BIN/gameData/a_0%s.bin"%(hex(ar)[2:].upper()), hex(ar+4-0x8000000)[2:]))
  p.wait()
  
for hr in h:
  print "Decode heights: ", hex(hr)[2:].upper(), ":     "
  p = Popen(DECOMPRESS_TEMPLATE % ("../BIN/gameData/h_0%s.bin"%(hex(hr)[2:].upper()), hex(hr+4-0x8000000)[2:]))
  p.wait()