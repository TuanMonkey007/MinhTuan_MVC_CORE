<template>
    <div>
        <a-modal v-model:open="open" title="Cập nhật danh mục" :footer="null">
            <a-form ref="formRef" :model="category" layout="vertical">
                <a-form-item ref="code" label="Mã danh mục" name="code">
                    <a-input v-model:value="category.code" @input="formatInput" :rules="[
                        {
                            required: true,
                            message: 'Mã danh mục là bắt buộc',
                            trigger: 'change',
                        },
                        {
                            min: 5,
                            max: 20,
                            message: 'Độ dài từ 5-20',
                            trigger: 'blur',
                        },
                    ]" />
                </a-form-item>
                <a-form-item ref="name" label="Tên danh mục" name="name">
                    <a-input v-model:value="category.name" :rules="[{
                        required: true,
                        message: 'Tên danh mục là bắt buộc',
                        trigger: 'change'
                    }, {
                        min: 5,
                        max: 50,
                        message: 'Độ dài từ 5-50',
                        trigger: 'blur',
                    }]" />
                </a-form-item>



                <a-form-item ref="description" label="Mô tả" name="description">
                    <!-- <a-textarea v-model:value="category.description" /> -->
                    <ckeditor :editor="editor" v-model="category.description" :config="editorConfig"></ckeditor>

                </a-form-item>
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
    import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
    import MyBase64UploadAdapterPlugin from "@/helpers/UploadAdapter";
    export default {
        setup() {

        },// end setup
        data() {

            const category = reactive({
                name: '',
                code: '',
                description: ''
            })

            return {
                id: undefined,
                category,
                isLoading: false,

                labelCol: {
                    span: 6,
                },
                wrapperCol: {
                    span: 14,
                },

                open: false,
                editor: ClassicEditor,

                editorConfig: {
                     extraPlugins: [MyBase64UploadAdapterPlugin], // Add the Base64 upload adapter plugin
     
                }

            }
        },

        methods: {
            formatInput(event) {
                // Chuyển đổi thành chữ in hoa
                let inputValue = event.target.value.toUpperCase();
                // Loại bỏ khoảng trắng
                inputValue = inputValue.replace(/\s+/g, '');
                inputValue = inputValue.replace(/[^A-Z0-9_-]/g, '');
                this.category.code = inputValue;
            },
            async showModal(id) {
                this.open = true
                const serverResponse = await APIService.get(`datacategory/${id}`)
                this.category = serverResponse.data.data
                this.id = id
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
                    notification.info({
                        message: 'Thông báo',
                        description: 'Đang xử lý dữ liệu',
                        key: 'loadingKey',
                        duration: 0
                    })
                    const serverResponse = await APIService.put(`datacategory/update/${this.id}`, this.category)
                    if (serverResponse.data.message == "Cập nhật thành công") {
                        notification.success({
                            message: 'Thông báo',
                            description: serverResponse.data.message,
                            key: 'loadingKey',

                        })
                        this.closeModal()
                    } else {
                        notification.error({
                            message: 'Thất bại',
                            description: serverResponse.data.message,
                            key: 'loadingKey',
                        })
                    }
                    this.isLoading = false
                    this.$emit('updateSuccess')
                }).catch(error => {
                    notification.error({
                        message: 'Lỗi hệ thống',
                        description: 'Cập nhật thất bại',
                        key: 'loadingKey',

                    })
                }).finally(() => {
                    this.isLoading = false;
                })
            }

        }
    }

</script>