import { createApp } from 'vue'
import router from './router';
import App from './App.vue'
import axios from 'axios'; // Import axios
window.axios = axios;// Gán axios vào window để sử dụng axios ở bất kỳ đâu trong project
import Antd from 'ant-design-vue'; // Đăng ký tất cả component của ant-design-vue
//import icon của ant-design-vue

import "@ant-design/icons-vue";
//import 'ant-design-vue/dist/reset.css'; // Import css của ant-design-vue

import { message,notification } from 'ant-design-vue'; // Import message từ ant-design-vue
import '@ant-design/icons-vue'; // Import icon của ant-design-vue
import { library } from '@fortawesome/fontawesome-svg-core' // Import thư viện font awesome
import { fas } from '@fortawesome/free-solid-svg-icons'
import { far } from '@fortawesome/free-regular-svg-icons'
import { fab } from '@fortawesome/free-brands-svg-icons'
library.add(fas, far, fab)
/* import font awesome icon component */
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
const app = createApp(App)

app.component('font-awesome-icon', FontAwesomeIcon)
app.use(Antd)
app.use(router)
message.config({
  top: '24px', // Set the distance from the top of the screen
  right: '100px', // Set the distance from the right of the screen
    duration: 2, // Thời gian hiển thị message
    maxCount: 3, // Số lượng message tối đa hiển thị
});// Cấu hình message

app.config.globalProperties.$message = message;// Đăng ký message vào globalProperties để sử dụng ở bất kỳ đâu trong project
app.config.globalProperties.$notification = notification;// Đăng ký notification vào globalProperties để sử dụng ở bất kỳ đâu trong project
notification.config({
  placement: 'topRight',
  top: '50px',
  duration: 2,
  maxCount: 3
 
});
app.mount('#app')
