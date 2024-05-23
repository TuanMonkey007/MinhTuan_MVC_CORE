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
    import dayjs from 'dayjs';
    import { computed, watch } from "vue";
    import { message } from "ant-design-vue";
    import APIService from "@/helpers/APIService"
    import { ref, reactive } from "vue";

    export default {

        data() {
            const account = reactive({
                fullName: '',
                phoneNumber: '',
                email: '',
                gender: '',
                birthDay: '',
                address: '',
                avatar: '',


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
            }
        },

        methods: {

            async showModal(id) {
                this.open = true
                const serverResponse = await APIService.get(`account/${id}`)
                this.account = {
                    ...serverResponse.data.data,
                    birthDay: serverResponse.data.data.birthDay ? dayjs(serverResponse.data.data.birthDay) : null
                }; // Gán lại account.birthDay sau khi chuyển đổi


                this.id = id
            },
            closeModal() {
                this.open = false

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


                    const response = await APIService.put(`account/update/${this.id}`, payload);
                    // if (response.data.message == "Cập nhật thành công") {
                    //     message.success(response.data.message);
                    //     this.closeModal();
                    //     this.$emit('updateSuccess');
                    // } else {
                    //     message.error(response.data.message);
                    // }
                     // Tắt hiệu ứng chờ sau 2 giây
                     if (response.data.message != 'Cập nhật thành công') {
                        const contentMessage = (response.data.message == 'DuplicateEmail') ? 'Email đã được đăng ký' : 'Số điện thoại đã được đăng ký';
                        message.warning(
                            {
                                content: contentMessage,
                                key: 'loadingKey',
                                duration: 2
                            }
                        );

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