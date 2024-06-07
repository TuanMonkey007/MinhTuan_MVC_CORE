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

                    </a-breadcrumb>
                </template>

            </a-page-header>
        </a-col>
    </a-row>
    <a-row align="center" justify="center" style="margin-top: 20px;">
        <a-col :xs="24" :sm="24" :md="16" :lg="14" :xl="14">
            <a-steps :current="0" :direction="horizontal" :responsive="false">
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
    <a-row v-if="isHasItem" align="center" justify="center" style="margin-top: 20px; ">
        <a-col :xs="0" :sm="0" :md="12" :lg="12" :xl="12" class="border-deptrai">
            <a-table :columns="tableColumns" :dataSource="dataSourceTable" :pagination="false" :loading="false"
                style="margin: 5px;">
                <template #bodyCell="{ column, record }">
                    <template v-if="column.key == 'action'">
                        <a-row>
                            <a-col :span="24">
                                <a-popconfirm title="Xác nhận xóa?" ok-text="Xóa" cancel-text="Hủy"
                                    @confirm="handleDeleteCartItem(record)" placement="topLeft">
                                    <delete-outlined style="font-size: 20px; color: #b04f46" />
                                </a-popconfirm>
                            </a-col>
                        </a-row>

                    </template>
                    <template v-if="column.key == 'thumbnail'">
                        <img v-if="record.thumbnailBase64"
                            :src="'data:' + record.thumbnailContentType + ';base64,' + record.thumbnailBase64"
                            style="max-width: 100px; max-height: 100px; object-fit: contain; border-radius: 5px" />
                        <span v-else>Chưa cập nhật</span>
                    </template>
                    <template v-if="column.key == 'productName'">
                        <span style="font-size: 14px; font-weight: 500;">{{ record.productName }}</span>

                        <br>
                        <a-button style="border-radius: 20px;background-color:#f2f2f2;font-size: 12px; ">{{
                            record.colorName }}
                            | {{ record.sizeName }}</a-button>
                    </template>
                    <template v-if="column.key == 'productPrice'">
                        <span class="price-product" style="font-size:14px ; font-weight: 600; color: #292929">{{
                            record.productPrice }}&#8363;</span>
                    </template>
                    <template v-if="column.key == 'quantity'">
                        <a-input-number v-model:value="record.quantity" min="1" max="10" size="small" :controls="false">
                            <template #addonBefore>
                                <a-button size="small" type="text" @click="decreaseQuantity(record)">-</a-button>
                            </template>
                            <template #addonAfter>
                                <a-button size="small" type="text" @click="increaseQuantity(record)">+</a-button>
                            </template>
                        </a-input-number>
                    </template>
                    <template v-if="column.key == 'total'">

                        <a-row>
                            <a-col :span="24">
                                <span class="price-product" style="font-size:14px ; font-weight: 600;">{{
                                    record.productPrice * record.quantity }}&#8363;</span>

                            </a-col>
                        </a-row>

                    </template>
                </template>
            </a-table>
        </a-col>
        <a-col :xs="24" :sm="24" :md="0" :lg="0" :xl="0" class="border-deptrai">
            <a-table :columns="tableColumnsMobile" :dataSource="dataSourceTable" :pagination="false"
                :loading="isLoading" style="margin:5px">
                <template #bodyCell="{ column, record }">
                    <template v-if="column.key == 'thumbnail'">
                        <img v-if="record.thumbnailBase64"
                            :src="'data:' + record.thumbnailContentType + ';base64,' + record.thumbnailBase64"
                            style="max-width: 100px; max-height: 100px; object-fit: contain;" />
                        <span v-else>Chưa cập nhật</span>
                    </template>
                    <template v-if="column.key == 'detail'">
                        <a-row justify="space-around">
                            <a-col :span="20">
                                <span style="font-size: 14px; font-weight: 400;">{{ record.productName }}</span>
                                <br>
                                <span class="price-product" style="font-size:14px ; font-weight: 600;">{{
                                    record.productPrice }}&#8363;</span>
                            </a-col>
                            <a-col :span="2">
                                <a-popconfirm title="Xác nhận xóa?" ok-text="Xóa" cancel-text="Hủy"
                                    @confirm="handleDeleteCartItem(record)" placement="topLeft">
                                    <delete-outlined style="font-size: 20px; color: #b04f46" />
                                </a-popconfirm>
                            </a-col>
                        </a-row>
                        <a-row style="margin-top: 5px" justify="space-around">
                            <a-col :span="8" style="display:  flex; justify-content: start;">
                                <a-button style="border-radius: 20px;background-color:#f2f2f2;font-size: 12px; ">{{
                                    record.colorName }}
                                    | {{ record.sizeName }}</a-button>
                            </a-col>
                            <a-col :span="16" style="display:  flex; justify-content: end;">
                                <a-input-number v-model:value="record.quantity" min="1" max="10" size="small"
                                    :controls="false">
                                    <template #addonBefore>
                                        <a-button size="small" type="text"
                                            @click="decreaseQuantity(record)">-</a-button>
                                    </template>
                                    <template #addonAfter>
                                        <a-button size="small" type="text"
                                            @click="increaseQuantity(record)">+</a-button>
                                    </template>
                                </a-input-number>
                            </a-col>
                        </a-row>
                    </template>

                </template>
            </a-table>
        </a-col>
        <a-col :xs="24" :sm="24" :md="8" :lg="8" :xl="8" style="  margin: 20px">
            <a-card class="border-deptrai">
                <template #title>
                    <h4 style="justify-content: center;display:  flex">GIỎ HÀNG CỦA BẠN</h4>
                </template>
                <div>
                    <h4>QUYỀN LỢI ĐỔI TRẢ</h4>
                    <p>- Đổi <b>miễn phí</b> tại tất cả cửa hàng của VStyle</p>
                    <p>- Đổi <b>miễn phí</b> online do lỗi của VStyle.</p>
                    <p>- Được kiểm tra hàng trước khi thanh toán.</p>
                    <p>- Thời gian đổi trả: trong vòng 7 ngày kể từ khi nhận được hàng với yêu cầu sản phẩm chưa sử dụng
                        còn tem
                        mạc của sản phẩm.</p>
                    <p>- Số lần đổi trả cho 1 sản phẩm là 1 lần.</p>
                </div>
                <a-card-meta>
                    <template #title>
                        <a-row>
                            <a-col :span="12">
                                <span style="font-weight: 600;text-align: left; font-size: 16px">TỔNG THANH TOÁN</span>
                            </a-col>
                            <a-col :span="12" style="text-align: end;">
                                <span style="font-weight: 600;text-align: right;color: #c12227; font-size: 20px;">
                                    {{ updateTotalPayment() }}&#8363;
                                </span>
                            </a-col>
                        </a-row>
                    </template>
                    <template #description>
                        <a-row justify="space-around">
                            <a-col :span="10">
                                <router-link :to="{name: 'CustomerHome'}">
                                <a-button type="primary"
                                    style="border-radius: 20px; width: 100%; background-color: #ffffff; border-color: #dc3a35; height: 40px">
                                  <a  style="color: #dc3a35;"> Tiếp tục mua sắm</a>
                                </a-button>
                            </router-link> 
                            </a-col>
                            <a-col :span="10">
                                <router-link :to="{name: 'Order'}">
                                <a-button type="primary" class="btn-buy-now"
                                    style="border-radius: 20px; width: 100%; background-color: #c12227; height: 40px;">
                                    <a  style="color: white;">Thanh toán</a>
                                </a-button>
                            </router-link>
                            </a-col>


                        </a-row>
                    </template>
                </a-card-meta>
            </a-card>
        </a-col>
    </a-row>
    <a-row v-else align="center" justify="center" style="margin-top: 20px;">
        <a-col :span="10">
            <a-row>
                <a-col :span="24">
                    <span style="display: flex; justify-content:center;     font-family: Archivo;
    font-weight: 500;
    font-size: 24px;
    line-height: 28px;
    letter-spacing: .0015em;
    text-transform: uppercase;
    color: #292929;
    margin-bottom: 14px;">GIỎ HÀNG CỦA BẠN</span>
                </a-col>
                <a-col :span="24">
                    <span style="display: flex; justify-content:center;">Giỏ hàng của bạn không có sản phẩm nào</span>
                    <br>
                </a-col>
                <a-col :span="24" style="display: flex; justify-content:center;">
                    <router-link :to="{name: 'CustomerHome'}">
                    <a-button type="primary" class="btn-buy-now"
                        style=" display: flex; justify-content:center;border-radius: 20px; background-color: #c12227; height: 40px;">
                       <a style="color: white;">Quay lại cửa hàng</a>
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
    import { notification } from 'ant-design-vue';
    import { debounce } from 'lodash';
    export default {
        components: {
            deleteOutlined
        },
        data() {
            const tableColumnsMobile = [{
                title: "",
                key: "thumbnail",
                width: "30%"
            },
            {
                title: '',
                key: 'detail',
            }
            ];
            const tableColumns = [{
                title: '',

                key: 'action',
                width: "5%",
            },
            {
                title: 'Sản phẩm',

                key: 'thumbnail',

                width: "20%",
            },
            {
                title: '',
                dataIndex: 'productName',
                key: 'productName',
                width: "35%",
            },
            {
                title: 'Giá',
                dataIndex: 'productPrice',
                key: 'productPrice',
                width: "15%",
            },
            {
                title: 'Số lượng',
                dataIndex: 'quantity',
                key: 'quantity',
                width: "15%",
            },

            {
                title: 'Thành tiền',

                key: 'total',
                width: "15%",
            },

            ];

            return {
                isLoading: false,
                tableColumns,
                dataSourceTable: [],
                tableColumnsMobile,
                debounceTimeout: null,
                updatedRecords: {}, // Lưu trữ các bản ghi đã được cập nhật
                TotalPayment: 0,
                isHasItem: false

            }
        },
        async mounted() {
            this.fetchData();
        },
        methods: {
            async fetchData() {
                try {
                    //    Nếu là khách chưa đăng nhập
                    const cartId = localStorage.getItem('userCartId') ? localStorage.getItem('userCartId') : localStorage.getItem('cartId');

                    const response = await APIService.get(`/cart/${cartId}`);
                    this.dataSourceTable = response.data.data;
                    if (this.dataSourceTable.length > 0) {
                        this.isHasItem = true;
                    }
                    else {
                        this.isHasItem = false;
                    }

                } catch (error) {
                    console.log(error);
                }
            },

            truncateProductName(productName) {
                const limit = 20;
                if (!productName) return '';
                return productName.length <= limit
                    ? productName
                    : productName.substring(0, limit) + '...';
            },
            decreaseQuantity(record) {
                if (record.quantity > 1) {
                    record.quantity--;
                    this.updatedRecords[record.id] = record.quantity; // Lưu lại số lượng đã cập nhật
                }

                this.debounceUpdateQuantity(); // Gọi hàm debounce
            },
            increaseQuantity(record) {
                if (record.quantity < 1000) {
                    record.quantity++;
                    this.updatedRecords[record.id] = record.quantity; // Lưu lại số lượng đã cập nhật
                }

                this.debounceUpdateQuantity(); // Gọi hàm debounce
            },
            debounceUpdateQuantity: debounce(function () {
                // Lặp qua các record đã cập nhật và gửi request
                for (const recordId in this.updatedRecords) {
                    this.handleChangeQuantity({ id: recordId, quantity: this.updatedRecords[recordId] });
                }
                this.updatedRecords = {}; // Reset lại danh sách record đã cập nhật
            }, 2000),

            async handleChangeQuantity(record) {
                const response = await APIService.put(`cart/update-quantity/${record.id}?quantity=${record.quantity}`);
                if (response.data.message == 'Cập nhật thành công') {
                    notification.success({
                        message: 'Cập nhật số lượng thành công',
                    });
                }
                else {
                    notification.error({
                        message: 'Cập nhật số lượng thất bại',
                    });
                }
            },
            async handleDeleteCartItem(record) {
                const response = await APIService.delete(`cart/soft-delete/${record.id}`);
                if (response.data.message == 'Xóa thành công') {
                    this.fetchData();
                    notification.success({
                        message: 'Xóa sản phẩm thành công',
                    });
                }
                else {
                    notification.error({
                        message: 'Xóa sản phẩm thất bại',
                    });
                }

            },
            updateTotalPayment() {
                this.TotalPayment = this.dataSourceTable.reduce((total, item) => {
                    return total + item.productPrice * item.quantity;
                }, 0);
                return this.TotalPayment;
            }
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

</style>
