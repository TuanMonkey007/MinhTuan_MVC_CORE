<template>
    <div>
        <a-modal v-model:open="open" title="Thêm tài khoản" :footer="null" width="700px" :afterClose="closeModal"
            style="top: 20px;">
            <a-divider />
            <a-form ref="formRef" :model="account" layout="vertical">
                <a-row :gutter="70">
                    <a-col :span="12">

                        <a-form-item label="Avatar" name="avatar">
                            <a-upload list-type="picture-card" :showUploadList="true" accept=".jpg,.jpeg,.png,.bmp,.webp"
                                maxCount="1" :fileList="fileList" :before-upload="beforeUpload"
                                :action="apiUrl" @change="handleChangeAvatar">


                                <template #default>
                                    <div>
                                        <font-awesome-icon icon="fa-regular fa-image" />
                                        <div class="ant-upload-text">Tải ảnh lên</div>
                                    </div>
                                </template>
                            </a-upload>
                        </a-form-item>
                    </a-col>
                    <a-col :span="12">
                        <a-form-item label="Giới tính" name="gender"   :rules="[{ required: true, message: 'Vui lòng chọn giới tính' }]">
                            <a-radio-group v-model:value="account.gender" >
                                <a-radio v-for="item in listGender" :key="item.id"
                                    :value="item.id">{{ item.name }}</a-radio>
                            </a-radio-group>
                        </a-form-item>
                        <a-form-item label="Ngày sinh" name="birthDay" :rules="[{ validator: validateBirthDay }]">
                            <a-date-picker v-model:value="account.birthDay" format="YYYY-MM-DD"
                                placeholder="Chọn ngày sinh" />
                        </a-form-item>
                    </a-col>



                </a-row>

                <a-row>
                    <a-col :span="24">
                        <a-form-item label="Họ và tên" name="fullName"
                            :rules="[{ required: true, message: 'Vui lòng nhập thông tin này' }]">
                            <a-input type="text" v-model:value="account.fullName" />
                        </a-form-item>
                    </a-col>
                    

                </a-row>
                <a-row :gutter="30">
                    <a-col :span="12">
                        <a-form-item label="Email" name="email"
                            :rules="[{ required: true, message: 'Vui lòng nhập thông tin này' }, { type: 'email', message: 'Email không hợp lệ' }]">
                            <a-input type="email" v-model:value="account.email" />
                        </a-form-item>
                    </a-col>
                    <a-col :span="12">
                        <a-form-item label="Số điện thoại" name="phoneNumber"
                            :rules="[{ required: true, message: 'Vui lòng nhập thông tin này' }, { pattern: /^[0-9]{10}$/, message: 'Số điện thoại không hợp lệ!' }]">
                            <a-input v-model:value="account.phoneNumber" />
                        </a-form-item>
                    </a-col>
                </a-row>
                <a-row :gutter="30">
                    <a-col :span="12">
                        <a-form-item label="Mật khẩu" name="password" :rules="[{ required: true, message: 'Vui lòng nhập thông tin này' },
                        { min: 8, message: 'Tôi thiểu 8 ký tự' },
                        { max: 25, message: 'Tối đa 25 ký tự' },
                        {
                            pattern: /^(?=.*[0-9])[a-zA-Z0-9!#$%&()*+,\-./:;<=>?@[\\\]^_`{|}~]+$/,
                            message: 'Tối thiểu 1 số'
                        },

                        {
                            pattern: /^(?=.*[a-z])[a-zA-Z0-9!#$%&()*+,\-./:;<=>?@[\\\]^_`{|}~]+$/,
                            message: 'Tối thiểu 1 chữ thường'
                        },

                        {
                            pattern: /^(?=.*[A-Z])[a-zA-Z0-9!#$%&()*+,\-./:;<=>?@[\\\]^_`{|}~]+$/,
                            message: 'Tối thiểu 1 chữ hoa'
                        }
                            ,
                        {
                            pattern: /^(?=.*[!#$%&()*+,\-./:;<=>?@[\\\]^_`{|}~])[a-zA-Z0-9!#$%&()*+,\-./:;<=>?@[\\\]^_`{|}~]+$/,
                            message: 'Ít nhất 1 ký tự đặc biệt'
                        }


                        ]">
                            <a-input-password v-model:value="account.password" placeholder="Yêu cầu nhập mật khẩu" />
                        </a-form-item>
                    </a-col>
                    <a-col :span="12">
                        <a-form-item label="Xác nhận mật khẩu" name="confirmPassword"
                            :rules="[{ required: true, message: 'Vui lòng nhập thông tin này' }, { validator: checkConfirmPassword }]">
                            <a-input-password type="password" v-model:value="account.confirmPassword" />
                        </a-form-item>
                    </a-col>
                </a-row>



                <!-- <a-row>
                    <a-col :span="24">
                        <a-form-item label="Địa chỉ" name="address">
                            <a-input type="text" v-model:value="account.address" />
                        </a-form-item>
                    </a-col>

                </a-row> -->
                <a-row>
                    <a-col :span="24" style="justify-content: center">
                        <a-form-item>
                            <a-button @click="handleSubmitAsync" :disabled="disableBtnSubmit" :loading="isLoading"
                                type="primary" style="margin-right: 20px;" class="login-form-button">
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
    import { computed, watch } from "vue";
    import { message,notification } from "ant-design-vue";
    import APIService from "@/helpers/APIService"
    import { ref, reactive } from "vue";
    import dayjs from 'dayjs';
    export default {
        setup() {
            const beforeUpload = file => {
                // Danh sách các phần mở rộng tệp được chấp nhận
                const allowedExtensions = ['jpg', 'jpeg', 'png', 'bmp', 'webp'];
                var isJpgOrPng = true;
                // Kiểm tra phần mở rộng file
                const extension = file.name.split('.').pop().toLowerCase();
                console.log(extension)
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
            const account = reactive({
                fullname: '',
                phoneNumber: '',
                email: '',
                gender: '',
                birthDay: '',
                address: '',
                password: '',
                confirmPassword: ''

            });
            const checkConfirmPassword = (rule, value, callback) => {
                if (value !== account.password) {
                    callback(new Error('Mật khẩu không khớp'));
                } else {
                    callback();
                }
            };
            // Watch sự thay đổi của formRegis.Password
            watch(
                () => account.password,
                (newVal, oldVal) => {
                    if (newVal !== oldVal) {
                        account.confirmPassword = ''; // Reset giá trị của ConfirmPassword
                    }
                }
            );
            const validateBirthDay = (rule, value) => {


                // Kiểm tra xem ngày sinh có hợp lệ không (ví dụ: không được lớn hơn ngày hiện tại)
                if (dayjs(value).isAfter(dayjs(), 'day')) {
                    return Promise.reject('Ngày sinh không hợp lệ');
                }

                return Promise.resolve();
            };
            return {
                account,

                isLoading: ref(false),

                labelCol: {
                    span: 6,
                },
                wrapperCol: {
                    span: 14,
                },
                checkConfirmPassword,
                beforeUpload,
                validateBirthDay


            }
        },// end setup
        data() {
            return {
                open: false,

                fileList: [],
                listGender: [],
                apiUrl: '',
            }
        },
        mounted() {
            this.apiUrl = process.env.VUE_APP_URL + 'Account/valid-upload'; // Truy cập trong mounted
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
                            description: `${info.file.name} Lỗi .`,
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


            formatInput(event) {
                // Chuyển đổi thành chữ in hoa
                let inputValue = event.target.value.toUpperCase();
                // Loại bỏ khoảng trắng
                inputValue = inputValue.replace(/\s+/g, '');
                inputValue = inputValue.replace(/[^A-Z0-9_-]/g, '');
                this.category.code = inputValue;
            },
            async showModal() {
                this.open = true
                const response = await APIService.get('datacategory/get-list-by-parent-code/GIOI_TINH');
                this.listGender = response.data.data;
            },
            closeModal() {
                this.open = false
                this.fileList = [];
                this.$refs.formRef.resetFields();
            },
           
            async handleSubmitAsync() {
                this.$refs.formRef.validate().then(async () => {
                    message.loading(
                        {
                            content: 'Đang xử lý...',
                            key: 'loadingKey',
                            duration: 0
                        }
                    );
                    this.isLoading = true
                    // Loại bỏ các trường rỗng
                    const payload = Object.fromEntries(
                        Object.entries(this.account).filter(([_, v]) => v !== null && v !== undefined && v !== '')
                    );
                    if (payload.birthDay != null && payload.birthDay != '') {
                        payload.birthDay = dayjs(payload.birthDay).format('YYYY-MM-DD');
                    }

                    const formData = new FormData();
                    for (const key in payload) {
                        formData.append(key, payload[key]);
                    }

                    if (this.fileList.length > 0 && this.fileList[0].originFileObj != null) {
                        formData.append('avatarFile', this.fileList[0].originFileObj);
                    }

                    // for (const [key, value] of formData.entries()) {
                    //     console.log(key, value);
                    // }


                    const response = await APIService.post('account/create', formData);

                    // Tắt hiệu ứng chờ sau 2 giây
                    if (response.data.message != 'Tạo tài khoản thành công') {
                        const contentMessage = (response.data.message == 'DuplicateEmail') ? 'Email đã được đăng ký' : 'Số điện thoại đã được đăng ký';
                       notification.error(
                            {
                                message: 'Thông báo',
                                description: contentMessage,
                                key: 'loadingKey',
                                duration: 2
                            }
                        );
                        this.isLoading = false;


                    }
                    else {
                        notification.success(
                            {
                                message: 'Thông báo',
                                description: response.data.message,
                                key: 'loadingKey',
                                duration: 2
                            }
                        );
                        this.closeModal();
                        this.$emit('addSuccess')

                    }
                }).catch(error => {
                    console.log('error', error);
                    notification.error(
                        {
                            message: 'Thông báo',
                            description: 'Vui lòng kiểm tra lại thông tin', // Thông báo lỗi
                            key: 'loadingKey',
                            duration: 2
                        }
                    );
                }).finally(() => {
                    this.isLoading = false;
                })


            }

        }
    }

</script>