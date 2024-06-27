import tensorflow as tf

# Tải mô hình ResNet-50
model = tf.keras.applications.resnet50.ResNet50(weights='imagenet', include_top=False)  

img_path1 = r"E:\BE_STUDYING\DATN\Code\MinhTuan_MVC_CORE\VStyle.MLServer\images\anh4.jpg"
# Tiền xử lý ảnh (ví dụ)    
img1 = tf.keras.preprocessing.image.load_img(img_path1, target_size=(224, 224))
img_array1 = tf.keras.preprocessing.image.img_to_array(img1)
img_array1 = tf.image.rgb_to_grayscale(img_array1)  # Chuyển sang ảnh xám
# Chuyển sang ảnh đen trắng bằng thresholding
threshold = 128  # Giá trị ngưỡng
img_array1 = tf.where(img_array1 > threshold, 255, 0)

img_array1 = tf.expand_dims(img_array1, 0)
img_array1 = tf.keras.applications.resnet50.preprocess_input(img_array1)

features1 = model.predict(img_array1)



img_path2 = r"E:\BE_STUDYING\DATN\Code\MinhTuan_MVC_CORE\VStyle.MLServer\images\anh1.jpg"
# Tiền xử lý ảnh (ví dụ)
img2 = tf.keras.preprocessing.image.load_img(img_path2, target_size=(224, 224))
img_array2 = tf.keras.preprocessing.image.img_to_array(img2)

img_array2 = tf.expand_dims(img_array2, 0)
img_array2 = tf.keras.applications.resnet50.preprocess_input(img_array2)

features2 = model.predict(img_array2)

# Giảm chiều (flatten) các đặc trưng thành vector 1 chiều
features1_flat = features1.flatten()
features2_flat = features2.flatten()
from sklearn.metrics.pairwise import cosine_similarity
# Tính khoảng cách Cosine
cosine_sim = cosine_similarity([features1_flat], [features2_flat])[0][0]
print("Cosine Similarity:", cosine_sim)
# Tính toán khoảng cách Euclidean
import numpy as np
euclidean_distance = np.linalg.norm(features1_flat - features2_flat)
print("Euclidean Distance:", euclidean_distance)