(function(){"use strict";var e={2120:function(e,t,n){n.d(t,{M:function(){return o},X:function(){return r}});const o={Name:"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",Email:"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress",MobilePhone:"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone",Role:"http://schemas.microsoft.com/ws/2008/06/identity/claims/role"},r={ADMIN:"ADMINISTRATOR",STAFF:"STAFF",CUSTOMER:"CUSTOMER"}},522:function(e,t,n){var o=n(5130),r=n(469),a=n(6768);function i(e,t,n,o,r,i){const c=(0,a.g2)("router-view");return(0,a.uX)(),(0,a.Wv)(c)}var c={name:"App",components:{}},u=n(1241);const s=(0,u.A)(c,[["render",i]]);var l=s,m=n(8355),f=n(9679),d=n(7694),h=n(8950),p=n(2353),g=n(92),v=n(4996),b=n(292);window.axios=m.A,h.Yv.add(p.X7I,g.C91,v.Cvc);const y=(0,o.Ef)(l);y.component("font-awesome-icon",b.gc),y.use(f.Ay),y.use(r.A),d.Ay.config({top:"24px",right:"100px",duration:2,maxCount:3}),y.config.globalProperties.$message=d.Ay,y.mount("#app")},469:function(e,t,n){n.d(t,{A:function(){return m}});var o=n(1387),r=n(2120);const a=[{path:"/Administrator",component:()=>n.e(985).then(n.bind(n,1985)),children:[{path:"",name:"AdminHome",component:()=>n.e(696).then(n.bind(n,2696)),meta:{requiresAuth:!0,roles:[r.X.ADMIN,r.X.STAFF]}},{path:"Category",name:"CategoryHome",component:()=>n.e(764).then(n.bind(n,8764)),meta:{requiresAuth:!0,roles:[r.X.ADMIN,r.X.STAFF],title:"Quản lý danh mục"}},{path:"DataCategory/:id?",name:"DataCategoryHome",component:()=>n.e(220).then(n.bind(n,2220)),meta:{requiresAuth:!0,roles:[r.X.ADMIN,r.X.STAFF],title:"Quản lý dữ liệu danh mục"}},{path:"Account",name:"AccountHome",component:()=>n.e(373).then(n.bind(n,7373)),meta:{title:"Quản lý tài khoản"}}]}];var i=a;const c=[{path:"/",component:()=>n.e(752).then(n.bind(n,5468)),children:[{path:"",name:"CustomerHome",component:()=>n.e(375).then(n.bind(n,8375))},{path:"/reset-password",name:"ResetPassword",component:()=>n.e(351).then(n.bind(n,3351)),props:e=>({token:e.query.token,email:e.query.email})},{path:"/confirm-email",name:"ConfirmEmail",component:()=>n.e(888).then(n.bind(n,7094)),props:e=>({token:e.query.token,email:e.query.email})}]}];var u=c;const s=[...i,...u,{path:"/Login",name:"Login",component:()=>n.e(67).then(n.bind(n,3067)),beforeEnter(e,t,n){const o=localStorage.getItem("accessToken");o?localStorage.getItem("role")==r.X.ADMIN||localStorage.getItem("role")==r.X.STAFF?n({name:"AdminHome"}):n({name:"CustomerHome"}):n()}},{path:"/unauthorized",name:"Unauthorized",component:()=>n.e(822).then(n.bind(n,1822))}],l=(0,o.aE)({history:(0,o.LA)(),routes:s});l.beforeEach(((e,t,n)=>{const o=localStorage.getItem("accessToken"),r=localStorage.getItem("role");document.title=e.meta.title||"VStyle - Thời trang Việt",e.matched.some((e=>e.meta.requiresAuth))?o?e.meta.roles&&!e.meta.roles.includes(r)?n({name:"Unauthorized"}):n():n({name:"Login"}):n()}));var m=l}},t={};function n(o){var r=t[o];if(void 0!==r)return r.exports;var a=t[o]={exports:{}};return e[o].call(a.exports,a,a.exports,n),a.exports}n.m=e,function(){var e=[];n.O=function(t,o,r,a){if(!o){var i=1/0;for(l=0;l<e.length;l++){o=e[l][0],r=e[l][1],a=e[l][2];for(var c=!0,u=0;u<o.length;u++)(!1&a||i>=a)&&Object.keys(n.O).every((function(e){return n.O[e](o[u])}))?o.splice(u--,1):(c=!1,a<i&&(i=a));if(c){e.splice(l--,1);var s=r();void 0!==s&&(t=s)}}return t}a=a||0;for(var l=e.length;l>0&&e[l-1][2]>a;l--)e[l]=e[l-1];e[l]=[o,r,a]}}(),function(){n.n=function(e){var t=e&&e.__esModule?function(){return e["default"]}:function(){return e};return n.d(t,{a:t}),t}}(),function(){n.d=function(e,t){for(var o in t)n.o(t,o)&&!n.o(e,o)&&Object.defineProperty(e,o,{enumerable:!0,get:t[o]})}}(),function(){n.f={},n.e=function(e){return Promise.all(Object.keys(n.f).reduce((function(t,o){return n.f[o](e,t),t}),[]))}}(),function(){n.u=function(e){return"js/"+e+"."+{67:"12c42003",220:"a55e41c0",351:"2dc7252b",373:"5c65d126",375:"e343093c",696:"1ffc9a33",752:"b702ecac",764:"5a089ce0",822:"15f62b6f",888:"f50b1201",985:"33c8d695"}[e]+".js"}}(),function(){n.miniCssF=function(e){return"css/"+e+"."+{67:"cf398afa",220:"3a9664e5",351:"281b5ca5",373:"70ed0430",752:"a7353127",764:"0969e2cc"}[e]+".css"}}(),function(){n.g=function(){if("object"===typeof globalThis)return globalThis;try{return this||new Function("return this")()}catch(e){if("object"===typeof window)return window}}()}(),function(){n.o=function(e,t){return Object.prototype.hasOwnProperty.call(e,t)}}(),function(){var e={},t="VStyle:";n.l=function(o,r,a,i){if(e[o])e[o].push(r);else{var c,u;if(void 0!==a)for(var s=document.getElementsByTagName("script"),l=0;l<s.length;l++){var m=s[l];if(m.getAttribute("src")==o||m.getAttribute("data-webpack")==t+a){c=m;break}}c||(u=!0,c=document.createElement("script"),c.charset="utf-8",c.timeout=120,n.nc&&c.setAttribute("nonce",n.nc),c.setAttribute("data-webpack",t+a),c.src=o),e[o]=[r];var f=function(t,n){c.onerror=c.onload=null,clearTimeout(d);var r=e[o];if(delete e[o],c.parentNode&&c.parentNode.removeChild(c),r&&r.forEach((function(e){return e(n)})),t)return t(n)},d=setTimeout(f.bind(null,void 0,{type:"timeout",target:c}),12e4);c.onerror=f.bind(null,c.onerror),c.onload=f.bind(null,c.onload),u&&document.head.appendChild(c)}}}(),function(){n.r=function(e){"undefined"!==typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(e,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(e,"__esModule",{value:!0})}}(),function(){n.p="/MinhTuan_MVC_CORE/"}(),function(){if("undefined"!==typeof document){var e=function(e,t,o,r,a){var i=document.createElement("link");i.rel="stylesheet",i.type="text/css",n.nc&&(i.nonce=n.nc);var c=function(n){if(i.onerror=i.onload=null,"load"===n.type)r();else{var o=n&&n.type,c=n&&n.target&&n.target.href||t,u=new Error("Loading CSS chunk "+e+" failed.\n("+o+": "+c+")");u.name="ChunkLoadError",u.code="CSS_CHUNK_LOAD_FAILED",u.type=o,u.request=c,i.parentNode&&i.parentNode.removeChild(i),a(u)}};return i.onerror=i.onload=c,i.href=t,o?o.parentNode.insertBefore(i,o.nextSibling):document.head.appendChild(i),i},t=function(e,t){for(var n=document.getElementsByTagName("link"),o=0;o<n.length;o++){var r=n[o],a=r.getAttribute("data-href")||r.getAttribute("href");if("stylesheet"===r.rel&&(a===e||a===t))return r}var i=document.getElementsByTagName("style");for(o=0;o<i.length;o++){r=i[o],a=r.getAttribute("data-href");if(a===e||a===t)return r}},o=function(o){return new Promise((function(r,a){var i=n.miniCssF(o),c=n.p+i;if(t(i,c))return r();e(o,c,null,r,a)}))},r={524:0};n.f.miniCss=function(e,t){var n={67:1,220:1,351:1,373:1,752:1,764:1};r[e]?t.push(r[e]):0!==r[e]&&n[e]&&t.push(r[e]=o(e).then((function(){r[e]=0}),(function(t){throw delete r[e],t})))}}}(),function(){var e={524:0};n.f.j=function(t,o){var r=n.o(e,t)?e[t]:void 0;if(0!==r)if(r)o.push(r[2]);else{var a=new Promise((function(n,o){r=e[t]=[n,o]}));o.push(r[2]=a);var i=n.p+n.u(t),c=new Error,u=function(o){if(n.o(e,t)&&(r=e[t],0!==r&&(e[t]=void 0),r)){var a=o&&("load"===o.type?"missing":o.type),i=o&&o.target&&o.target.src;c.message="Loading chunk "+t+" failed.\n("+a+": "+i+")",c.name="ChunkLoadError",c.type=a,c.request=i,r[1](c)}};n.l(i,u,"chunk-"+t,t)}},n.O.j=function(t){return 0===e[t]};var t=function(t,o){var r,a,i=o[0],c=o[1],u=o[2],s=0;if(i.some((function(t){return 0!==e[t]}))){for(r in c)n.o(c,r)&&(n.m[r]=c[r]);if(u)var l=u(n)}for(t&&t(o);s<i.length;s++)a=i[s],n.o(e,a)&&e[a]&&e[a][0](),e[a]=0;return n.O(l)},o=self["webpackChunkVStyle"]=self["webpackChunkVStyle"]||[];o.forEach(t.bind(null,0)),o.push=t.bind(null,o.push.bind(o))}();var o=n.O(void 0,[504],(function(){return n(522)}));o=n.O(o)})();
//# sourceMappingURL=app.be18891f.js.map