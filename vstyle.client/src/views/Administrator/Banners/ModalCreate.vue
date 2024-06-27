<template>
    <div>
        <a-modal v-model:open="open" title="THÊM BANNER" :footer="null">
            <a-divider style="margin-top: 0px;"></a-divider>
            <a-form ref="formRef" :model="banner" layout="vertical">
                <a-row :gutter="70">
                    
                    <a-col :span="12">
                        <a-form-item label="Hiển thị" name="isDisplay">
                            <a-radio-group v-model:value="banner.isDisplay" :rules="[{required: true, message:'Chọn trạng thái hiển thị'}]">
                                <a-radio :value="true">Hiển thị</a-radio>
                                <a-radio :value="false">Ẩn</a-radio>
                            </a-radio-group>
                        </a-form-item>
                        <a-form-item ref="name" label="Thứ tự hiển thị" name="orderDisplay" :rules="[{required:true, message:'Nhập thứ tự hiển thị'}]" >
                            <a-input-number v-model:value="banner.orderDisplay"></a-input-number>   
                        </a-form-item>
                        <a-form-item ref="name" label="Danh mục hiển thị" name="categoryId" :rules="[{required:true, message:'Chọn danh mục banner'}]">
                            <a-select v-model:value="banner.categoryId" :options="categoryOptions" placeholder="-- Danh mục hiển thị --">
                            </a-select>
                        </a-form-item>
                    </a-col>
                    <a-col :span="12">
                        <a-form-item label="Ảnh banner" name="bannerImage">
                            <a-upload list-type="picture-card" :showUploadList="true"   accept=".jpg,.jpeg,.png,.bmp,.webp"
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
            const categoryOptions = ref([]);
            const banner = reactive({
                isDisplay: true,
                orderDisplay: 0,
                categoryId: '',
            })
            
            return {
                banner,
                isLoading: false,
                categoryOptions,
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
            async fetchCategory() {
                const serverResponse = await APIService.get('datacategory/get-list-by-parent-code/BANNER')
                this.categoryOptions = serverResponse.data.data.map(item => {
                    return {
                        label: item.name,
                        value: item.id
                    }
                })

            },
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


           async showModal() {
               
                this.fetchCategory()
                this.open = true
            },
            closeModal() {
                this.open = false
                this.fileList = [],
                
                this.$refs.formRef.resetFields();
            },
            handleOk() {
                console.log(this.category)
            },
            async handleSubmitAsync() {
                console.log('submit')
               
                    
                    this.$refs.formRef.validate().then(async () => {
                        this.isLoading = true
                    notification.info(
                        {
                            message: 'Trạng thái',
                            description: 'Đang xử lý...',
                            key: 'loadingKey',
                            duration: 0
                        }
                    );
                    const formData = new FormData();
                    
                    formData.append('isDisplay', this.banner.isDisplay);
                    formData.append('orderDisplay', this.banner.orderDisplay);
                    formData.append('categoryId', this.banner.categoryId);
                    if(this.fileList.length > 0){
                        formData.append('bannerImage', this.fileList[0].originFileObj);
                    }
                    const serverResponse = await APIService.post('banner/create', formData)
                    if (serverResponse.data.message == "Tạo banner mới thành công") {
                        notification.success({ message: "Thành công", description: serverResponse.data.message,key: 'loadingKey'})
                        this.isLoading = false

                        this.closeModal()
                        this.$emit('addSuccess')
                    } else {
                        this.isLoading = false
                        notification.error({ message: "Thất bại", description: serverResponse.data.message,key: 'loadingKey'})
                    }
                }
                ).catch((e) => {
                    this.isLoading = false
                    //notification.error({ message: "Thất bại", description: e.message ,key: 'loadingKey'})
                })
                


            }

        }
    }

</script>