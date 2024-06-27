<template>
    <div>
        <a-modal v-model:open="open" :footer="null" width="900px" style="top:20px">
            <template #title>
                <div style="text-align: center; width: 100%; font-size: 20px">THÊM SẢN PHẨM</div>
            </template>
            <a-divider style="margin-top: 0px;"></a-divider>
            <a-form ref="formRef" :model="product" layout="vertical">
                <a-row :gutter="30">
                    <a-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
                        <a-form-item ref="code" label="Mã sản phẩm" name="code" :rules="[
                            {
                                required: true,
                                message: 'Mã sản phẩm là bắt buộc',
                                trigger: 'change',
                            },
                            {
                                min: 5,
                                max: 20,
                                message: 'Độ dài từ 5-20',
                                trigger: 'blur',
                            },
                        ]">
                            <a-input v-model:value="product.code" @input="formatInput" />
                        </a-form-item>
                    </a-col>
                    <a-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
                        <a-form-item ref="name" label="Tên sản phẩm" name="name" :rules="[{
                            required: true,
                            message: 'Tên sản phẩm là bắt buộc',
                            trigger: 'change'
                        }, {
                            min: 5,
                            max: 100,
                            message: 'Độ dài từ 5-100',
                            trigger: 'blur',
                        }]">
                            <a-input v-model:value="product.name" />
                        </a-form-item>
                    </a-col>
                </a-row>

                <a-row gutter="30">
                    <a-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
                        <a-form-item name="price" label="Giá tiền" :rules="[{ required: true, message: 'Vui lòng nhập giá tiền' },
                        ]">
                            <a-input-number style="width: 100%" v-model:value="product.price"></a-input-number>
                        </a-form-item>

                    </a-col>
                    <a-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
                        <a-form-item label="Danh mục/Tags"
                            :rules="[{ required: true, message: 'Vui lòng chọn danh mục sản phấm' }]"
                            name="listCategory">
                            <a-select showSearch optionFilterProp="label" :max-tag-count="2" v-model:value="product.listCategory" :options="categoryOptions"
                                mode="multiple" size="middle" placeholder="Chọn danh mục sản phẩm"></a-select>
                        </a-form-item>
                    </a-col>
                    <a-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
                        <a-form-item label="Ảnh Thumbnail" name="thumbnail">
                            <a-upload list-type="picture-card" :showUploadList="true"
                                accept=".jpg,.jpeg,.png,.bmp,.webp" maxCount="1" :fileList="fileListThumbnail"
                                :before-upload="beforeUpload" :action="apiUrl" @change="handleChangeThumbnail">
                                <template #default>
                                    <div>
                                        <font-awesome-icon icon="fa-regular fa-image" />
                                        <div class="ant-upload-text">Tải ảnh lên</div>
                                    </div>
                                </template>
                            </a-upload>
                        </a-form-item>
                    </a-col>
                    <a-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
                        <a-form-item label="Ảnh chi tiết" name="imageDetail">
                            <!-- <a-upload list-type="picture-card" :showUploadList="true"
                                accept=".jpg,.jpeg,.png,.bmp,.webp" maxCount="25" :fileList="fileList"
                                :before-upload="beforeUpload" :action="apiUrl" @change="handleChangeImageDetail">
                                <template #default>
                                    <div>
                                        <font-awesome-icon icon="fa-regular fa-image" />
                                        <div class="ant-upload-text">Tải ảnh lên</div>
                                    </div>
                                </template>
                            </a-upload> -->
                            <a-upload-dragger list-type="picture-card" :showUploadList="true"
                                accept=".jpg,.jpeg,.png,.bmp,.webp" maxCount="25" :fileList="fileList" :multiple="true"
                                :before-upload="beforeUpload" :action="apiUrl" @change="handleChangeImageDetail">
                                <p class="ant-upload-drag-icon">
                                    <font-awesome-icon icon="fa-solid fa-upload" style="color: #0d63f8; font-size: 25px" />
                                </p>
                                <p class="ant-upload-text">Chọn hoặc kéo thả và đây để tải ảnh lên</p>
                                <p class="ant-upload-hint">
                                    Chỉ nhận ảnh định dạng jpg, jpeg, png, bmp, webp
                                </p>
                            </a-upload-dragger>
                        </a-form-item>
                    </a-col>
                </a-row>





                <a-row>
                    <a-col :span="24"><a-form-item ref="description" label="Mô tả" name="description">
                            <!-- <a-textarea v-model:value="product.description" /> -->
                            <ckeditor :editor="editor" v-model="product.description" :config="editorConfig"></ckeditor>
                        </a-form-item></a-col>
                </a-row>

                <a-row>
                    <a-col :span="24">
                        <a-form-item>
                            <a-button @click="handleSubmitAsync" :loading="isLoading" type="primary"
                                style="margin-right: 20px;" class="login-form-button">
                                Hoàn thành
                            </a-button>
                            <a-button @click="closeModal" style="margin-right: 20px;" class="login-form-button">
                                Đóng
                            </a-button>


                        </a-form-item>
                    </a-col>
                </a-row>





            </a-form>

        </a-modal>
    </div>
</template>
<script>
    import { computed } from "vue";
    import APIService from "@/helpers/APIService"
    import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
    import { ref, reactive } from "vue";
    import { message, notification } from "ant-design-vue";
    
    export default {
      
        setup() {
            const beforeUpload = file => {
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
            };
            const product = reactive({
                name: '',
                code: '',
                price: 0,
                description: '',
                listCategory: []
            })
            const categoryOptions = ref([]);

            return {
                product,
                isLoading: false,
                categoryOptions,
                labelCol: {
                    span: 6,
                },
                wrapperCol: {
                    span: 14,
                },

                beforeUpload

            }
        },// end setup
        data() {
            return {
                open: false,
                apiUrl: '',
                fileListThumbnail: [],
                fileList: [],
                editor: ClassicEditor,

                editorConfig: {
                    toolbar: [
                        'heading', '|',
                        'bold', 'italic', 'link', 'bulletedList', 'numberedList', 'blockQuote', '|',
                        'undo', 'redo'
                    ],
               
                }
            }
        },
        mounted() {
            this.fetchCategory()
            this.apiUrl = process.env.VUE_APP_URL + 'Account/valid-upload'; // Truy cập trong mounted
        },

        methods: {
            handleChangeImageDetail(info) {
                this.fileList = [...info.fileList];

                if (info.file.status == 'done') {
                    this.fileList = [...info.file];
                } else if (info.file.status == 'error') {
                    message.error(`${info.file.name} Lỗi .`);
                  
                    info.file = null

                } else if (info.file.status == null) {
                    this.fileList = [];
                    info.file = null

                }
            },
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
            async fetchCategory() {
                const serverResponse = await APIService.get('datacategory/get-list-by-parent-code/SAN_PHAM')
                this.categoryOptions = serverResponse.data.data.map(item => {
                    return {
                        label: item.name,
                        value: item.id
                    }
                })

            },
            formatInput(event) {
                // Chuyển đổi thành chữ in hoa
                let inputValue = event.target.value.toUpperCase();
                // Loại bỏ khoảng trắng
                inputValue = inputValue.replace(/\s+/g, '');
                inputValue = inputValue.replace(/[^A-Z0-9_-]/g, '');
                this.product.code = inputValue;
            },
            showModal() {
                this.open = true
            },
            closeModal() {
                this.open = false
                this.product = reactive({
                    name: '',
                    code: '',
                    price: 0,
                    description: '',
                    listCategory: []
                })
                this.fileListThumbnail = [],
                    this.fileList = []
                this.isLoading = false
                this.$refs.formRef.resetFields();
            },

            async handleSubmitAsync() {
                this.$refs.formRef.validate().then(async () => {
                    this.isLoading = true
                    notification.info({ message: 'Đang xử lý...', key: 'keyLoading', duration: 0 });
                    console.log(this.product)
                    const formData = new FormData();
                    formData.append('code', this.product.code);
                    formData.append('name', this.product.name);
                    formData.append('description', this.product.description);
                    formData.append('price', this.product.price);

                    if (this.fileListThumbnail.length > 0 && this.fileListThumbnail[0].originFileObj != null) {
                        formData.append('thumbnailFile', this.fileListThumbnail[0].originFileObj);
                    }
                    if (this.fileList.length > 0) {
                        this.fileList.forEach(file => {
                            formData.append('listImageFile', file.originFileObj);
                        });
                    }

                    this.product.listCategory.forEach(item => {
                        formData.append('listCategory', item);
                    });

                    const serverResponse = await APIService.post('product/create', formData)
                    if (serverResponse.data.message == "Tạo mới thành công") {



                        this.closeModal()
                        notification.success({ message: serverResponse.data.message, key: 'keyLoading', duration: 2 });
                        this.$emit('addSuccess')
                    } else {
                        this.isLoading = false
                        notification.error({ message: serverResponse.data.message, key: 'keyLoading', duration: 2 });
                    }
                }).catch(error => {
                    console.log('error', error);
                    this.isLoading = false
                }).finally(() => {
                    this.isLoading = false
                })


            }

        }
    }

</script>
<style scoped>
:deep(.ck-editor__main .ck-content){
    min-height: 200px !important;
}
</style>