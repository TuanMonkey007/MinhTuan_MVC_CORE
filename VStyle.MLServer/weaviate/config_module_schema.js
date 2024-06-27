import weaviate from 'weaviate-ts-client';
const client = weaviate.client({
    scheme: 'http',
    host: 'localhost:8080',
});

const schemaRes = await client.schema.getter().do();
console.log(schemaRes);
 // Kiểm tra xem lớp 'ProductImage' đã tồn tại chưa
const classExists = schemaRes.classes.some(cls => cls.class === 'ProductImage');
 if (!classExists) {
    // Định nghĩa cấu hình schema
    const schemaConfig = {
        Class: 'ProductImage',
        vectorizer: 'img2vec-neural',
       // vectorIndexType: 'hnsw',
        moduleConfig: {
            'img2vec-neural': {
                imageFields: ['image'],
                excludedPropertyNames: ['path']
            }
        },
        properties: [
            {
                name: 'image',
                dataType: ['blob']
            },
            {
                name: 'path',
                dataType: ['string']
            }
        ]
    };
    // Thêm schema vào Weaviate
    await client.schema.classCreator().withClass(schemaConfig).do();
} else {
    console.log("Đã tồn tại");
}