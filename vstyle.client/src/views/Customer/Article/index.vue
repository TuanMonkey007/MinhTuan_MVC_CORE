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

                            <span style="color: #fff;">Bài viết</span>
                        </a-breadcrumb-item>

                    </a-breadcrumb>
                </template>

            </a-page-header>
        </a-col>
    </a-row>
    <a-row justify="center">
        <a-col :xs="24" :sm="24" :md="20" :lg="20" :xl="20">
            <a-row justify="space-between" :gutter="[16,16]"> 
                <a-col :xs="24" :sm="24" :md="24" :lg="16" :xl="16">
                    <a-row justify="center">
                        <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
                            <h2 style="display: flex; justify-content: start; justify-self: start;font-weight: 500;">
                                {{ this.article.title }}</h2>

                        </a-col>
                        <a-col :span="24" style="display: flex; justify-content:start;">

                            <div style="font-weight: 300;width: 100%; display: flex; justify-content:space-between">
                                <div  style="display: flex; align-items: center;">
                                    <a-avatar   :src="'data:' + this.authorInfo.avatarContentType + ';base64,' + this.authorInfo.avatarBase64">
                                        <template #icon>
                                            <font-awesome-icon icon="fa-solid fa-hat-cowboy" />
                                        </template>
                                    </a-avatar>
                                    <p style="font-weight: 500;margin-left:5px; "> {{ this.article.author }}</p>
                                </div>
                                
                                <div style="display: flex; align-items: center;">
                                    <font-awesome-icon icon="fa-regular fa-clock" style="color: #a3b0c7;" />
                                    <span style="font-size: 12px; margin-left:5px; align-items: center;">{{
                                        formatCreatedDate(article.publishDate)
                                        }}</span>
                                       <font-awesome-icon icon="fa-regular fa-eye" style="color: #bccce6;margin-left: 10px;" />
                                    <span style="font-size: 12px; margin-left:5px;margin-right: 10px; align-items: center;">{{
                                        article.countView
                                        }}</span>
                                </div>


                            </div>


                        </a-col>
                    </a-row>

              

                    <p style="display: flex; justify-content: start; justify-self: start;font-weight: 300;">
                        {{ this.article.summary }}</p>
                    <div v-html="this.article.content">
                        
                    </div>
                    <h5>Từ khóa bài viết:</h5>
                    <div v-if="this.article.tag != null">
                        <a-tag v-for="tag in this.tags" :key="tag" >{{ tag }}</a-tag>
                    </div>  
                </a-col>
                <a-col :xs="24" :sm="24" :md="24" :lg="8" :xl="8">
                    <a-card style="background-color: #e6d8d8;">
                        <template #title>
                            <p style="font-weight: 500;font-size: 20px;">Bài viết liên quan</p>
                        </template>
                        <a-row  v-for="blog in this.articleRelative" :key="blog.id">
                <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
                    <router-link :to="{ name: 'BlogDetail', params: { slug: blog.slug } }">
                        <a-card :bordered="false" style="margin-bottom: 5px;">
                            <a-row justify="space-around">
                                <a-col  :xs="8" :sm="8" :md="6" :lg="6" :xl="6" style="display: flex; justify-content: center; align-items: center;
                                max-height:100px;width: 160px">
                                   <div style="width: 160; height: fit-content;overflow: hidden; max-height:100px">
                                    <img alt="example"  style="object-fit: contain; width: 100%"
                                        :src="'data:' + blog.thumbnailContentType + ';base64,' + blog.thumbnailBase64" />
                                    </div>
                                    </a-col>
                                    <a-col :xs="14" :sm="14" :md="16" :lg="16" :xl="16">
                                        <span style="font-size: 14px; align-items: center;font-weight: 600;">{{ blog.title }}</span>
                                        <p style="font-size: 14px; align-items: center;">{{ blog.summary.substring(0, 100) + (blog.summary.length > 100 ? '...' : '') }}</p>
                                        <font-awesome-icon icon="fa-regular fa-clock" style="color: #a3b0c7;" />
                                        <span style="font-size: 12px; align-items: center;">{{formatCreatedDate(blog.publishDate) }}</span>
                                    
                                    </a-col>  
                            </a-row>  
                        </a-card>
                    </router-link>
                </a-col>
            </a-row>
                    </a-card>
                </a-col>
            </a-row>
        </a-col>
    </a-row>

</template>
<script>
    import dayjs from 'dayjs';
    import APIService from "@/helpers/APIService";
    export default {
        async mounted() {
            await this.fetchData()
            await this.fetchArticleRelative()
        },
        data() {
            return {
                article: {},
                articleRelative: [],
                tags: [],
                authorInfo: {},
            }
        },
        watch: {
            '$route'() {
                window.location.reload();
            },
        },
        methods: {
            async fetchAuthorInfo(id) {
                try {
                    const response = await APIService.get(`account/${id}`);
                    return response.data.data;
                } catch (error) {
                    console.error('Error fetching product info:', error);
                }
            },
            formatCreatedDate(date) {
                return dayjs(date).format("HH:mm:ss DD/MM/YYYY ");
            },
            async fetchArticleRelative() {
                try {
                    const response = await APIService.get(`article/get-relative-article-by-id/${this.article.id}`);
                    this.articleRelative = response.data.data.items;
                    
                } catch (error) {
                    console.error('Error fetching product info:', error);
                }
            },
            async fetchData() {
                try {
                    const res = await APIService.post(`article/get-data-by-page`, {
                        pageIndex: 1,
                        pageSize: 1,
                        slug_Filter: this.$route.params.slug,
                        status_Filter: true
                    })
                    this.article = res.data.data.items[0]
                    this.authorInfo = await this.fetchAuthorInfo(this.article.authorId);
                    console.log(this.authorInfo)
                    if(res.data.data.items[0].tag[0] != null){
                        this.tags = res.data.data.items[0].tag[0].split(',').map(tag => tag.trim());
                      
                   }
                } catch (error) {
                    console.error('Error fetching')
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
</style>