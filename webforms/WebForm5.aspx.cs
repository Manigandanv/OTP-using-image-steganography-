using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Media;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Collections;
using System.IO;
using System.ComponentModel;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
namespace WebApplication4
{
    public partial class WebForm5 : System.Web.UI.Page
    {
        String SourceFileName = @"E:\studies\sem8 projects\zebra-clipart-png-8.png";
        String destinationFileName = @"E:\studies\sem8 projects\otpimage.bmp";
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /*protected void Button1_Click(object sender, EventArgs e)
        {
            Gmailcheck();
        }
        private Boolean InsertUpdateData(SqlCommand cmd)
        {
            String strConnString = @"Data Source=(localdb)\ProjectsV12;Initial Catalog=picture;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False";
            SqlConnection con = new SqlConnection(strConnString);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                return false;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }*/
       /* protected void btnUpload_Click(object sender, EventArgs e)
        {
            string str = uploads.FileName;
            uploads.PostedFile.SaveAs(Server.MapPath("~/Upload/" + str));
            string Image = "~/Upload/" + str.ToString();
            int length = uploads.PostedFile.ContentLength;
            byte[] imgbyte = new byte[length];
            HttpPostedFile img = uploads.PostedFile;
            img.InputStream.Read(imgbyte, 0, length);
            string imagename = name.Text;
            string strQuery = "insert into imdb1(email,img,ipath) values (@email,@img,@ipath)";
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = imagename;
            cmd.Parameters.Add("@img", SqlDbType.Image).Value = imgbyte;
            cmd.Parameters.Add("@ipath", SqlDbType.VarChar).Value = Image;
            InsertUpdateData(cmd);
            Response.Write("inserted");
        }
        public String GetImageData()
        {
            string path = "";
            try
            {
                String strConnString = @"Data Source=(localdb)\ProjectsV12;Initial Catalog=picture;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False";
                string strQuery = "select distinct(ipath) from imdb1 where email='image'";
                SqlConnection con = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand(strQuery);
                cmd.Connection = con;
                con.Open();
                path = cmd.ExecuteScalar().ToString();
                con.Close();
            }
            catch (SqlException e)
            {
                Response.Write(e.Message);
            }
            return path;
        }*/
        /*public int Gmailcheck()
        {
            String toemail = email.Text;
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.From = new MailAddress("manigandanv2012@vit.ac.in");
            mail.To.Add(toemail);
            mail.Subject = "One Time Password";
            mail.Body = "This is your one time password image";
            String strFileName = @"E:\studies\sem8 projects\output.bmp";
            Attachment attach = new Attachment(strFileName);
            mail.Attachments.Add(attach);
            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            smtpServer.Port = 587;
            smtpServer.Credentials = new System.Net.NetworkCredential("manigandanv2012@vit.ac.in", "vm8015966");
            smtpServer.EnableSsl = true;
            ServicePointManager.ServerCertificateValidationCallback =
                            delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                            { return true; };
            smtpServer.Send(mail);
            return 0;
        }
        public void btnHide_Click(object sender, System.EventArgs e)
        {
            Bitmap bmp = new Bitmap(@"E:\studies\sem8 projects\zebra-clipart-png-8.png");
            int maxPaletteSize = (bmp.Palette.Entries.Length * 3);
            if (maxPaletteSize > 256)
            {
                maxPaletteSize = 256;
            }
            bmp.Dispose();
            String msg = "otp";
            byte[] messageBytes = ASCIIEncoding.ASCII.GetBytes(msg);
            BinaryWriter messageWriter = new BinaryWriter(new MemoryStream());
            messageWriter.Write(messageBytes.Length);
            messageWriter.Write(messageBytes);
            messageWriter.Seek(0, SeekOrigin.Begin);
            FileStream key = new FileStream(@"C:\keyfiles\key.txt", FileMode.Open);
            long countUseablePixels = CountUseableUnits(key);
            if (countUseablePixels < (messageWriter.BaseStream.Length * 8))
            {
                Label2.Text = "The image is too small";
            }
            else
            {
                Hide(maxPaletteSize, messageWriter.BaseStream, key);
            }

            key.Close();
            messageWriter.Close();
        }*/
        public void btnExtract_Click(object sender, System.EventArgs e)
        {
            MemoryStream message = new MemoryStream();
            FileStream key = new FileStream(@"C:\keyfiles\key.txt", FileMode.Open);
            Extract(message, key);
            message.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(message);
            //Label3.Text = reader.ReadToEnd();
            String ch = reader.ReadToEnd();
            if (ch == "hell")
            { 
                Response.Redirect("re.html");
            }
            else
            {
                Label1.Text = "invalid image";
            }
            reader.Close();
            key.Close();
        }
        public long CountUseableUnits(Stream keyStream)
        {
            long countUseableUnits = 0;
            long unitIndex = 0;
            byte key;

            Bitmap bmp = new Bitmap(@SourceFileName);
            long countUnits = bmp.Width * bmp.Height;
            bmp.Dispose();

            while (true)
            {
                key = GetKey(keyStream);
                if (unitIndex + key < countUnits)
                {
                    unitIndex += key;
                    countUseableUnits++;
                }
                else
                {
                    break;
                }
            }
            keyStream.Seek(0, SeekOrigin.Begin);
            return countUseableUnits;
        }
        public void Hide(int maxPaletteSize, Stream messageStream, Stream keyStream)
        {
            Bitmap bmp = new Bitmap(@SourceFileName);
            ArrayList newPalette = null;
            Hashtable colorIndexToNewIndices = null;
            StretchPalette(bmp.Palette, maxPaletteSize, ref newPalette, ref colorIndexToNewIndices);
            Bitmap newBmp = CreateBitmap(bmp, newPalette, colorIndexToNewIndices, messageStream, keyStream);

            newBmp.Save(destinationFileName);
            newBmp.Dispose();
            bmp.Dispose();
        }
        public void Extract(Stream messageStream, Stream keyStream)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(uploads.PostedFile.InputStream);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            byte[] pixels = new byte[bmpData.Stride * bmpData.Height];
            Marshal.Copy(bmpData.Scan0, pixels, 0, pixels.Length);
            Color[] palette = bmp.Palette.Entries;
            byte messageByte = 0, messageBitIndex = 0, pixel = 0;
            int messageLength = 0, pixelIndex = 0;
            while ((messageLength == 0) || (messageStream.Length < messageLength))
            {
                pixelIndex += GetKey(keyStream);
                pixel = pixels[pixelIndex];

                if ((palette[pixel].B % 2) == 1)
                {
                    messageByte += (byte)(1 << messageBitIndex);
                }
                if (messageBitIndex == 7)
                {
                    messageStream.WriteByte(messageByte);
                    messageBitIndex = 0;
                    messageByte = 0;

                    if ((messageLength == 0) && (messageStream.Length == 4))
                    {
                        messageStream.Seek(0, SeekOrigin.Begin);
                        messageLength = new BinaryReader(messageStream).ReadInt32();
                        messageStream.SetLength(0);
                    }
                }
                else
                {
                    messageBitIndex++;
                }
            }
            bmp.UnlockBits(bmpData);
            bmp.Dispose();
        }
        private void StretchPalette(ColorPalette oldPalette, int maxPaletteSize, ref ArrayList newPalette, ref Hashtable colorIndexToNewIndices)
        {
            newPalette = new ArrayList(maxPaletteSize);
            colorIndexToNewIndices = new Hashtable(oldPalette.Entries.Length);

            Random random = new Random();
            byte indexInNewPalette;
            Color color, newColor;
            ColorIndexList colorIndexList;

            while (newPalette.Count < maxPaletteSize)
            {
                for (byte n = 0; n < oldPalette.Entries.Length; n++)
                {
                    color = oldPalette.Entries[n];
                    if (colorIndexToNewIndices.ContainsKey(n))
                    {
                        colorIndexList = (ColorIndexList)colorIndexToNewIndices[n];
                    }
                    else
                    {
                        if (color.B % 2 > 0)
                        {
                            color = Color.FromArgb(color.R, color.G, color.B - 1);
                        }
                        indexInNewPalette = (byte)newPalette.Add(color);
                        colorIndexList = new ColorIndexList(random);
                        colorIndexList.Add(indexInNewPalette);
                        colorIndexToNewIndices.Add(n, colorIndexList);
                    }

                    if (newPalette.Count < maxPaletteSize)
                    {
                        newColor = GetSimilarColor(random, newPalette, color);

                        if (newColor.B % 2 == 0)
                        {
                            newColor = Color.FromArgb(newColor.R, newColor.G, newColor.B + 1);
                        }
                        indexInNewPalette = (byte)newPalette.Add(newColor);
                        colorIndexList.Add(indexInNewPalette);
                    }
                    colorIndexToNewIndices[n] = colorIndexList;

                    if (newPalette.Count == maxPaletteSize)
                    {
                        break;
                    }
                }
            }
        }
        private Color GetSimilarColor(Random random, ArrayList excludeColors, Color color)
        {
            Color newColor = color;
            int countLoops = 0, red, green, blue;
            do
            {
                red = GetSimilarColorComponent(random, newColor.R);
                green = GetSimilarColorComponent(random, newColor.G);
                blue = GetSimilarColorComponent(random, newColor.B);
                newColor = Color.FromArgb(red, green, blue);
                countLoops++;
            } while (excludeColors.Contains(newColor) && (countLoops < 10));

            return newColor;
        }
        private byte GetSimilarColorComponent(Random random, byte colorValue)
        {
            if (colorValue < 128)
            {
                colorValue = (byte)(colorValue * (1 + random.Next(1, 8) / (float)100));
            }
            else
            {
                colorValue = (byte)(colorValue / (1 + random.Next(1, 8) / (float)100));
            }
            return colorValue;
        }
        private Bitmap CreateBitmap(Bitmap bmp, ArrayList palette, Hashtable colorIndexToNewIndices, Stream messageStream, Stream keyStream)
        {
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            int imageSize = (bmpData.Height * bmpData.Stride) + (palette.Count * 4);
            byte[] pixels = new byte[imageSize];
            Marshal.Copy(bmpData.Scan0, pixels, 0, (bmpData.Height * bmpData.Stride));
            int messageByte = 0, messageBitIndex = 7;
            bool messageBit;
            ColorIndexList newColorIndices;
            Random random = new Random();
            int nextUseablePixelIndex = GetKey(keyStream);
            for (int pixelIndex = 0; pixelIndex < pixels.Length; pixelIndex++)
            {
                newColorIndices = (ColorIndexList)colorIndexToNewIndices[pixels[pixelIndex]];
                if ((pixelIndex < nextUseablePixelIndex) || messageByte < 0)
                {
                    pixels[pixelIndex] = newColorIndices.GetIndex();
                }
                else
                {
                    if (messageBitIndex == 7)
                    {
                        messageBitIndex = 0;
                        messageByte = messageStream.ReadByte();
                    }
                    else
                    {
                        messageBitIndex++;
                    }
                    messageBit = (messageByte & (1 << messageBitIndex)) > 0;
                    pixels[pixelIndex] = newColorIndices.GetIndex(messageBit);
                    nextUseablePixelIndex += GetKey(keyStream);
                }
            }
            BinaryWriter bw = new BinaryWriter(new MemoryStream());
            bw.Write(System.Text.ASCIIEncoding.ASCII.GetBytes("BM"));
            bw.Write((Int32)(55 + imageSize));
            bw.Write((Int16)0);
            bw.Write((Int16)0);
            bw.Write(
                (Int32)(
                Marshal.SizeOf(typeof(BITMAPINFOHEADER))
                + Marshal.SizeOf(typeof(BITMAPFILEHEADER))
                + palette.Count * 4)
                );
            bw.Write((Int32)Marshal.SizeOf(typeof(BITMAPINFOHEADER)));
            bw.Write((Int32)bmp.Width);
            bw.Write((Int32)bmp.Height);
            bw.Write((Int16)1);
            bw.Write((Int16)8);
            bw.Write((UInt32)0);
            bw.Write((Int32)(bmpData.Height * bmpData.Stride) + (palette.Count * 4));
            bw.Write((Int32)0);
            bw.Write((Int32)0);
            bw.Write((UInt32)palette.Count);
            bw.Write((UInt32)palette.Count);
            foreach (Color color in palette)
            {
                bw.Write((UInt32)color.ToArgb());
            }
            bw.Write(pixels);
            bmp.UnlockBits(bmpData);
            Bitmap newImage = (Bitmap)System.Drawing.Image.FromStream(bw.BaseStream);
            newImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
            bw.Close();
            return newImage;
        }
        private byte GetKey(Stream keyStream)
        {
            int keyByte = keyStream.ReadByte();
            if (keyByte < 0)
            {
                keyStream.Seek(0, SeekOrigin.Begin);
                keyByte = keyStream.ReadByte();
            }
            if (keyByte == 0) { keyByte = 1; }

            return (byte)keyByte;
        }
        public class ColorIndexList : ArrayList
        {
            private int maxIndexAlreadyUsed = 0;
            private Random random;
            public ColorIndexList(Random random)
            {
                this.random = random;
            }
            public byte GetIndex(bool messageBit)
            {
                if (messageBit)
                {
                    if (maxIndexAlreadyUsed < this.Count - 1)
                    {
                        maxIndexAlreadyUsed++;
                        return (byte)this[maxIndexAlreadyUsed];
                    }
                    else
                    {
                        return (byte)this[random.Next(1, this.Count - 1)];
                    }
                }
                else
                {
                    return (byte)this[0];
                }
            }
            public byte GetIndex()
            {
                if (maxIndexAlreadyUsed < this.Count - 1)
                {
                    maxIndexAlreadyUsed++;
                    return (byte)this[maxIndexAlreadyUsed];
                }
                else
                {
                    int index = random.Next(0, this.Count) - 1;
                    return (index < 0) ? (byte)this[0] : (byte)this[index];
                }
            }

        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BITMAPINFOHEADER
        {
            public Int32 biSize;
            public Int32 biWidth;
            public Int32 biHeight;
            public Int16 biPlanes;
            public Int16 biBitCount;
            public UInt32 biCompression;
            public Int32 biSizeImage;
            public Int32 biXPelsPerMeter;
            public Int32 biYPelsPerMeter;
            public UInt32 biClrUsed;
            public UInt32 biClrImportant;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BITMAPFILEHEADER
        {
            public Int16 bfType;
            public Int32 bfSize;
            public Int16 bfReserved1;
            public Int16 bfReserved2;
            public Int32 bfOffBits;
        }
    }
}