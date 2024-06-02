<template>
    <div>
        <a-modal v-model:open="open" title="THÊM BANNER" :footer="null">
            <a-divider style="margin-top: 0px;"></a-divider>
            <a-form ref="formRef" :model="category" layout="vertical">
                <a-row :gutter="70">
                    
                    <a-col :span="12">
                        <a-form-item label="Hiển thị" name="status">
                            <a-radio-group>
                                <a-radio value="1">Hiển thị</a-radio>
                                <a-radio value="0">Ẩn</a-radio>
                            </a-radio-group>
                        </a-form-item>
                        <a-form-item ref="name" label="Thứ tự hiển thị" name="name">
                            <a-input v-model:value="category.name" />
                        </a-form-item>
                        <a-form-item ref="name" label="Danh mục hiển thị" name="name">
                            <a-select>
                              <a-select-option key="" value=""></a-select-option>
                            </a-select>
                        </a-form-item>
                    </a-col>
                    <a-col :span="12">
                        <a-form-item label="Ảnh banner" name="banner">
                            <a-upload list-type="picture-card" :showUploadList="true" accept=".jpg,.jpeg,.png"
                                maxCount="1" :fileList="fileList" :before-upload="beforeUpload" :action="apiUrl"
                                @change="handleChangeAvatar">
                                <template #default>
                                    <div>
                                        <font-awesome-icon icon="fa-regular fa-image" />
                                        <div class="ant-upload-text">Tải ảnh lên</div>
                                    </div>
                                </template>
                            </a-upload>
                        </a-form-item>
                       
                    </a-col>
                </a-row>
                






                <a-form-item>
                    <a-button @click="handleSubmitAsync" :loading="isLoading" type="primary" style="margin-right: 20px;"
                        class="login-form-button">
                        Hoàn thành
                    </a-button>
                    <a-button type="normal" @click="closeModal" style="margin-right: 20px;" class="login-form-button">
                        Đóng
                    </a-button>


                </a-form-item>
            </a-form>

        </a-modal>
    </div>
</template>
<script>
    import { computed } from "vue";
    import APIService from "@/helpers/APIService"
    import { ref, reactive } from "vue";
    import { message, notification } from "ant-design-vue";

    export default {
        setup() {
            const beforeUpload = file => {
                const isJpgOrPng = file.type === 'image/jpeg' || file.type === 'image/png' || file.type === 'image/jpg';
                if (!isJpgOrPng) {
                    notification.error(
                        {
                            message: 'Thông báo',
                            description: 'Chỉ chấp nhận định dạng ảnh JPG/PNG',
                            key: 'loadingKey',
                            duration: 2
                        }
                    );
                }
                const isLt2M = file.size / 1024 / 1024 < 10;
                if (!isLt2M) {
                    notification.error(
                        {
                            message: 'Thông báo',
                            description: 'Kích thước ảnh phải nhỏ hơn 10MB!',
                            key: 'loadingKey',
                            duration: 2
                        }
                    );
                }

                return isJpgOrPng && isLt2M;
            };
            const category = reactive({
                name: '',
                code: '',
                description: ''
            })

            return {
                category,
                isLoading: false,

                labelCol: {
                    span: 6,
                },
                wrapperCol: {
                    span: 14,
                },


                beforeUpload,
                apiUrl: process.env.VUE_APP_URL + 'Account/valid-upload' // Truy cập trong mounted


            }
        },// end setup
        data() {
            return {
                open: false,
                fileList: [],
            }
        },

        methods: {
            handleChangeAvatar(info) {
                this.fileList = [...info.fileList];

                if (info.file.status == 'done') {
                    this.fileList = [...info.file];
                } else if (info.file.status == 'error') {
                    notification.error(
                        {
                            message: 'Thông báo',
                            description: 'Tải ảnh lên thất bại',
                            key: 'loadingKey',
                            duration: 2
                        }
                    );
                    this.fileList = [];
                    info.file = null

                } else if (info.file.status == null) {
                    this.fileList = [];
                    info.file = null

                }
            },


            showModal() {
                this.open = true
            },
            closeModal() {
                this.open = false

                this.$refs.formRef.resetFields();
            },
            handleOk() {
                console.log(this.category)
            },
            async handleSubmitAsync() {
                this.$refs.formRef.validate().then(async () => {
                    this.isLoading = true

                    const serverResponse = await APIService.post('category/create', this.category)
                    if (serverResponse.data.message == "Tạo danh mục mới thành công") {
                        notification.success({ message: "Thành công", description: serverResponse.data.message })
                        this.isLoading = false

                        this.closeModal()
                        this.$emit('addSuccess')
                    } else {
                        this.isLoading = false
                        notification.error({ message: "Thất bại", description: serverResponse.data.message })
                    }
                }).catch(error => {
                    console.log('error', error);
                }).finally(() => {
                    this.isLoading = false;
                })


            }

        }
    }

</script>