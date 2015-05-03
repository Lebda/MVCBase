using System;
using System.Linq;
using System.Web;
using EFHelp.Concrete;

namespace EFHelp.Extensions
{
    public static class HttpPostedFileBaseExtension
    {
        public static void ImageUpdate<Tentity>(this HttpPostedFileBase image, Tentity item) where Tentity : IImageHolder
        {
            if (image != null)
            {
                item.ImageMimeType = image.ContentType;
                item.ImageData = new byte[image.ContentLength];
                image.InputStream.Read(item.ImageData, 0, image.ContentLength);
            }
        }
    }
}
