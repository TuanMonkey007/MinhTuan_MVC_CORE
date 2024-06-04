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

                            <span style="color: #fff;"> {{ this.productInfo.name }}</span>
                        </a-breadcrumb-item>

                    </a-breadcrumb>
                </template>

            </a-page-header>
        </a-col>
    </a-row>
    <a-row style="margin-top: 30px">
        <a-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12" :xxl="12">
            <a-carousel arrows :autoplay="true" :slidesToShow="1" style="height: 80%;">

                <div v-for="image in mainImages" :key="image.id"
                >
                    <div  style="width: 556px;">
                        <img :src="'data:' + image.contentType + ';base64,' + image.base64" :alt="image.path"
                        style="border-radius: 5%;height: 100%;max-height: 80%;" />
                    </div>
                   

                </div>

                <!-- <template #customPaging="props">
                    <img :src="'data:' + mainImages[props.i].contentType + ';base64,' + mainImages[props.i].base64"
                        style="width: 50px;" />
                </template> -->
                <template #prevArrow>
                    <div class="custom-slick-arrow" style="left: 10px; z-index: 1">
                        <left-circle-outlined />
                    </div>
                </template>
                <template #nextArrow>
                    <div class="custom-slick-arrow" style="right: 10px">
                        <right-circle-outlined />
                    </div>
                </template>


            </a-carousel>
        </a-col>
        <a-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12" :xxl="12">
            <a-row>
                <a-col :span="24">
                    <h3 style="font-size: 24px; font-weight: 600;">{{ this.productInfo.name }}</h3>
                    <h4
                        style="font-size: 14px;font-weight: 400;line-height: 22px;letter-spacing: 0;text-align: left;color: #807878;">
                        Mã sản phẩm: {{ this.productInfo.code }}</h4>
                        <p class="price-product" style="font-size: 24px; font-weight: 600; line-height: 36px;"> {{ this.productPriceChange }}&#8363;</p>
             
                </a-col>
            
                <a-col :span="24">
                    <span>Chọn màu: <span style="font-weight: 500;"> {{ getSelectedColorLabel() }}</span></span>
                    <br>
                    <a-radio-group v-model:value="selectedColor" :options="productColorOptions" button-style="solid"
                        optionType="button" @change="handleColorChange">
                    </a-radio-group>
                </a-col>
                <a-col :span="24">
                    <span>Chọn size: <span style="font-weight: 500;"> {{ getSelectedSizeLabel() }} </span></span>
                    <br>
                    <a-radio-group v-model:value="selectedSize" :options="productSizeOptions" button-style="solid"
                        optionType="button" @change="handleSizeChange">
                    </a-radio-group>
                    <br>
                    <a-button @click="handleResetSizeColor">Chọn lại</a-button>



                </a-col>
            </a-row>
            <a-row>
                <a-col :span="24">
                    <span>Số lượng: <span style="font-weight: 500;"> {{ this.quantity }} </span></span>
                </a-col>
            </a-row>
            <a-row justify="flex-start" style="margin-top: 20px;">
                <a-col :xs="24" :sm="24" :md="7" :lg="7" :xl="7">


                    <a-input-number v-model:value="quantity" min="1" max="10" size="small" :controls="false"
                        class="inputQuantity">
                        <template #addonBefore>
                            <a-button size="small" type="text" @click="decreaseQuantity">-</a-button>
                        </template>
                        <template #addonAfter>
                            <a-button size="small" type="text" @click="increaseQuantity">+</a-button>
                        </template>
                    </a-input-number>
                </a-col>
                <a-col :xs="10" :sm="10" :md="4" :lg="4" :xl="4">
                    <a-button @click="handleAddToCart" style="width: 120px;
    background-color: #fff;
    border: 1px solid #b11d1b;
    box-shadow: none;
    border-radius: 15px;
    display: inline-block;
    box-sizing: border-box;
    min-width: 100px;
    position: relative;
    overflow: visible;
    transition: .5s linear;
    ">
                        <font-awesome-icon icon="fa-solid fa-cart-plus" style="color: #ad2b1f;" />
                    </a-button>
                </a-col>
                <a-col :xs="10" :sm="10" :md="7" :lg="7" :xl="7">
                    <a-button type="primary" @click="handleBuyNow" class="btn-buy-now">Mua ngay</a-button>
                </a-col>
            </a-row>



        </a-col>
    </a-row>
    <a-row justify="center" style="margin-top: 20px"> 
        <a-col :xs="24" :sm="24" :md="24" :lg="16" :xl="16" style="display: contents">
            <h3 style="font-size: 24px; font-weight: 600; align-items: center; justify-content: center;">Thông tin sản phẩm</h3>
            <p style="font-size: 16px; font-weight: 400; line-height: 24px;">{{ this.productInfo.description }}</p>
        </a-col>
    </a-row>

</template>
<script>
    import { defineComponent, onMounted, ref, reactive } from 'vue';
    import { useRoute } from 'vue-router';
    import APIService from '@/helpers/APIService';
    import { LeftCircleOutlined, RightCircleOutlined } from '@ant-design/icons-vue';
    import { message, notification } from 'ant-design-vue';
    export default defineComponent({
        components: {
            LeftCircleOutlined,
            RightCircleOutlined,
        },

        data() {
            return {
                id: null,
                mainImages: [],
                productVariant: [],
                availableColors: [], // Mảng lưu trữ các màu khả dụng
                availableSizes: [], // Mảng lưu trữ các size khả dụng
                selectedColor: '',
                selectedSize: '',
                productColorOptions: [],
                productSizeOptions: [],
                quantity: 1,
                productInfo: {},
                productPriceChange: 0,
            };
        },
        async mounted() {
            this.id = this.$route.params.id;
            this.fetchProductImage();
            this.fetchProductVariant();
            this.fetchProductColor();
            this.fetchProductSize();
            this.fetchProductInfo();
        },
        methods: {
            async fetchProductInfo() {
                try {
                    const response = await APIService.get(`product/${this.id}`);
                    this.productInfo = response.data.data.items[0];
                    this.productPriceChange = this.productInfo.price;

                } catch (error) {
                    console.error('Error fetching product info:', error);
                }
            },
            async fetchProductImage() {
                try {

                    const response = await APIService.get(`product/get-all-product-image-by-id/${this.id}`);
                    this.mainImages = response.data.data;

                } catch (error) {
                    console.error('Error fetching banner images:', error);
                }
            },
            async fetchProductVariant() {
                try {
                    const response = await APIService.get(`product/get-product-variant/${this.id}`);
                    this.productVariant = response.data.data;
                } catch (error) {
                    console.error('Error fetching product variant:', error);
                }
            },
            async fetchProductColor() {
                try {
                    const response = await APIService.get(`product/get-all-color-of-product-by-id/${this.id}`);

                    this.productColorOptions = response.data.data.map(color => ({
                        label: color.name,
                        value: color.id,
                    }));
                } catch (error) {
                    console.error('Error fetching product color:', error);
                }
            },
            async fetchProductSize() {
                try {
                    const response = await APIService.get(`product/get-all-size-of-product-by-id/${this.id}`);

                    this.productSizeOptions = response.data.data.map(size => ({
                        label: size.name,
                        value: size.id,
                    }));

                } catch (error) {
                    console.error('Error fetching product size:', error);
                }
            },//end fetchProductSize
            handleColorChange(colorId) {
                this.selectedColor = colorId.target.value;
                this.updateAvailableSizes();
            },

            handleSizeChange(sizeId) {
                this.selectedSize = sizeId.target.value;
                this.updateAvailableColors();
            },

            updateAvailableSizes() {
                const availableSizes = this.productVariant
                    .filter(variant => variant.colorId === this.selectedColor && variant.stockQuantity > 0)
                    .map(variant => variant.sizeId);

                this.productSizeOptions = this.productSizeOptions.map(sizeOption => ({
                    ...sizeOption,
                    disabled: !availableSizes.includes(sizeOption.value),
                }));
                const variant = this.productVariant.find(
                    v => v.colorId === this.selectedColor && v.sizeId === this.selectedSize
                );

                // Cập nhật productPriceChange nếu tìm thấy variant
                if (variant) {
                    this.productPriceChange = variant.price;
                } else {
                    this.productPriceChange = this.productInfo.price; // Hoặc giá trị mặc định khác
                }
            },

            updateAvailableColors() {
                const availableColors = this.productVariant
                    .filter(variant => variant.sizeId === this.selectedSize && variant.stockQuantity > 0)
                    .map(variant => variant.colorId);

                this.productColorOptions = this.productColorOptions.map(colorOption => ({
                    ...colorOption,
                    disabled: !availableColors.includes(colorOption.value),
                }));
                const variant = this.productVariant.find(
                    v => v.colorId === this.selectedColor && v.sizeId === this.selectedSize
                );

                // Cập nhật productPriceChange nếu tìm thấy variant
                if (variant) {
                    this.productPriceChange = variant.price;
                } else {
                    this.productPriceChange = this.productInfo.price; // Hoặc giá trị mặc định khác
                }
            },
            handleResetSizeColor() {
                this.selectedColor = '';
                this.selectedSize = '';
                this.productSizeOptions = this.productSizeOptions.map(sizeOption => ({
                    ...sizeOption,
                    disabled: false,
                }));
                this.productColorOptions = this.productColorOptions.map(colorOption => ({
                    ...colorOption,
                    disabled: false,
                }));
                this.productPriceChange = this.productInfo.price;
            },
            getSelectedColorLabel() {
                const selectedOption = this.productColorOptions.find(option => option.value === this.selectedColor);
                return selectedOption ? selectedOption.label : ''; // Nếu không tìm thấy option, trả về chuỗi rỗng
            },
            getSelectedSizeLabel() {
                const selectedOption = this.productSizeOptions.find(option => option.value === this.selectedSize);
                return selectedOption ? selectedOption.label : ''; // Nếu không tìm thấy option, trả về chuỗi rỗng
            },
            decreaseQuantity() {
                if (this.quantity > 1) {
                    this.quantity--;
                }
            },
            increaseQuantity() {
                if (this.quantity < 10) {
                    this.quantity++;
                }
            },
           async handleAddToCart() {
                if (!this.selectedColor || !this.selectedSize) {
                    this.$message.error('Vui lòng chọn màu và size trước khi thêm vào giỏ hàng');
                    return;
                }
                const variant = this.productVariant.find(
                    v => v.colorId === this.selectedColor && v.sizeId === this.selectedSize
                );
                if (!variant) {
                    this.$message.error('Không tìm thấy sản phẩm phù hợp');
                    return;
                }
                const  response = await APIService.post('cart/add-to-cart', {
                    productVariantId: variant.id,
                    cartId : localStorage.getItem('cartId'),
                    quantity: this.quantity,
                });
                localStorage.setItem('cartId', response.data.message);
                
            },
                
                


        },//end methods


    });
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

    /* pagging carousel */
    /* .ant-carousel :deep(.slick-dots) {
  position: absolute;
  height: auto;
} */
    .ant-carousel :deep(.slick-slide img) {
        border: 5px solid #fff;
        display: block;
        margin: auto;
        max-width: 80%;
        max-height: 80%;
    }



    .ant-carousel :deep(.slick-thumb) {
        bottom: 0px;
    }

    .ant-carousel :deep(.slick-thumb li) {
        width: 60px;
        height: 45px;
    }

    .ant-carousel :deep(.slick-thumb li img) {
        width: 100%;
        height: 100%;
        filter: grayscale(100%);
        display: block;
    }


    .ant-carousel :deep(.slick-arrow.custom-slick-arrow) {
        width: 25px;
        height: 25px;
        font-size: 25px;
        color: #eb4a4a;
        background-color: rgba(31, 45, 61, 0.11);
        opacity: 0.3;
        z-index: 1;
    }

    :deep(.ant-input-number-input) {
        width: 50px !important;
    }

    /* Trong file CSS của component hoặc file global */
    /* Đảm bảo bạn sử dụng :deep() để xuyên qua Shadow DOM của a-input-number */

    /* Tùy chỉnh border-radius cho nút đầu tiên và nút cuối cùng */

    :deep(.ant-input-number-group-addon) {
        border-top-right-radius: 20px;
        border-bottom-right-radius: 20px;
        border-top-left-radius: 20px;
        border-bottom-left-radius: 20px;
    }





</style>