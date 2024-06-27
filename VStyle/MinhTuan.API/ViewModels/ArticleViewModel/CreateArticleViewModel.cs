namespace MinhTuan.API.ViewModels.ArticleViewModel
{
    public class CreateArticleViewModel
    {
  
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }
        public DateTime PublishDate { get; set; }
        public string[] Tag { get; set; }
        public int CountView { get; set; }
        public bool Status { get; set; }
        public string? OldThumbnail { get; set; }
        public IFormFile ThumbnailFile { get; set; }
        public List<Guid> ListCategory { get; set; }

    }
}
