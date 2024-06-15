<template>
 

  <router-view></router-view>
  

</template>

<script>

  export default {
    name: 'App',
 
   
  }
</script>


<style>

body, html {
    margin: 0;
    padding: 0;
    background-color: #f6f9ff;;
    min-height: 100vh;
    background: #f6f9ff
  }



.route-enter-active {
  transition: all 0.2s ease-out; /* Transition mượt mà */
}

.route-leave-active {
  transition: all 0.2s ease-in;
}

.route-enter-from {
  opacity: 0;
  transform: translate(100px, 100px) scale(0.5); /* Bắt đầu nhỏ và mờ */
}

.route-enter-to {
  opacity: 1;
  transform: translate(0, 0) scale(1); /* Kết thúc bình thường */
}

.route-leave-from {
  opacity: 1;
  transform: translate(0, 0) scale(1); /* Bắt đầu bình thường */
}

.route-leave-to {
  opacity: 0;
  transform: translate(-100px, -100px) scale(0.5); /* Kết thúc nhỏ và mờ */
}
.image-product {
  width: 100%;
  height: 200px;
  object-fit: cover;
  transition: transform 0.7s ease; /* Hiệu ứng zoom */
}
.ant-card-cover{
  overflow: hidden;
  border-radius: 10%;
}
.image-product:hover {
  transform: scale(1.3);
  transform-origin: center; /* Zoom từ trung tâm */
}
.price-product {
  font-size: 20px;
  color: #b63034e8;
}
.btn-buy-now{
  display: inline-block;
    box-sizing: border-box;
    min-width: 100px;
    width: 300px;
    text-decoration: none;
    color: #fff;
    text-shadow: 0 1px 2px rgba(0, 0, 0, .75);
  background: #b11d1b;
    outline: none;
    border-radius: 15px;
    border: 1px solid #9a3a37;
    box-shadow: inset 1px 1px 0 hsla(0, 0%, 100%, .25), inset 0 0 6px #a23227, inset 0 80px 80px -40px #ac3223, 1px 1px 3px rgba(0, 0, 0, .75);
    overflow: visible;
    transition: .5s linear;
}
img {
  width: 100%; /* Đảm bảo ảnh chiếm toàn bộ chiều rộng của cột */
  height: auto; /* Giữ tỷ lệ khung hình của ảnh */
  object-fit: contain; /* Hoặc object-fit: cover; */
}

</style>
