<template>
    <div>
        <a-modal v-model:open="open" title="Thêm tài khoản" :footer="null" width="700px">
            <a-form ref="formRef" :model="account" layout="vertical">
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
                <a-row :gutter="70">
                    <a-col :span="12">
                        <a-form-item label="Giới tính" name="gender">
                            <a-radio-group v-model:value="account.gender">
                                <a-radio value="1">Nam</a-radio>
                                <a-radio value="2">Nữ</a-radio>
                            </a-radio-group>
                        </a-form-item>
                        <a-form-item label="Ngày sinh" name="birthDay">
                            <a-date-picker v-model:value="account.birthDay" format="YYYY-MM-DD"
                                placeholder="Chọn ngày sinh" />
                        </a-form-item>
                    </a-col>
                    <a-col :span="12">

                        <a-form-item label="Avatar" name="avatar">
                            <a-upload action="https://www.mocky.io/v2/5cc8019d300000980a055e76" list-type="picture-card"
                                :defaultFileList="[{ url: account.avatar }]" :showUploadList="false"
                                :beforeUpload="beforeUpload" :customRequest="customRequest">
                                <template #default>
                                    <div>

                                        <font-awesome-icon v-if="!account.avatar" icon="fa-regular fa-image" />
                                        <img v-else :src="account.avatar" style="width: 100%" />
                                    </div>
                                </template>
                            </a-upload>
                        </a-form-item>
                    </a-col>


                </a-row>



                <a-row>
                    <a-col :span="24">
                        <a-form-item label="Địa chỉ" name="address">
                            <a-input type="text" v-model:value="account.address" />
                        </a-form-item>
                    </a-col>

                </a-row>
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
    import { message } from "ant-design-vue";
    import APIService from "@/helpers/APIService"
    import { ref, reactive } from "vue";
    import dayjs from 'dayjs';
    export default {
        setup() {
            const account = reactive({
                fullname: '',
                phoneNumber: '',
                email: '',
                gender: '',
                birthDay: '',
                address: '',
                avatar: '',
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
                console.log("jajaja")
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
                        payload.birthDay = dayjs(payload.birthDay).format('YYYY-MM-DD');

                    const response = await APIService.post('account/create', payload);

                    // Tắt hiệu ứng chờ sau 2 giây
                    if (response.data.message != 'Tạo tài khoản thành công') {
                        const contentMessage = (response.data.message == 'DuplicateEmail') ? 'Email đã được đăng ký' : 'Số điện thoại đã được đăng ký';
                        message.warning(
                            {
                                content: contentMessage,
                                key: 'loadingKey',
                                duration: 2
                            }
                        );
                        this.isLoading = false;


                    }
                    else {
                        message.success(
                            {
                                content: response.data.message,
                                key: 'loadingKey',
                                duration: 2
                            }
                        );
                        this.closeModal();
                        this.$emit('addSuccess')

                    }
                }).catch(error => {
                    console.log('error', error);
                    message.error(
                            {
                                content: 'Có lỗi xảy ra',
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