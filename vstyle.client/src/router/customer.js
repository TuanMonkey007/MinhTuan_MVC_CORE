const customerRouter=[
    { 
        path: "/",
        component: ()=> import("../layouts/_layoutCustomer.vue"),
        children: [
            {
                path: "",
                name: "CustomerHome",
                component: ()=> import("../views/Customer/Home/index")
            },
            { 
                path: "/reset-password", 
                  name: "ResetPassword",
                  component: ()=> import("../views/Auth/resetPassword.vue"),
                  props: (route) => ({ token: route.query.token, email: route.query.email }) 
              },
              { 
                path: "/confirm-email", 
                  name: "ConfirmEmail",
                  component: ()=> import("../views/Auth/ConfirmEmail.vue"),
                  props: (route) => ({ token: route.query.token, email: route.query.email }) 
              },
           
           
        ]
    },
   
]
export default customerRouter;