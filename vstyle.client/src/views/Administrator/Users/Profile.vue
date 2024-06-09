<template>
    <a-row>
        <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <a-page-header style="
         border: 1px solid rgb(235, 237, 240);
          background-color: #c21f24;
        " @back="goBack">
                <template #backIcon>
                    <font-awesome-icon :icon="['fas', 'backward']" style="color: white; margin-left: 30px" />
                </template>
                <template #title>
                    <span style="color: white; font-size: 16px; margin-bottom: auto;display: block;">Trang cá
                        nhân</span>
                </template>
                <template #extra>
                    <a-breadcrumb separator=">" style="margin-right:30px;">
                        <a-breadcrumb-item href="">
                            <font-awesome-icon icon="fa-solid fa-house" style="color: #fff;" />
                            <span style="color: #fff;"> Trang quản trị</span>
                        </a-breadcrumb-item>
                        <a-breadcrumb-item href="">
                            <font-awesome-icon :icon="['fas', 'gear']" style="color: #fff;" />
                            <span style="color: #fff;"> Hệ thống</span>
                        </a-breadcrumb-item>
                        <a-breadcrumb-item href="">
                            <font-awesome-icon :icon="['fas', 'fa-user']" style="color: #fff;" />
                            <span style="color: #fff;"> Tài khoản</span>
                        </a-breadcrumb-item>

                    </a-breadcrumb>
                </template>
            </a-page-header>
        </a-col>
    </a-row>

    <transition name="route" mode="out-in" appear>
        <a-row>
            <a-col :xs="24" :sm="24" :md="6" :lg="6" :xl="6">
                <a-card :bordered="true" style="margin:20px; min-height: 25vh">
                    <a-row justify="center" align="center">
                        <a-col :span="24" style="display: flex;align-items: center;justify-content: center;">
                            <a-badge>
                                <template #count>
                                    <font-awesome-icon v-if="checkEmailConfirmed" icon="fa-regular fa-thumbs-up"
                                        style="color: #015efe; font-size: 16px;" />
                                    <font-awesome-icon v-else icon="fa-regular fa-thumbs-down"
                                        style="color: #aaaeb6;" />
                                </template>
                                <a-avatar v-if="avatarBase64"
                                    :src="'data:' + avatarContentType + ';base64,' + avatarBase64"
                                    :size="100"></a-avatar>
                                <a-avatar v-else :size="100"></a-avatar>
                            </a-badge>
                        </a-col>
                        <a-col style="display: flex;align-items: center;justify-content: center;" :span="24">
                            <h3>{{ this.account.fullName }}</h3>
                        </a-col>
                        <a-col style="display: flex;align-items: center; justify-content: center;" :span="24">{{
                            this.account.email }}</a-col>
                        <a-col style="display: flex;align-items: center; justify-content: center;    font-size: 12px;
    font-style: italic;
    font-weight: 300;" :span="24">{{
        checkEmailConfirmed ? "Đã xác thực" : " Chưa xác thực" }}</a-col>

                    </a-row>
                </a-card>
            </a-col>
            <a-col :xs="24" :sm="24" :md="16" :lg="16" :xl="16">
                <a-card :bordered="true" style="margin:20px;min-height: 50vh">
                    <a-row>
                        <a-col :span="24">
                            <a-tabs v-model:activeKey="activeKey">
                                <a-tab-pane key="1" tab="Tổng quan">
                                    <a-row>
                                        <a-col :span="24">
                                            <a-descriptions title="Thông tin cá nhân">
                                                <a-descriptions-item label="Họ và tên">{{ this.account.fullName
                                                    }}</a-descriptions-item>
                                                <a-descriptions-item label="Ngày sinh">{{ formattedBirthDay
                                                    }}</a-descriptions-item>

                                                <a-descriptions-item label="Giới tính">{{ this.account.nameGender
                                                    }}</a-descriptions-item>

                                                <a-descriptions-item label="Số điện thoại">{{ this.account.phoneNumber
                                                    }}</a-descriptions-item>
                                                <a-descriptions-item label="Email">{{ this.account.email
                                                    }}</a-descriptions-item>
                                                <a-descriptions-item label="Địa chỉ">{{ this.account.address
                                                    }}</a-descriptions-item>
                                            </a-descriptions>

                                        </a-col>
                                    </a-row>

                                </a-tab-pane>
                                <a-tab-pane key="2" tab="Cập nhật thông tin" force-render>
                                    <a-form ref="formRef" :model="account" layout="vertical">
                                        <a-row :gutter="30">
                                            <a-col :span="12">

                                                <a-form-item label="Avatar" name="avatar">
                                                    <a-upload list-type="picture-card" :showUploadList="true"
                                                        accept=".jpg,.jpeg,.png" maxCount="1" :fileList="fileList"
                                                        :before-upload="beforeUpload" :action="apiUrl"
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
                                                        <a-radio v-for="item in listGender" :key="item.id"
                                                            :value="item.id">{{ item.name
                                                            }}</a-radio>
                                                    </a-radio-group>
                                                </a-form-item>
                                                <a-form-item label="Ngày sinh" name="birthDay"
                                                    :rules="[{ validator: validateBirthDay }]">
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


                                        <a-row justify="space-between">
                                            <a-col :xs="24" :sm="24" :md="7" :lg="7" :xl="7">
                                                <a-form-item label="Tỉnh" name="provinceId"
                                                    :rules="[{ required: true, message: 'Chọn Tỉnh/Thành phố' }]">
                                                    <a-select show-search optionFilterProp="label"
                                                        v-model:value="account.provinceId" :options="provinceOptions"
                                                        @change="handleChangeProvince" />
                                                </a-form-item></a-col>
                                            <a-col :xs="24" :sm="24" :md="7" :lg="7" :xl="7">
                                                <a-form-item label="Huyện" name="districtId"
                                                    :rules="[{ required: true, message: 'Chọn Quận/Huyện' }]">
                                                    <a-select show-search optionFilterProp="label"
                                                        v-model:value="account.districtId" :options="districtOptions"
                                                        @change="handleChangeDistrict" />
                                                </a-form-item></a-col>
                                            <a-col :xs="24" :sm="24" :md="7" :lg="7" :xl="7">
                                                <a-form-item label="Xã" name="wardId"
                                                    :rules="[{ required: true, message: 'Chọn Huyện/Xã' }]">
                                                    <a-select show-search optionFilterProp="label"
                                                        v-model:value="account.wardId" :options="wardOptions"
                                                        @change="handleChangeWard" />
                                                </a-form-item></a-col>
                                        </a-row>

                                        <a-row>
                                            <a-col :span="24">
                                                <a-form-item label="Địa chỉ" name="addressChange">
                                                    <a-textarea v-model:value="this.addressChange" />
                                                </a-form-item>
                                            </a-col>

                                        </a-row>
                                        <a-row>
                                            <a-col :span="20" style="justify-content: center">
                                                <a-form-item>
                                                    <a-button @click="handleSubmitAsync" :disabled="disableBtnSubmit"
                                                        :loading="isLoading" type="primary" style="margin-right: 20px;"
                                                        class="login-form-button">
                                                        Hoàn thành
                                                    </a-button>

                                                </a-form-item>

                                            </a-col>

                                        </a-row>

                                    </a-form>

                                </a-tab-pane>
                                <a-tab-pane key="3" tab="Đổi mật khẩu">
                                    <a-row justify="center">
                                        <a-col :span="18" style="margin-top: 30px;">
                                            <a-form :model="formChangePassword" :ref="formChangePassword">
                                                <a-form-item label="Mật khẩu cũ" name="password"
                                                    :rules="[{ required: true, message: 'Nhập mật khẩu cũ' }]">
                                                    <a-input-password v-model:value="formChangePassword.password">
                                                        <template #prefix>
                                                            <font-awesome-icon :icon="['fas', 'lock']" />
                                                        </template></a-input-password>
                                                </a-form-item>
                                                <a-form-item label="Mật khẩu mới" name="newPassword" :rules="[
                                                    { required: true, message: 'Vui lòng nhập mật khẩu!' },
                                                    { min: 8, message: 'Tôi thiểu 8 ký tự' },
                                                    { max: 25, message: 'Tối đa 25 ký tự' },
                                                    {
                                                        pattern:
                                                            /^(?=.*[0-9])[a-zA-Z0-9!#$%&()*+,\-./:;<=>?@[\\\]^_`{|}~]+$/,
                                                        message: 'Tối thiểu 1 số',
                                                    },

                                                    {
                                                        pattern:
                                                            /^(?=.*[a-z])[a-zA-Z0-9!#$%&()*+,\-./:;<=>?@[\\\]^_`{|}~]+$/,
                                                        message: 'Tối thiểu 1 chữ thường',
                                                    },

                                                    {
                                                        pattern:
                                                            /^(?=.*[A-Z])[a-zA-Z0-9!#$%&()*+,\-./:;<=>?@[\\\]^_`{|}~]+$/,
                                                        message: 'Tối thiểu 1 chữ hoa',
                                                    },
                                                    {
                                                        pattern:
                                                            /^(?=.*[!#$%&()*+,\-./:;<=>?@[\\\]^_`{|}~])[a-zA-Z0-9!#$%&()*+,\-./:;<=>?@[\\\]^_`{|}~]+$/,
                                                        message: 'Ít nhất 1 ký tự đặc biệt',
                                                    },
                                                ]">
                                                    <a-input-password v-model:value="formChangePassword.newPassword">
                                                        <template #prefix>
                                                            <font-awesome-icon :icon="['fas', 'lock']" />
                                                        </template></a-input-password>
                                                </a-form-item>
                                                <a-form-item label="Nhập lại mật khẩu" name="confirmPassword" :rules="[
                                                    {
                                                        required: true,
                                                        message: 'Vui lòng nhập lại mật khẩu!',
                                                    },
                                                    { validator: checkConfirmPassword },
                                                ]">
                                                    <a-input-password
                                                        v-model:value="formChangePassword.confirmPassword">
                                                        <template #prefix>
                                                            <font-awesome-icon :icon="['fas', 'lock']" />
                                                        </template></a-input-password>
                                                </a-form-item>
                                                <a-form-item>
                                                    <a-button type="primary" @click="handleChangePassword">Đổi mật
                                                        khẩu</a-button>
                                                </a-form-item>
                                            </a-form>
                                        </a-col>
                                    </a-row>
                                </a-tab-pane>
                            </a-tabs>
                        </a-col>
                    </a-row>
                </a-card>
            </a-col>
        </a-row>
    </transition>

</template>
<script>
    import axios from 'axios';
    import { ref, reactive } from 'vue';
    import dayjs from 'dayjs';
    import { notification } from 'ant-design-vue';
    import APIService from '@/helpers/APIService';
    export default {
        setup() {
            const formChangePassword = reactive({
                password: '',
                newPassword: '',
                confirmPassword: '',
            });
            const checkConfirmPassword = (rule, value, callback) => {
                if (value !== formChangePassword.newPassword) {
                    callback(new Error("Mật khẩu không khớp"));
                } else {
                    callback();
                }
            };
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
                    notification.error({
                        message: 'Thông báo',
                        description: 'Ảnh phải có định dạng JPG/PNG!',
                    });
                }
                const isLt2M = file.size / 1024 / 1024 < 10;
                if (!isLt2M) {
                    notification.error({
                        message: 'Thông báo',
                        description: 'Ảnh phải nhỏ hơn 10MB!',
                    });
                }

                return isJpgOrPng && isLt2M;
            };
            const activeKey = ref('1');
            return {
                activeKey,
                beforeUpload,
                checkEmailConfirmed: ref(false),
                validateBirthDay,
                checkConfirmPassword,
                formChangePassword,

            };
        },
        methods: {
            goBack() {
                this.$router.go(-1);
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
                     
                        avatar: this.account.avatar,
                        address: `${this.addressChange} | ${this.selectedWard}, ${this.selectedDistrict}, ${this.selectedProvince}`,
                        provinceId: this.account.provinceId,
                        districtId: this.account.districtId,
                        wardId: this.account.wardId,
                    }
                    console.log(payload)
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
                        window.location.reload();
                    }

                }).catch(error => {
                    console.log('error', error);
                }).finally(() => {
                    this.isLoading = false;
                })


            },
            async handleChangePassword() {

                notification.info({
                    message: 'Đang xử lý...',
                    key: 'loadingKey',
                    duration: 0
                });
                this.isLoading = true
                const response = await APIService.put(`account/change-password/${this.id}`, {
                    password: this.formChangePassword.password,
                    newPassword: this.formChangePassword.newPassword,
                });
                console.log(response.data);
                if (response.data.message != 'Đổi mật khẩu thành công') {
                    notification.error({
                        message: 'Thất bại',
                        description: 'Mật khẩu cũ không đúng',
                        key: 'loadingKey',
                        duration: 2
                    });
                }
                else {
                    notification.success({
                        message: 'Thành công',
                        description: 'Đổi mật khẩu thành công',
                        key: 'loadingKey',
                        duration: 2
                    });
                    window.location.reload();
                }


            },

            async fetchProvice() {
                try {
                    // Tạo Axios instance mới
                    const apiClient = axios.create();

                    apiClient.interceptors.request.use(config => {
                        config.headers['token'] = process.env.VUE_APP_GHN_TOKEN; // Gán token trực tiếp
                        return config;
                    });
                    const response = await apiClient.get('https://online-gateway.ghn.vn/shiip/public-api/master-data/province');
                    this.provinceOptions = response.data.data.map(province => ({
                        label: province.ProvinceName,
                        value: province.ProvinceID
                    }));


                } catch (error) {
                    notification.error({
                        message: 'Lỗi',
                        description: 'Không thể lấy danh sách tỉnh thành'
                    });
                }
            },// Hàm xử lý khi chọn tỉnh
            async handleChangeProvince(value, label) {
                try {

                    // Tạo Axios instance mới
                    const apiClient = axios.create();

                    apiClient.interceptors.request.use(config => {
                        config.headers['token'] = process.env.VUE_APP_GHN_TOKEN; // Gán token trực tiếp
                        return config;
                    });

                    this.selectedProvince = label.label
                    this.selectedDistrict = null
                    this.selectedWard = null
                    this.account.districtId = null;
                    this.account.wardId = null;
                    this.districtOptions = [];
                    this.wardOptions = [];
                    const response = await apiClient.get(`https://online-gateway.ghn.vn/shiip/public-api/master-data/district?province_id=${value}`);
                    this.districtOptions = response.data.data.map(district => ({
                        label: district.DistrictName,
                        value: district.DistrictID
                    }));
                } catch (error) {
                    notification.error({
                        message: 'Lỗi',
                        description: 'Không thể lấy danh sách huyện'
                    });
                }
            },// Hàm xử lý khi chọn huyện
            async handleChangeDistrict(value, label) {
                try {
                    // Tạo Axios instance mới
                    const apiClient = axios.create();

                    apiClient.interceptors.request.use(config => {
                        config.headers['token'] = process.env.VUE_APP_GHN_TOKEN; // Gán token trực tiếp
                        return config;
                    });

                    this.account.wardId = null;
                    this.selectedDistrict = label.label
                    this.selectedWard = null
                    this.wardOptions = [];
                    const response = await apiClient.get(`https://online-gateway.ghn.vn/shiip/public-api/master-data/ward?district_id=${value}`);
                    this.wardOptions = response.data.data.map(ward => ({
                        label: ward.WardName,
                        value: ward.WardCode
                    }));
                } catch (error) {
                    notification.error({
                        message: 'Lỗi',
                        description: 'Không thể lấy danh sách xã'
                    });
                }
            },// Hàm xử lý khi chọn xã
            handleChangeWard(value,label) {
                this.selectedWard = label.label;
            }
        },
        computed: {
            formattedBirthDay() {
                return this.account.birthDay ? dayjs(this.account.birthDay).format('DD/MM/YYYY') : '';
            },
        },
        async mounted() {
            this.apiUrl = process.env.VUE_APP_URL + 'Account/valid-upload'; // Truy cập trong mounted
            const response = await APIService.get('datacategory/get-list-by-parent-code/GIOI_TINH');
            this.listGender = response.data.data;
            const email = localStorage.getItem('userEmail');
            const serverResponse = await APIService.get(`account/get-user-by-email/${email}`)
            this.account = {
                ...serverResponse.data.data,
                birthDay: serverResponse.data.data.birthDay ? dayjs(serverResponse.data.data.birthDay) : null
            }; // Gán lại account.birthDay sau khi chuyển đổi
            const fullAddress = this.account.address
            const addressParts = fullAddress.split('|');
            this.addressChange = addressParts[0].trim(); // Lấy phần tử đầu tiên và loại bỏ khoảng trắng thừa
            this.id = serverResponse.data.data.id;
            this.avatarBase64 = serverResponse.data.data.avatarBase64;
            this.avatarContentType = serverResponse.data.data.avatarContentType;
            if (serverResponse.data.data.lockoutEnd && dayjs(serverResponse.data.data.lockoutEnd) > dayjs()) {
                this.checkLockOut = true;
            } else {
                this.checkLockOut = false;
            }
            this.checkEmailConfirmed = serverResponse.data.data.emailConfirmed;
            await this.fetchProvice()   

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
            // Tạo Axios instance mới
            const apiClient = axios.create();

            apiClient.interceptors.request.use(config => {
                config.headers['token'] = process.env.VUE_APP_GHN_TOKEN; // Gán token trực tiếp
                return config;
            });

            if (this.account.provinceId != null) {
                const responseDistrict = await apiClient.get(`https://online-gateway.ghn.vn/shiip/public-api/master-data/district?province_id=${this.account.provinceId}`);
                this.districtOptions = responseDistrict.data.data.map(district => ({
                    label: district.DistrictName,
                    value: district.DistrictID,
                }));
                this.selectedProvince = this.provinceOptions.find(province => province.value === this.account.provinceId).label
                this.selectedDistrict = this.districtOptions.find(district => district.value === this.account.districtId).label


                this.account.wardId = this.account.wardId.toString()
                if (this.account.districtId != null) {
                    const responseWard = await apiClient.get(`https://online-gateway.ghn.vn/shiip/public-api/master-data/ward?district_id=${this.account.districtId}`);
                    this.wardOptions = responseWard.data.data.map(ward => ({
                        label: ward.WardName,
                        value: ward.WardCode,

                    }));
                    this.selectedWard = this.wardOptions.find(ward => ward.value === this.account.wardId).label
                }
            
            }

        },



        data() {
            const account = reactive({
                fullName: '',
                phoneNumber: '',
                email: '',
                gender: '',
                nameGender: '',
                birthDay: '',
                address: '',
                avatar: '',
                avatarBase64: '',
                avatarContentType: '',
                provinceId: '',
                districtId: '',
                wardId: '',
            });

            return {

                account,
                fileList: [],
                avatarBase64: null,
                avatarContentType: null,
                listGender: [],
                isChangeAvatar: false,
                apiUrl: '',
                id: '',

                provinceOptions: [],
                selectedProvince: '',
                districtOptions: [],
                selectedDistrict: '',
                wardOptions: [],
                selectedWard: '',
                addressChange: '',

            }
        },//end data
    }
</script>
<style scoped>
    .ant-page-header {
        padding: 0px;
        /* Giảm padding */
        margin-bottom: 8px;
        /* Giảm margin dưới */
    }

    /* Ẩn chữ trên màn hình nhỏ hơn hoặc bằng 768px */
    @media (max-width: 767px) {
        .ant-breadcrumb-link>span {
            display: none;
        }
    }

    .ant-page-header {
        height: 35px;
        /* Tự động điều chỉnh chiều cao */
    }

    :deep(.ant-breadcrumb-separator) {
        color: #fff;
        /* Đặt màu chữ cho breadcrumb */
    }

    .ant-page-header-heading-title {
        line-height: normal;
        /* Đặt màu chữ cho tiêu đề */
        display: flex;
        /* Hiển thị theo chiều ngang */
    }
</style>