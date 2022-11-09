using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageCompressor.Controllers
{
    public class CompressController : Controller
    {
        // GET: Compress
        public ActionResult Index()
       {
            return View();
        }
        [HttpPost]
        public IActionResult Index(IFormFile file)
        {
            //IFormFile file = "";
            string filename = string.Empty;
            string path = string.Empty;
            if (file.Length > 0)
            {
                filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                //path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Content/CompressedImage"));
                path = Server.MapPath("/Content/CompressedImage/" + Path.GetFileNameWithoutExtension(file.FileName) + "_Compressed" + DateTime.Now.ToString("ddMMyyyyh")) + Path.GetExtension(file.FileName);
                string fullpath = Path.Combine(path, filename);
                using (var image = Image.Load(file.OpenReadStream()))
                {
                    string newSize = ImageResize(image, 600, 600);
                    string[] sizearray = newSize.Split(',');
                    image.Mutate(x => x.Resize(Convert.ToInt32(sizearray[1]), Convert.ToInt32(sizearray[0])));
                    image.Save(fullpath);
                    TempData["msg"] = "Image compress sucessfully";
                }
            }
            return (IActionResult)RedirectToAction("Index");
        }

        public string ImageResize(Image img, int MaxWidth, int MaxHeight)
        {
            if(img.Width>MaxHeight || img.Height > MaxHeight)
            {
                double widthraio = (double)img.Width /(double)MaxWidth;
                double heightratio = (double)img.Height /(double)MaxHeight;
                double ratio = Math.Max(widthraio, heightratio);
                int newWidth = (int)(img.Width/ratio);
                int newHeight = (int)(img.Height/ratio);
                return newHeight.ToString() + "," + newWidth.ToString();
            }
            else
            {
                return img.Height.ToString() + "," + img.Width.ToString();

            }
        }
    }
}