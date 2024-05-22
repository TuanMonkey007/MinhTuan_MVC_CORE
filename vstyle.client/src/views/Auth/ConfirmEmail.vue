<template>
    <div style="display: grid; justify-content: center; align-items: center; padding-top: 50px;">
        <img v-if="isVerified" src="@/assets/image/check-mail.png" style="width:300px" />
        <img v-else src="@/assets/image/unauthorized.png" style="width:300px" />

        <h1 v-if="isVerified">Xác nhận thành công!</h1>
        <h1 v-else>Xác nhận thất bại!</h1>

        <span v-if="isVerified">Bây giờ bạn có thể đăng nhập vào hệ thống.</span>
        <span v-else>Vui lòng kiểm tra lại liên kết hoặc thử lại sau.</span>
    </div>
</template>

<script>
    import { ref } from 'vue';
    import APIService from '@/helpers/APIService';
    export default {
    props: ['token', 'email'],
    setup(props) {
        const isVerified = ref(false);

        const confirmEmail = async () => {
            try {
                const response = await APIService.post("account/confirm-email", {
                    Token: props.token,
                    Email: props.email
                });

                if (response.data === "Email confirmed successfully!") {
                    isVerified.value = true; // Cập nhật giá trị của isVerified
                }
            } catch (error) {
                console.error("Xác nhận email thất bại:", error); 
                // Xử lý lỗi ở đây (ví dụ: hiển thị thông báo lỗi)
            }
        };
        confirmEmail();
        return {
            isVerified
        };
    },
};
</script>