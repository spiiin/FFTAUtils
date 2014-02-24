import Image

# import os
# import glob

# BOX = (0, 0, 16*16, 8*4)
# def imageCrop(im):
  # return im.crop(BOX)
 
#flist = glob.glob(r"PATH_TO_IMAGES\???.png") 
# for fn in flist:
  # imCropped = imageCrop(Image.open(fn))
  # imCropped.save(os.path.join(os.path.split(fn)[0],"crop_"+os.path.split(fn)[1]))
  
def convertPicToLine(im, height):
	count = im.size[1] / height
	width = im.size[0]
	newIm = Image.new("RGB",(width*count, height))
	for x in xrange(count):
	  cr1 = im.crop((0,height*x,width,height*(x+1)))
	  newIm.paste(cr1,(width*x,0,width*(x+1),height))
	return newIm
  
#flist = glob.glob(r"PATH_TO_IMAGES\crop_???.png")
# for fn in flist:
  # imCropped = convertPicToLine(Image.open(fn),8)
  # imCropped.save(os.path.join(os.path.split(fn)[0],"line_"+os.path.split(fn)[1]))
  
def clayHorizontal(imArray):
  totalWidth = 0
  for im in imArray:
    totalWidth += im.size[0]
  imNew = Image.new("RGB",(totalWidth,imArray[0].size[1]))
  curX = 0
  for im in imArray:
    imNew.paste(im, (curX,0,curX+im.size[0],im.size[1]))
    curX += im.size[0]
    
# flist = glob.glob(r"PATH_TO_IMAGES\line_crop_???.png")
# imNew = clayHorizontal(map(Image.open, flist))
# imNew.save("PATH_TO_IMAGES/outStrip.png")

#clean ffta transparent colors
def cleanTransparentColors(img):
  img = img.convert("RGBA")
  datas = img.getdata()
  newData = []
  for item in datas:
      #clear ffta transparent colors
      if item[0] == 8 and item[1] == 72 and item[2] == 248:
          newData.append((0, 0, 0, 0))
      elif item[0] == 248 and item[1] == 0 and item[2] == 248:
          newData.append((0, 0, 0, 0))
      elif item[0] == 248 and item[1] == 128 and item[2] == 128:
          newData.append((0, 0, 0, 0))
      elif item[0] == 80 and item[1] == 216 and item[2] == 248:
          newData.append((0, 0, 0, 0))
      elif item[0] == 64 and item[1] == 248 and item[2] == 0:
          newData.append((0, 0, 0, 0))
      elif item[0] == 51 and item[1] == 102 and item[2] == 255:
          newData.append((0, 0, 0, 0))
      #elif item[0] == 51 and item[1] == 255 and item[2] == 0:
      #    newData.append((0, 0, 0, 0))
      #elif item[0] == 102 and item[1] == 255 and item[2] == 255:
      #    newData.append((0, 0, 0, 0))
      #elif item[0] == 102 and item[1] == 204 and item[2] == 255:
      #    newData.append((0, 0, 0, 0))
      else:
          newData.append(item)
  img.putdata(newData)
  return img

