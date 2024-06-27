import cv2
import numpy as np

def extract_sift_features(image_path):
    # Đọc ảnh từ đường dẫn
    image = cv2.imread(image_path, cv2.IMREAD_GRAYSCALE)
    
    if image is None:
        print("Error: Image not found or unable to load.")
        return None, None

    # Khởi tạo SIFT
    sift = cv2.SIFT_create()

    # Tìm các keypoints và tính toán các descriptor
    keypoints, descriptors = sift.detectAndCompute(image, None)

    return keypoints, descriptors

def match_features(descriptors1, descriptors2):
    # Sử dụng FLANN-based matcher
    FLANN_INDEX_KDTREE = 1
    index_params = dict(algorithm=FLANN_INDEX_KDTREE, trees=5)
    search_params = dict(checks=50)
    
    flann = cv2.FlannBasedMatcher(index_params, search_params)
    
    matches = flann.knnMatch(descriptors1, descriptors2, k=2)

    # Lọc các cặp match tốt bằng cách sử dụng tỉ lệ Lowe's ratio test
    good_matches = []
    for m, n in matches:
        if m.distance < 0.7 * n.distance:
            good_matches.append(m)
    
    return good_matches

def calculate_similarity_score(image_path1, image_path2):
    keypoints1, descriptors1 = extract_sift_features(image_path1)
    keypoints2, descriptors2 = extract_sift_features(image_path2)

    if descriptors1 is None or descriptors2 is None:
        return 0

    good_matches = match_features(descriptors1, descriptors2)
    return len(good_matches)

if __name__ == "__main__":
    # Đường dẫn tới ảnh cần so sánh
    image_path1 = r"E:\BE_STUDYING\DATN\Code\MinhTuan_MVC_CORE\VStyle.MLServer\images\anh1.jpg"
    image_path2 = r"E:\BE_STUDYING\DATN\Code\MinhTuan_MVC_CORE\VStyle.MLServer\images\anh3.jpg"

    # Tính toán điểm số độ tương đồng giữa hai ảnh
    similarity_score = calculate_similarity_score(image_path1, image_path2)
    print(f"Similarity score between the images: {similarity_score}")
