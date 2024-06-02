<template>
    <div>
        <a-modal v-model:open="open" title="Thêm dữ liệu danh mục" :footer="null">
            <a-form ref="formRef" :model="category" layout="vertical">
                <a-form-item ref="code" label="Mã" name="code" :rules="[
                    {
                        required: true,
                        message: 'Vui lòng nhập thông tin này',
                        trigger: 'change',
                    },
                    {
                        min: 2,
                        max: 20,
                        message: 'Độ dài từ 2-20',
                        trigger: 'blur',
                    },
                ]">
                    <a-input v-model:value="category.code" @input="formatInput" />
                </a-form-item>
                <a-form-item ref="name" label="Tên " name="name" :rules="[{
                    required: true,
                    message: 'Vui lòng nhập thông tin này',
                    trigger: 'change'
                }, {
                    min: 1,
                    max: 50,
                    message: 'Độ dài từ 1-50',
                    trigger: 'blur',
                }]">
                    <a-input v-model:value="category.name" />
                </a-form-item>



                <a-form-item ref="description" label="Mô tả" name="description">
                    <a-textarea v-model:value="category.description" />
                </a-form-item>
                <a-form-item>
                    <a-button @click="handleSubmitAsync" :disabled="disableBtnSubmit" :loading="isLoading"
                        type="primary" style="margin-right: 20px;" class="login-form-button">
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
        props: {
            parentId: String
        },
        setup(props) {
            const category = reactive({
                name: '',
                code: '',
                description: '',
                parentId: props.parentId

            })
            const disableBtnSubmit = computed(() => {
                return category.name == '' || category.code == ''
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
                disableBtnSubmit

            }
        },// end setup
        data() {
            return {
                open: false,
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
                    notification.info({
                        message: 'Thông báo',
                        description: 'Đang xử lý...',
                        key : 'loadingKey',
                        duration: 0
                    })
                    const serverResponse = await APIService.post('datacategory/create', this.category)
                    if (serverResponse.data.message == "Tạo mới thành công") {
                        notification.success({
                            message: 'Thông báo',
                            description: 'Tạo mới thành công',
                            key : 'loadingKey',
                            
                        })
                        this.isLoading = false

                        this.closeModal()
                        this.$emit('addSuccess')
                    } else {
                        this.isLoading = false
                        notification.error({
                            message: 'Thông báo',
                            description: 'Tạo mới thất bại',
                            key : 'loadingKey',
                        })
                    }
                }).catch(error => {
                    this.isLoading = false
                    notification.error({
                        message: 'Thông báo',
                        description: 'Vui lòng kiểm tra lại thông tin',
                        key : 'loadingKey',
                    })
                }).finally(() => {
                    this.isLoading = false;
                })


            }//end handleSubmitAsync

        }
    }

</script>