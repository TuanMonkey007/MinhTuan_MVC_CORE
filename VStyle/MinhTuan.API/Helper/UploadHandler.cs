namespace MinhTuan.API.Helper
{
    public class UploadHandler
    {
        public string UploadProfileImage(IFormFile file)
        {
            var isValid = ValidateImage(file);
            if (isValid.Contains("Ảnh hợp lệ"))
            {
                string extention = Path.GetExtension(file.FileName);
                string fileName = Guid.NewGuid().ToString() + extention;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Avatar");
                using FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
                file.CopyTo(stream);
                return fileName;
            }
            return isValid;
        }
        public string UploadProductImage(IFormFile file)
        {
            var isValid = ValidateImage(file);
            if(isValid.Contains("Ảnh hợp lệ")){
                string extention = Path.GetExtension(file.FileName);
                string fileName = Guid.NewGuid().ToString() + extention;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Product");
                using FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
                file.CopyTo(stream);
                return fileName;
            }
            return isValid;
           
        }
        public string UploadBannerImage(IFormFile file)
        {
            var isValid = ValidateImage(file);
            if (isValid.Contains("Ảnh hợp lệ"))
            {
                string extention = Path.GetExtension(file.FileName);
                string fileName = Guid.NewGuid().ToString() + extention;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Banner");
                using FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
                file.CopyTo(stream);
                return fileName;
            }
            return isValid;

        }

        //public string ValidateImage(IFormFile file)
        //{

        //    //Extention
        //    List<string> validExtentions = new List<string>() { ".png", ".jpg", ".jpeg", ".gif" };
        //    string extention = Path.GetExtension(file.FileName);
        //    if (!validExtentions.Contains(extention))
        //    {
        //        return $"Định dạng ảnh không phù hợp ({string.Join(',', validExtentions)})";

        //    }
        //    long size = file.Length;
        //    if (size > 10 * 1024 * 1024)
        //    {
        //        return "Kích thước tối đa 10Mb";
        //    }
        //    return "Ảnh hợp lệ";
        //}
        public string ValidateImage(IFormFile file)
        {
            // Kiểm tra ContentType
            List<string> validContentTypes = new List<string>() { "image/png", "image/jpeg", "image/gif" }; // Add image/jpg
            if (!validContentTypes.Contains(file.ContentType))
            {
                return $"Định dạng ảnh không phù hợp ({string.Join(',', validContentTypes)})";
            }

            // Kiểm tra kích thước file (giữ nguyên)
            long size = file.Length;
            if (size > 10 * 1024 * 1024)
            {
                return "Kích thước tối đa 10Mb";
            }

            return "Ảnh hợp lệ";
        }

    }
}
