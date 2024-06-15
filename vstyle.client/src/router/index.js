//Khai báo router
import { createRouter,createWebHistory } from "vue-router";
import adminRouter from "./admin";
import customerRouter from "./customer";
import { Role } from "@/helpers/Constants";

const routes=[
    ...adminRouter,
    ...customerRouter,
    
   
 
    {
        path: '/403',
        name: 'Unauthorized',
        component:()=> import("../views/403.vue"),
        meta: {title: '403' }
    },
   // Tuyến đường cuối cùng chuyển hướng tất cả các đường dẫn không xác định về trang chủ
    {
    path: '/:pathMatch(.*)*',
    component: () => import('../views/404.vue'),
    meta: {title: '404' }
    }
    
]
const router = createRouter({ 
    history: createWebHistory(),
    routes 
});
// Thêm navigation guard
router.beforeEach((to, from, next) => {
    const isAuthenticated = localStorage.getItem('accessToken');
    const userRole = localStorage.getItem('role');
    document.title = to.meta.title + " | VStyle - Thời trang Việt" || 'VStyle - Thời trang Việt'; // Nếu route không có meta.title, sử dụng title mặc định
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
