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
            using (var ms= new MemoryStream())
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
                        float maxHeight = 900.0f;
                        float maxWidth = 900.0f;
                        int newWidth;
                        int newHeight;
                        string extension;
                        Bitmap originalBMP = new Bitmap(memstr);
                        int originalWidth = originalBMP.Width;
                        int originalHeight = originalBMP.Height;

                        if (originalWidth > maxWidth || originalHeight > maxHeight)
                        {
                            //To preserve the aspect ratio
                            float ratioX = (float)maxWidth / (float)originalWidth;
                            float ratioY = (float)maxHeight / (float)originalHeight;
                            float ratio = Math.Min(ratioX, ratioY);
                            newWidth  = (int)(originalWidth * ratio);
                            newHeight = (int)(originalHeight * ratio);
                        }
                        else
                        {
                            newWidth = (int)originalWidth;
                            newHeight = (int)originalHeight;
                        }
                        Bitmap bitMAPI = new Bitmap(originalBMP, newWidth, newHeight);
                        Graphics imgGraph = Graphics.FromImage(bitMAPI);
                        extension = Path.GetExtension(targetPath);
                        if (extension.ToLower() != ".png" || extension.ToLower() != ".jpeg")
                        {

                        }
                        if (extension.ToLower() == ".png" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".jpg")
                        {
                            imgGraph.SmoothingMode =  SmoothingMode.AntiAlias;
                            imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);
                            bitMAPI.Save(targetPath,image.RawFormat);
                            bitMAPI.Dispose();
                            imgGraph.Dispose();
                            originalBMP.Dispose();
                        }
                        //else if(extension.ToLower() == ".jpg")

                        //    imgGraph.SmoothingMode = SmoothingMode.AntiAlias;
                        //    imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        //    imgGraph.DrawImage(originalBMP,0,0,newWidth,newHeight);
                        //    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                        //    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                        //    EncoderParameters myEncoderParameters = new EncoderParameters(1);

                        //    EncoderParameters myEncoderParameter = new EncoderParameters(50L);

                        //}
                          
                    }
                }
            }
            catch(Exception ex)
            {
                string exc = ex.Message;
                throw;
            }
        }

       
    }
}