#function for fill arrange map 0 with values from start to start+0x40 for make screenshots with all 
def fillArrangeMap0(filename, startVal):
  #
  f = open(filename, "rb")
  d = f.read()
  f.close()
  #
  di = map(ord, d)
  di[4:7] = [0xB4, 0x10, 0xFF]
  for v in xrange(256):
    di[7 + v*2] = 0xFF
    di[8 + v*2] = 00
  for row in xrange(4):
    for v in xrange(16):
      fullVal = startVal + v + row * 0x10
      b1 = fullVal / 256
      b2 = fullVal % 256
      di[0x0A + v*2 + 0x40*row] = b1
      di[0x09 + v*2 + 0x40*row] = b2
  d ="".join(map(chr, di))
  #
  f = open(filename, "wb")
  f.write(d)
  f.close()
