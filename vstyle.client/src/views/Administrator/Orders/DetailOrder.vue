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
                            <a-steps>
                                <a-step status="finish" title="Login">
                                    <template #icon>
                                        <user-outlined />
                                    </template>
                                </a-step>
                                <a-step status="finish" title="Verification">
                                    <template #icon>
                                        <solution-outlined />
                                    </template>
                                </a-step>
                                <a-step status="process" title="Pay">
                                    <template #icon>
                                        <loading-outlined />
                                    </template>
                                </a-step>
                                <a-step status="wait" title="Done">
                                    <template #icon>
                                        <smile-outlined />
                                    </template>
                                </a-step>
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
                                                <a-descriptions-item span="24" label="Họ và tên">{{ orderInfo.userName }}</a-descriptions-item>
                                                <a-descriptions-item span="24"
                                                    label="Email">{{ orderInfo.userEmail }}</a-descriptions-item>
                                                <a-descriptions-item span="24"
                                                    label="Số điện thoại">{{ orderInfo.userPhoneNumber }}</a-descriptions-item>
                                                <a-descriptions-item span="24" label="Địa chỉ">{{ orderInfo.userAddress }}</a-descriptions-item>
                                            </a-descriptions>
                                        </a-col>
                                        <a-col :xs="24" :sm="24" :md="12" :lg="12" :xl="12" style="padding: 10px">
                                            <a-descriptions title="Thông tin đơn hàng">
                                                <a-descriptions-item span="24"
                                                    label="Mã đơn">{{ orderInfo.code }}</a-descriptions-item>
                                                <a-descriptions-item span="24" label="Trạng thái">
                                                   <a-tag color="green"> {{ orderInfo.statusName }}</a-tag>
                                                </a-descriptions-item>
                                                <a-descriptions-item span="24" label="Ghi chú">{{ orderInfo.shopNote }}</a-descriptions-item>
                                              
                                            </a-descriptions>
                                        </a-col>
                                        <a-col :xs="24" :sm="24" :md="12" :lg="12" :xl="12" style="padding: 10px">
                                            <a-descriptions title="Thông tin giao hàng">
                                                <a-descriptions-item span="24" label="Họ và tên người nhận">{{ orderInfo.customerName }}</a-descriptions-item>
                                                <a-descriptions-item span="24"
                                                    label="Số điện thoại">{{ orderInfo.customerPhoneNumber }}</a-descriptions-item>
                                                <a-descriptions-item span="24" label="Địa chỉ">{{ orderInfo.shippingAddress }}</a-descriptions-item>
                                                <a-descriptions-item span="24" label="Ghi chú cho người bán">{{ orderInfo.customerNote }}
                                                </a-descriptions-item>
                                                <a-descriptions-item span="24" label="Thời gian đặt">{{ orderInfo.createdDate }}</a-descriptions-item>
                                                <a-descriptions-item span="24"
                                                    label="Đơn vị vận chuyển ">GHN</a-descriptions-item>
                                            </a-descriptions>
                                        </a-col>
                                        <a-col :xs="24" :sm="24" :md="12" :lg="12" :xl="12" style="padding: 10px">
                                            <a-descriptions title="Thông tin thanh toán">
                                                <a-descriptions-item span="24"
                                                    label="Phương thức thanh toán"><a-tag color="default">{{ orderInfo.paymentMethodName }}</a-tag></a-descriptions-item>
                                                <a-descriptions-item span="24"
                                                    label="Trạng thái thanh toán">
                                                    <a-tag color="green"> {{ orderInfo.statusName }}</a-tag>
                                                   </a-descriptions-item>
                                                    <a-descriptions-item span="24"
                                                    label="Tổng tiền">{{ orderInfo.totalAmount }}</a-descriptions-item>
                                                
                                                <a-descriptions-item span="24"
                                                    label="Ghi chú">{{ orderInfo.shopNote }}</a-descriptions-item>
                                            </a-descriptions>
                                        </a-col>
                                    </a-row>
                                </a-tab-pane>
                                <a-tab-pane key="orderChange">
                                    <template #tab>
                                        <span>
                                            <font-awesome-icon icon="fa-solid fa-hammer" />
                                            Cập nhật đơn hàng
                                        </span>
                                    </template>
                                    <a-row>
                                        <a-col :xs="24" :sm="24" :md="10" :lg="10" :xl="10" class="border-deptrai">
                                            <a-form layout="vertical" ref="formOrder" :model="orderInfo"
                                                style="margin: 20px">
                                                <a-form-item label="Họ và tên" name="fullName" :rules="[
                                                    {
                                                        required: true,
                                                        message: 'Nhập họ tên người nhận',
                                                    },
                                                ]">
                                                    <a-input v-model:value="orderInfo.fullName"
                                                        placeholder="Nhập họ tên người nhận" />
                                                </a-form-item>
                                                <a-form-item label="Số điện thoại" name="phoneNumber" :rules="[
                                                    {
                                                        required: true,
                                                        message: 'Nhập số điện thoại người nhận',
                                                    },
                                                ]">
                                                    <a-input v-model:value="orderInfo.phoneNumber"
                                                        placeholder="Nhập số điện thoại" />
                                                </a-form-item>

                                                <a-row justify="space-between">
                                                    <a-col :xs="24" :sm="24" :md="7" :lg="7" :xl="7">
                                                        <a-form-item label="Tỉnh" name="selectedProvince" :rules="[
                                                            {
                                                                required: true,
                                                                message: 'Chọn Tỉnh/Thành phố',
                                                            },
                                                        ]">
                                                            <a-select show-search optionFilterProp="label"
                                                                v-model:value="orderInfo.selectedProvince"
                                                                :options="provinceOptions"
                                                                @change="handleChangeProvince" />
                                                        </a-form-item></a-col>
                                                    <a-col :xs="24" :sm="24" :md="7" :lg="7" :xl="7">
                                                        <a-form-item label="Huyện" name="selectedDistrict" :rules="[
                                                            { required: true, message: 'Chọn Quận/Huyện' },
                                                        ]">
                                                            <a-select show-search optionFilterProp="label"
                                                                v-model:value="orderInfo.selectedDistrict"
                                                                :options="districtOptions"
                                                                @change="handleChangeDistrict" />
                                                        </a-form-item></a-col>
                                                    <a-col :xs="24" :sm="24" :md="7" :lg="7" :xl="7">
                                                        <a-form-item label="Xã" name="selectedWard" :rules="[
                                                            { required: true, message: 'Chọn Huyện/Xã' },
                                                        ]">
                                                            <a-select show-search optionFilterProp="label"
                                                                v-model:value="orderInfo.selectedWard"
                                                                :options="wardOptions" @change="handleChangeWard" />
                                                        </a-form-item></a-col>
                                                </a-row>
                                                <a-form-item label="Địa chỉ cụ thể" name="address" :rules="[
                                                    {
                                                        required: true,
                                                        message: 'Nhập địa chỉ giao hàng cụ thể',
                                                    },
                                                ]">
                                                    <a-input v-model:value="orderInfo.address"
                                                        placeholder="Nhập địa chỉ giao hàng" />
                                                </a-form-item>
                                                <a-form-item label="Ghi chú cho người bán" name="note">
                                                    <a-textarea v-model:value="orderInfo.note"
                                                        placeholder="Nhập ghi chú giao hàng (Nếu cần)" />
                                                </a-form-item>
                                            </a-form>
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
    import APIService from "@/helpers/APIService";
import {
        UserOutlined,
        SolutionOutlined,
        LoadingOutlined,
        SmileOutlined,
    } from "@ant-design/icons-vue";
    export default {
        components: {
            UserOutlined,
            SolutionOutlined,
            LoadingOutlined,
            SmileOutlined,
        },
        data() {
            return {
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
                },
            };
        },
        mounted() {
            this.fetchOrderInfo();
        },//end mounted
        methods: {
            async fetchOrderInfo(){
                const id = this.$route.params.id;
                const response = await APIService.get(`order/get-order-info-by-id/${id}`);
                this.orderInfo = response.data.data;
            
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
</style>
