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
        meta: { title: "Trang chủ" },
      },
      {
        path: "/search/:gender?",
        name: "ProductList",
        component: () => import("../views/Customer/Search/ProductList.vue"),
        meta: { title: "VSTYLE - Thời trang Việt" }
      },
      {
        path: ":code?&:id?",
        name: "ProductDetail",
        component: () => import("../views/Customer/Home/DetailProduct.vue"),
      },
      {
        path: "/reset-password",
        name: "ResetPassword",
        meta: { title: "Đặt lại mật khẩu" },
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
        meta: { title: "Xác thực tài khoản" },
        props: (route) => ({
          token: route.query.token,
          email: route.query.email,
        }),
      },
      // {
      //   path: "/Login",
      //   name: "Login",
      //   component: () => import("../views/Auth/login.vue"),
      //   meta: { title: "Đăng nhập" },
      //   beforeEnter(to, from, next) {
      //     //Chặn khi đã đăng nhập
      //     const isAuthenticated = localStorage.getItem("accessToken");
      //     if (isAuthenticated) {
      //       //Nếu đã đăng nhập thì không cho chuyển hướng
      //       //stay on the same page
      //       if (
      //         localStorage.getItem("role") == Role.ADMIN ||
      //         localStorage.getItem("role") == Role.STAFF
      //       ) {
      //         next({ name: "AdminHome" });
      //       } else {
      //         next({ name: "CustomerHome" });
      //       }
      //     } else {
      //       next();
      //     }
      //   }, //beforeEnter
      // },
      {
        path: "Profile",
        name: "CustomerProfile",
        component: () => import("../views/Customer/ProfileCustomer.vue"),
        meta: { requiresAuth: true, title: "Trang cá nhân" },
      },
      {
        path: "Tracking-order",
        name: "TrackingOrder",
        component: () => import("../views/Customer/TrackingOrder.vue"),
        meta: { requiresAuth: false, title: "Theo dõi đơn hàng" },
      },
      {
        path: "Tracking-order-detail/:id?",
        name: "TrackingOrderDetail",
        component: () => import("../views/Customer/TrackingOrderDetail.vue"),
        meta: { requiresAuth: false, title: "Chi tiết đơn hàng" },
      },
      {
        path: "Cart",
        name: "Cart",
        component: () => import("../views/Customer/Home/Cart.vue"),
        meta: { requiresAuth: false, title: "Giỏ hàng" },
      },
      {
        path: "Order",
        name: "Order",
        component: () => import("../views/Customer/Home/Order.vue"),
        meta: { requiresAuth: false, title: "Tạo đơn hàng" },
      },
      {
        path: "BuyNow/:productVariantId?/:quantity?/:price?",
        name: "BuyNow",
        component: () => import("../views/Customer/Home/BuyNow.vue"),
        meta: { requiresAuth: false, title: "Tạo đơn hàng" },
      },
      {
        path: "/ordersuccess/:success?/:ordercode?", // Thêm dấu gạch chéo (/) trước "ordersuccess"
        name: "OrderSuccess",
        component: () => import("../views/Customer/Home/OrderSuccess.vue"),
        meta: { requiresAuth: false, title: "HOÀN THÀNH" },
      },
    ],
  },
];
export default customerRouter;
