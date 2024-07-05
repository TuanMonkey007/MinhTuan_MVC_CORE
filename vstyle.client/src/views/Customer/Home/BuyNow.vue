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
            <a-steps :current="1" :direction="horizontal" :responsive="false">
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
        <a-row align="center" justify="center" style="margin-top: 20px; ">
            <a-col :xs="24" :sm="24" :md="10" :lg="10" :xl="10" class="border-deptrai">
                <a-form layout="vertical" ref="formOrder" :model="orderInfo" style="margin: 20px;">

                    <a-form-item label="Họ và tên" name="fullName"
                        :rules="[{ required: true, message: 'Nhập họ tên người nhận' }]">
                        <a-input v-model:value="orderInfo.fullName" placeholder="Nhập họ tên người nhận" />
                    </a-form-item>
                    <a-form-item label="Số điện thoại" name="phoneNumber"
                        :rules="[{ required: true, message: 'Nhập số điện thoại người nhận' },
                            {
                                            pattern: /^[0-9]{10}$/,
                                            message: 'Số điện thoại không hợp lệ!',
                                        },
                        ]">
                        <a-input v-model:value="orderInfo.phoneNumber" placeholder="Nhập số điện thoại" />
                    </a-form-item>

                    <a-row justify="space-between">
                        <a-col :xs="24" :sm="24" :md="7" :lg="7" :xl="7">
                            <a-form-item label="Tỉnh" name="selectedProvince"
                                :rules="[{ required: true, message: 'Chọn Tỉnh/Thành phố' }]">
                                <a-select show-search optionFilterProp="label"
                                    v-model:value="orderInfo.selectedProvince" :options="provinceOptions"
                                    @change="handleChangeProvince" />
                            </a-form-item></a-col>
                        <a-col :xs="24" :sm="24" :md="7" :lg="7" :xl="7">
                            <a-form-item label="Huyện" name="selectedDistrict"
                                :rules="[{ required: true, message: 'Chọn Quận/Huyện' }]">
                                <a-select show-search optionFilterProp="label"
                                    v-model:value="orderInfo.selectedDistrict" :options="districtOptions"
                                    @change="handleChangeDistrict" />
                            </a-form-item></a-col>
                        <a-col :xs="24" :sm="24" :md="7" :lg="7" :xl="7">
                            <a-form-item label="Xã" name="selectedWard"
                                :rules="[{ required: true, message: 'Chọn Huyện/Xã' }]">
                                <a-select show-search optionFilterProp="label" v-model:value="orderInfo.selectedWard"
                                    :options="wardOptions" @change="handleChangeWard" />
                            </a-form-item></a-col>
                    </a-row>
                    <a-form-item label="Địa chỉ cụ thể" name="address"
                        :rules="[{ required: true, message: 'Nhập địa chỉ giao hàng cụ thể' }]">
                        <a-input v-model:value="orderInfo.address" placeholder="Nhập địa chỉ giao hàng" />
                    </a-form-item>
                    <a-form-item label="Ghi chú cho người bán" name="note">
                        <a-textarea v-model:value="orderInfo.note" placeholder="Nhập ghi chú giao hàng (Nếu cần)" />
                    </a-form-item>


                </a-form>
            </a-col>

            <a-col :xs="24" :sm="24" :md="8" :lg="8" :xl="8" style="  margin: 20px">
                <a-card class="border-deptrai">
                    <a-row justify="center">
                        <a-col :span="24">
                            <span
                                style="display: flex;justify-content: center; font-weight: 600;text-align: left; font-size: 16px ; color: #c22026">PHƯƠNG
                                THỨC THANH TOÁN</span>
                        </a-col>
                        <a-col :span="24">

                            <a-radio-group style="display: flex; justify-content: space-around;"
                                v-model:value="orderInfo.paymentMethod"
                                :rules="[{ required: true, message: 'Chọn phương thức thanh toán' }]">
                                <a-radio v-for="item in paymentMethods" :key="item.id" :value="item.id">{{ item.name
                                    }}</a-radio>
                            </a-radio-group>
                        </a-col>
                        <a-col :span="24">
                            <a-divider />
                        </a-col>
                        <a-col :span="24">
                            <a-input-search :loading="loadingSearch" class="input-voucher" v-model:value="searchVoucher"
                                placeholder="Nhập mã giảm giá" style="width: 100%;border-radius: 20px;"
                                @search="onSearchVoucher">
                                <template #enterButton>
                                    <a-button type="primary"
                                        style="border-end-end-radius: 20px;border-start-end-radius: 20px; background-color: #c12227">Áp
                                        dụng</a-button>
                                </template>

                            </a-input-search>
                        </a-col>

                        <a-divider></a-divider>
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

                                <a-col :span="16">
                                    <a-button @click="handleOrderSubmit" type="primary" class="btn-buy-now"
                                        style="border-radius: 20px; width: 100%; background-color: #c12227; height: 40px;">
                                        <a style="color: white;">Đặt
                                            hàng</a>
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
                voucherId: null,
                productVariantId: null,
                quantity: 0,
                price: 0,



            }
        },
        mounted() {
            this.productVariantId = this.$route.params.productVariantId;
            this.price = this.$route.params.price;
            this.quantity = this.$route.params.quantity;
            this.totalPrice = this.price * this.quantity
            this.shipingCost = this.totalPrice > 300000 ? 0 : 30000; //Phí vận chuyển theo tổng tiền hàng
            this.total = this.totalPrice + this.shipingCost;
            this.fetchProvice();
            this.fetchPaymentMethods();
            this.fetchUserInfo();
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
                            this.voucherId = response.data.data.id;
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
                        this.voucherId = response.data.data.id;

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
                return this.price * this.quantity
            },
            async fetchUserInfo() {
                try {
                   
                       

                    


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
            },
            async handleOrderSubmit() {
                console.log(this.orderInfo);
                try {
                    await this.$refs.formOrder.validate();
                    if (this.orderInfo.paymentMethod == '') {
                        notification.warning({
                            message: 'Lỗi',
                            description: 'Vui lòng chọn phương thức thanh toán'
                        });
                        return;
                    }

                    const selectedProvince = this.provinceOptions.find(province => province.value == this.orderInfo.selectedProvince).label;
                    const selectedDistrict = this.districtOptions.find(district => district.value == this.orderInfo.selectedDistrict).label;
                    const selectedWard = this.wardOptions.find(ward => ward.value == this.orderInfo.selectedWard).label;
                   
                    const payload = {
                        customerName: this.orderInfo.fullName,
                        customerPhoneNumber: this.orderInfo.phoneNumber,
                        shippingAddress: `${this.orderInfo.address} | ${selectedWard},${selectedDistrict},${selectedProvince}`,
                        customerNote: this.orderInfo.note,
                        paymentMethod: this.orderInfo.paymentMethod,
                        shippingCost: this.shipingCost,
                        totalAmount: this.total,
                        price: this.price,
                        quantity: this.quantity,
                        productVariantId: this.productVariantId
                    };
                    if (this.voucherId != null) {
                        payload.voucherId = this.voucherId
                    }
                    if (localStorage.getItem('userEmail') != null) { //Lấy userId từ email nếu đã đăng nhập
                        const resGetUserId = await APIService.get(`account/get-user-by-email/${localStorage.getItem('userEmail')}`);
                        payload.userId = resGetUserId.data.data.id;
                    }
                    const response = await APIService.post('order/buy-now', payload);
                    console.log(response)
                    const userCartId = localStorage.getItem('userCartId') || localStorage.getItem('cartId');
                    //Xử lý khi người dùng chưa đăng nhập
                    if (userCartId == localStorage.getItem('cartId')) {
                        localStorage.removeItem('cartId'); //Xóa cartId trong localStorage
                        localStorage.removeItem('userCartId'); //Xóa userCartId trong localStorage
                        const orderIds = JSON.parse(localStorage.getItem('orderIds')) || [];
                        orderIds.push(response.data.status);
                        localStorage.setItem('orderIds', JSON.stringify(orderIds));
                    } else {
                        localStorage.removeItem('userCartId'); //Xóa userCartId trong localStorage
                    }
                    if (response.data.message.includes('http')) {
                        window.location.href = response.data.message

                    } else {
                        this.$router.push({
                            name: 'OrderSuccess',
                            query: {
                                success: 'true', // Hoặc 'false' tùy thuộc vào trạng thái đặt hàng
                                ordercode: response.data.status // Mã đơn hàng
                            }
                        });

                        notification.success({
                            message: 'Thống báo',
                            description: 'Đặt hàng thành công'
                        });
                    }




                }
                catch (error) {
                    notification.warning({
                        message: 'Lỗi',
                        description: "Vui lòng điền đầy đủ thông tin"
                    });

                }


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
