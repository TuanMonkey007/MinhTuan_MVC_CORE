<template>
 

  <router-view> </router-view>


</template>

<script>
  export default {
    name: 'App',
    components: {
    },//end components
   
  }
</script>

<style>
body, html {
    margin: 0;
    padding: 0;
    background-color: #ffffff;;
    min-height: 100vh;
  }
  #app {
  font-family: 'Lora', serif; /* Thay Merriweather thành Lora */
 
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: left;
  color: #ffffff;
  min-height: 100vh;
  max-width: 100%;
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
</style>
