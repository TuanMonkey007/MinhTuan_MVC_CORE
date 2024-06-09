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
        <a-row :gutter="[8,30]" align="center" justify="center" style="margin-top: 20px; " >
            <a-col :span="24" style="text-align: center">

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
      
            <a-col :xs="24" :sm="24" :md="10" :lg="10" :xl="10">
                <h2>THÔNG TIN NHẬN HÀNG</h2>
               <a-descriptions class="border-deptrai" style="padding:10px">
                    <a-descriptions-item :span="24" label="Tên người nhận"></a-descriptions-item>
                    <a-descriptions-item :span="24" label="Số điện thoại"></a-descriptions-item>
                    <a-descriptions-item :span="24" label="Địa chỉ nhận hàng"></a-descriptions-item>
                    <a-descriptions-item :span="24" label="Phương thức thanh toán"></a-descriptions-item>
                    <a-descriptions-item :span="24" label="Trạng thái thanh toán"></a-descriptions-item>
               </a-descriptions>
            </a-col>

            <a-col :xs="24" :sm="24" :md="8" :lg="8" :xl="8" style="  margin: 20px">
                <h2>THÔNG TIN ĐƠN HÀNG</h2>
                <a-card class="border-deptrai">
                    <a-row justify="center">
                        <a-col :span="12">
                            <span style="font-weight: 600;text-align: left; font-size: 14px">Tổng tiền hàng</span>

                        </a-col>
                        <a-col :span="12" style="text-align: end;">
                            <span style="font-weight: 600;text-align: right;color: #c12227; font-size: 14px;">
                                {{ fomartPrice(this.totalPrice) }} &#8363;
                            </span>
                        </a-col>
                        <a-col :span="12">
                            <span style="font-weight: 600;text-align: left; font-size: 14px">Phí vận chuyển</span>

                        </a-col>
                        <a-col :span="12" style="text-align: end;">
                            <span style="font-weight: 600;text-align: right;color: #c12227; font-size: 14px;">
                                {{ fomartPrice(this.shipingCost) }} &#8363;
                            </span>
                        </a-col>
                        <a-col :span="12">
                            <span style="font-weight: 600;text-align: left; font-size: 14px">Áp dụng giảm giá</span>
                        </a-col>
                        <a-col :span="12" style="text-align: end;">
                            <span style="font-weight: 600;text-align: right;color: green; font-size: 14px;">
                                {{ fomartPrice(this.savingVoucher) }} &#8363;
                            </span>
                        </a-col>
                    </a-row>
                    <a-divider />
                    <a-card-meta style="margin-top: 20px;">
                        <template #title>
                            <a-row>
                                <a-col :span="12">
                                    <span style="font-weight: 600;text-align: left; font-size: 16px">TỔNG THANH
                                        TOÁN</span>
                                </a-col>
                                <a-col :span="12" style="text-align: end;">
                                    <span style="font-weight: 600;text-align: right;color: #c12227; font-size: 20px;">
                                        {{ fomartPrice(this.total) }} &#8363;

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
                                    <a-button type="primary" class="btn-buy-now"
                                        style="border-radius: 20px; width: 100%; background-color: #c12227; height: 40px;">
                                        <router-link :to="{ name: 'CustomerHome' }"><a style="color: white;">Theo dõi đơn hàng</a></router-link>
                                    </a-button>
                                </a-col>


                            </a-row>
                        </template>
                    </a-card-meta>
                </a-card>
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

    export default {
        components: {
            deleteOutlined
        },

        data() {
            return {
                provinceOptions: [],
                selectedProvince: '',
                districtOptions: [],
                selectedDistrict: '',
                wardOptions: [],
                selectedWard: '',
                orderInfo: {
                    fullName: '',
                    phoneNumber: '',
                    selectedProvince: '',
                    selectedDistrict: '',
                    selectedWard: '',
                    address: '',
                    note: '',
                    paymentMethod: ''
                },
                paymentMethods: [],
                dataSourceTable: [],
                totalPrice: 0,
                shipingCost: 0,
                total: 0,
                savingVoucher: 0,
                searchVoucher: '',
                loadingSearch: false,



            }
        },
        mounted() {
            this.fetchProvice();
            this.fetchPaymentMethods();
            // this.fetchUserInfo();
        },
        methods: {
            async onSearchVoucher() {
                if (this.searchVoucher == '') {
                    return;
                }
                try {
                    this.loadingSearch = true;
                    const response = await APIService.get(`voucher/get-voucher-by-code/${this.searchVoucher}`);
                    if (response.data.data == null) {
                        notification.error({
                            message: 'Lỗi',
                            description: 'Mã giảm giá không tồn tại'
                        });
                        return;
                    }
                    if (response.data.data.type == 1) { //Giảm theo số tiền
                        if (this.getTotalPrice() >= response.data.data.minimumPurchaseAmount) {
                            this.savingVoucher = response.data.data.discountAmount;
                            this.total = this.getTotalPrice() + this.shipingCost - this.savingVoucher;
                            notification.success({
                                message: 'Thành công',
                                description: `Áp dụng mã giảm giá thành công`
                            });
                            console.log(response.data.data.type)
                            return;
                        } else {
                            notification.warning({
                                message: 'Lỗi',
                                description: `Đơn hàng phải có giá trị tối thiểu ${response.data.data.minimumPurchaseAmount}đ`
                            });
                        }

                    } else {

                        this.savingVoucher = this.getTotalPrice() * response.data.data.discountPercent / 100;
                        if (this.savingVoucher > response.data.data.maxValue) {
                            this.savingVoucher = response.data.data.maxValue;
                        }
                        this.total = this.getTotalPrice() + this.shipingCost - this.savingVoucher;
                        notification.success({
                            message: 'Thành công',
                            description: `Áp dụng mã giảm giá thành công`
                        });

                        return


                    }
                } catch (error) {
                    notification.error({
                        message: 'Lỗi',
                        description: error.message
                    });
                } finally {
                    this.loadingSearch = false;
                }

            },
            fomartPrice(price) {
                return price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.');
            },
            getTotalPrice() {
                return this.dataSourceTable.reduce((total, item) => total + item.productPrice * item.quantity, 0);
            },
            async fetchUserInfo() {
                try {
                    const userCartId = localStorage.getItem('userCartId') ? localStorage.getItem('userCartId') : localStorage.getItem('cartId');
                    if (userCartId != null) {
                        const responseCart = await APIService.get(`cart/${userCartId}`);

                        if (responseCart.data.data.length == 0) {
                            this.$router.push({ name: 'CustomerHome' });
                            return;
                        } else {
                            this.dataSourceTable = responseCart.data.data;
                            this.totalPrice = this.dataSourceTable.reduce((total, item) => total + item.productPrice * item.quantity, 0);
                            this.shipingCost = this.totalPrice > 300000 ? 0 : 30000; //Phí vận chuyển theo tổng tiền hàng
                            this.total = this.totalPrice + this.shipingCost;
                        }

                    }


                    const userEmail = localStorage.getItem('userEmail');

                    if (userEmail != null) {
                        const response = await APIService.get(`account/get-user-by-email/${userEmail}`);
                        this.orderInfo.fullName = response.data.data.fullName;
                        this.orderInfo.phoneNumber = response.data.data.phoneNumber;
                        this.orderInfo.address = response.data.data.address;
                        this.orderInfo.selectedProvince = response.data.data.provinceId;
                        // Tạo Axios instance mới
                        const apiClient = axios.create();

                        apiClient.interceptors.request.use(config => {
                            config.headers['token'] = process.env.VUE_APP_GHN_TOKEN; // Gán token trực tiếp
                            return config;
                        });
                        if (this.orderInfo.selectedProvince != null) {
                            const responseDistrict = await apiClient.get(`https://online-gateway.ghn.vn/shiip/public-api/master-data/district?province_id=${this.orderInfo.selectedProvince}`);
                            this.districtOptions = responseDistrict.data.data.map(district => ({
                                label: district.DistrictName,
                                value: district.DistrictID,
                                // selected: district.DistrictID == response.data.data.districtId // Đánh dấu selected


                            }));
                            this.orderInfo.selectedDistrict = response.data.data.districtId;
                            this.orderInfo.selectedWard = response.data.data.wardId.toString();
                            if (this.orderInfo.selectedDistrict != null) {
                                const responseWard = await apiClient.get(`https://online-gateway.ghn.vn/shiip/public-api/master-data/ward?district_id=${this.orderInfo.selectedDistrict}`);
                                this.wardOptions = responseWard.data.data.map(ward => ({
                                    label: ward.WardName,
                                    value: ward.WardCode,

                                }));


                            }
                        }


                    }

                } catch (error) {
                    notification.error({
                        message: 'Lỗi',
                        description: error.message
                    });
                }
            },
            async fetchPaymentMethods() {
                try {
                    const response = await APIService.get('datacategory/get-list-by-parent-code/PAYMENT_METHOD');

                    this.paymentMethods = response.data.data;
                } catch (error) {
                    notification.error({
                        message: 'Lỗi',
                        description: 'Không thể lấy danh sách phương thức thanh toán'
                    });
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
                    this.selectedProvince = label;
                    this.orderInfo.selectedDistrict = null;
                    this.orderInfo.selectedWard = null;
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
                    this.selectedDistrict = label;
                    this.orderInfo.selectedWard = null;
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
            handleChangeWard(label) {
                this.selectedWard = label;
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
