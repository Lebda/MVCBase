using System;
using System.Linq;
using System.Web.Mvc;
using EFHelp.Abstract;

namespace EFHelp.Concrete.ControllerHelp
{
    public class Conroller4ImageHolder<T> : ConrollerBase<T> where T : class, IImageHolder
    {
        public Conroller4ImageHolder(IGenericRepository<T> repo)
            : base(repo)
        {
        }

        #region IMAGES
        protected FileContentResult GetImageBase(int id)
        {
            var item = m_repo.SelectByID(id);
            if (item == null)
            {
                return null;
            }
            return File(item.ImageData, item.ImageMimeType);
        }
        #endregion
    }
}
