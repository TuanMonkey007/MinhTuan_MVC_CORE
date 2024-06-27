using MinhTuan.Domain.Helper.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.DTOs.ProductDTO
{
    public class ImageSearchDTO :SearchBase
    {
        public  List<string> ImagePaths { get; set; }
    }
}
