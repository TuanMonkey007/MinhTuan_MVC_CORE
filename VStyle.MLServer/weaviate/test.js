import weaviate from 'weaviate-ts-client';
import { readFileSync, writeFileSync } from 'fs';
const client = weaviate.client({
    scheme: 'http',
    host: 'localhost:8080',
});

const schemaRes = await client.schema.getter().do();
console.log(schemaRes);
const test = Buffer.from(readFileSync('./test.webp')).toString('base64');
const resImage = await client.graphql.get()
    .withClassName('ProductImage')
    .withFields(['image', 'path', '_additional { certainty, score }']) // Thêm rerankScore
    .withNearImage({ image: test, certainty: 0.8 })
    .withLimit(5)
    .do();
// Sắp xếp kết quả theo rerankScore giảm dần
resImage.data.Get.ProductImage.sort((a, b) => b._additional.score - a._additional.score);

if (resImage.data.Get.ProductImage && resImage.data.Get.ProductImage.length > 0) {
    resImage.data.Get.ProductImage.forEach((image, index) => {
        const imagePath = image?.path;
        const certainty = image?._additional?.certainty;
        const score = image?._additional?.score; 

        if (imagePath && certainty !== undefined && score !== undefined) {
            console.log(`Ảnh ${index + 1}:`);
            console.log(`  Đường dẫn: ${imagePath}`);
            console.log(`  Độ chính xác: ${certainty.toFixed(2)}`);
            console.log(`  Điểm rerank: ${score}`);
            console.log("---"); 
        } else {
            console.error(`Lỗi ảnh ${index + 1}: Đường dẫn hoặc độ chính xác hoặc điểm rerank không hợp lệ.`);
        }
    });
} else {
    console.error('Không tìm thấy hình ảnh sản phẩm nào.'); 
}