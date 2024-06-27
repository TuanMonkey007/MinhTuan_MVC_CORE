<template>
    <a-row>
        <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <a-page-header style="
         border: 1px solid rgb(235, 237, 240);
          background-color: #c22026;
          height: fit-content;
        ">

                <template #title>
                    <a-breadcrumb separator=">" style="margin-left:30px;">
                        <a-breadcrumb-item href="">

                            <span style="color: #fff;"> Trang chủ</span>
                        </a-breadcrumb-item>

                  
                        <a-breadcrumb-item class="active">

                            <span style="color: #fff;"> Giới thiệu các chính sách của cửa hàng</span>
                        </a-breadcrumb-item>

                    </a-breadcrumb>
                </template>

            </a-page-header>
        </a-col>
    </a-row>
    <a-row justify="center">
        
        <a-col :xs="22" :sm="22" :md="14" :lg="14" :xl="14">
            <h2 style="display: flex; justify-content: center; justify-self: center">CHÍNH SÁCH MUA HÀNG</h2>
            <div v-html="this.buying.description">

            </div>
        </a-col>
    </a-row>
    <a-row justify="center">
        
        <a-col :xs="22" :sm="22" :md="14" :lg="14" :xl="14">
            <h2 style="display: flex; justify-content: center; justify-self: center">CHÍNH SÁCH THANH TOÁN</h2>
            <div v-html="this.payment.description">

            </div>
        </a-col>
    </a-row>
    <a-row justify="center">
        
        <a-col :xs="22" :sm="22" :md="14" :lg="14" :xl="14">
            <h2 style="display: flex; justify-content: center; justify-self: center">CHÍNH SÁCH VẬN CHUYỂN</h2>
            <div v-html="this.shipping.description">

            </div>
        </a-col>
    </a-row>
    
    <a-row justify="center">
        
        <a-col :xs="22" :sm="22" :md="14" :lg="14" :xl="14">
            <h2 style="display: flex; justify-content: center; justify-self: center">CHÍNH SÁCH BẢO HÀNH</h2>
            <div v-html="this.warranty.description">

            </div>
        </a-col>
    </a-row>
    <a-row justify="center">
        
        <a-col :xs="22" :sm="22" :md="14" :lg="14" :xl="14">
            <h2 style="display: flex; justify-content: center; justify-self: center">CHÍNH SÁCH BẢO MẬT THÔNG TIN</h2>
            <div v-html="this.privacy.description">

            </div>
        </a-col>
    </a-row>



</template>
<script>
    import APIService from "@/helpers/APIService";
import Warranty from "../Policy/Warranty.vue";
    export default {
        data() {
            return {
               
                buying:{},
                privacy:{},
                payment:{},
                shipping:{},
                warranty:{},

            }
        },
        async mounted() {
            await this.fetchData();
        },

        methods: {
            async fetchData() {
                try {
                    const response = await APIService.post("datacategory/get-data-by-page", {
                        pageIndex: 1,
                        pageSize: 1,
                        code_Filter: "CS_MUAHANG"
                    });
                    this.buying = response.data.data.items[0];
                    const response1 = await APIService.post("datacategory/get-data-by-page", {
                        pageIndex: 1,
                        pageSize: 1,
                        code_Filter: "CS_THANHTOAN"
                    });
                    this.payment = response1.data.data.items[0];
                    const response2 = await APIService.post("datacategory/get-data-by-page", {
                        pageIndex: 1,
                        pageSize: 1,
                        code_Filter: "CS_BAOMAT"
                    });
                    this.privacy = response2.data.data.items[0];
                    const response3 = await APIService.post("datacategory/get-data-by-page", {
                        pageIndex: 1,
                        pageSize: 1,
                        code_Filter: "CS_GIAOHANG"
                    });
                    this.shipping = response3.data.data.items[0];
                    const response4 = await APIService.post("datacategory/get-data-by-page", {
                        pageIndex: 1,
                        pageSize: 1,
                        code_Filter: "CS_BAOHANH"
                    });
                    this.warranty = response4.data.data.items[0];
                } catch (error) {
                    console.error(error);
                }
            },
        }
    }
</script>
<style scoped>

.ant-page-header {
        padding: 0px;
        /* Giảm padding */
        margin-bottom: 8px;
        /* Giảm margin dưới */
    }

    .ant-page-header {
        height: 35px;
        /* Tự động điều chỉnh chiều cao */
    }

    :deep(.ant-breadcrumb-separator) {
        color: #ffffff86;
        /* Đặt màu chữ cho breadcrumb */
    }

    :deep(.ant-breadcrumb-link a) {
        color: #ffffff86;
        /* Đặt màu chữ cho breadcrumb */
    }

    .active {
        color: #e60b0b;
        /* Đặt màu chữ cho breadcrumb */
    }

    .ant-page-header-heading-title {
        line-height: normal;
        /* Đặt màu chữ cho tiêu đề */
        display: flex;
        /* Hiển thị theo chiều ngang */
        color: #ffffff86;
    }

</style>
