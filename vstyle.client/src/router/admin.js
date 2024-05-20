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
                meta: { requiresAuth: true, roles: [Role.ADMIN, Role.STAFF] }
            },
           
        ]
    }
    
]
export default adminRouter;