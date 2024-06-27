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

                        <a-breadcrumb-item>

                            <span style="color: #fff;"> Chính sách</span>
                        </a-breadcrumb-item>
                        <a-breadcrumb-item class="active">

                            <span style="color: #fff;"> Chính sách bảo mật</span>
                        </a-breadcrumb-item>

                    </a-breadcrumb>
                </template>

            </a-page-header>
        </a-col>
    </a-row>
    <a-row justify="center">
        
        <a-col :xs="22" :sm="22" :md="14" :lg="14" :xl="14">
            <h2 style="display: flex; justify-content: center; justify-self: center">CHÍNH SÁCH BẢO MẬT</h2>
            <div v-html="this.policy.description">

            </div>
        </a-col>
    </a-row>



</template>
<script>
    import APIService from "@/helpers/APIService";
    export default {
        data() {
            return {
                policy: {},

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
                        code_Filter: "CS_BAOMAT"
                    });
                    this.policy = response.data.data.items[0];
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
