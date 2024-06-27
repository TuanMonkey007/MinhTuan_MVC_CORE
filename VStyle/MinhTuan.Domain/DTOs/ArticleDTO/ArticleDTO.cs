using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.DTOs.ArticleDTO
{
    public class ArticleDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }
        public DateTime PublishDate { get; set; }
        public string[] Tag { get; set; }
        public int CountView { get; set; }
        public bool Status { get; set; }
        public string Thumbnail { get; set; }
        public string? ThumbnailBase64 { get; set; }
        public string? ThumbnailContentType { get; set; }
        public  string? Author { get; set; }
        public Guid? AuthorId { get; set; }
        public List<Guid>? ListCategory { get; set; }
        public List<string>? ListCategoryName { get; set; }

    }
}
