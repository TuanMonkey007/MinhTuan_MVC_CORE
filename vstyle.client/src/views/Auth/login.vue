<template>
    <a-row class="containerPage" >
        <a-col  :xs="0" :sm="0" :md="0" :lg="12" :xl="14"> </a-col>
        <a-col :xs="24" :sm="24" :md="24" :lg="12" :xl="10">
            <a-card class="login-card" style="
            background: white;
            margin-top: 30%;
            max-width: fit-content;
            padding: 20px;
            border-radius: 20px;
            box-shadow: rgba(150, 150, 150, 0.5) 0px 1px 20px 20px;
          "  >
         
                <a-tabs v-model:activeKey="activeKey" centered size="large">
                    <a-tab-pane key="LoginTab" force-render>
                        <template #tab>
                            <span>
                                <home-outlined />
                                Đăng nhập
                            </span>
                        </template>
                        <a-row>
                            <a-col :span="24">
                                <a-form :model="formLogin" name="normal_login" class="login-form" @finish="onFinish"
                                    @finishFailed="onFinishFailed">
                                    <a-form-item label="SĐT/Email" name="phoneOrEmail" :rules="[
                                        {
                                            required: true,
                                            message: 'Vui lòng nhập Số điện thoại / Email!',
                                        },
                                    ]">
                                        <a-input v-model:value="formLogin.phoneOrEmail">
                                            <template #prefix>
                                                <UserOutlined class="site-form-item-icon" />
                                            </template>
                                        </a-input>
                                    </a-form-item>

                                    <a-form-item label="Mật khẩu" name="password" :rules="[
                                        { required: true, message: 'Vui lòng nhập mật khẩu!' },
                                    ]">
                                        <a-input-password v-model:value="formLogin.password">
                                            <template #prefix>
                                                <LockOutlined class="site-form-item-icon" />
                                            </template>
                                        </a-input-password>
                                    </a-form-item>

                                    <a-form-item>
                                        <a-form-item name="remember" no-style>
                                            <a-checkbox v-model:checked="formLogin.remember">Ghi nhớ đăng
                                                nhập</a-checkbox>
                                        </a-form-item>
                                        <a class="login-form-forgot" @click="handleBtnForgot">Quên mật khẩu?</a>
                                    </a-form-item>

                                    <a-form-item>
                                        <a-button :disabled="disableBtnLogin" :loading="loading" type="primary"
                                            html-type="submit" style="margin-right: 20px" class="login-form-button">
                                            Đăng nhập
                                        </a-button>
                                    </a-form-item>
                                </a-form></a-col>
                        </a-row>
                    </a-tab-pane>
                    <a-tab-pane key="RegisterTab">
                        <template #tab>
                            <span>
                                <user-add-outlined />
                                Đăng ký
                            </span>
                        </template>
                        <a-row>
                            <a-col :span="24">
                                <a-form :model="formRegis" name="normal_login" class="login-form"
                                    @finish="onFinishRegister" v-bind="layout" @finishFailed="onFinishFailed">
                                    <a-form-item label="Họ tên" name="FullName" :rules="[
                                        { required: true, message: 'Vui lòng nhập họ tên!' },
                                    ]">
                                        <a-input v-model:value="formRegis.FullName">
                                            <template #prefix>
                                                <user-outlined />
                                            </template>
                                        </a-input>
                                    </a-form-item>

                                    <a-form-item label="Số điện thoại" name="PhoneNumber" :rules="[
                                        {
                                            required: true,
                                            message: 'Vui lòng nhập số điện thoại!',
                                        },
                                        {
                                            pattern: /^[0-9]{10}$/,
                                            message: 'Số điện thoại không hợp lệ!',
                                        },
                                    ]">
                                        <a-input v-model:value="formRegis.PhoneNumber">
                                            <template #prefix>
                                                <phone-outlined />
                                            </template>
                                        </a-input>
                                    </a-form-item>
                                    <a-form-item label="Email" name="Email" :rules="[
                                        { required: true, message: 'Vui lòng nhập email!' },
                                        { type: 'email', message: 'Email không hợp lệ!' },
                                    ]">
                                        <a-input v-model:value="formRegis.Email">
                                            <template #prefix>
                                                <MailOutlined class="site-form-item-icon" />
                                            </template>
                                        </a-input>
                                    </a-form-item>

                                    <a-form-item label="Mật khẩu" name="Password" :rules="[
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
                                        <a-input-password v-model:value="formRegis.Password">
                                            <template #prefix>
                                                <LockOutlined class="site-form-item-icon" />
                                            </template>
                                        </a-input-password>
                                    </a-form-item>
                                    <a-form-item label="Nhập lại mật khẩu" name="ConfirmPassword" :rules="[
                                        {
                                            required: true,
                                            message: 'Vui lòng nhập lại mật khẩu!',
                                        },
                                        { validator: checkConfirmPassword },
                                    ]">
                                        <a-input-password v-model:value="formRegis.ConfirmPassword">
                                            <template #prefix>
                                                <LockOutlined class="site-form-item-icon" />
                                            </template>
                                        </a-input-password>
                                    </a-form-item>

                                    <!--                            
                                <a-form-item>
                                    <a-button :disabled="disableBtnRegis" type="primary" html-type="submit"
                                        class="login-form-button">
                                        Đăng ký
                                    </a-button>


                                </a-form-item> -->

                                    <a-button :loading="loading" :disabled="disableBtnRegis" type="primary"
                                        html-type="submit" class="login-form-button">
                                        Đăng ký
                                    </a-button>
                                </a-form></a-col>
                        </a-row>
                    </a-tab-pane>
                    <a-tab-pane key="ForgotTab" force-render>
                        <template #tab>
                            <span>
                                <font-awesome-icon icon="fa-solid fa-backward" />
                                Quên mật khẩu
                            </span>
                        </template>
                        <a-row>
                            <a-col :span="24">
                                <a-form :model="formForgot" name="normal_login" class="login-form"
                                    @finish="onFinishForgot" @finishFailed="onFinishFailed">
                                    <a-form-item label="Email" name="Email" :rules="[
                                        { required: true, message: 'Vui lòng nhập email!' },
                                        { type: 'email', message: 'Email không hợp lệ!' },
                                    ]">
                                        <a-input v-model:value="formForgot.Email">
                                            <template #prefix>
                                                <MailOutlined class="site-form-item-icon" />
                                            </template>
                                        </a-input>
                                    </a-form-item>

                                    <a-form-item>
                                        <a-button :loading="loading" :disabled="disableBtnForgot" type="primary"
                                            html-type="submit" style="margin-right: 20px" class="login-form-button">
                                            Lấy lại mật khẩu
                                        </a-button>
                                    </a-form-item>
                                </a-form></a-col>
                        </a-row>
                    </a-tab-pane>
                </a-tabs>
            </a-card>
        </a-col>
    </a-row>
</template>
<script>
    import { Avatar, message } from "ant-design-vue";
    import { ClaimTypes, Role } from "@/helpers/Constants";
    import { jwtDecode } from "jwt-decode";
    import { reactive, computed } from "vue";
    import APIService from "@/helpers/APIService";
    import router from "@/router";
    import { defineComponent, ref, watch } from "vue";
    import {
        HomeOutlined,
        UserAddOutlined,
        UserOutlined,
        LockOutlined,
        PhoneOutlined,
        MailOutlined,
        FacebookOutlined,
        GoogleOutlined,
    } from "@ant-design/icons-vue";
    import EventBus from "@/helpers/EventBus";

    export default defineComponent({
        components: {
            HomeOutlined,
            UserAddOutlined,
            UserOutlined,
            LockOutlined,
            PhoneOutlined,
            MailOutlined,
            FacebookOutlined,
            GoogleOutlined,
        },
        setup() {
            const layout = {
                labelCol: {
                    span: 10,
                },
                wrapperCol: {
                    span: 14,
                },
            };
            const formRegis = reactive({
                FullName: "",
                PhoneNumber: "",
                Email: "",
                Password: "",
                ConfirmPassword: "",
            });
            const formLogin = reactive({
                phoneOrEmail: "",
                password: "",
                remember: true,
            });
            const formForgot = reactive({
                Email: "",
            });
            // Watch sự thay đổi của formRegis.Password
            watch(
                () => formRegis.Password,
                (newVal, oldVal) => {
                    if (newVal !== oldVal) {
                        formRegis.ConfirmPassword = ""; // Reset giá trị của ConfirmPassword
                    }
                }
            );
            const checkConfirmPassword = (rule, value, callback) => {
                if (value !== formRegis.Password) {
                    callback(new Error("Mật khẩu không khớp"));
                } else {
                    callback();
                }
            };
            const disableBtnLogin = computed(() => {
                return !formLogin.phoneOrEmail || !formLogin.password;
            });
            const disableBtnRegis = computed(() => {
                return (
                    !formRegis.FullName ||
                    !formRegis.PhoneNumber ||
                    !formRegis.Email ||
                    !formRegis.Password ||
                    !formRegis.ConfirmPassword
                );
            });
            const disableBtnForgot = computed(() => {
                return !formForgot.Email;
            });

            return {
                activeKey: ref("LoginTab"),
                formRegis,
                checkConfirmPassword,
                formLogin,
                disableBtnLogin,
                disableBtnRegis,
                layout,
                formForgot,
                disableBtnForgot,
                loading: ref(false),
            };
        }, //end setup

        methods: {
            handleBtnForgot() {
                this.activeKey = "ForgotTab";
            }, //end handleBtnForgot
            async onFinishForgot() {
                this.loading = true;
                message.loading({
                    content: "Đang xử lý...",
                    key: "loadingKey",
                    duration: 0,
                });
                try {
                    const forgotPasswordViewModel = {
                        Email: this.formForgot.Email,
                    };
                    const response = await APIService.post(
                        "account/forgot-password",
                        forgotPasswordViewModel
                    );

                    if (response.data.data != "") {
                        message.success({
                            content: response.data.message,
                            key: "loadingKey",
                            duration: 2,
                        });
                    } else {
                        message.warning({
                            content: response.data.message,
                            key: "loadingKey",
                            duration: 2,
                        });
                    }
                } catch {
                    message.error({
                        content: "Lấy lại mật khẩu thất bại!",
                        key: "loadingKey",
                        duration: 2,
                    });
                } finally {
                    setTimeout(() => {
                        message.destroy("loadingKey");
                        this.loading = false;
                    }, 2000);

                    this.loading = false;
                }
            }, //end onFinishForgot
            async onFinishRegister() {
                this.loading = true;
                message.loading({
                    content: "Đang xử lý...",
                    key: "loadingKey",
                    duration: 0,
                });
                try {
                    const registerViewModel = {
                        FullName: this.formRegis.FullName,
                        PhoneNumber: this.formRegis.PhoneNumber,
                        Email: this.formRegis.Email,
                        Password: this.formRegis.Password,
                        ConfirmPassword: this.formRegis.ConfirmPassword,
                    };
                    const response = await APIService.post(
                        "account/register",
                        registerViewModel
                    );
                    // Tắt hiệu ứng chờ sau 2 giây
                    if (response.data.message != "Tạo tài khoản thành công") {
                        const contentMessage =
                            response.data.message == "DuplicateEmail"
                                ? "Email đã được đăng ký"
                                : "Số điện thoại đã được đăng ký";
                        message.warning({
                            content: contentMessage,
                            key: "loadingKey",
                            duration: 2,
                        });
                    } else {
                        message.success({
                            content: response.data.message,
                            key: "loadingKey",
                            duration: 2,
                        });
                        this.activeKey = "LoginTab";
                    }
                } catch (error) {
                    message.error("Đăng ký thất bại!");
                } finally {
                    this.loading = false;
                }
            },

            async onFinish() {
                this.loading = true;
                message.loading({
                    content: "Đang xử lý...",
                    key: "loadingKey",
                    duration: 0,
                });
                try {
                    const loginViewModel = {
                        phoneOrEmail: this.formLogin.phoneOrEmail,
                        password: this.formLogin.password,
                        // remember: this.formLogin.remember
                    };

                    const response = await APIService.post("account/login", loginViewModel);
                    // Parse the token to get user role and user name
                    if (response.data.data.length < 100) {
                        message.warning({
                            content: response.data.data,
                            key: "loadingKey",
                            duration: 2,
                        });
                    } else {
                        const decodedToken = jwtDecode(response.data.data);
                        const role = decodedToken[ClaimTypes.Role];
                        const userName = decodedToken[ClaimTypes.Name];
                        // Save token to local storage
                        localStorage.setItem("accessToken", response.data.data);
                        // Save user role and user name to local storage
                        localStorage.setItem("role", role);
                        localStorage.setItem("userName", userName);

                        //check role and redirect to the corresponding page
                        if (role === Role.ADMIN || role === Role.STAFF) {
                            router.push({ name: "AdminHome" });
                        } else {
                            router.push({ name: "CustomerHome" });
                        }

                        message.success({
                            content: "Đăng nhập thành công!",
                            key: "loadingKey",
                            duration: 2,
                        });
                    }
                } catch (error) {
                    message.error({
                        content: "Lỗi server",
                        key: "loadingKey",
                        duration: 2,
                    });
                } finally {
                    this.loading = false;
                }
            },
            async onFinishFailed(errorInfo) {
                console.log("Failed:", errorInfo);
            },
        }, //end methods
    }); //end export default
</script>
<style scoped>

.containerPage {
        background-image: url("@/assets/image/background_login.jpg") !important;
        background-size: cover !important;
        min-height: 100vh;
        min-width: 100%
    }

    .full-screen-spin {
        align-items: center;
    }

    .custom-spin-wrapper {
        display: flex;
        align-items: center;
        justify-content: center;
        height: 100vh;
        /* Chiều cao 100% của màn hình */
        width: 100%;
        /* Chiều rộng 100% của màn hình */
    }

    .login-form-forgot {
        float: right;
    }

    .login-form-button {
        width: 100%;
    }
</style>
