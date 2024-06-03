<template>
    <!-- Slider -->
    <a-row>
        <a-col :span="24">
            <a-carousel arrows :autoplay="true">
                <div v-for="image in images" :key="image.id" style="height: 450px; width: 100%;">
                    <img :src="'data:' + image.bannerContentType + ';base64,' + image.bannerBase64" alt="Banner Image"
                        style="max-width: 100%; max-height: 100%" />
                </div>
            </a-carousel>
        </a-col>
    </a-row>
    <a-row justify="center" style="margin-top: 20px;">
        <a-col :xs="20" :sm="20" :md="16" :lg="12" :xl="12">
            <a-carousel arrows :autoplay="true" dots-class="slick-dots slick-thumb" :slidesToShow="3">
                <template #customPaging>

                </template>

                <div v-for="(category, index) in Carouselcategories" :key="index"
                    style="height: fit-content; display: grid;">
                    <a-row justify="center">
                        <a-col :span="24" style="display: flex;align-items: center;justify-content: center;">
                            <a-avatar :src="category.imageUrl" :alt="category.name"
                                style="height: 70px ;width: 70px;; object-fit: contain; background-color:#fff0f0 ;">
                            </a-avatar>

                        </a-col>
                        <a-col :span="24" style="display: flex;align-items: center;justify-content: center;">
                            <span style="font-size: 16px; align-items: center">{{ category.name }}</span>
                        </a-col>
                    </a-row>
                </div>
            </a-carousel>
        </a-col>
    </a-row>
    <!-- end Slider -->
    <a-row style="margin-top: 50px">
        <a-col :span="24" style="display: flex; justify-content: center;">
            <h1>BEST SELLER</h1>
        </a-col>
        <a-col :xs="24" :sm="24" :md="24" :lg="10" :xl="10" style="padding-right:20px; ">
            <div v-for="image in bestSellerBanner" :key="image.id" style="margin: 20px; width: 100%;">
                <img :src="'data:' + image.bannerContentType + ';base64,' + image.bannerBase64" alt="Banner Image"
                    style="max-width: 100%; max-height: 100%;border-radius: 5%" />
            </div>

        </a-col>
        <a-col :xs="24" :sm="24" :md="24" :lg="14" :xl="14" >
            <a-carousel arrows  dots-class="slick-dots slick-thumb" :slidesToShow="3">
                <template #customPaging>

                </template>

                <div v-for="(item, index) in bestSellerImages" :key="index"
                    style="height: fit-content; display: grid;">
                    <a-card hoverable style="width: 180px;margin: 20px;" :bordered="false">
                        <template #cover>
                            <div style="width: 100%;">
                            <img alt="example" class="image-product"
                            :src="'data:' + item.thumbnailContentType + ';base64,' + item.thumbnailBase64"/>
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
                </div>
            </a-carousel>

        </a-col>
    </a-row>
</template>

<script>
    import { LeftCircleOutlined, RightCircleOutlined } from '@ant-design/icons-vue';
    import APIService from '@/helpers/APIService';
    import { ref, onMounted } from 'vue';

    export default {
        components: {
            LeftCircleOutlined,
            RightCircleOutlined,
        },
        setup() {
            const images = ref([]);
            const bestSellerBanner = ref([]);
            const bestSellerImages = ref([]);
            const Carouselcategories = ref([
                { name: 'ÁO', imageUrl: require('@/assets/image/slick/ao.webp') },
                { name: 'QUẦN', imageUrl: require('@/assets/image/slick/quan.webp') },
                { name: 'ĐẦM', imageUrl: require('@/assets/image/slick/dam.webp') },
                { name: 'CHÂN VÁY', imageUrl: require('@/assets/image/slick/chanvay.webp') },
                { name: 'ĐỒ BỘ', imageUrl: require('@/assets/image/slick/dobo.webp') },
                { name: 'ĐỒ LÓT', imageUrl: require('@/assets/image/slick/underwear.webp') },
                { name: 'ÁO', imageUrl: require('@/assets/image/slick/ao_nam.webp') },
                { name: 'ÁO DÀI', imageUrl: require('@/assets/image/slick/aodai.webp') },
                { name: 'QUẦN', imageUrl: require('@/assets/image/slick/quan_nam.webp') },
                { name: 'JUMPSUIT', imageUrl: require('@/assets/image/slick/jumpsuit-set.webp') },
            ]);
            onMounted(async () => {
                try {
                    const response = await APIService.post('banner/get-data-by-page', {
                        pageIndex: 1,
                        pageSize: 5,
                        categoryCode_Filter: 'BANNER_SLIDER',
                        isDisplay_Filter: true,
                    });
                    images.value = response.data.data.items;

                } catch (error) {
                    console.error('Error fetching banner images:', error);
                }
                try {
                    const response = await APIService.post('banner/get-data-by-page', {
                        pageIndex: 1,
                        pageSize: 1,
                        categoryCode_Filter: 'BANNER_BESTSELLER',
                        isDisplay_Filter: true,
                    });
                    bestSellerBanner.value = response.data.data.items;

                } catch (error) {
                    console.error('Error fetching banner images:', error);
                }
                try {
                    const response = await APIService.post('product/get-data-by-page', {
                        pageIndex: 1,
                        pageSize: 10,
                        categoryCode_Filter: 'BEST_SELLER',
                       
                    });
                    bestSellerImages.value = response.data.data.items;

                } catch (error) {
                    console.error('Error fetching banner images:', error);
                }


            });

            return {
                images,
                Carouselcategories,
                bestSellerBanner,
                bestSellerImages,

            };
        },
    };
</script>
<style scoped></style>