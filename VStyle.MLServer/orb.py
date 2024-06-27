import cv2
import numpy as np

def extract_orb_features(image_path):
    # Đọc ảnh từ đường dẫn
    image = cv2.imread(image_path, cv2.IMREAD_GRAYSCALE)
    
    if image is None:
        print("Error: Image not found or unable to load.")
        return None, None

    # Khởi tạo ORB
    orb = cv2.ORB_create()

    # Tìm các keypoints và tính toán các descriptor
    keypoints, descriptors = orb.detectAndCompute(image, None)

    return keypoints, descriptors

def match_features(descriptors1, descriptors2):
    # Sử dụng BFMatcher với Hamming distance
    bf = cv2.BFMatcher(cv2.NORM_HAMMING, crossCheck=True)
    matches = bf.match(descriptors1, descriptors2)
    
    # Sắp xếp các match theo khoảng cách
    matches = sorted(matches, key=lambda x: x.distance)
    
    return matches

def calculate_similarity_score(image_path1, image_path2):
    keypoints1, descriptors1 = extract_orb_features(image_path1)
    keypoints2, descriptors2 = extract_orb_features(image_path2)

    if descriptors1 is None or descriptors2 is None:
        return 0

    matches = match_features(descriptors1, descriptors2)
    return len(matches)

if __name__ == "__main__":
    # Đường dẫn tới ảnh cần so sánh
    image_path1 = r"E:\BE_STUDYING\DATN\Code\MinhTuan_MVC_CORE\VStyle.MLServer\images\anh1.jpg"
    image_path2 = r"E:\BE_STUDYING\DATN\Code\MinhTuan_MVC_CORE\VStyle.MLServer\images\anh5.jpg"

    # Tính toán điểm số độ tương đồng giữa hai ảnh
    similarity_score = calculate_similarity_score(image_path1, image_path2)
    print(f"Similarity score between the images: {similarity_score}")
