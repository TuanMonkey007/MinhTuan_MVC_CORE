namespace MinhTuan.API.ViewModels.DataCategoryViewModel
{
    public class CreateDataCategoryViewModel
    {
        public Guid ParentId { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
