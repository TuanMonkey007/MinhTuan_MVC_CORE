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

                        <a-breadcrumb-item class="active">

                            <span style="color: #fff;">{{ this.gender }}</span>
                        </a-breadcrumb-item>

                    </a-breadcrumb>
                </template>

            </a-page-header>
        </a-col>
    </a-row>

    <!-- Filter -->
    <a-row justify="center">
        <a-col :xs="20" :sm="20" :md="20" :lg="20" :xl="20">
            <a-row justify="space-between" style="align-items: center;">
                <a-col :span="20">
                    <a-row :gutter="[16, 16]" justify="start" style="width:100%">
                <a-col :xs="12" :sm="12" :md="12" :lg="3" :xl="3">
                    <a-select v-model:value="selectedCategory" style="width: 100%" :options="categoryOptions"
                        placeholder="Danh mục" mode="multiple" :max-tag-count="1" @blur="handleChangeFilter">

                    </a-select>
                </a-col>
                <a-col :xs="12" :sm="12" :md="12" :lg="3" :xl="3">
                    <a-select v-model:value="selectedColor" style="width: 100%" :options="colorOptions"
                        placeholder="Màu sắc" mode="multiple" :max-tag-count="1" @blur="handleChangeFilter">

                    </a-select>
                </a-col>
                <a-col :xs="12" :sm="12" :md="12" :lg="3" :xl="3">
                    <a-select v-model:value="selectedSize" style="width: 100%" :options="sizeOptions"
                        placeholder="Kích cỡ" mode="multiple" :max-tag-count="1" @blur="handleChangeFilter">

                    </a-select>
                </a-col>
                <a-col :xs="12" :sm="12" :md="12" :lg="3" :xl="3">
                    <a-select v-model:value="selectedPriceRange" style="width: 100%" :options="priceRangeOptions"
                        placeholder="Khoảng giá" @change="handleChangeFilter">

                    </a-select>
                </a-col>
                <a-col :xs="12" :sm="12" :md="12" :lg="3" :xl="3">
                    <a-select v-model:value="selectedSortType" style="width: 100%" :options="sortTypeOptions"
                        placeholder="Sắp xếp" @change="handleChangeFilter">

                    </a-select>
                </a-col>

            </a-row>
                </a-col>
                <a-col :span="3" style="color:grey;font-size: 16px;align-items: center">{{ this.pagination.total }} sản phẩm</a-col>
            </a-row>
            
        </a-col>
        
    </a-row>
    <!-- End Filter -->

    <!-- Product List -->
   
    <a-row :gutter="[16, 16]" justify="center" style="width:100%;">
        <a-col :xs="22" :sm="22" :md="22" :lg="20" :xl="20">
            <a-row justify="start" style="margin-top: 20px" :gutter="[16, 16]">
                 
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
                                    <span class="price-product">{{fomartPrice(item.price)}}&#8363;</span>
                                </template>

                            </a-card-meta>
                        </a-card>
                    </router-link>

                </a-col>
            </a-row>
            <a-row  >
                <a-col :span="24">
                    <a-pagination v-if="pagination.total > 0" style="margin-top: 20px; text-align: center"
                        v-model:current="pagination.current" :pageSize="pagination.pageSize" :total="pagination.total"
                        :showSizeChanger="false" :show-total="(total) => `Tổng ${total} sản phẩm`"
                        @change="handlePaginationChange" />
                </a-col>
                <a-col v-for="i in 6"  :key="i" :xs="10" :sm="10" :md="10" :lg="4" :xl="4" style="justify-content: center;">
                    <a-skeleton v-if="isLoading" :loading="isLoading" active :paragraph="{ rows: 4 }" />
                 </a-col>
            </a-row>
        </a-col>
    </a-row>
    
    <a-row v-if="isNotFound">
        <a-col :span="24">
            <a-result status="404" title="Không tìm thấy" sub-title="Vui lòng chọn lại bộ lọc sản phẩm">
            </a-result>
        </a-col>
    </a-row>

    <!-- End Product List -->
</template>
<script>
    import APIService from "@/helpers/APIService";
    export default {
        data() {
            const categoryOptions = []
            return {
                isNotFound: false,
                isLoading: true,
                gender: "",
                categoryOptions,
                selectedCategory: [],
                colorOptions: [],
                selectedColor: [],
                sizeOptions: [],
                selectedSize: [],
                priceRangeOptions: [
                    {
                        value: null,
                        label: 'Tất cả',
                    },
                    {
                        value: '0 - 100000',  // Sử dụng chuỗi để biểu diễn khoảng giá
                        label: 'Nhỏ hơn 100.000đ',
                    },
                    {
                        value: '100000-200000',
                        label: '100.000đ - 200.000đ',
                    },
                    {
                        value: '200000-350000',
                        label: '200.000đ - 350.000đ',
                    },
                    {
                        value: '350000-500000',
                        label: '350.000đ - 500.000đ',
                    },
                    {
                        value: '500000-100000000',
                        label: 'Lớn hơn 500.000đ',
                    },
                ],

                selectedPriceRange: null,
                sortTypeOptions: [
                    {
                        value: 'createdDate desc',
                        label: 'Hàng mới nhất',

                    },
                    {
                        value: 'createdDate asc',
                        label: 'Hàng cũ nhất',
                    },
                    {
                        value: 'price asc',
                        label: 'Giá tăng dần',
                    },
                    {
                        value: 'price desc',
                        label: 'Giá giảm dần',
                    },
                ],
                selectedSortType: null,
                bestSellerImages: [],
                defaultGender: null,
                pagination: {
                    current: 1,
                    pageSize: 30,
                    total: 0,
                },
            }
        },//end data
        watch: {
            '$route.params.gender'(newGender) {
                // this.gender = newGender;
                // Thực hiện lại logic tìm kiếm sản phẩm với this.gender
                window.location.reload();
            }
        },
        async mounted() {

            if (this.$route.params.gender == "Nam") {
                this.gender = "NAM"
                await this.fetchCategoryNam();
                const resDefault = await APIService.get(`datacategory/get-id-by-code-and-parent-code/DO_NAM/SAN_PHAM`)
                this.defaultGender = resDefault.data.data
            }
            else {
                this.gender = "NỮ"
                await this.fetchCategoryNu();
                const resDefault = await APIService.get(`datacategory/get-id-by-code-and-parent-code/DO_NU/SAN_PHAM`)
                this.defaultGender = resDefault.data.data
            }
            try {
                const response = await APIService.post('product/get-data-by-page', {
                    pageIndex: 1,
                    pageSize:30,
                    category_Filter: [this.defaultGender],
                    isDisplay_Filter: true
                });
                this.bestSellerImages = response.data.data.items;
                this.isLoading = false;
                this.pagination.total = response.data.data.totalCount;
                this.pagination.current = response.data.data.pageIndex;
                this.pagination.pageSize = response.data.data.pageSize;
                if (this.bestSellerImages.length == 0) {
                    this.isNotFound = true
                }

            } catch (error) {
                console.error('Error fetching banner images:', error);
            }

            await this.fetchColorAndSize();

        },//end mounted
        methods: {
            
            fomartPrice(price) {
                return price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.');
            },
            handlePaginationChange(){
                this.handleChangeFilter()
            },
            async fetchColorAndSize() {
                const serverResponse = await APIService.get(`datacategory/get-list-by-parent-code/MAU_SAC`)
                this.colorOptions = serverResponse.data.data.map((item) => {
                    return {
                        label: item.name,
                        value: item.id
                    }
                })
                const serverResponse2 = await APIService.get(`datacategory/get-list-by-parent-code/KICH_CO`)
                this.sizeOptions = serverResponse2.data.data.map((item) => {
                    return {
                        label: item.name,
                        value: item.id
                    }
                })
            },
            async handleChangeFilter() {
                try {

                    this.isLoading = true;
                    const payload = {
                        pageIndex: this.pagination.current,
                        pageSize: this.pagination.pageSize,
                        sortQuery: this.selectedSortType,
                        size_Filter: this.selectedSize,
                        color_Filter: this.selectedColor,
                        isDisplay_Filter: true
                    }
                    if (this.selectedCategory.length == 0) {
                        payload.category_Filter = [this.defaultGender]
                    } else {
                        payload.category_Filter = this.selectedCategory
                    }
                    
                    var priceRange = []
                    if (this.selectedPriceRange != null) {
                        priceRange = this.selectedPriceRange.split('-').map(Number);
                        payload.price_Filter = priceRange
                    }

                    const response = await APIService.post('product/get-data-by-page', payload);
                    this.bestSellerImages = response.data.data.items;
                    this.pagination.total = response.data.data.totalCount;
                    this.pagination.current = response.data.data.pageIndex;
                    this.pagination.pageSize = response.data.data.pageSize;
                    if (this.bestSellerImages.length == 0) {
                        this.isNotFound = true
                    }else{
                        this.isNotFound = false
                    }
                    this.isLoading = false;


                } catch (error) {
                    console.error('Error fetching banner images:', error);
                }

            },
            async fetchCategoryNam() {
                try {
                    const response = await APIService.get(`datacategory/get-id-by-code-and-parent-code/AO_NAM/SAN_PHAM`);
                    this.categoryOptions.push({
                        value: response.data.data,
                        label: "Áo"
                    })
                    const response2 = await APIService.get(`datacategory/get-id-by-code-and-parent-code/QUAN_NAM/SAN_PHAM`);
                    this.categoryOptions.push({
                        value: response2.data.data,
                        label: "Quần"
                    })
                } catch (error) {
                    console.error('Error fetching banner images:', error);
                }
            },
            async fetchCategoryNu() {
                try {
                    Promise.all([
                        APIService.get(`datacategory/get-id-by-code-and-parent-code/AO_NU/SAN_PHAM`),
                        APIService.get(`datacategory/get-id-by-code-and-parent-code/AO_DAI/SAN_PHAM`),
                        APIService.get(`datacategory/get-id-by-code-and-parent-code/CHAN_VAY/SAN_PHAM`),
                        APIService.get(`datacategory/get-id-by-code-and-parent-code/DAM/SAN_PHAM`),
                        APIService.get(`datacategory/get-id-by-code-and-parent-code/QUAN_NU/SAN_PHAM`),
                        APIService.get(`datacategory/get-id-by-code-and-parent-code/UNDERWEAR/SAN_PHAM`),

                    ]).then(([response, response2, response3, response4, response5, response6]) => {
                        this.categoryOptions.push({
                            value: response.data.data,
                            label: "Áo"
                        });
                        this.categoryOptions.push({
                            value: response2.data.data,
                            label: "Áo dài"
                        });
                        this.categoryOptions.push({
                            value: response3.data.data,
                            label: "Chân váy"
                        });
                        this.categoryOptions.push({
                            value: response4.data.data,
                            label: "Đầm"
                        });
                        this.categoryOptions.push({
                            value: response5.data.data,
                            label: "Quần"
                        });
                        this.categoryOptions.push({
                            value: response6.data.data,
                            label: "Underwear"
                        });
                    });

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