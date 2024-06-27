import weaviate from 'weaviate-ts-client';
import { readFileSync, writeFileSync } from 'fs';
const client = weaviate.client({
    scheme: 'http',
    host: 'localhost:8080',
});

const schemaRes = await client.schema.getter().do();

// // Kiểm tra xem lớp 'ProductImage' tồn tại chưa
const classExists2 = schemaRes.classes.some(cls => cls.class == 'ProductImage');
if (classExists2) {
  await client.schema.classDeleter().withClassName('ProductImage').do();
  console.log("Xóa thành công");
} else {
  console.log("Không có để xóa");
}
console.log(schemaRes);



// Nạp đoạn này để nạp ảnh vào  mô hình với Weaviate

// Hàm chuyển đổi file ảnh sang base64
// function toBase64(filePath) {
//     return Buffer.from(readFileSync(filePath)).toString('base64');
// }
// const  imgFiles = readdirSync('./images');
// const promises = imgFiles.map(async (imgFile) => {
//     const b64 = toBase64(`./images/${imgFile}`);
//     await client.data.creator()
//     .withClassName('ProductImage')
//     .withProperties({
//         image: b64,
//         path: imgFile
//     })
//     .do();
// })
// await Promise.all(promises)
// console.log('Ảnh đã được nạp vào mô hình: ', imgFiles.length);