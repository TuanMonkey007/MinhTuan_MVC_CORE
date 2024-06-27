using Microsoft.ML;
using MinhTuan.Domain.Helper.ImageSearch;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Image;

using SixLabors.ImageSharp;

using SixLabors.ImageSharp.PixelFormats;
using TensorFlow;
using MinhTuan.Domain.Entities;
using Microsoft.ML.Transforms;
using Tensorflow;
using SixLabors.ImageSharp.ColorSpaces;


namespace MinhTuan.Service.Services.ImageSearch
{
    public class ImageSearchService : IImageSearchSevice
    {
        public double CosineSimilarity(float[] vectorA, float[] vectorB)
        {
            // Ensure vectors are of the same length
            if (vectorA.Length != vectorB.Length)
                throw new ArgumentException("Vectors must be of the same length");

            // Calculate dot product
            double dotProduct = DotProduct(vectorA, vectorB);

            // Calculate magnitudes
            double magnitudeA = Magnitude(vectorA);
            double magnitudeB = Magnitude(vectorB);

            // Calculate cosine similarity
            double similarity = dotProduct / (magnitudeA * magnitudeB);

            return similarity;
        }
        private static double DotProduct(float[] vectorA, float[] vectorB)
        {
            return vectorA.Zip(vectorB, (a, b) => (double)a * b).Sum();
        }

        private static double Magnitude(float[] vector)
        {
            return Math.Sqrt(vector.Select(x => (double)x * x).Sum());
        }
        public double EuclideanDistance(float[] vectorA, float[] vectorB)
        {
            double sum = 0.0;
            for (int i = 0; i < vectorA.Length; i++)
            {
                sum += Math.Pow(vectorA[i] - vectorB[i], 2);
            }
            return Math.Sqrt(sum);
        }

        public List<ImageOutputData> GetPredictions()
        {
            MLContext mlContext = new MLContext();

            // Đường dẫn tới thư mục chứa ảnh bạn muốn trích xuất đặc trưng
            string imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "TestSearch");

            var imageDatas = new List<ImageInputData>()
        {
            new ImageInputData {
                ImagePath = Path.Combine(imagesFolder, "938f395f-264b-44b9-9cd0-438b02ce101c.jpg")
            },
             new ImageInputData {
                ImagePath = Path.Combine(imagesFolder, "eb6a8102-cae7-4bf8-aa8c-1c87ad775bf1.jpg")
            },
              new ImageInputData {
                ImagePath = Path.Combine(imagesFolder, "0b5b9091-896d-42f7-b650-bb072b3adaf8.jpg")
            },
               new ImageInputData {
                ImagePath = Path.Combine(imagesFolder, "aoNam.jpg")
            },
                new ImageInputData {
                ImagePath = Path.Combine(imagesFolder, "vayDen.jpg")
            },
        };

           
            // Tạo DataView chứa các đường dẫn tới ảnh
            var data = mlContext.Data.LoadFromEnumerable(imageDatas);

            // Đường dẫn tới mô hình TensorFlow đã được huấn luyện
            var modelPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Resnet-50");

            var pipeline = mlContext.Transforms.LoadImages(outputColumnName: "serving_default_input_1", imageFolder: imagesFolder, inputColumnName: nameof(ImageInputData.ImagePath))
        .Append(mlContext.Transforms.ResizeImages(outputColumnName: "serving_default_input_1", imageWidth: ResNet50Settings.ImageWidth, imageHeight: ResNet50Settings.ImageHeight, inputColumnName: "serving_default_input_1"))
        .Append(mlContext.Transforms.ExtractPixels(outputColumnName: "serving_default_input_1", inputColumnName: "serving_default_input_1", colorsToExtract: Microsoft.ML.Transforms.Image.ImagePixelExtractingEstimator.ColorBits.Rgb))
        .Append(mlContext.Model.LoadTensorFlowModel(modelPath)
            .ScoreTensorFlowModel(outputColumnNames: new[] { "StatefulPartitionedCall" }, inputColumnNames: new[] { "serving_default_input_1" }, addBatchDimensionInput: true))
               .Append(mlContext.Transforms.CopyColumns("Featured", "StatefulPartitionedCall")); // Đổi tên cột đầu ra trong pipeline



            // Tạo danh sách để lưu kết quả
            var results = new List<ImageOutputData>();
            // Transform the data using the pre-trained model
            var transformedData = pipeline.Fit(data).Transform(data);

            // Extract the features and save them to a list
             results = mlContext.Data.CreateEnumerable<ImageOutputData>(transformedData, reuseRowObject: false).ToList();

            return results;
        }
    }
}