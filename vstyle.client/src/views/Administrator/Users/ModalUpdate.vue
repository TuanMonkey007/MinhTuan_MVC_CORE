<template>
    <div>
        <a-modal v-model:open="open" title="Chỉnh sửa tài khoản" :footer="null" width="700px" style="top: 20px;">
            <a-form ref="formRef" :model="account" layout="vertical">
                <a-row :gutter="30">
                    <a-col :span="12">

                        <a-form-item label="Avatar" name="avatar">
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
                    <a-col :span="12">
                        <a-form-item label="Giới tính" name="gender"
                            :rules="[{ required: true, message: 'Vui lòng chọn giới tính' }]">
                            <a-radio-group v-model:value="account.gender">
                                <a-radio v-for="item in listGender" :key="item.id" :value="item.id">{{ item.name
                                    }}</a-radio>
                            </a-radio-group>
                        </a-form-item>
                        <a-form-item label="Ngày sinh" name="birthDay" :rules="[{ validator: validateBirthDay }]">
                            <a-date-picker v-model:value="account.birthDay" format="YYYY-MM-DD"
                                placeholder="Chọn ngày sinh" />

                        </a-form-item>
                    </a-col>



                </a-row>

                <a-row :gutter="30">
                    <a-col :span="12">
                        <a-form-item label="Họ và tên" name="fullName"
                            :rules="[{ required: true, message: 'Vui lòng nhập thông tin này' }, { min: 5, max: 50, message: 'Độ dài 5-50 ký tự' }]">
                            <a-input type="text" v-model:value="account.fullName" />
                        </a-form-item>
                    </a-col>

                    <a-col :span="12">
                        <a-form-item label="Xác thực" name="EmailConfirmed">

                            <a-switch v-model:checked="checkEmailConfirmed" :loading="isLoading"
                                @change="handleChangeEmailcomfirmed">
                                <template #checkedChildren><font-awesome-icon :icon="['fas', 'thumbs-up']"
                                        style="color: #0fff;" /></template>
                                <template #unCheckedChildren><font-awesome-icon :icon="['fas', 'thumbs-down']"
                                        style="color: #0764fc;" /></template>
                            </a-switch>
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




                <a-row>
                    <a-col :span="24">
                        <a-form-item label="Địa chỉ" name="address">
                            <a-textarea v-model:value="account.address" />
                        </a-form-item>
                    </a-col>

                </a-row>
                <a-row>
                    <a-col :span="20" style="justify-content: center">
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
                    <a-col :span="4">
                        <a-form-item>
                            <a-switch v-model:checked="checkLockOut" :loading="isLoading" @change="handleChangeLockOut">
                                <template #checkedChildren><font-awesome-icon icon="fa-solid fa-lock"
                                        style="color: #bf0d0d;" /></template>
                                <template #unCheckedChildren><font-awesome-icon icon="fa-solid fa-unlock"
                                        style="color: #3d9900;" /></template>
                            </a-switch>
                        </a-form-item></a-col>
                </a-row>

            </a-form>

        </a-modal>
    </div>
</template>
<script>
    import dayjs from 'dayjs';
    import { computed, watch } from "vue";
    import { message, notification } from "ant-design-vue";
    import APIService from "@/helpers/APIService"
    import { ref, reactive } from "vue";


    export default {

        setup() {
            const validateBirthDay = (rule, value) => {


                // Kiểm tra xem ngày sinh có hợp lệ không (ví dụ: không được lớn hơn ngày hiện tại)
                if (dayjs(value).isAfter(dayjs(), 'day')) {
                    return Promise.reject('Ngày sinh không hợp lệ');
                }

                return Promise.resolve();
            };
            const beforeUpload = file => {
                const isJpgOrPng = file.type === 'image/jpeg' || file.type === 'image/png' || file.type === 'image/jpg';
                if (!isJpgOrPng) {
                    message.error('Định dạng ảnh không hợp lệ');
                }
                const isLt2M = file.size / 1024 / 1024 < 10;
                if (!isLt2M) {
                    message.error('Ảnh tối đa 10MB');
                }

                return isJpgOrPng && isLt2M;
            };
            return {
                beforeUpload,
                checkLockOut: ref(false),
                validateBirthDay,
                checkEmailConfirmed: ref(false),
            }
        },
        data() {
            const account = reactive({
                fullName: '',
                phoneNumber: '',
                email: '',
                gender: '',
                birthDay: '',
                address: '',
                avatar: '',
                avatarBase64: '',
                avatarContentType: '',
            });

            return {
                open: false,
                account,
                isLoading: ref(false),

                labelCol: {
                    span: 6,
                },
                wrapperCol: {
                    span: 14,
                },
                fileList: [],
                avatarBase64: null,
                avatarContentType: null,
                listGender: [],
                isChangeAvatar: false,
                

            }
        },
        mounted() {
            this.apiUrl = process.env.VUE_APP_URL + 'Account/valid-upload'; // Truy cập trong mounted
        },

        methods: {
            async handleChangeLockOut() {
                this.isLoading = true;
                const response = await APIService.get(`account/change-lock/${this.id}`);
                notification.success({
                    message: response.data.message,
                    key: 'loadingKey',
                    duration: 2
                });
                this.isLoading = false;
            },
            async handleChangeEmailcomfirmed() {
                this.isLoading = true;
                const response = await APIService.get(`account/change-email-confirmed/${this.id}`);
                notification.success({
                    message: response.data.message,
                    key: 'loadingKey',
                    duration: 2
                });
                this.isLoading = false;
            },
            handleChangeAvatar(info) {
                this.fileList = [...info.fileList];

                if (info.file.status == 'done') {
                    this.fileList = [...info.file];
                } else if (info.file.status == 'error') {
                    notification.error({
                        message: 'Tải ảnh thất bại',
                        key: 'loadingKey',
                        duration: 2
                    });
                    this.fileList = [];
                    info.file = null

                } else if (info.file.status == null) {
                    this.fileList = [];
                    info.file = null
                }
                this.isChangeAvatar = true;
            },

            async showModal(id) {
                const response = await APIService.get('datacategory/get-list-by-parent-code/GIOI_TINH');
                this.listGender = response.data.data;

                const serverResponse = await APIService.get(`account/${id}`)
                this.account = {
                    ...serverResponse.data.data,
                    birthDay: serverResponse.data.data.birthDay ? dayjs(serverResponse.data.data.birthDay) : null
                }; // Gán lại account.birthDay sau khi chuyển đổi
                this.avatarBase64 = serverResponse.data.data.avatarBase64;
                this.avatarContentType = serverResponse.data.data.avatarContentType;
                if (serverResponse.data.data.lockoutEnd && dayjs(serverResponse.data.data.lockoutEnd) > dayjs()) {
                    this.checkLockOut = true;
                } else {
                    this.checkLockOut = false;
                }
                this.checkEmailConfirmed = serverResponse.data.data.emailConfirmed;

                this.id = id
                // Kiểm tra nếu có avatar và chuyển đổi thành fileList
                if (this.avatarBase64) {
                    const byteCharacters = atob(this.avatarBase64);
                    const byteNumbers = new Array(byteCharacters.length);
                    for (let i = 0; i < byteCharacters.length; i++) { // Sửa vòng lặp for
                        byteNumbers[i] = byteCharacters.charCodeAt(i);
                    }
                    const byteArray = new Uint8Array(byteNumbers);
                    const blob = new Blob([byteArray], { type: this.avatarContentType });
                    const file = new File([blob], 'avatar', { type: this.avatarContentType });

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
            },
            closeModal() {
                this.open = false
                this.fileList = [],
                    this.checkLockOut = false,
                    this.checkEmailConfirmed = false,
                    this.isChangeAvatar = false,
                    this.avatarBase64 = null,
                    this.avatarContentType = null
                this.$refs.formRef.resetFields();
            },

            async handleSubmitAsync() {
                this.$refs.formRef.validate().then(async () => {
                    notification.info({
                        message: 'Đang xử lý...',
                        key: 'loadingKey',
                        duration: 0
                    });
                    this.isLoading = true
                    // Loại bỏ các trường rỗng
                    // const payload = Object.fromEntries(
                    //     Object.entries(this.account).filter(([_, v]) => v !== null && v !== undefined && v !== '')
                    // );
                    const payload = {
                        fullName: this.account.fullName,
                        phoneNumber: this.account.phoneNumber,
                        email: this.account.email,
                        gender: this.account.gender,
                        birthDay: this.account.birthDay
                            ? this.account.birthDay.format('YYYY-MM-DD') // Format nếu birthDay có giá trị
                            : null, // Hoặc một giá trị mặc định khác nếu birthDay là null
                        address: this.account.address,
                        avatar: this.account.avatar,
                    }
                    if (payload.birthDay == null) {
                        delete payload.birthDay;
                    }
                    const formData = new FormData();
                    for (const key in payload) {
                        formData.append(key, payload[key]);
                    }

                    if (this.isChangeAvatar == true && this.fileList.length > 0 && this.fileList[0].originFileObj != null) {
                        formData.append('avatarFile', this.fileList[0].originFileObj);
                    }



                    const response = await APIService.put(`account/update/${this.id}`, formData);//

                    if (response.data.message != 'Cập nhật thành công') {
                        const contentMessage = (response.data.message == 'DuplicateEmail') ? 'Email đã được đăng ký' : 'Số điện thoại đã được đăng ký';
                        notification.error({
                            message: 'Thất bại',
                            description: contentMessage,
                            key: 'loadingKey',
                            duration: 2
                        });

                    }
                    else {
                        notification.success({
                            message: 'Thành công',
                            description: 'Cập nhật tài khoản thành công',
                            key: 'loadingKey',
                            duration: 2
                        });
                        this.closeModal();
                        this.$emit('updateSuccess');
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