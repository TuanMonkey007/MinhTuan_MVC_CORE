<template>
    <div>
        <a-modal v-model:open="open" title="THÊM DANH MỤC" :footer="null">
            <a-form ref="formRef" :model="category" layout="vertical">
                <a-form-item ref="code" label="Mã danh mục" name="code" :rules=" [
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
                    ]">
                    <a-input v-model:value="category.code" @input="formatInput" />
                </a-form-item>
                <a-form-item ref="name" label="Tên danh mục" name="name"  :rules="[{
                        required: true,
                        message: 'Tên danh mục là bắt buộc',
                        trigger: 'change'
                    }, {
                        min: 5,
                        max: 50,
                        message: 'Độ dài từ 5-50',
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
    import { message,notification } from "ant-design-vue";
    
    export default {
        setup() {
            const category = reactive({
                name: '',
                code: '',
                description: ''
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
        data(){
            return{
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
           
            async handleSubmitAsync() {
                this.$refs.formRef.validate().then(async () => {
                this.isLoading = true
               
                const serverResponse = await APIService.post('category/create', this.category)
                if (serverResponse.data.message == "Tạo danh mục mới thành công") {
                    notification.success({message:"Thành công", description: serverResponse.data.message})
                    this.isLoading = false
                   
                    this.closeModal()
                    this.$emit('addSuccess')
                } else {
                    this.isLoading = false
                    notification.error({message:"Thất bại", description: serverResponse.data.message})
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