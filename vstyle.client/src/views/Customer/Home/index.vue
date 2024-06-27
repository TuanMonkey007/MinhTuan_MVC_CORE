<template>
    <!-- Slider -->
    <a-row>
        <a-col :span=24 v-if="isLoading">
            <a-skeleton-image :active="true" style="width: 100%" />
        </a-col>

        <a-col :span="24">
            <swiper style="width: 100%" :grabCursor="true" :effect="'creative'" :creativeEffect="{
                prev: {
                    shadow: true,
                    translate: ['-120%', 0, -500],
                },
                next: {
                    shadow: true,
                    translate: ['120%', 0, -500],
                },
            }" :autoplay="{
                delay: 2500,
                disableOnInteraction: false,
            }" :pagination="{
                clickable: true,
            }" :navigation="true" :modules="modules" class="mySwiper2">
                <swiper-slide v-for="image in images" :key="image.id">
                    <a-image :src="'data:' + image.bannerContentType + ';base64,' + image.bannerBase64"
                        alt="Banner Image" style=" object-fit: contain; width: 100%; height: auto;" :preview="false" />
                </swiper-slide>

            </swiper>


        </a-col>
    </a-row>
    <a-row justify="center" style="margin-top: 20px;">
        <a-col :xs="20" :sm="20" :md="16" :lg="16" :xl="16">
            <swiper :loop="true" :spaceBetween="30" :freeMode="true" :autoplay="{
                delay: 3000,
                disableOnInteraction: false,
            }" :breakpoints="categoryBreakpoints" :modules="modules" class="mySwiper">
                <swiper-slide v-for="category in Carouselcategories" :key="category.id">
                    <a-row v-if="!isLoading" justify="center">
                        <a-col :span="24" style="display: flex;align-items: center;justify-content: center;">
                            <a-avatar :src="category.imageUrl" :alt="category.name"
                                style="height: 70px ;width: 70px;; object-fit: contain; background-color:#fff0f0 ;">
                            </a-avatar>

                        </a-col>
                        <a-col :span="24" style="display: flex;align-items: center;justify-content: center;">
                            <span style="font-size: 16px; align-items: center">{{ category.name }}</span>
                        </a-col>
                    </a-row>
                    <a-row v-else justify="center">
                        <a-col :span="24" style="display: flex;align-items: center;justify-content: center;">
                            <a-skeleton-avatar
                                style="height: 70px !important ;width: 70px !important; object-fit: contain;"
                                :active="true" shape="circle" />
                        </a-col>

                    </a-row>
                </swiper-slide>
            </swiper>

        </a-col>
    </a-row>
    <!-- end Slider -->
    <!-- Collection -->
    <div v-if="isLoadingCollection">
        <a-skeleton :loading="true" :paragraph="{ rows: 10 }" />
    </div>
    <div v-else>
    <div  v-for="collection in this.collections" :key="collection.banner.id">
        <a-row style="margin-top: 30px">
            <a-col :span="24" style="display: flex; justify-content: center;">
                <h1 style="text-transform: uppercase; font-size: 24px;font-weight: 500">{{
                    collection.banner[0].categoryName }}</h1>
            </a-col>
        </a-row>
        <a-row justify="center" :gutter="30">

            <a-col :xs="24" :sm="24" :md="24" :lg="10" :xl="10" style=" max-height: 500px; overflow: hidden">
                <div v-for="banner in collection.banner" :key="banner.id" style="margin: 20px; width: 100%;"
                    class="banner-container">
                    <img :src="'data:' + banner.bannerContentType + ';base64,' + banner.bannerBase64" alt="Banner Image"
                        style="max-width: 100%; max-height: 100%;border-radius: 5%" />
                </div>
                <router-link :to="{ name: 'Collection', params: { collectionCode: collection.code } }"><button
                        class="center-button">Xem thêm</button></router-link>
            </a-col>
            <a-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
                <swiper :loop="true" :modules="modules" :centeredSlides="false" :breakpoints="swiperBreakpoints"
                    :navigation="true">
                    <!-- :breakpoints="swiperBreakpoints" > -->
                    <swiper-slide v-for="product in collection.products" :key="product.id">
                        <router-link :to="{ name: 'ProductDetail', params: { code: product.code, id: product.id } }">
                            <a-card hoverable class="highlighted-card" style="width: 160px; margin-top: 20px"
                                :bordered="false">
                                <template #cover>
                                    <div style="width: 100%;">
                                        <img alt="example" class="image-product"
                                            :src="'data:' + product.thumbnailContentType + ';base64,' + product.thumbnailBase64" />
                                    </div>
                                </template>
                                <a-card-meta>
                                    <template #title>
                                        <span style="font-size: 16px; align-items: center">{{ product.name }}</span>
                                    </template>
                                    <template #description>
                                        <span style="font-size: 16px; align-items: center">{{ product.code }}</span>
                                        <br>
                                        <span class="price-product">{{ fomartPrice(product.price) }}&#8363;</span>
                                    </template>
                                </a-card-meta>
                            </a-card>
                        </router-link>
                    </swiper-slide>
                </swiper>

            </a-col>
        </a-row>
    </div>
    </div>
    <!-- end collection -->
    <a-row v-if="isLoadingArticle" justify="center" style="margin-bottom: 30px">
        <a-col :xs="24" :sm="24" :md="20" :lg="16" :xl="16">
            <a-skeleton :loading="true" :paragraph="{ rows: 10 }" />
        </a-col>    
    </a-row>
    <a-row v-else justify="center" style="margin-bottom: 30px">
        <a-col :span="24">
            <a-divider style="margin-top: 10px" />
            <span style="font-size: 24px;display: flex;justify-content: center;font-weight: 500">SỰ KIỆN - TIN
                TỨC</span>
            <a-divider style="margin-top: 10px" />
        </a-col>
        <a-col :xs="24" :sm="24" :md="20" :lg="16" :xl="16">
            <a-row v-for="blog in this.articles" :key="blog.id">
                <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
                    <router-link :to="{ name: 'BlogDetail', params: { slug: blog.slug } }">
                        <a-card :bordered="false">
                            <a-row justify="space-around">
                                <a-col :xs="8" :sm="8" :md="4" :lg="4" :xl="4" style="display: flex; justify-content: center; align-items: center;
                                max-height:100px; overflow: hidden;width: 160px">
                                    <div style="width: 160; height: fit-content">
                                        <img alt="example" style="object-fit: contain;"
                                            :src="'data:' + blog.thumbnailContentType + ';base64,' + blog.thumbnailBase64" />
                                    </div>
                                </a-col>
                                <a-col :xs="14" :sm="14" :md="16" :lg="16" :xl="16">
                                    <span style="font-size: 14px; align-items: center;font-weight: 600;">{{ blog.title
                                        }}</span>
                                    <p style="font-size: 14px; align-items: center;">{{ blog.summary.substring(0, 100) +
                                        (blog.summary.length > 100 ? '...' : '') }}</p>
                                    <font-awesome-icon icon="fa-regular fa-clock" style="color: #a3b0c7;" />
                                    <span style="font-size: 12px; align-items: center;">{{
                                        formatCreatedDate(blog.publishDate)
                                        }}</span>

                                </a-col>
                            </a-row>
                        </a-card>
                    </router-link>
                </a-col>
            </a-row>
        </a-col>
    </a-row>
    


</template>

<script>
    import dayjs from 'dayjs';
    import { Navigation, Pagination, Scrollbar, A11y, Autoplay } from 'swiper/modules';

    // Import Swiper Vue.js components
    import { Swiper, SwiperSlide } from 'swiper/vue';
    // import required modules
    import { EffectCreative } from 'swiper/modules';
    // Import Swiper styles
    import 'swiper/css';
    import 'swiper/css/navigation';
    import 'swiper/css/pagination';
    import 'swiper/css/scrollbar';
    import 'swiper/css/a11y';
    import 'swiper/css/effect-creative';
    import 'swiper/css/free-mode';
    import { LeftCircleOutlined, RightCircleOutlined } from '@ant-design/icons-vue';
    import APIService from '@/helpers/APIService';
    import { ref, onMounted } from 'vue';

    export default {
        components: {
            LeftCircleOutlined,
            RightCircleOutlined,
            Swiper,
            SwiperSlide,
        },
        async mounted() {
            try {
                this.isLoading = true
                const response = await APIService.post('banner/get-data-by-page', {
                    pageIndex: 1,
                    pageSize: 5,
                    categoryCode_Filter: 'BANNER_SLIDER',
                    isDisplay_Filter: true,
                });
                this.images = response.data.data.items;

            } catch (error) {
                console.error('Error fetching banner images:', error);
            }
            finally {
                this.isLoading = false
            }
            await this.fetchAllCollectionCode();
            await this.fetchAllDataCollection();
            await this.fetchArticleData();
            // await this.fetchBannerCollection('BST_BESTSELLER');
            // await this.fetchProductCollection('BST_BESTSELLER');
        },
        methods: {
            formatCreatedDate(date) {
                return dayjs(date).format("HH:mm:ss DD/MM/YYYY ");
            },
            goToCollection(code) {
                this.$router.push({ name: 'Collection', query: { collectionCode: code } });
            },
            fomartPrice(price) {
                return price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.');
            },
            async fetchAllDataCollection() {
                try {
                    this.isLoadingCollection = true
                    for (let i = 0; i < this.collectionCode.length; i++) {

                        const resBanner = await this.fetchBannerCollection(this.collectionCode[i]);
                        const resProduct = await this.fetchProductCollection(this.collectionCode[i]);
                        // Kết hợp hai đối tượng vào một đối tượng duy nhất
                        const combinedCollection = {
                            banner: resBanner,
                            products: resProduct,
                            code: this.collectionCode[i],
                        };
                        this.collections.push(combinedCollection);


                    }
                    console.log(this.collections)
                } catch {
                    console.error('Error fetching code collection:');
                }
                finally {
                    this.isLoadingCollection = false
                }
            },
            async fetchAllCollectionCode() {
                try {
                    const response = await APIService.get('datacategory/get-list-by-parent-code/BO_SUU_TAP');
                    this.collectionCode = response.data.data.map(item => item.code);



                } catch {
                    console.error('Error fetching code collection:');
                }

            },
            async fetchBannerCollection(nameCollection) {
                try {
                    const response = await APIService.post('banner/get-data-by-page', {
                        pageIndex: 1,
                        pageSize: 1,
                        categoryCode_Filter: nameCollection,
                        isDisplay_Filter: true,
                    });
                    // this.bestSellerBanner = response.data.data.items;
                    return response.data.data.items;
                } catch (error) {
                    console.error('Error fetching banner images:', error);
                }
            },
            async fetchProductCollection(nameCollection) {
                try {
                    const response = await APIService.post('product/get-data-by-page', {
                        pageIndex: 1,
                        pageSize: 10,
                        categoryCode_Filter: nameCollection,

                    });
                    // this.collections.banner[0] = response.data.data.items;
                    // this.bestSellerImages = response.data.data.items;
                    return response.data.data.items;

                } catch (error) {
                    console.error('Error fetching banner images:', error);
                }
            },
            async fetchArticleData() {
                try {
                    this.isLoadingArticle = true
                    const resCategory = await APIService.get('datacategory/get-id-by-code-and-parent-code/TIN_TUC_TRANG_CHU/BAI_VIET');
                    const catetgoryId = resCategory.data.data;
                    const response = await APIService.post('article/get-data-by-page', {
                        pageIndex: 1,
                        pageSize: 5,
                        sortQuery: 'publishDate desc',
                        status_Filter: true,
                        category_Filter: [catetgoryId]

                    });
                    console.log("Tin tức:", response.data)
                    this.articles = response.data.data.items;
                } catch {
                    console.error('Error fetching code collection:');
                }
                finally {
                    this.isLoadingArticle = false
                }
            }

        },
        data() {
            const images = ref([]);
            const collections = ref([]);
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
            return {
                isLoadingArticle: true,
                isLoadingCollection:false,
                isLoading: false,
                collectionCode: [],
                articles: [],
                images,
                Carouselcategories,
                bestSellerBanner,
                collections,
                bestSellerImages,
                modules: [EffectCreative, Autoplay, Pagination, Navigation],
                swiperBreakpoints: {
                    320: {
                        slidesPerView: 2,
                        spaceBetween: 0
                    },
                    640: {
                        slidesPerView: 2,
                        spaceBetween: 0
                    },
                    768: {
                        slidesPerView: 4,
                        spaceBetween: 40
                    },
                    1024: {
                        slidesPerView: 4,
                        spaceBetween: 10
                    }
                },
                categoryBreakpoints: {
                    320: {
                        slidesPerView: 3,
                        spaceBetween: 0
                    },
                    640: {
                        slidesPerView: 3,
                        spaceBetween: 0
                    },
                    768: {
                        slidesPerView: 4,
                        spaceBetween: 40
                    },
                    1024: {
                        slidesPerView: 6,
                        spaceBetween: 10
                    }
                },

            }
        },

    };
</script>
<style scoped>

    :deep(.swiper-button-prev) {
        color: #b11d1b !important;
    }

    :deep(.swiper-button-next) {
        color: #b11d1b !important;
    }

    :deep(.swiper-pagination-bullet-active) {
        background-color: #b11d1b !important;
    }

    .banner-container {
        position: relative;
        margin: 20px;
        width: 100%;
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .banner-image {
        max-width: 100%;
        max-height: 100%;
        border-radius: 5%;
    }

    .center-button {
        position: absolute;
        top: 80%;
        left: 50%;
        transform: translate(-50%, -50%);
        cursor: pointer;
        box-sizing: border-box;
        min-width: 100px;
        width: 200px;
        height: 40px;
        padding: 12px;
        text-decoration: none;
        font-size: 16px;
        font-weight: 700;
        color: #fff;
        text-shadow: 0 1px 2px rgba(0, 0, 0, .75);
        background: #b11d1b;
        outline: none;
        border-radius: 15px;
        border: 1px solid #9a3a37;
        box-shadow: inset 1px 1px 0 hsla(0, 0%, 100%, .25), inset 0 0 6px #a23227, inset 0 80px 80px -40px #ac3223, 1px 1px 3px rgba(0, 0, 0, .75);
        overflow: visible;
        transition: .5s linear;
    }

    .highlighted-card {
        border: 1px solid rgba(0, 0, 0, 0.1);
        /* Viền nhẹ để tạo cảm giác phân tách */
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1), 0 6px 20px rgba(0, 0, 0, 0.1);
        /* Hiệu ứng bóng để nổi bật */
        border-radius: 5px;
        /* Làm tròn góc */
        transition: box-shadow 0.3s ease-in-out;
        /* Hiệu ứng mượt khi hover */
    }

    .highlighted-card:hover {
        box-shadow: 0 8px 16px rgba(194, 61, 61, 0.2), 0 12px 40px rgba(184, 83, 83, 0.2);
        /* Hiệu ứng bóng mạnh hơn khi hover */
    }

    :deep(.ant-skeleton-image) {
        width: 100% !important;
        height: 400px !important;
    }
</style>