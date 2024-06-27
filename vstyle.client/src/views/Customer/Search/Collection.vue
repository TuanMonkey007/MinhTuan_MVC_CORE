<template>
    <a-row>
        <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <a-page-header style="
         border: 1px solid rgb(235, 237, 240);
          background-color: #c22026;
        ">
                <template #title>
                    <a-breadcrumb separator=">" style="margin-left:30px;">
                        <a-breadcrumb-item href="">

                            <span style="color: #fff;"> Trang chủ</span>
                        </a-breadcrumb-item>

                        <a-breadcrumb-item >

                            <span style="color: #fff;"> Bộ sưu tập</span>
                        </a-breadcrumb-item>
                        <a-breadcrumb-item class="active">

<span style="color: #fff;"> {{ this.nameCollection }}</span>
</a-breadcrumb-item>

                    </a-breadcrumb>
                </template>

            </a-page-header>
        </a-col>
    </a-row>



    <!-- Product List -->
    <a-row :gutter="[16, 16]" justify="center" style="width:100%">
        <a-col :xs="22" :sm="22" :md="22" :lg="20" :xl="20">
            <h1 style="text-transform: uppercase; font-size: 24px;font-weight: 500;justify-content: center;display: flex">{{ this.nameCollection }}</h1>
            </a-col>    
    </a-row>    
    <a-row :gutter="[16, 16]" justify="center" style="width:100%">
        <a-col :xs="22" :sm="22" :md="22" :lg="20" :xl="20">
            <a-row justify="start" style="margin-top: 20px;" :gutter="[16, 16]">

                <a-col :xs="10" :sm="10" :md="10" :lg="4" :xl="4" style="justify-content: center;"
                    v-for="(item, index) in bestSellerImages" :key="index">
                    <router-link :to="{ name: 'ProductDetail', params: { code: item.code, id: item.id } }">

                        <a-card hoverable style="width: 100%;" :bordered="false">
                            <template #cover>
                                <div style="width: 100%;">
                                    <img alt="example" class="image-product"
                                        :src="'data:' + item.thumbnailContentType + ';base64,' + item.thumbnailBase64" />
                                </div>
                            </template>

                            <a-card-meta>
                                <template #title>
                                    <span style="font-size: 16px; align-items: center">{{ item.name }}</span>
                                </template>
                                <template #description>
                                    <span style="font-size: 16px; align-items: center">{{ item.code }}</span>
                                    <br>
                                    <span class="price-product">{{ item.price }}&#8363;</span>
                                </template>

                            </a-card-meta>
                        </a-card>
                    </router-link>

                </a-col>
            </a-row>
            <a-row>
                <a-col :span="24">
                    <a-pagination v-if="pagination.total > 0" style="margin-top: 20px; text-align: center"
                        v-model:current="pagination.current" :pageSize="pagination.pageSize" :total="pagination.total"
                        :showSizeChanger="false" :show-total="(total) => `Tổng ${total} sản phẩm`"
                        @change="handlePaginationChange" />
                </a-col>
                <a-col v-for="i in 6" :key="i" :xs="10" :sm="10" :md="10" :lg="4" :xl="4"
                    style="justify-content: center;">
                    <a-skeleton v-if="isLoading" :loading="isLoading" active :paragraph="{ rows: 4 }" />
                </a-col>
            </a-row>
        </a-col>
    </a-row>

    <a-row v-if="isNotFound">
        <a-col :span="24">
            <a-result status="404" title="Không tìm thấy" sub-title="Bộ sưu tập này hiện chưa có sản phẩm nào">
            </a-result>
        </a-col>
    </a-row>

    <!-- End Product List -->
</template>
<script>
    import APIService from "@/helpers/APIService";
    import { inject, watchEffect, ref } from "vue";
    export default {
        watch: {
            '$route'() {
                this.fetchData();
            },

        },
        data() {


            return {
                nameCollection: '',
                isNotFound: false,
                isLoading: true,
                bestSellerImages: [],

                pagination: {
                    current: 1,
                    pageSize: 30,
                    total: 0,
                },

            }
        },//end data
        props: {
    imagePaths: Array
  },
        async mounted() {
            try{
                const res = await APIService.get("datacategory/get-list-by-parent-code/BO_SUU_TAP")
                this.nameCollection = res.data.data.find(item => item.code == this.$route.params.collectionCode).name
            }catch{
                console.error('Error fetching ');
            }
            
            await this.fetchData();
           
        },//end mounted
        methods: {
            handlePaginationChange() {
                this.fetchData();
            },
            async fetchData() {
                try {
                    
                    const res = await APIService.post("product/get-data-by-page", {
                        pageIndex: this.pagination.current,
                        pageSize: this.pagination.pageSize,
                        categoryCode_Filter: this.$route.params.collectionCode
                    })

                    this.bestSellerImages = res.data.data.items;
                    this.pagination.total = res.data.data.totalCount
                    if(this.bestSellerImages.length == 0){
                        this.isNotFound = true
                       
                    }
                    this.isLoading = false
                    



                } catch (error) {
                    console.error('Error fetching banner images:', error);
                }
            }

        }
    }
</script>
<style scoped>
    .ant-page-header {
        padding: 0px;
        /* Giảm padding */
        margin-bottom: 8px;
        /* Giảm margin dưới */
        height: 35px;
        padding-left: 5%;
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

    :deep(.ant-steps-item-active .ant-steps-item-icon) {
        background-color: #c12227 !important;
        color: #fff;
        border: 1px solid #c12227;
        border-radius: 20px;
        width: fit-content;

    }

    :deep(.ant-steps-item-icon) {
        background-color: #807878 !important;
        color: #fff;
        border: 1px solid #807878;
        border-radius: 20px;
        width: fit-content;
    }

    :deep(.ant-steps-item-title::after) {

        height: 5px !important;
    }

    :deep(.ant-steps-item-finish .ant-steps-item-title::after) {

        background-color: #c12227 !important;
    }

    :deep(.ant-input-number-group-addon) {
        border-top-right-radius: 20px;
        border-bottom-right-radius: 20px;
        border-top-left-radius: 20px;
        border-bottom-left-radius: 20px;
    }

</style>