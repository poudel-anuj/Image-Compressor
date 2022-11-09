using ImageCompressor.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageCompressor.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            foreach (string item in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[item] as HttpPostedFileBase;
                if (file.ContentLength == 0)
                    continue;

                if (file.ContentLength > 0)
                {
                    // width + height will force size, care for distortion
                    //Exmaple: ImageUpload imageUpload = new ImageUpload { Width = 800, Height = 700 };

                    // height will increase the width proportially 
                    //Example: ImageUpload imageUpload = new ImageUpload { Height= 600 };

                    // width will increase the height proportially 
                    ImageUpload imageUpload = new ImageUpload();

                    // rename, resize, and upload 
                    //return object that contains {bool Success,string ErrorMessage,string ImageName}
                    ImageResult imageResult = imageUpload.RenameUploadFile(file);

                    if (imageResult.Success)
                    {
                        //TODO: write the filename to the db
                        Console.WriteLine(imageResult.ImageName);
                    }
                    else
                    {
                        //TODO: show view error
                        // use imageResult.ErrorMessage to show the error
                        ViewBag.Error = imageResult.ErrorMessage;
                    }
                }
            }

            return View();
        }

        private void GenerateThumbnails(double scaleFactor, string sourcePath, string targetPath)
        {

            // Get a bitmap.
            Bitmap bmp1 = new Bitmap(sourcePath);

            //Or you do can use buil-in method
            //ImageCodecInfo jgpEncoder GetEncoderInfo("image/gif");//"image/jpeg",...
            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

            // Create an Encoder object based on the GUID
            // for the Quality parameter category.
            System.Drawing.Imaging.Encoder myEncoder =
            System.Drawing.Imaging.Encoder.Quality;

            // Create an EncoderParameters object.
            // An EncoderParameters object has an array of EncoderParameter
            // objects. In this case, there is only one
            // EncoderParameter object in the array.
            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            bmp1.Save(targetPath, jgpEncoder, myEncoderParameters);


        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
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

    