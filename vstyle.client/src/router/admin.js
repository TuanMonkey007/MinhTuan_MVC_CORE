import { Role } from "@/helpers/Constants";

const adminRouter=[
    
    { 
        path: "/Administrator",
        component: ()=> import("../layouts/_layoutAdmin.vue"),
        children: [
            {
                path:"",
                name:"AdminHome",
                component:()=>import("../views/Administrator/Statistics/index.vue"),
                meta: { requiresAuth: true, roles: [Role.ADMIN, Role.STAFF], title:"Dashboard" } //Thêm meta để kiểm tra quyền truy cập
            },
            {
                path:"Profile",
                name:"AdminProfile",
                component:()=>import("../views/Administrator/Users/Profile.vue"),
                meta: { requiresAuth: true, roles: [Role.ADMIN, Role.STAFF], title: "Trang cá nhân" }
            },
            {
                path:"Product",
                name:"ProductHome",
                component:()=>import("../views/Administrator/Products/index.vue"),
                meta: { requiresAuth: true, roles: [Role.ADMIN, Role.STAFF], title: "Quản lý sản phẩm" }
            },
            {
                path:"Category",
                name:"CategoryHome",
                component:()=>import("../views/Administrator/Categories/index.vue"),
                meta: { requiresAuth: true, roles: [Role.ADMIN, Role.STAFF], title: "Quản lý danh mục" }
            },
            {
                path:"Order",
                name:"OrderHome",
                component:()=>import("../views/Administrator/Orders/index.vue"),
                meta: { requiresAuth: true, roles: [Role.ADMIN, Role.STAFF], title: "Quản lý đơn hàng" }
            },
            {
                path:"Voucher",
                name:"VoucherHome",
                component:()=>import("../views/Administrator/Vouchers/index.vue"),
                meta: { requiresAuth: true, roles: [Role.ADMIN, Role.STAFF], title: "Quản lý phiếu giảm giá" }
            },
            {
                path:"Article",
                name:"ArticleHome",
                component:()=>import("../views/Administrator/Articles/index.vue"),
                meta: { requiresAuth: true, roles: [Role.ADMIN, Role.STAFF], title: "Quản lý bài viết" },
              
            },
            {
                path:"Article/Create",
                name:"ArticleCreate",
                component:()=>import("../views/Administrator/Articles/Create.vue"),
                meta: { requiresAuth: true, roles: [Role.ADMIN, Role.STAFF], title: "Tạo bài viết mới", }
            },
            {
                path:"Article/Update/:id?",
                name:"ArticleUpdate",
                component:()=>import("../views/Administrator/Articles/Update.vue"),
                meta: { requiresAuth: true, roles: [Role.ADMIN, Role.STAFF], title: "Sửa bài viết", }
            },
            {
                path:"DataCategory/:id?",
                name:"DataCategoryHome",
                component:()=>import("../views/Administrator/DataCategories/index.vue"),
                meta: { requiresAuth: true, roles: [Role.ADMIN, Role.STAFF], title: "Quản lý dữ liệu danh mục", }
            },
            {
                path:"DetailOrder/:id?",
                name:"DetailOrder",
                component:()=>import("../views/Administrator/Orders/DetailOrder.vue"),
                meta: { requiresAuth: true, roles: [Role.ADMIN, Role.STAFF], title: "Chi tiết đơn hàng", }
            },
            {
                path:"Account",
                name:"AccountHome",
                component:()=>import("../views/Administrator/Users/index.vue"),
                meta: { 
                    requiresAuth: true,
                    roles: [Role.ADMIN],
                    title: "Quản lý tài khoản"
                }
            },
            {
                path:"Banner",
                name:"BannerHome",
                component:()=>import("../views/Administrator/Banners/index.vue"),
                meta: { requiresAuth: true, roles: [Role.ADMIN, Role.STAFF], title: "Quản lý banner" }
            },
            {
                path:"PaymentInfo",
                name:"PaymentInfoHome",
                component:()=>import("../views/Administrator/PaymentInfos/index.vue"),
                meta: { requiresAuth: true, roles: [Role.ADMIN, Role.STAFF], title: "Quản lý thông tin thanh toán" }
            },
           
        ]
    }
    
]
export default adminRouter;