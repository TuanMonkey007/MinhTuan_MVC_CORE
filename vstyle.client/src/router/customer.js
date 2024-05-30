import { Role } from "@/helpers/Constants";
const customerRouter = [
  {
    path: "/",
    component: () => import("../layouts/_layoutCustomer.vue"),
    children: [
      {
        path: "",
        name: "CustomerHome",
        component: () => import("../views/Customer/Home/index"),
        meta: { title: "Trang chủ", }
      },
      {
        path: "/reset-password",
        name: "ResetPassword",
        meta: { title: "Đặt lại mật khẩu", },
        component: () => import("../views/Auth/resetPassword.vue"),
        props: (route) => ({
          token: route.query.token,
          email: route.query.email,
        }),
      },
      {
        path: "/confirm-email",
        name: "ConfirmEmail",
        component: () => import("../views/Auth/ConfirmEmail.vue"),
        meta: {  title: "Xác thực tài khoản", },
        props: (route) => ({
          token: route.query.token,
          email: route.query.email,
        }),
      },
      { 
        path: "/Login",
        name: "Login",
        component: ()=> import("../views/Auth/login.vue"),
        meta: { title: "Đăng nhập", },
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
    ],
  },
];
export default customerRouter;
