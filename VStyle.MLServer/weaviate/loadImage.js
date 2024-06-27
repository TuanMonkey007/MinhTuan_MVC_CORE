import weaviate from 'weaviate-ts-client';
import { readFileSync, writeFileSync, readdirSync } from 'fs';


const client = weaviate.client({
    scheme: 'http',
    host: 'localhost:8080',
});

const schemaRes = await client.schema.getter().do();


//Hàm chuyển đổi file ảnh sang base64
function toBase64(filePath) {
        return Buffer.from(readFileSync(filePath)).toString('base64');
    }
    console.log('Ảnh đang được nạp vào mô hình ....');
    const imgFolder = "E:\\BE_STUDYING\\DATN\\Code\\MinhTuan_MVC_CORE\\VStyle\\MinhTuan.API\\Uploads\\Product"
    const  imgFiles = readdirSync(imgFolder);
    var count = 1;
    const promises = imgFiles.map(async (imgFile) => {
        const b64 = toBase64(`${imgFolder}/${imgFile}`);
        await client.data.creator()
        .withClassName('ProductImage')
        .withProperties({
            image: b64,
            path: imgFile
        })
        .do();
        console.log('Số ảnh đã nạp được ', count++);
    })
    await Promise.all(promises)
    console.log('Đã xong !! ', imgFiles.length);