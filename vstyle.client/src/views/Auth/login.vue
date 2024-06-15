<template>
    <a-modal v-model:open="open"  :footer="null" width="500px" style="top:50px">
    <a-row justify="center">
        <a-col :span="24" >
                <a-tabs v-model:activeKey="activeKey" centered >
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
                                        <a class="login-form-forgot" style="color: #db3a35; " @click="handleBtnForgot">Quên mật khẩu?</a>
                                    </a-form-item>

                                    <a-form-item>
                                        <a-button :disabled="disableBtnLogin" :loading="loading" type="primary"
                                            html-type="submit" style="width: 100%;margin-right: 20px" class="login-form-button">
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
            
        </a-col>
    </a-row>
</a-modal>
</template>
<script>
    import { Avatar, message,notification } from "ant-design-vue";
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
        data() {
            return {
                open: false
            }
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
            showModal() {
                this.open = true
            },
            handleBtnForgot() {
                this.activeKey = "ForgotTab";
            }, //end handleBtnForgot
            async onFinishForgot() {
                this.loading = true;
                notification.info({
                    message: "Đang xử lý...",
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
                        notification.success({
                            message: "Thành công",
                            description: response.data.data,
                            key: "loadingKey",
                            duration: 2,
                        });
                    } else {
                       notification.error({
                            message: "Thất bại",
                            description: response.data.message,
                            key: "loadingKey",
                            duration: 2,
                        });
                    }
                } catch {
                    notification.error({
                        message: "Thất bại",
                        description: "Lỗi server",
                        key: "loadingKey",
                        duration: 2,
                    });
                } finally {
                    this.loading = false;
                }
            }, //end onFinishForgot
            async onFinishRegister() {
                this.loading = true;
                notification.info({
                    message: "Đang xử lý...",
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
                        notification.error({
                            message: "Thất bại",
                            description: contentMessage,
                            key: "loadingKey",
                            duration: 2,
                        });
                    } else {
                        notification.success({
                            message: "Thành công",
                            description: "Đăng ký thành công",
                            key: "loadingKey",
                            duration: 2,
                        });
                        this.activeKey = "LoginTab";
                    }
                } catch (error) {
                    notification.error({
                        message: "Thất bại",
                        description: error.message,
                        key: "loadingKey",
                        duration: 2,
                    });
                } finally {
                    this.loading = false;
                }
            },

            async onFinish() {
                this.loading = true;
                notification.info({
                    message: "Đang xử lý...",
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
                        notification.error({
                            message: "Thất bại",
                            description: response.data.data,
                            key: "loadingKey",
                            duration: 2,
                        });
                    } else {
                        const decodedToken = jwtDecode(response.data.data);
                        var role = decodedToken[ClaimTypes.Role];
                        const userName = decodedToken[ClaimTypes.Name];
                        const userPhone = decodedToken[ClaimTypes.MobilePhone];
                        const userEmail = decodedToken[ClaimTypes.Email];
                        // Kiểm tra và chuyển đổi nếu cần
                        if (typeof role === 'string') {
                            role = [role]; // Chuyển chuỗi thành mảng
                        }
                        const defaultRole = role[0];
                        // Save token to local storage
                        localStorage.setItem("accessToken", response.data.data);
                        // Save user role and user name to local storage
                        localStorage.setItem("role", defaultRole);
                        localStorage.setItem("userName", userName);
                        localStorage.setItem("userPhone", userPhone);
                        localStorage.setItem("userEmail", userEmail);

                        //check role and redirect to the corresponding page
                        if (defaultRole === Role.ADMIN || defaultRole === Role.STAFF) {
                            router.push({ name: "AdminHome" });
                            //Gửi api lấy dữ liệu 

                        } else {
                            router.push({ name: "CustomerHome" }).then(() => {
                                window.location.reload();
                            });
                        }
                        const resUserCartId = await APIService.get(`cart/get-user-cart-id/${userEmail}`);
                        if (resUserCartId.data.message != null) {
                            localStorage.setItem("userCartId", resUserCartId.data.message);
                        }
                        console.log(resUserCartId)
                        notification.success({
                            message: "Thành công",
                            description: "Đăng nhập thành công",
                            key: "loadingKey",
                            duration: 2,
                        });
                    }
                } catch (error) {
                   notification.error({
                        message: "Thất bại",
                        description: error.message,
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
.login-form-button {
    width: 100%;
    background-color: #db3a35;
    color: #ffffff;
    border-radius: 48px;
    font-size: 14px;
    font-weight: 500;
}
:deep(.ant-tabs-tab.ant-tabs-tab-active .ant-tabs-tab-btn) {
    color: #db3a35;
    text-shadow: 0 0 0.25px currentcolor;
}
</style>
