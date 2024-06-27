using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Image;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using Microsoft.ML.Vision;


namespace MinhTuan.Domain.Helper.ImageSearch

{
    public class ImageInputData
    {
        [LoadColumn(0)]
        public string ImagePath { get; set; }
    }

    public class ImageOutputData
    {
        [ColumnName("Featured")]
        [VectorType(2048)]
        public float[] Featured { get; set; }
    }

    public class ResNet50Settings
    {
        public const int ImageWidth = 224; // Thay đổi kích thước ảnh cho ResNet-50
        public const int ImageHeight = 224;
    }

}
