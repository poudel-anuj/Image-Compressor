using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageCompressor.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Compression(HttpPostedFileBase file)
        {
            using (var ms = new MemoryStream())
            {
                var targetImagePath = Server.MapPath("/Content/CompressedImage/" + Path.GetFileNameWithoutExtension(file.FileName) + "_Compressed" + DateTime.Now.ToString("ddMMyyyyh")) + Path.GetExtension(file.FileName);
                byte[] image = new byte[file.ContentLength];
                file.InputStream.Read(image, 0, image.Length);
                Compressimage(targetImagePath, "", image);

            }
            return RedirectToAction("Index");
        }

        public static void Compressimage(string targetPath, String filename, Byte[] byteArrayIn)
        {
            try
            {
                System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
                using (MemoryStream memstr = new MemoryStream(byteArrayIn))
                {
                    using (var image = Image.FromStream(memstr))
                    {
                        string extension;
                        Bitmap originalBMP = new Bitmap(memstr);
                        int originalWidth = originalBMP.Width;
                        int originalHeight = originalBMP.Height;
                        Bitmap bitMAPI = new Bitmap(originalBMP, originalWidth, originalHeight);
                        Graphics imgGraph = Graphics.FromImage(bitMAPI);
                        extension = Path.GetExtension(targetPath);
                        
                        if (extension.ToLower() == ".jpeg" ||extension.ToLower() == ".jpg"  || extension.ToLower() == ".png")
                        {
                            //imgGraph.SmoothingMode =  SmoothingMode.AntiAlias;
                            //imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            //imgGraph.DrawImage(originalBMP, 0, 0, originalWidth, originalHeight);
                            //bitMAPI.Save(targetPath, image.RawFormat);
                            //bitMAPI.Dispose();
                            //imgGraph.Dispose();                           
                            //originalBMP.Dispose();

                           imgGraph.SmoothingMode = SmoothingMode.AntiAlias;
                            imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            imgGraph.DrawImage(originalBMP, 0, 0, originalWidth, originalHeight);
                            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                            EncoderParameters myEncoderParameters = new EncoderParameters(1);
                            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                            myEncoderParameters.Param[0] = myEncoderParameter;
                            bitMAPI.Save(targetPath, jpgEncoder, myEncoderParameters);
                            bitMAPI.Dispose();
                            imgGraph.Dispose();
                            originalBMP.Dispose();
                        }
                        else
                        {
                            bitMAPI.Dispose();
                            imgGraph.Dispose();
                            originalBMP.Dispose();
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                string exc = ex.Message;
                throw;
            }
        }


        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }


    }
}