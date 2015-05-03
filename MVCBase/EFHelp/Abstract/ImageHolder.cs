using System;
using System.Linq;

namespace EFHelp.Concrete
{
    public interface IImageHolder
    {
        byte[] ImageData { get; set; }
        string ImageMimeType { get; set; }
    }
}
