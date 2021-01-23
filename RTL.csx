using System.Text;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using UndertaleModLib;
using UndertaleModLib.Models;
using System.Linq;

//int spacecharwidth     Width in pixels of the space char.
//int fontsheetWidth     THIS NUMBER MUST ONLY BE A MULTIPLE OF 1024. change if you need more characters (you shouldnt)
//int fontsheetHeight    must be same as width
//bool firstrun          True if calling RTLimport for the first time in the script

//==applies to RTL only==
//int rtlstart           Right-to-left text scroll start X position
//int averagecharwidth   Includes transparent pixels. Affects right-to-left character spacing
//int cpl                Number of characters per line

RTLimport("maintext", 5, 256, 8, 35, 1024, 1024, true);
removeUnusedEmbeds();
ScriptMessage("Done");

public void RTLimport (String fnt, int spacecharwidth, int rtlstart, int averagecharwidth, int cpl, int fontsheetWidth, int fontsheetHeight, bool firstrun){
fontsheetHeight=fontsheetWidth;

int offset=0;
UndertaleFont font = Data.Fonts.ByName("fnt_" + fnt);

//Get folder of data.win
string winFolder = GetFolder(FilePath);
string GetFolder(string path)
{
	return Path.GetDirectoryName(path) + Path.DirectorySeparatorChar;
}

//Get list of .png files in glyphs folder
string[] indexFilenames = Directory.GetFiles(@winFolder + "glyph\\" + fnt, "*.png", SearchOption.TopDirectoryOnly);
string[] indexChars = new string[indexFilenames.Length];
for (int i = 0; i < indexFilenames.Length; i++) 
{
  if ((indexFilenames[i][indexFilenames[i].Length-14]+"")=="l"){
      indexChars[i]=(indexFilenames[i][indexFilenames[i].Length-5]+"");
  }
  else if ((indexFilenames[i][indexFilenames[i].Length-14]+"")=="u"){
      indexChars[i]=(indexFilenames[i][indexFilenames[i].Length-5]+"");
  }
}

//Generate fontsheet of fnt images
Bitmap firstImage = new Bitmap(indexFilenames[0]); // your source images - assuming they're the same size
Bitmap secondImage = new Bitmap(indexFilenames[0]);

 if (firstImage == null)
        {
            throw new ArgumentNullException("firstImage");
        }

        if (secondImage == null)
        {
            throw new ArgumentNullException("secondImage");
        }
		
int outputImageWidth = fontsheetWidth;
int outputImageHeight = fontsheetHeight;
        int y=0;

        Bitmap outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        using (Graphics graphics = Graphics.FromImage(outputImage))
        {
            
            for (int i=0; i<indexFilenames.Length; i++){
                if (((firstImage.Width*i)) == outputImageWidth+(y*outputImageWidth)){
                  y++;
                }
            graphics.DrawImage(new Bitmap(indexFilenames[i]), new Rectangle(new Point(((firstImage.Width*i)-(y*outputImageWidth)), y*secondImage.Height), secondImage.Size),
                new Rectangle(new Point(), secondImage.Size), GraphicsUnit.Pixel);
            }
        }

//Initialize and index chars
char[] chars = new char[indexFilenames.Length];
for (int i=0; i<chars.Length;i++){
    if (((indexFilenames[i].Substring(winFolder.Length)).Contains("lowercase"))){
        chars[i]=((((((indexFilenames[i].Substring(winFolder.Length)).Substring(((indexFilenames[i].Substring((winFolder.Length)).IndexOf("lowercase")+9))))[0])+"").ToLower())[0]);

    }
    else if (((indexFilenames[i].Substring(winFolder.Length)).Contains("uppercase"))){
        chars[i]=((((((indexFilenames[i].Substring(winFolder.Length)).Substring(((indexFilenames[i].Substring((winFolder.Length)).IndexOf("uppercase")+9))))[0])+"").ToUpper())[0]);

    }
    else if (((indexFilenames[i].Substring(winFolder.Length)).Contains("SPECIAL"))){
        if (((indexFilenames[i].Substring(winFolder.Length)).Contains("asterisk"))){
            chars[i]="*"[0];
        }
        else if (((indexFilenames[i].Substring(winFolder.Length)).Contains("backward"))){
            chars[i]="\\"[0];
        }
        else if (((indexFilenames[i].Substring(winFolder.Length)).Contains("colon"))){
            chars[i]=":"[0];
        }
        else if (((indexFilenames[i].Substring(winFolder.Length)).Contains("forward"))){
            chars[i]="/"[0];
        }
        else if (((indexFilenames[i].Substring(winFolder.Length)).Contains("greater"))){
            chars[i]=">"[0];
        }
        else if (((indexFilenames[i].Substring(winFolder.Length)).Contains("less"))){
            chars[i]="<"[0];
        }
        else if (((indexFilenames[i].Substring(winFolder.Length)).Contains("question"))){
            chars[i]="?"[0];
        }
        else if (((indexFilenames[i].Substring(winFolder.Length)).Contains("quotation"))){
            chars[i]="\""[0];
        }
        else if (((indexFilenames[i].Substring(winFolder.Length)).Contains("vertical"))){
            chars[i]="|"[0];
        }
		else if (((indexFilenames[i].Substring(winFolder.Length)).Contains("space"))){
            chars[i]=" "[0];
        }
    }
    else{
            chars[i]=(((indexFilenames[i].Substring(winFolder.Length)).Substring(("glyph\\"+fnt).Length+1))[0]);
        }
}


//Add/replace embedded texture of the generated font sheet

using (var stream = new MemoryStream())
    {
outputImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
UndertaleEmbeddedTexture texture = new UndertaleEmbeddedTexture();
texture.TextureData.TextureBlob = stream.ToArray();

(Data.Fonts.ByName("fnt_" + fnt).Texture).SourceX = 0;
(Data.Fonts.ByName("fnt_" + fnt).Texture).SourceY = 0;
(Data.Fonts.ByName("fnt_" + fnt).Texture).SourceWidth = (ushort)outputImageWidth;
(Data.Fonts.ByName("fnt_" + fnt).Texture).SourceHeight = (ushort)outputImageHeight;
(Data.Fonts.ByName("fnt_" + fnt).Texture).TargetX = 0;
(Data.Fonts.ByName("fnt_" + fnt).Texture).TargetY = 0;
(Data.Fonts.ByName("fnt_" + fnt).Texture).TargetWidth = (ushort)outputImageWidth;
(Data.Fonts.ByName("fnt_" + fnt).Texture).TargetHeight = (ushort)outputImageHeight;
(Data.Fonts.ByName("fnt_" + fnt).Texture).BoundingWidth = (ushort)outputImageWidth;
(Data.Fonts.ByName("fnt_" + fnt).Texture).BoundingHeight = (ushort)outputImageHeight;
(Data.Fonts.ByName("fnt_" + fnt).Texture).TexturePage = texture;
try
{
    Data.EmbeddedTextures.Add(texture);
}
catch (Exception ex)
{
    ScriptMessage("Failed to export file: " + ex.Message);
}
}

//Generate glyph data
Bitmap img = new Bitmap(indexFilenames[0]);
string[] charData = new string [chars.Length];
int charX=0;
int charY=0;
int shift=0;
for (int i=0;i<charData.Length;i++){
img = new Bitmap(indexFilenames[i]);
shift=((1+(secondImage.Width-(blankpixels(img)))));
if (((int)chars[i])==32){
	shift=spacecharwidth;
	charData[i]=(int)chars[i]+";"+(charX)+";"+charY+";"+(secondImage.Width)+";"+secondImage.Height+";"+ shift + ";" + offset;
}
else{
charData[i]=(int)chars[i]+";"+(charX+blankpixels(img))+";"+charY+";"+(secondImage.Width-blankpixels(img))+";"+secondImage.Height+";"+ shift + ";" + offset;
}
charX=charX+secondImage.Width;
shift=0;
if (charX>=outputImageWidth){charX=0;charY=charY+secondImage.Height;}
}

//Insert glyph data
font.Glyphs.Clear();
	string line;
	for(int i=0; i<charData.Length;i++)
	{
		string[] s = charData[i].Split(';');
		font.Glyphs.Add(new UndertaleFont.Glyph() {
			Character = UInt16.Parse(s[0]),
			SourceX = UInt16.Parse(s[1]),
			SourceY = UInt16.Parse(s[2]),
			SourceWidth = UInt16.Parse(s[3]),
			SourceHeight = UInt16.Parse(s[4]),
			Shift = Int16.Parse(s[5]),
			Offset = Int16.Parse(s[6]),
		});
	}
	
sortglyphs(font);
				
//Generate right-to-left char glyph data
int rtlcharval=0;
int e = 0;
int horz=0;
int rtlx=0;
int rtloff=0;
int rtlcap=rtlstart;
int rtli=0;
string[] rtlChar= new string[indexFilenames.Length*(cpl-1)];
for (int i=0; i<rtlChar.Length;i++){
    rtlx=10000+(e*100);
    rtlcharval=(rtlx+horz);
    horz++;
    int ogcharx=(Int32.Parse((charData[e].Split(';'))[1])+(Int32.Parse((charData[e].Split(';'))[3]))-secondImage.Width);
    int ogchary=Int32.Parse((charData[e].Split(';'))[2]);
    rtloff=(rtlcap-(rtli*(averagecharwidth*2)));
    rtlChar[i]=(rtlcharval + ";" + ogcharx + ";" + ogchary + ";" + secondImage.Width + ";" + secondImage.Height + ";"
    + (averagecharwidth+1) + ";" + rtloff);
    rtli++;
    if (horz>=(cpl-1)){horz=0;e++;rtli=0;}
}

//Insert glyph data again
	for(int i=0; i<rtlChar.Length;i++)
	{
		string[] s = rtlChar[i].Split(';');
		font.Glyphs.Add(new UndertaleFont.Glyph() {
			Character = UInt16.Parse(s[0]),
			SourceX = UInt16.Parse(s[1]),
			SourceY = UInt16.Parse(s[2]),
			SourceWidth = UInt16.Parse(s[3]),
			SourceHeight = UInt16.Parse(s[4]),
			Shift = Int16.Parse(s[5]),
			Offset = Int16.Parse(s[6]),
		});
	}


//Right-to-left string conversion
if (firstrun){
foreach (var str in Data.Strings){
string result="";
string convertcall="RTL";
//ScriptMessage(str.Content);
string curstring = str.Content;//"RTL(5)Long ago^1, two races&ruled over Earth^1:&HUMANS and MONSTERS^6. \\E1 ^1 %";
string bldata1="";
string bldata2="";
string[] tblacklist=new string[]{

"^1",
"^2",
"^3",
"^4",
"^5",
"^6",
"^7",
"^8",
"^9",
"%",
"\\E0",
"\\E1",
"\\E2",
"\\E3",
"\\E4",
"\\E5",
"\\E6",
"\\E7",
"\\E8",
"\\E9",
"\\[1]",
"\\[2]",
"\\[C]",
"\\[G]",
"\\M0",
"\\M1",
"\\M2",
"\\M3",
"\\M4",
"\\M5",
"\\M6",
"\\M7",
"\\M8",
"\\M9",
"\\Y",
"\\B",
"\\G",
"\\B",
"\\X",
"\\L",
"\\O",
"\\P",
"/",
//Will not convert these ones

""};

string[] blacklist=new string[tblacklist.Length-1];for(int i=0;i<blacklist.Length;i++){blacklist[i]=tblacklist[i];}
string tstring="";
int readstart=0;
if (curstring.Length>convertcall.Length){
if ((curstring.Substring(0, convertcall.Length+1)).Equals(convertcall+ "(")){
    readstart=readstart+Int32.Parse(curstring.Substring((convertcall.Length+1),((curstring.Substring((convertcall.Length+1))).IndexOf(')'))));
    curstring=convertcall+(curstring.Substring(("" + readstart).Length+2+convertcall.Length));
}}

if (curstring.Length>convertcall.Length){
if ((curstring.Substring(0, convertcall.Length)).Equals(convertcall)){
tstring=curstring.Substring(convertcall.Length);

tstring.Replace("^3", "");
int ix=0;
foreach (string s in blacklist){
    while(tstring.Contains(s)){
        bldata1=ix+";"+bldata1;
        bldata2=tstring.IndexOf(s)+";"+bldata2;
        tstring=tstring.Remove(tstring.IndexOf(s),s.Length);
    }

    ix++;
}

bldata1=bldata1+"-1";
bldata2=bldata2+"-1";

string rtloutput="";
int xline=0;
foreach (char lett in tstring.ToCharArray()){
    if (lett!='&' && lett!='#'){
    rtloutput=rtloutput+(((rtlChar[((cpl-1)*(findchar(lett, charData, tstring)))+xline+readstart]).Split(';'))[0])+";";
    xline++;
}
    else{
    rtloutput=rtloutput+"38;";
    xline=0;
    }
    if ((xline+readstart)>cpl){
        ScriptMessage("Conflict: String '" + curstring + "' has a line more than " + cpl + " characters. Revise then retry");
        xline=Int32.MaxValue;//bruh
    }
}
rtloutput=rtloutput.Substring(0,rtloutput.Length-1);

foreach (string rtl in rtloutput.Split(';')){
    result=result+((char)(Int32.Parse(rtl)));

}

ix=0;
foreach(string s in bldata1.Split(';')){if (s!="-1"){
    result=result.Insert(Int32.Parse(bldata2.Split(';')[ix]), blacklist[Int32.Parse(s)]);
    ix++;
}}

//ScriptMessage(result);
str.Content=result;
}}}}}
			
//Count horizontal number of transparent pixels on the left side of the image (for example in maintext 'A' has 2, 'B' has 2, 'C' has 2, and 'W' has 1 because 'W' is wider)
public int blankpixels(Bitmap img){
            string cell="";
            int x=0;
            int y=0;
            int hit=-1;
            for (int i=0; i<((img.Width)*(img.Height));i++){
                y++;
                if (y==img.Height){y=0;x++;}
                if(hit==-1){if (x<img.Width){if (!((""+(img.GetPixel(x,y))).Equals("Color [A=0, R=0, G=0, B=0]"))){if (!((""+(img.GetPixel(x,y))).Equals("Color [A=0, R=255, G=255, B=255]"))){hit=x;}}}}
            }
            return hit;
        }
		
//Sort glyphs
public void sortglyphs(UndertaleFont font){

var copy = font.Glyphs.ToList();
            copy.Sort((x, y) => x.Character.CompareTo(y.Character));
            font.Glyphs.Clear();
            foreach (var glyph in copy)
                font.Glyphs.Add(glyph);
}

//Remove all unused embedded textures
public void removeUnusedEmbeds(){
UndertaleEmbeddedTexture texture;
int q=0;
int recur=0;
string unused="";
foreach(var graphic in Data.EmbeddedTextures){

texture=graphic;


string output="";
int i=0;
foreach(var spr in Data.TexturePageItems){
if (spr.TexturePage.TextureData.TextureBlob==texture.TextureData.TextureBlob){
output = output + i + ";";
}
i++;
}
if (output==""){unused = unused + q + ";";}
q++;
}
if (unused!="")
	unused = unused.Substring(0, unused.Length-1);
//ScriptMessage("Unused: " + unused);

if (unused!=""){
if (unused.Contains(";")){
	foreach(string str in unused.Split(';')){
		Data.EmbeddedTextures.Remove(Data.EmbeddedTextures[Int32.Parse(str)-recur]);
		recur++;
	}}
else{
	Data.EmbeddedTextures.Remove(Data.EmbeddedTextures[Int32.Parse(unused)]);
}
}
}

//Find char. Return error if unknown
public int findchar(char x, string[] indexFilenames, string err){
            for(int i=0; i<indexFilenames.Length;i++){
                if (((indexFilenames[i].Split(';'))[0])==("" +(int)x)){
                    return i;
                }
                
            }
            ScriptMessage("Stopped. Char not found '" + x + "' in the following string:\n\n\n " + err);
            return -1;
        }
