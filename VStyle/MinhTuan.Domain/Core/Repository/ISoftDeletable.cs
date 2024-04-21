namespace MinhTuan.Domain.Core.Repository
{
    public interface ISoftDeletable
    {
        bool IsDelete { get; set; } //Bản ghi đã bị xóa chưa
    }
}