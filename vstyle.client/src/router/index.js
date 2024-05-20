//Khai báo router
import { createRouter,createWebHistory } from "vue-router";
import adminRouter from "./admin";
import customerRouter from "./customer";
import { Role } from "@/helpers/Constants";

const routes=[
    ...adminRouter,
    ...customerRouter,
    { 
      path: "/Login",
      name: "Login",
      component: ()=> import("../views/Customer/Home/login.vue"),
      beforeEnter(to, from, next) { //Chặn khi đã đăng nhập
        const isAuthenticated = localStorage.getItem('accessToken');
        if (isAuthenticated) {//Nếu đã đăng nhập thì không cho chuyển hướng
           //stay on the same page
           if(localStorage.getItem('role')==Role.ADMIN || localStorage.getItem('role')==Role.STAFF){
            next({ name: 'AdminHome' });
          }
          else{
            next({ name: 'CustomerHome' });
            }
        }else{
            next();
       }
      }//beforeEnter

      
  },
 
    {
        path: '/unauthorized',
        name: 'Unauthorized',
        component:()=> import("../views/Unauthorized.vue"),
    },
    // Tuyến đường cuối cùng chuyển hướng tất cả các đường dẫn không xác định về trang chủ
    // {
    // path: '/:pathMatch(.*)*',
    // redirect: '/'
    // }
]
const router = createRouter({ 
    history: createWebHistory(),
    routes 
});
// Thêm navigation guard
router.beforeEach((to, from, next) => {
    const isAuthenticated = localStorage.getItem('accessToken');
    const userRole = localStorage.getItem('role');
  
    if (to.matched.some(record => record.meta.requiresAuth)) {
      if (!isAuthenticated) {
        next({ name: 'Login' });
      } else {
        if (to.meta.roles && !to.meta.roles.includes(userRole)) {
          next({ name: 'Unauthorized' });
        } else {
          next();
        }
      }
    } else {
      next();
    }
  });
export default router;
