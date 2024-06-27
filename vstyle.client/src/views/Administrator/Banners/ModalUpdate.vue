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
                id: '',
                bannerBase64: '',
                bannerContentType: '',
                isChangeBanner: false
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
                this.isChangeBanner = true
            },


           async showModal(id) {
               
                this.fetchCategory()
                const serverResponse = await APIService.get(`banner/${id}`)
                this.banner.isDisplay = serverResponse.data.data.isDisplay
                this.banner.orderDisplay = serverResponse.data.data.orderDisplay
                this.banner.categoryId = serverResponse.data.data.categoryId
                this.bannerBase64 = serverResponse.data.data.bannerBase64
                this.bannerContentType = serverResponse.data.data.bannerContentType
                // Kiểm tra nếu có avatar và chuyển đổi thành fileList
                if (this.bannerBase64 != null) {
                    const byteCharacters = atob(this.bannerBase64);
                    const byteNumbers = new Array(byteCharacters.length);
                    for (let i = 0; i < byteCharacters.length; i++) { // Sửa vòng lặp for
                        byteNumbers[i] = byteCharacters.charCodeAt(i);
                    }
                    const byteArray = new Uint8Array(byteNumbers);
                    const blob = new Blob([byteArray], { type: this.bannerContentType });
                    const file = new File([blob], 'banner', { type: this.bannerContentType });

                    this.fileList = [
                        {
                            uid: '-1', // ID duy nhất
                            name: 'avatar', // Tên tệp
                            status: 'done', // Trạng thái tải lên thành công
                            url: URL.createObjectURL(file), // Hoặc đường dẫn tạm thời khác
                            originFileObj: file // Đối tượng File để upload
                        },
                    ];
                } else {
                    this.fileList = [];
                }

                this.open = true
                this.id = id
            },
            closeModal() {
                this.open = false
                this.fileList = [],
                this.isChangeBanner = false
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
                    if(this.isChangeBanner == true && this.fileList.length > 0 ){
                        formData.append('bannerImage', this.fileList[0].originFileObj);
                    }
                    const serverResponse = await APIService.put(`banner/update/${this.id}`, formData)
                    if (serverResponse.data.message == "Cập nhật thành công") {
                        notification.success({ message: "Thành công", description: serverResponse.data.message,key: 'loadingKey'})
                        this.isLoading = false

                        this.closeModal()
                        this.$emit('updateSuccess')
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