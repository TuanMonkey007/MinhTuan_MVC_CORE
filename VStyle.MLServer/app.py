from flask import Flask, request, jsonify
import numpy as np
import tensorflow as tf
from tensorflow.keras.layers import GlobalAveragePooling2D
from tensorflow.keras.models import Model
app = Flask(__name__)

# Tải mô hình ResNet-50 và thêm GlobalAveragePooling2D ở cuối
base_model = tf.keras.applications.resnet50.ResNet50(weights='imagenet', include_top=False)
x = base_model.output
x = GlobalAveragePooling2D()(x)  # Thêm GlobalAveragePooling2D
model = Model(inputs=base_model.input, outputs=x)

def extract_features(img_path):
# Tiền xử lý ảnh (ví dụ)   
    img = tf.keras.preprocessing.image.load_img(img_path, target_size=(224, 224))
    img_array = tf.keras.preprocessing.image.img_to_array(img)
    img_array = tf.expand_dims(img_array, 0)
    img_array = tf.keras.applications.resnet50.preprocess_input(img_array)
    features = model.predict(img_array)
    # Chuẩn hóa vector đặc trưng
    normalized_features = features / np.linalg.norm(features)
    return normalized_features.flatten()
@app.route('/', methods=['GET'])
def index():
    return 'Hello, World!'

@app.route('/extract-features', methods=['POST'])
def extract_features_endpoint():
    data = request.json
    img_path = data['ImagePath']
    print("Đang xử lý ảnh >>>>>>   ", img_path)
    features = extract_features(img_path)
    feature_bytes = features.astype(np.float32).tobytes()  # Chuyển thành bytes
    return feature_bytes  # Trả về trực tiếp bytes
if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5006)
