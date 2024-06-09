<template>
    <a-row>
        <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <a-page-header style="border: 1px solid rgb(235, 237, 240); background-color: #c21f24" @back="goBack">
                <template #backIcon>
                    <font-awesome-icon :icon="['fas', 'backward']" style="color: white; margin-left: 30px" />
                </template>
                <template #title>
                    <span style="
              color: white;
              font-size: 16px;
              margin-bottom: auto;
              display: block;
            ">Quản lý đơn hàng</span>
                </template>
                <template #extra>
                    <a-breadcrumb separator=">" style="margin-right: 30px">
                        <a-breadcrumb-item href="">
                            <font-awesome-icon icon="fa-solid fa-house" style="color: #fff" />
                            <span style="color: #fff"> Trang quản trị</span>
                        </a-breadcrumb-item>

                        <a-breadcrumb-item href="">
                            <font-awesome-icon icon="fa-solid fa-cart-shopping" style="color: #fff" />
                            <span style="color: #fff"> Đơn hàng</span>
                        </a-breadcrumb-item>
                    </a-breadcrumb>
                </template>
            </a-page-header>
        </a-col>
    </a-row>

    <transition name="route" mode="out-in" appear>
        <!-- Card status đơn hàng -->
        <a-row>
            <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
                <a-card :bordered="true" style="margin: 30px">
                    <a-row>
                        <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">

                            <a-steps v-model:current="currentStep"
                                :status="this.orderInfo.isCancelled ? 'error' : ''">
                                <a-step v-for="(status, index) in orderStatusList" :key="index" :title="status.name"
                                    :disabled="true" />
                            </a-steps>
                        </a-col>
                    </a-row>
                </a-card>
            </a-col>

            <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
                <a-card :bordered="true" style="margin: 10px 30px">
                    <a-row>
                        <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
                            <a-tabs v-model:activeKey="activeKey">
                                <a-tab-pane key="orderInfo" force-render>
                                    <template #tab>
                                        <span>
                                            <font-awesome-icon :icon="['fas', 'circle-info']" />
                                            Thông tin đơn hàng
                                        </span>
                                    </template>
                                    <a-row>
                                        <a-col :xs="24" :sm="24" :md="12" :lg="12" :xl="12" style="padding: 10px">
                                            <a-descriptions title="Thông tin khách hàng">
                                                <a-descriptions-item span="24" label="Họ và tên">{{ orderInfo.userName
                                                    }}</a-descriptions-item>
                                                <a-descriptions-item span="24" label="Email">{{ orderInfo.userEmail
                                                    }}</a-descriptions-item>
                                                <a-descriptions-item span="24" label="Số điện thoại">{{
                                                    orderInfo.userPhoneNumber }}</a-descriptions-item>
                                                <a-descriptions-item span="24" label="Địa chỉ">{{ orderInfo.userAddress
                                                    }}</a-descriptions-item>
                                            </a-descriptions>
                                        </a-col>
                                        <a-col :xs="24" :sm="24" :md="12" :lg="12" :xl="12" style="padding: 10px">
                                            <a-descriptions title="Thông tin đơn hàng">
                                                <a-descriptions-item span="24" label="Mã đơn">{{ orderInfo.code
                                                    }}</a-descriptions-item><a-descriptions-item span="24"
                                                    label="Trạng thái">
                                                    <a-tag :color="getColor(orderInfo.isCancelled)">
                                                        {{ orderInfo.isCancelled ? 'Hủy đơn' : (orderInfo.statusName ||
                                                        'Chưa xác định') }}
                                                    </a-tag>
                                                </a-descriptions-item>

                                                <a-descriptions-item span="24" label="Ghi chú">{{
                                                    this.orderInfo.reasonCancelled
                                                }}</a-descriptions-item>


                                            </a-descriptions>
                                        </a-col>
                                        <a-col :xs="24" :sm="24" :md="12" :lg="12" :xl="12" style="padding: 10px">
                                            <a-descriptions title="Thông tin giao hàng">
                                                <a-descriptions-item span="24" label="Họ và tên người nhận">{{
                                                    orderInfo.customerName }}</a-descriptions-item>
                                                <a-descriptions-item span="24" label="Số điện thoại">{{
                                                    orderInfo.customerPhoneNumber }}</a-descriptions-item>
                                                <a-descriptions-item span="24" label="Địa chỉ">{{
                                                    orderInfo.shippingAddress
                                                    }}</a-descriptions-item>
                                                <a-descriptions-item span="24" label="Ghi chú cho người bán">{{
                                                    orderInfo.customerNote }}
                                                </a-descriptions-item>
                                                <a-descriptions-item span="24" label="Thời gian đặt">{{
                                                   formatCreatedDate(orderInfo.createdDate) }}</a-descriptions-item>
                                                <a-descriptions-item span="24"
                                                    label="Đơn vị vận chuyển ">GHN</a-descriptions-item>
                                            </a-descriptions>
                                        </a-col>
                                        <a-col :xs="24" :sm="24" :md="12" :lg="12" :xl="12" style="padding: 10px">
                                            <a-descriptions title="Thông tin thanh toán">
                                                <a-descriptions-item span="24" label="Phương thức thanh toán"><a-tag
                                                        color="default">{{ orderInfo.paymentMethodName
                                                        }}</a-tag></a-descriptions-item>
                                                <a-descriptions-item v-if="isVnpay" span="24" label="Trạng thái thanh toán">
                                                    <a-tag color="green"> {{ this.orderInfo.paymentStatusName}}</a-tag>
                                                </a-descriptions-item>
                                                <a-descriptions-item span="24" label="Tổng tiền"><span
                                                        style="color: red">{{
                                                            fomartPrice(orderInfo.totalAmount)
                                                        }}&#8363;</span></a-descriptions-item>

                                                <a-descriptions-item span="24" label="Ghi chú">{{ orderInfo.shopNote
                                                    }}</a-descriptions-item>
                                            </a-descriptions>
                                        </a-col>
                                    </a-row>
                                </a-tab-pane>
                                <a-tab-pane key="orderItem">
                                    <template #tab>
                                        <span>
                                            <font-awesome-icon icon="fa-solid fa-box" />
                                            Danh sách sản phẩm
                                        </span>
                                    </template>
                                    <a-row>
                                        <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
                                            <a-table :columns="orderItemscolumns" :data-source="orderItemsTable"
                                                :pagination="false" :scroll="{ x: 1000 }">
                                                <template #bodyCell="{ column, record }">
                                                    <template v-if="column.key == 'price'">
                                                        <span style="color: red">{{ fomartPrice(record.price) }}&#8363;</span>
                                                    </template>
                                                    <template v-if="column.key == 'variant'">
                                                        <a-tag color="green">{{ record.colorName }} | {{ record.sizeName
                                                            }} </a-tag>
                                                    </template>
                                                    <template v-if="column.key == 'thumbnail'">
                                                        <img v-if="record.thumbnailBase64"
                                                            :src="'data:' + record.thumbnailContentType + ';base64,' + record.thumbnailBase64"
                                                            style="max-width: 100px; max-height: 100px; object-fit: contain;" />
                                                        <a-tag color="red" v-else>Chưa cập nhật</a-tag>
                                                    </template>
                                                </template>

                                            </a-table>
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
    import dayjs from "dayjs";
  
    import { Modal } from "ant-design-vue";
    import APIService from "@/helpers/APIService";
    import { message, notification } from "ant-design-vue";
    import {
        UserOutlined,
        SolutionOutlined,
        LoadingOutlined,
        SmileOutlined,
    } from "@ant-design/icons-vue";
    import { backTopProps } from "ant-design-vue/es/float-button/interface";
    export default {
        components: {
            UserOutlined,
            SolutionOutlined,
            LoadingOutlined,
            SmileOutlined,
        },
        data() {
            return {
                isVnpay: false,
                loadingChangeStep: false,
                currentStep: 0,
                orderStatus: 'cancelled',
                orderInfo: {
                    id: "",
                    userId: "",
                    code: "",
                    customerName: "",
                    customerPhoneNumber: "",
                    shippingAddress: "",
                    customerNote: "",
                    shopNote: null,
                    paymentMethod: "",
                    paymentMethodName: null,
                    status: "",
                    statusName: null,
                    totalAmount: 0,
                    voucherId: null,
                    shippingCost: 0,
                    cartId: "",
                    createdDate: "",
                    userName: "",
                    userEmail: "",
                    userPhoneNumber: "",
                    userAddress: "",
                    paymentStatusName: null,
                    isCancelled: false,
                    reasonCancelled: []
                },
                orderItemscolumns: [
                    {
                        title: "Hình ảnh",
                        dataIndex: "thumbnail",
                        key: "thumbnail",
                        width: "15%",
                        sorter: false,
                    },
                    {
                        title: "Mã sản phẩm",
                        dataIndex: "productCode",
                        key: "productCode",
                        width: "15%",
                        sorter: false,
                    },
                    {
                        title: "Tên sản phẩm",
                        dataIndex: "productName",
                        key: "name",
                        width: "15%",
                        sorter: false,
                    },
                    {
                        title: "Kiểu dáng",
                        key: "variant",
                        width: "15%",
                        sorter: false,
                    },
                    {
                        title: "Số lượng",
                        dataIndex: "quantity",
                        key: "quantity",
                        width: "15%",
                        sorter: false,
                    },
                    {
                        title: "Giá tiền",
                        dataIndex: "price",
                        key: "price",
                        width: "15%",
                        sorter: false,
                    },
                ],
                orderItemsTable: [],
                orderStatusList: [],
                previousStep: 0,
            };
        },
       async mounted() {
          await  this.fetchOrderInfo();
            this.fetchOrderItem();
            this.fetchListStatus();
         

        },//end mounted
        methods: {
          

            getColor(isCancelled) {
                return isCancelled ? "red" : "green";
            },
            
            async fetchOrderInfo() {
                const id = this.$route.params.id;
                const response = await APIService.get(`order/get-order-info-by-id/${id}`);
                this.orderInfo = response.data.data;
                if (this.orderInfo.isCancelled) {
                    this.loadingChangeStep = true
                }
                if(this.orderInfo.paymentMethodName == 'VNPay'){
                    this.isVnpay = true
                }


            },
            async fetchOrderItem() {
                const id = this.$route.params.id;
                const response = await APIService.get(`order/get-order-item-by-id/${id}`);

                this.orderItemsTable = response.data.data;
            },
            async fetchListStatus() {
                var response = {};
                if (this.orderInfo.paymentMethodName == 'VNPay') {
                    response = await APIService.get(`datacategory/get-list-by-parent-code/STATUS_VNPAY`);
                } else {
                    response = await APIService.get(`datacategory/get-list-by-parent-code/STATUS_COD`);
                }
                this.orderStatusList = response.data.data;
              
              
                
                this.currentStep = this.orderStatusList.findIndex(
                    (item) => item.id == this.orderInfo.status
                );
              
              
            

            },
            fomartPrice(price) {
                return price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.');
            },
            formatCreatedDate(date) {
        return dayjs(date).format("HH:mm:ss DD/MM/YYYY ");
      }




        },//end methods
    };
</script>
<style scoped>
    .btnSearch:hover {
        background-color: rgb(229 127 123);
        color: white;
    }

    /* Ẩn chữ trên màn hình nhỏ hơn hoặc bằng 768px */
    @media (max-width: 767px) {
        .ant-breadcrumb-link>span {
            display: none;
        }
    }

    .ant-page-header {
        padding: 0px;
        /* Giảm padding */
        margin-bottom: 8px;
        /* Giảm margin dưới */
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

    .border-deptrai {
        border-radius: 20px;
        overflow: visible;
        border: 0.5px solid #d1c4c4;
        transition: 0.5s linear;
        box-shadow: rgba(87, 104, 110, 0.5) 2px 0px 2px 0px;
    }

    :deep(.ant-steps-item-active .ant-steps-item-icon) {
        background-color: #c12227 !important;
        color: #fff;
        border: 1px solid #c12227;


    }

    :deep(.ant-steps-item-finish .ant-steps-item-title::after) {

        background-color: #c12227 !important;
    }
</style>
