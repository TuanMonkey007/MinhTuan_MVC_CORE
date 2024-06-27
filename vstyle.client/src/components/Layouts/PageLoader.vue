<template>
    <div class="loaderContainer" v-if="isLoading">
        <div class="lds-ripple"><div></div><div></div></div>

    </div>
</template>
<script>
    import { ref, watch } from 'vue';

    export default {
        name: "PageLoader",
        setup() {
            const isLoading = ref(false);

            // theo dõi thay đổi trong isLoading
            watch(isLoading, (newVal) => {
                if (!newVal) {
                    // Ẩn loader sau khi nội dung đã tải xong
                    setTimeout(() => {
                        isLoading.value = false;
                    }, 500); // Thời gian delay trước khi ẩn loader (miligiây)
                }
            });

            const loadingHandler = (status) => {
                isLoading.value = status;
            };

            document.addEventListener('loading', (event) => {
                loadingHandler(event.detail);
            });

            return { isLoading };
        },
    };
</script>


<style>
    .loaderContainer {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100vh;
        display: flex;
        justify-content: center;
        align-items: center;
        background-color: rgba(255, 255, 255, 0.7);
        z-index: 9999;
    }

    .lds-ripple,
.lds-ripple div {
  box-sizing: border-box;
}
.lds-ripple {
  display: inline-block;
  position: relative;
  width: 80px;
  height: 80px;
  color: #c21f24; /* Đặt màu sắc cho toàn bộ animation */
}
.lds-ripple div {
  position: absolute;
  border: 4px solid currentColor;
  opacity: 1;
  border-radius: 50%;
  animation: lds-ripple 1s cubic-bezier(0, 0.2, 0.8, 1) infinite;
}
.lds-ripple div:nth-child(2) {
  animation-delay: -0.5s;
}
@keyframes lds-ripple {
  0% {
    top: 36px;
    left: 36px;
    width: 8px;
    height: 8px;
    opacity: 0;
  }
  4.9% {
    top: 36px;
    left: 36px;
    width: 8px;
    height: 8px;
    opacity: 0;
  }
  5% {
    top: 36px;
    left: 36px;
    width: 8px;
    height: 8px;
    opacity: 1;
  }
  100% {
    top: 0;
    left: 0;
    width: 80px;
    height: 80px;
    opacity: 0;
  }
}


    
</style>