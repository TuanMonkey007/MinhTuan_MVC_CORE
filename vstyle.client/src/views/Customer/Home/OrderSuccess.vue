<template>
    <a-row>
        <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <a-page-header style="
         border: 1px solid rgb(235, 237, 240);
          background-color: #c22026;
        ">

                <template #title>
                    <a-breadcrumb separator=">" style="margin-left:30px;">
                        <a-breadcrumb-item href="">

                            <span style="color: #fff;"> Trang chủ</span>
                        </a-breadcrumb-item>

                        <a-breadcrumb-item class="active">

                            <span style="color: #fff;"> Giỏ hàng</span>
                        </a-breadcrumb-item>
                        <a-breadcrumb-item class="active">

                            <span style="color: #fff;"> Thanh toán</span>
                        </a-breadcrumb-item>

                    </a-breadcrumb>
                </template>

            </a-page-header>
        </a-col>
    </a-row>
    <a-row align="center" justify="center" style="margin-top: 20px;">
        <a-col :xs="24" :sm="24" :md="16" :lg="14" :xl="14">
            <a-steps :current="2" :direction="horizontal" :responsive="false">
                <a-step>
                    <template #icon>
                        <a-button type="text" style="color: #fff; font-size:16px ;">Giỏ hàng</a-button>
                    </template>
                </a-step>
                <a-step>
                    <template #icon>
                        <a-button type="text" style="color: #fff; font-size:16px ;">Thanh toán</a-button>
                    </template>
                </a-step>
                <a-step>
                    <template #icon>
                        <a-button type="text" style="color: #fff; font-size:16px ;">Thành công</a-button>
                    </template>
                </a-step>
            </a-steps>
        </a-col>
    </a-row>
    <transition name="route" mode="out-in" appear>
        <a-row :gutter="[8, 30]" align="center" justify="center" style="margin-top: 20px; ">
            <a-col v-if="this.success" :span="24" style="text-align: center">
                <div><span
                        style="color: #c12227;font-family: Archivo;font-size: 24px;font-weight: 600;line-height: 36px;letter-spacing: 0;text-align: center;">ĐẶT
                        HÀNG THÀNH CÔNG!</span></div>
                <div>
                    <p
                        style="font-family: Archivo; font-size: 20px;font-weight: 400;line-height: 24px;letter-spacing: 0;text-align: center;margin-block-start: 5px;margin-block-end: 5px;">
                        Cám ơn bạn đã lựa chọn mua sắm tại VStyle!
                    </p>
                    <p
                        style="font-family: Archivo; font-size: 20px;font-weight: 400;line-height: 24px;letter-spacing: 0;text-align: center;margin-block-start: 5px;margin-block-end: 5px;">
                        Đơn hàng của bạn CHẮC CHẮN đã được chuyển tới hệ thống xử lý đơn hàng của VStyle.
                    </p>
                    <p
                        style="font-family: Archivo; font-size: 20px;font-weight: 400;line-height: 24px;letter-spacing: 0;text-align: center;margin-block-start: 5px;margin-block-end: 5px;">
                        Bạn sẽ được nhận hàng trong vòng 1 đến 2 ngày.
                    </p>
                </div>
            </a-col>
            <a-col v-else :span="24" style="text-align: center">

                <div><span
                        style="color: #c12227;font-family: Archivo;font-size: 24px;font-weight: 600;line-height: 36px;letter-spacing: 0;text-align: center;">ĐƠN
                        HÀNG CHƯA THANH TOÁN!</span></div>
                <div>
                    <p
                        style="font-family: Archivo; font-size: 20px;font-weight: 400;line-height: 24px;letter-spacing: 0;text-align: center;margin-block-start: 5px;margin-block-end: 5px;">
                        Cám ơn bạn đã lựa chọn mua sắm tại VStyle!
                    </p>
                    <p
                        style="font-family: Archivo; font-size: 20px;font-weight: 400;line-height: 24px;letter-spacing: 0;text-align: center;margin-block-start: 5px;margin-block-end: 5px;">
                        Đơn hàng của bạn CHẮC CHẮN đã được chuyển tới hệ thống xử lý đơn hàng của VStyle.
                    </p>
                    <p
                        style="font-family: Archivo; font-size: 20px;font-weight: 400;line-height: 24px;letter-spacing: 0;text-align: center;margin-block-start: 5px;margin-block-end: 5px;">
                        Vui lòng hoàn tất thanh toán để giao đơn hàng được gửi đi !!!.
                    </p>
                </div>
            </a-col>

            <a-col v-if="this.success" :xs="24" :sm="24" :md="16" :lg="16" :xl="16" class="border-deptrai">
                <a-row justify="center">
                    <a-col :span="10">

                        <a-divider orientation="center" style="margin-top: 10px; font-size: 20px">THÔNG TIN NHẬN HÀNG</a-divider>
                    </a-col>
                </a-row>
                <a-row>
                    <a-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12"><a-descriptions style="padding:10px"
                            title="Thông tin nhận hàng">
                            <a-descriptions-item :span="24" label="Tên người nhận"> <b>{{ orderInfo.customerName
                                    }}</b></a-descriptions-item>
                            <a-descriptions-item :span="24" label="Số điện thoại"><b> {{ orderInfo.customerPhoneNumber
                                    }}</b> </a-descriptions-item>
                            <a-descriptions-item :span="24" label="Địa chỉ nhận hàng"><b> {{ orderInfo.shippingAddress
                                    }}</b> </a-descriptions-item>
                            <a-descriptions-item :span="24" label="Ghi chú cho người bán"> {{ orderInfo.customerNote
                                }}</a-descriptions-item>
                            <a-descriptions-item :span="24" label="Trang thái"> <a-tag color="green"> {{
                                    orderInfo.statusName }}</a-tag></a-descriptions-item>

                        </a-descriptions></a-col>

                    <a-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
                        <a-descriptions style="padding:10px" title="Thông tin đơn hàng">
                            <a-descriptions-item :span="24" label="Mã đơn"><b> {{ orderInfo.code }}
                                </b></a-descriptions-item>
                            <a-descriptions-item :span="24" label="Ngày tạo"><b> {{
                                    formatCreatedDate(orderInfo.createdDate)
                                    }} </b></a-descriptions-item>
                            <a-descriptions-item :span="24" label="Phương thức thanh toán"><a-tag color="default"> {{
                                    orderInfo.paymentMethodName }}</a-tag></a-descriptions-item>
                            <a-descriptions-item :span="24" label="Trạng thái thanh toán"><a-tag color="green"> {{
                                    orderInfo.paymentStatusName }}</a-tag></a-descriptions-item>
                            <a-descriptions-item :span="24" label="Tổng thanh toán"><span style="color: red">{{
                                    fomartPrice(orderInfo.totalAmount) }}&#8363;</span> </a-descriptions-item>
                        </a-descriptions>
                    </a-col>
                </a-row>
               

            </a-col>
            <a-col :span="16" style="margin-bottom: 20px">
                <a-divider style="margin-top: 0px"></a-divider>
                <a-row justify="space-around">
                    <a-col :span="10">
                        <router-link :to="{ name: 'CustomerHome' }">
                            <a-button type="primary"
                                style="border-radius: 20px; width: 100%; background-color: #ffffff; border-color: #dc3a35; height: 40px">
                                <a style="color: #dc3a35;"> Tiếp tục mua sắm</a>
                            </a-button>
                        </router-link>
                    </a-col>
                    <a-col :span="10">
                        <router-link :to="{ name: 'TrackingOrder' }">
                        <a-button type="primary" class="btn-buy-now"
                            style="border-radius: 20px; width: 100%; background-color: #c12227; height: 40px;">
                           <a style="color: white;">Theo dõi
                                    đơn hàng</a>
                        </a-button>
                    </router-link>
                    </a-col>
                </a-row>
            </a-col>
        </a-row>
    </transition>
</template>
<script>
    import APIService from '@/helpers/APIService';
    import deleteOutlined from '@ant-design/icons-vue/DeleteOutlined';
    import { message, notification } from 'ant-design-vue';
    import { debounce } from 'lodash';
    import axios from 'axios';
    import dayjs from 'dayjs';
    export default {
        components: {
            deleteOutlined
        },

        data() {
            return {

                orderInfo: {
                    code: '',
                    customerName: '',
                    customerPhoneNumber: '',
                    statusName: '',
                    shippingAddress: '',
                    totalAmount: 0,
                    shippingCost: 0,
                    createdDate: '',
                    paymentStatusName: '',
                    paymentMethodName: '',

                },
                orderCode: '',
                success: false,

                dataSourceTable: [],




            }
        },
        mounted() {
            this.orderCode = this.$route.query.ordercode;
            this.success = this.$route.query.success;
            console.log(this.success)
            console.log(this.orderCode)
            this.fetchOrderInfo();

        },
        methods: {
            formatCreatedDate(date) {
                return dayjs(date).format("HH:mm:ss DD/MM/YYYY ");
            },

            fomartPrice(price) {
                return price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.');
            },
            async fetchOrderInfo() {
                try {
                    const response = await APIService.get(`order/get-order-info-by-code/${this.orderCode}`);
                    this.orderInfo = response.data.data;
                } catch (error) {
                    console.error('Error fetching order info:', error);
                }
            },
        }
    }
</script>
<style scoped>
    .ant-page-header {
        padding: 0px;
        /* Giảm padding */
        margin-bottom: 8px;
        /* Giảm margin dưới */
        height: 35px;
        padding-left: 5%;
    }


    :deep(.ant-breadcrumb-separator) {
        color: #ffffff86;
        /* Đặt màu chữ cho breadcrumb */
    }

    :deep(.ant-breadcrumb-link a) {
        color: #ffffff86;
        /* Đặt màu chữ cho breadcrumb */
    }

    .active {
        color: #e60b0b;
        /* Đặt màu chữ cho breadcrumb */
    }

    .ant-page-header-heading-title {
        line-height: normal;
        /* Đặt màu chữ cho tiêu đề */
        display: flex;
        /* Hiển thị theo chiều ngang */
        color: #ffffff86;
    }

    :deep(.ant-steps-item-active .ant-steps-item-icon) {
        background-color: #c12227 !important;
        color: #fff;
        border: 1px solid #c12227;
        border-radius: 20px;
        width: fit-content;

    }

    :deep(.ant-steps-item-icon) {
        background-color: #807878 !important;
        color: #fff;
        border: 1px solid #807878;
        border-radius: 20px;
        width: fit-content;
    }

    :deep(.ant-steps-item-title::after) {

        height: 5px !important;
    }

    :deep(.ant-steps-item-finish .ant-steps-item-title::after) {

        background-color: #c12227 !important;
    }

    :deep(.ant-input-number-group-addon) {
        border-top-right-radius: 20px;
        border-bottom-right-radius: 20px;
        border-top-left-radius: 20px;
        border-bottom-left-radius: 20px;
    }

    :deep(.ant-steps-item-finish .ant-steps-item-icon) {
        background-color: #c12227 !important;
        color: #fff;
        border: 1px solid #c12227;
        border-radius: 20px;
        width: fit-content;
    }

    :deep(.ant-input-number-input) {
        width: 40px !important;
    }

    .border-deptrai {
        border-radius: 20px;
        overflow: visible;
        border: 1px solid #9a3a37;
        transition: .5s linear;
        box-shadow: rgba(87, 104, 110, 0.5) 2px 0px 9px 0px;
    }

    :deep(.input-voucher .ant-input) {
        border-start-start-radius: 20px !important;

        border-end-start-radius: 20px !important;
    }

</style>
