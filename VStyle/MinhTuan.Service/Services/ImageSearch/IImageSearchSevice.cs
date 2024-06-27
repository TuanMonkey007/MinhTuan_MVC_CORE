using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Image;
using MinhTuan.Domain.Helper.ImageSearch;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace MinhTuan.Service.Services.ImageSearch
{
    public interface IImageSearchSevice
    {
        List<ImageOutputData> GetPredictions();
        double CosineSimilarity(float[] vectorA, float[] vectorB);
        double EuclideanDistance(float[] vectorA, float[] vectorB);
       



    }
}
