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
                meta: { requiresAuth: true, roles: [Role.ADMIN, Role.STAFF] } //Thêm meta để kiểm tra quyền truy cập
            },
            {
                path:"Category",
                name:"CategoryHome",
                component:()=>import("../views/Administrator/Categories/index.vue"),
                meta: { requiresAuth: true, roles: [Role.ADMIN, Role.STAFF], title: "Quản lý danh mục" }
            },
            {
                path:"DataCategory/:id?",
                name:"DataCategoryHome",
                component:()=>import("../views/Administrator/DataCategories/index.vue"),
                meta: { requiresAuth: true, roles: [Role.ADMIN, Role.STAFF], title: "Quản lý dữ liệu danh mục", }
            },
            {
                path:"Account",
                name:"AccountHome",
                component:()=>import("../views/Administrator/Users/index.vue"),
                meta: { 
                    // requiresAuth: true,
                    // roles: [Role.ADMIN, Role.STAFF],
                    title: "Quản lý tài khoản"


                }
            },
           
        ]
    }
    
]
export default adminRouter;