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
        meta: {requiresAuth: false, title: "Trang chủ" },
      },
      {
        path: "/search/:gender?",
        name: "ProductList",
        component: () => import("../views/Customer/Search/ProductList.vue"),
        meta: { title: "VSTYLE - Thời trang Việt" }
      },
      {
        path: "/image-search",
        name: "ResultImageSearch",
        component: () => import("../views/Customer/Search/ResultImageSearch.vue"),
        meta: { title: "VSTYLE - Thời trang Việt" },
        props: (route) => ({ imagePaths: route.query.imagePaths }) // Truyền imagePaths dưới dạng query parameter
      },
      {
        path: "/collection/:collectionCode?",
        name: "Collection",
        component: () => import("../views/Customer/Search/Collection.vue"),
        meta: { title: "VSTYLE - Thời trang Việt" },
      },
      
      
      {
        path: "/keyword-search/:keyword?",
        name: "KeywordSearch",
        component: () => import("../views/Customer/Search/ResultKeywordSearch.vue"),
        meta: { title: "VSTYLE - Thời trang Việt" },
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
      {
        path: "/policy/payment-policy",
        name: "PaymentPolicy",
        component: () => import("../views/Customer/Policy/Payment.vue"),
        meta: { requiresAuth: false, title: "Chính sách thanh toán" },
      },
      {
        path: "/policy/warranty-policy",
        name: "WarrantyPolicy",
        component: () => import("../views/Customer/Policy/Warranty.vue"),
        meta: { requiresAuth: false, title: "Chính sách bảo hành và đổi trả" },
      },
      {
        path: "/policy/shipping-policy",
        name: "ShippingPolicy",
        component: () => import("../views/Customer/Policy/Shipping.vue"),
        meta: { requiresAuth: false, title: "Chính sách vận chuyển" },
      },
      {
        path: "/policy/privacy-policy",
        name: "PrivacyPolicy",
        component: () => import("../views/Customer/Policy/Privacy.vue"),
        meta: { requiresAuth: false, title: "Chính sách bảo mật" },
      },
      {
        path: "/policy/buying-policy",
        name: "BuyingPolicy",
        component: () => import("../views/Customer/Policy/Buying.vue"),
        meta: { requiresAuth: false, title: "Chính sách mua hàng" },
      },
      
      {
        path: "/contact",
        name: "Contact",
        component: () => import("../views/Customer/Home/Contact.vue"),
        meta: { requiresAuth: false, title: "Thông tin liên hệ" },
      },
      {
        path: "/about",
        name: "About",
        component: () => import("../views/Customer/Home/About.vue"),
        meta: { requiresAuth: false, title: "Giới thiệu" },
      },
      {
        path: "/blog/:slug?",
        name: "BlogDetail",
        component: () => import("../views/Customer/Article/index.vue"),
        meta: { requiresAuth: false, title: "Tin tức" },
      },
      {
        path: "/blog-category/:categoryCode?",
        name: "BlogCategory",
        component: () => import("../views/Customer/Article/ListArticle.vue"),
        meta: { requiresAuth: false, title: "Tin tức" },
      },

    ],
  },
];
export default customerRouter;
