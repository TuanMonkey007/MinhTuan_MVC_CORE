<template>
    <a-row>
        <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <a-page-header style="
           border: 1px solid rgb(235, 237, 240);
            background-color: #c21f24;
          " @back="goBack">
                <template #backIcon>
                    <font-awesome-icon @click="goBack" :icon="['fas', 'backward']" style="color: white; margin-left: 30px" />
                </template>
                <template #title>
                    <span style="color: white; font-size: 16px; margin-bottom: auto;display: block;">Tạo bài viết
                        mới</span>
                </template>
                <template #extra>
                    <a-breadcrumb separator=">" style="margin-right:30px;">
                        <a-breadcrumb-item href="">
                            <font-awesome-icon icon="fa-solid fa-house" style="color: #fff;" />
                            <span style="color: #fff;"> Trang quản trị</span>
                        </a-breadcrumb-item>

                        <a-breadcrumb-item href="">
                            <font-awesome-icon :icon="['fas', 'fa-list']" style="color: #fff;" />
                            <span style="color: #fff;"> Bài viết</span>
                        </a-breadcrumb-item>
                        <a-breadcrumb-item href="">
                            <font-awesome-icon :icon="['fa-solid', 'fa-plus']" style="color: #fff;" />
                            <span style="color: #fff;"> Thêm mới</span>
                        </a-breadcrumb-item>

                    </a-breadcrumb>
                </template>
            </a-page-header>
        </a-col>
    </a-row>
    <transition name="route" mode="out-in" appear>
        <a-row>
            <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
                <a-form ref="formRef" :model="article" layout="vertical">



                    <a-row>
                        <a-col :xs="24" :sm="24" :md="24" :lg="8" :xl="8">
                            <a-card :bordered="true" style="margin: 5px;">
                                <a-form-item label="Danh mục" name="category" :rules="[
                                    {
                                        required: true,
                                        message: 'Danh mục bài viết là bắt buộc',
                                        trigger: 'change',
                                    },

                                ]">
                                    <a-select showSearch optionFilterProp="label" :max-tag-count="2" mode="multiple"
                                        size="middle" placeholder="Chọn danh mục bài viết"
                                        v-model:value="article.category" :options="this.categoryOptions" />
                                </a-form-item>
                                <a-form-item label="Ngày đăng bài" name="publishDate" :rules="[{ required: true, message: 'Vui loại chọn ngày đăng bài' }]">
                            <a-date-picker v-model:value="article.publishDate" format="YYYY-MM-DD"
                                placeholder="Chọn ngày đăng bài" />
                        </a-form-item>
                                <a-row>
                                    <a-col :span="12"> <a-form-item label="Trạng thái" name="status">
                                            <a-radio-group v-model:value="article.status">
                                                <a-radio :value="true">Hiển thị</a-radio>
                                                <a-radio :value="false">Ẩn</a-radio>

                                            </a-radio-group>
                                        </a-form-item></a-col>
                                    <a-col :span="12"><a-form-item label="Lượt xem" name="countView">
                                            <a-input-number v-model:valuue="article.countView"
                                                :rules="[{ required: true, message: 'Nhập lượt xem' }]">
                                                <template #prefix>
                                                    <font-awesome-icon :icon="['far', 'eye']" />
                                                </template>
                                            </a-input-number>
                                        </a-form-item></a-col>

                                </a-row>
                            <a-form-item label="Ảnh hiện thị" :rules="[{ required: true, message: 'Vui loại chọn ảnh hiển thị' }]">
                                <a-upload-dragger list-type="picture-card" :showUploadList="true" v-model:value="article.thumbnail"
                                    accept=".jpg,.jpeg,.png,.bmp,.webp" maxCount="25" :fileList="fileListThumbnail"
                                    :multiple="true" :before-upload="beforeUpload" :action="apiUrl"
                                    @change="handleChangeThumbnail">
                                    <p class="ant-upload-drag-icon">
                                        <font-awesome-icon icon="fa-solid fa-upload"
                                            style="color: #0d63f8; font-size: 25px" />
                                    </p>
                                    <p class="ant-upload-text">Chọn hoặc kéo thả và đây để tải ảnh lên</p>
                                    <p class="ant-upload-hint">
                                        Chỉ nhận ảnh định dạng jpg, jpeg, png, bmp, webp
                                    </p>
                                </a-upload-dragger>
                            </a-form-item>



                            </a-card>
                        </a-col>
                        <a-col :xs="24" :sm="24" :md="24" :lg="16" :xl="16">
                            <a-card :bordered="true" style="margin: 5px;">
                                <a-form-item label="Tiêu đề" name="title" :rules="[
                                    {
                                        required: true,
                                        message: 'Tiêu đề bài viết là bắt buộc',
                                        trigger: 'change',
                                    },
                                    {
                                        min: 10,
                                        max: 200,
                                        message: 'Độ dài từ 10-200',
                                        trigger: 'blur',
                                    },
                                ]">
                                    <a-input v-model:value="article.title" @input="generateSlug" />
                                </a-form-item>
                                <a-form-item ref="name" label="Đoạn tóm tắt" name="summary" :rules="[{
                                    required: true,
                                    message: 'Tóm tắt là bắt buộc',
                                    trigger: 'change'
                                }, {
                                    min: 10,
                                    max: 400,
                                    message: 'Độ dài từ 10-400',
                                    trigger: 'blur',
                                }]">
                                    <a-textarea v-model:value="article.summary"></a-textarea>
                                </a-form-item>
                                <a-form-item label="Slug">
                                    <a-input v-model:value="article.slug" disabled />
                                </a-form-item>
                                <a-form-item label="Từ khóa bài viết"  :rules="[{ required: true, message: 'Nhập từ khóa bài viết' }]">
                                    <template v-for="(tag, index) in tags" :key="tag">
                                        <a-tooltip v-if="tag.length > 20" :title="tag">
                                            <a-tag :closable="index !== 0" @close="handleClose(tag)">
                                                {{ `${tag.slice(0, 20)}...` }}
                                            </a-tag>
                                        </a-tooltip>
                                        <a-tag v-else :closable="index !== -1" @close="handleClose(tag)">
                                            {{ tag }}
                                        </a-tag>
                                    </template>
                                    <a-input v-if="inputVisible" ref="inputRef" v-model:value="inputValue" type="text"
                                        size="small" :style="{ width: '78px' }" @blur="handleInputConfirm"
                                        @keyup.enter="handleInputConfirm" />
                                    <a-tag v-else style="background: #fff; border-style: dashed" @click="showInput">
                                        <plus-outlined />
                                        New Tag
                                    </a-tag>
                                </a-form-item>
                            </a-card>
                        </a-col>




                    </a-row>




                    <a-card :bordered="true" style="margin: 5px;">
                        <a-form-item label="Nội dung chính" name="content" :rules="[{ required: true, message: 'Nhập nội dung chính' }]">
                            <div style="min-height: 300px">
                                <ckeditor :editor="editor" v-model="article.content" :config="editorConfig"></ckeditor>

                            </div>

                        </a-form-item>
                    </a-card>


                    <a-form-item>
                        <a-button @click="handleSubmitAsync" :disabled="disableBtnSubmit" :loading="isLoading"
                            type="primary" style="margin-right: 20px;" class="login-form-button">
                            Hoàn thành
                        </a-button>
                        <a-button type="normal" @click="closeModal" style="margin-right: 20px;"
                            class="login-form-button">
                            Đóng
                        </a-button>


                    </a-form-item>






                </a-form>
            </a-col>
        </a-row>
    </transition>

</template>
<script>
import dayjs from "dayjs";
    import slugify from 'slugify';
    import { defineComponent, ref, reactive, toRefs, nextTick } from 'vue';
    import { PlusOutlined } from '@ant-design/icons-vue';

    import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
    import MyBase64UploadAdapterPlugin from "@/helpers/UploadAdapter";
    import { message, notification } from "ant-design-vue";
    import {
        SmileOutlined,
        DownOutlined,
        CompassOutlined,
    } from "@ant-design/icons-vue";
    import APIService from "@/helpers/APIService";
    import { inject } from "vue";
    export default {
        components: {
            SmileOutlined,
            DownOutlined,
            PlusOutlined,




        },

        setup() {
            const inputRef = ref();
            const state = reactive({
                tags: [],
                inputVisible: false,
                inputValue: '',
            });
            const handleClose = removedTag => {
                const tags = state.tags.filter(tag => tag !== removedTag);

                state.tags = tags;
            };
            const showInput = () => {
                state.inputVisible = true;
                nextTick(() => {
                    inputRef.value.focus();
                });
            };
            const handleInputConfirm = () => {
                const inputValue = state.inputValue;
                let tags = state.tags;
                if (inputValue && tags.indexOf(inputValue) === -1) {
                    tags = [...tags, inputValue];
                }

                Object.assign(state, {
                    tags,
                    inputVisible: false,
                    inputValue: '',
                });
            };
            return {
                ...toRefs(state),
                handleClose,
                state,
                showInput,
                handleInputConfirm,
                inputRef,
            };
        },

        data() {
            return {
                article: {
                    title: "",
                    summary: "",
                    content: "",
                    status: true,
                    countView: 0,
                    thumbnail: "",
                    category: [],
                    slug: "",
                    publishDate: '',

                },
                categoryOptions: [],
                apiUrl: "",
                editor: ClassicEditor,

                editorConfig: {
                    toolbar: {
                        items: [
                            'undo', 'redo',
                            '|', 'heading',
                            '|', 'fontfamily', 'fontsize', 'fontColor', 'fontBackgroundColor',
                            '|', 'bold', 'italic', 'strikethrough', 'subscript', 'superscript', 'code',
                            '|', 'link', 'uploadImage', 'blockQuote', 'codeBlock',
                            '|', 'bulletedList', 'numberedList', 'todoList', 'outdent', 'indent'
                        ],
                        shouldNotGroupWhenFull: true,

                    },
                    placeholder: 'Nhập nội dung của bài viết',
                    extraPlugins: [MyBase64UploadAdapterPlugin], // Add the Base64 upload adapter plugin
                    image: {
                        resizeUnit: "%",
                        resizeOptions: [{
                            name: 'resizeImage:original',
                            value: null
                        },
                        {
                            name: 'resizeImage:50',
                            value: '50'
                        },
                        {
                            name: 'resizeImage:75',
                            value: '75'
                        }],
                        toolbar: ['resizeImage', 'imageStyle:inline', 'imageStyle:wrapText', 'imageStyle:breakText', '|',
                            'toggleImageCaption', 'imageTextAlternative'],
                    },


                    disableBtnSubmit: false,
                    isLoading: false,
                    fileListThumbnail: [],  

                }
            }//end return data
        },//end data,
        mounted() {
            const selectedMenu = inject("selectedMenu");
            const changeSelectedMenu = inject("changeSelectedMenu");
            if (this.$route.name === "ArticleCreate") {
                changeSelectedMenu("Article");
            }
            this.apiUrl = process.env.VUE_APP_URL + 'Account/valid-upload'; // Truy cập trong mounted
            this.fetchCategoryOption();
        }, //end mounted
        methods: {
            handleChangeThumbnail(info) {
                this.fileListThumbnail = [...info.fileList];

                if (info.file.status == 'done') {
                    this.fileListThumbnail = [...info.file];
                } else if (info.file.status == 'error') {
                    message.error(`${info.file.name} Lỗi .`);
                    this.fileListThumbnail = [];
                    info.file = null

                } else if (info.file.status == null) {
                    this.fileListThumbnail = [];
                    info.file = null

                }
            },
            generateSlug() {
                this.article.slug = slugify(this.article.title, {
                    lower: true,       // Chuyển đổi tất cả thành chữ thường
                    remove: /[*+~.()'"!:@]/g, // Loại bỏ các ký tự đặc biệt như *+~.()'"!:@
                    strict: true       // Tạo slug nghiêm ngặt hơn, loại bỏ ký tự đặc biệt
                });
            },
            async fetchCategoryOption() {
                const serverResponse = await APIService.get('datacategory/get-list-by-parent-code/BAI_VIET')
                this.categoryOptions = serverResponse.data.data.map(item => {
                    return {
                        label: item.name,
                        value: item.id
                    }
                })
            },
            goBack() {
                this.$router.go(-1); // Điều hướng trở lại trang trước
            },

            closeModal() {
                this.$router.push({ name: "ArticleHome" });
            },
            beforeUpload(file) {
                // Danh sách các phần mở rộng tệp được chấp nhận
                const allowedExtensions = ['jpg', 'jpeg', 'png', 'bmp', 'webp'];
                var isJpgOrPng = true;
                // Kiểm tra phần mở rộng file
                const extension = file.name.split('.').pop().toLowerCase();
                if (!allowedExtensions.includes(extension)) {
                    notification.error({
                        message: 'Định dạng ảnh không hợp lệ',
                        description: 'Chọn định dạng ảnh với một phần mở rộng: ' + allowedExtensions.join(', ')
                    })
                    isJpgOrPng = false;
                    return false;
                }

                const isLt2M = file.size / 1024 / 1024 < 10;
                if (!isLt2M) {
                    notification.error({
                        message: 'Tính tối đa 10MB',
                        description: 'Tính tối đa 10MB'
                    })
                    return false;
                }

                return isJpgOrPng && isLt2M;
            },

            async handleSubmitAsync() {
                this.$refs.formRef.validate().then(async () => {
                    this.isLoading = true
                    const formData = new FormData();
                    formData.append('title', this.article.title);
                    formData.append('slug', this.article.slug);
                    formData.append('summary', this.article.summary);
                    formData.append('content', this.article.content);
                    formData.append('tag', this.state.tags);
                    formData.append('publishDate', this.article.publishDate ? this.article.publishDate : dayjs(new Date()).format('YYYY-MM-DD'));
                    if (this.fileListThumbnail.length > 0 && this.fileListThumbnail[0].originFileObj != null) {
                        formData.append('thumbnailFile', this.fileListThumbnail[0].originFileObj);
                    }
                    
                    formData.append('status', this.article.status);
                    formData.append('countView', this.article.countView ? this.article.countView : 0);
                    this.article.category.forEach(item => {
                        formData.append('listCategory', item);
                    });
                    const serverResponse = await APIService.post('article/create', formData)
                
                    if (serverResponse.data.message == "Tạo bài viết mới thành công") {
                        notification.success({
                            message: "Thành công",
                            description: serverResponse.data.message,
                        })
                        this.$router.push({ name: "ArticleHome" });
                    }else if(serverResponse.data.message == "Bài viết đã bị trùng slug"){
                            notification.warning({
                                message: "Thất bại",
                                description:"Trùng slug bài viết, vui lòng thay đổi tiêu đề"
                            })
                    }else{
                        notification.error({
                            message: "Thất bại",
                            description: serverResponse.data.message,
                        })
                    }
                }).catch(error => {
                    notification.warning({
                        message: "Thống báo",
                        description:"Vui nhập đầy đủ thông tin"
                    })
                }).finally(() => {
                    this.isLoading = false;
                })

            }



        },
    };
</script>
<style scoped>
    .btnSearch:hover {
        background-color: rgb(229 127 123);
        color: white;
    }



    /* Ẩn chữ trên màn hình nhỏ hơn hoặc bằng 768px */
    @media (max-width: 767px) {
        .ant-breadcrumb-link>span {
            display: none;
        }
    }

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
        color: #fff;
        /* Đặt màu chữ cho breadcrumb */
    }

    .ant-page-header-heading-title {
        line-height: normal;
        /* Đặt màu chữ cho tiêu đề */
        display: flex;
        /* Hiển thị theo chiều ngang */
    }
</style>