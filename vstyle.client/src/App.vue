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
    background-color: rgb(230, 233, 240);
    min-height: 100vh;
  }
  #app {
  font-family: 'Lora', serif; /* Thay Merriweather thành Lora */
 
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: left;
  color: #2c3e50;
  min-height: 100vh;
  max-width: 100%;
}
/* .route-enter-from {
  opacity: 0;
  transform: translate(100px, 100px); 
}

.route-leave-to {
  opacity: 0;
  transform: translate(-100px, -100px);  
}

.route-enter-active{
  transition: all 0.5s ease-out;
}

.route-leave-active {
  transition: all 0.5s ease-in;
}
  */
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

</style>
