<template>
    <a-row>
        <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <a-page-header style="
         border: 1px solid rgb(235, 237, 240);
          background-color: #c21f24;
        ">

                <template #title>
                    <a-breadcrumb separator=">" style="margin-left:30px;">
                        <a-breadcrumb-item href="">

                            <span style="color: #fff;"> Trang chủ</span>
                        </a-breadcrumb-item>

                        <a-breadcrumb-item class="active">

                            <span style="color: #fff;"> Theo dõi đơn hàng</span>
                        </a-breadcrumb-item>

                    </a-breadcrumb>

                </template>
                <template #extra>

                </template>
            </a-page-header>
        </a-col>
    </a-row>

    <transition name="route" mode="out-in" appear>


        
        <a-row>
            <a-col :xs="24" :sm="24" :md="6" :lg="6" :xl="6">
                <a-row justify="center">
                    <a-col :span="24"> <a-card :bordered="true" style="margin:20px; min-height: 25vh">
                            <a-row justify="center" align="center">
                                <a-col :span="24" style="display: flex;align-items: center;justify-content: center;">
                                    <a-badge>
                                        <template #count>
                                            <font-awesome-icon v-if="checkEmailConfirmed" icon="fa-regular fa-thumbs-up"
                                                style="color: #015efe; font-size: 16px;" />
                                            <font-awesome-icon v-else icon="fa-regular fa-thumbs-down"
                                                style="color: #aaaeb6;" />
                                        </template>
                                        <a-avatar v-if="avatarBase64"
                                            :src="'data:' + avatarContentType + ';base64,' + avatarBase64"
                                            :size="100"></a-avatar>
                                        <a-avatar v-else :size="100">
                                            <template #icon><font-awesome-icon icon="fa-regular fa-user" /></template>
                                        </a-avatar>
                                    </a-badge>
                                </a-col>
                                <a-col style="display: flex;align-items: center;justify-content: center;" :span="24">
                                    <h3>{{ this.account.fullName }}</h3>
                                </a-col>
                                <a-col style="display: flex;align-items: center; justify-content: center;" :span="24">{{
                                    this.account.email }}</a-col>
                                <a-col style="display: flex;align-items: center; justify-content: center;    font-size: 12px;
    font-style: italic;
    font-weight: 300;" :span="24">{{
        checkEmailConfirmed ? "Đã xác thực" : " Chưa xác thực" }}</a-col>

                            </a-row>
                        </a-card>
                    </a-col>
                    <a-col :span="16">
                            <a-input-search :loading="loadingSearch" class="input-voucher" v-model:value="searchOrder"
                                placeholder="Nhập mã đơn hàng" style="width: 100%;border-radius: 20px;"
                                @search="onSearchOrder">
                                <template #enterButton>
                                    <a-button type="primary"
                                        style="border-end-end-radius: 20px;border-start-end-radius: 20px; background-color: #c12227">Tra cứu</a-button>
                                </template>

                            </a-input-search>
                        </a-col>
                </a-row>



            </a-col>
            <a-col  v-if="isNoOrder == true" :xs="24" :sm="24" :md="16" :lg="16" :xl="16">
                <a-card :bordered="true" style="margin:0px;min-height: 50vh">
                <a-row align="center" justify="center" style="margin-top: 20px;">

<a-col :span="24">
    <a-result status="404" title="Không tìm thấy đơn hàng" sub-title="Hãy mua hàng tại VStyle trước ">
        <template #extra>
            <router-link class="btn-buy-now"
                style="border-radius: 20px; max-width:200px; display: inline-flex;align-items: center;justify-content: center;background-color: #c12227; height: 40px;"
                :to="{ name: 'CustomerHome' }">Ghé thăm cửa hàng</router-link>
        </template>
    </a-result>
</a-col>
</a-row>
</a-card>
            </a-col>
           
            <a-col v-else :xs="24" :sm="24" :md="16" :lg="16" :xl="16">
                <a-card :bordered="true" style="margin:0px;min-height: 50vh">
                    <a-row>
                        <a-col :span="24">
                            <a-collapse v-model:activeKey="activeKey" :bordered="false" expand-icon-position="left">

                                <a-collapse-panel v-for="item in orderInfo" :key="item.id" :style="customStyle">
                                    <template #header>
                                        <a-row justify="space-between">
                                            <a-col :span="7">
                                                <span>MÃ: </span>
                                                <span style="font-weight: 600;text-align: left; font-size: 16px"> {{
                                                    item.code }}
                                                </span>
                                                <a-divider type="vertical"></a-divider>
                                            </a-col>
                                            <a-col :span="7">
                                                <span>THỜI GIAN ĐẶT: </span>
                                                <span style="font-weight: 600;text-align: left; font-size: 16px">
                                                    {{ formatCreatedDate(item.createdDate) }}
                                                </span>
                                                <a-divider type="vertical"></a-divider>
                                            </a-col>
                                            <a-col :span="7" style="text-align: right;">
                                                <span style="font-weight: 600; font-size: 16px"> <a-tag
                                                        :color="getColorTag(item.isCancelled)">
                                                        {{ item.isCancelled ? 'Đã hủy' : item.statusName }}</a-tag>
                                                </span>

                                            </a-col>
                                        </a-row>




                                    </template>
                                    <a-row>

                                        <a-row>
                                            <a-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
                                                <a-row>
                                                    <a-col :xs="16" :sm="16" :md="16" :lg="24" :xl="24">
                                                        <a-steps direction="vertical"
                                                            v-model:current="currentSteps[item.status]"
                                                            :status="item.isCancelled ? 'error' : 'process'">
                                                            <a-step
                                                                v-for="(status, index) in getStepList(item.paymentMethodName)"
                                                                :key="index" :title="status.name" :disabled="true" />
                                                        </a-steps>
                                                    </a-col>
                                                    <a-col :xs="8" :sm="8" :md="8" :lg="24" :xl="24">
                                                        <router-link
                                                            :to="{ name: 'TrackingOrderDetail', params: { id: item.id } }">
                                                            <a-button type="primary"
                                                                style="border-radius: 20px; width:fit-content; background-color: #ffffff; border-color: #dc3a35; height: 40px">
                                                                <a style="color: #dc3a35;"> Xem chi tiết</a>
                                                            </a-button>
                                                        </router-link>
                                                    </a-col>
                                                </a-row>


                                            </a-col>
                                            <a-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12" style="padding: 10px">
                                                <a-descriptions title="Thông tin giao hàng">
                                                    <a-descriptions-item span="24" label="Họ và tên người nhận">{{
                                                        item.customerName }}</a-descriptions-item>
                                                    <a-descriptions-item span="24" label="Số điện thoại">{{
                                                        item.customerPhoneNumber }}</a-descriptions-item>
                                                    <a-descriptions-item span="24" label="Địa chỉ">{{
                                                        item.shippingAddress
                                                    }}</a-descriptions-item>
                                                    <a-descriptions-item span="24" label="Ghi chú ">{{ item.customerNote
                                                        }}
                                                    </a-descriptions-item>
                                                    <a-descriptions-item span="24" label="Thời gian đặt">{{
                                                        formatCreatedDate(item.createdDate) }}</a-descriptions-item>
                                                    <a-descriptions-item span="24"
                                                        label="Đơn vị vận chuyển ">GHN</a-descriptions-item>
                                                </a-descriptions>
                                            </a-col>

                                        </a-row>
                                        <a-row>
                                            <a-col :span="24">


                                            </a-col>
                                        </a-row>

                                    </a-row>
                                </a-collapse-panel>

                            </a-collapse>
                        </a-col>
                    </a-row>
                </a-card>
            </a-col>
        </a-row>

    </transition>

</template>
<script>
    import { ref, reactive } from 'vue';
    import dayjs from 'dayjs';
    import { notification } from 'ant-design-vue';
    import APIService from '@/helpers/APIService';
    import axios from 'axios';

    export default {
        methods: {
            async onSearchOrder() {
                
                let orderIds = [this.searchOrder]
                const xxx = JSON.stringify(orderIds)
               
                
                const response = await APIService.post(`order/get-order-info-by-list-order-code`, JSON.parse(xxx));
                this.orderInfo = response.data.data;
               
                this.isNoOrder = this.orderInfo.length == 0 ? true : false
            },
            getColorTag(isCancelled) {
                if (isCancelled == true) {
                    return 'red'
                }
                return 'green'
            },
            fomartPrice(price) {
                return price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.');
            },
            formatCreatedDate(date) {
                return dayjs(date).format("HH:mm:ss DD/MM/YYYY ");
            },
            async fetchAllOrderInfo() {
                try {
                    const userEmail = localStorage.getItem('userEmail');
                    if (userEmail != null) {
                        const response = await APIService.get(`order/get-order-info-by-user-email/${userEmail}`);
                        this.orderInfo = response.data.data;
                    } else {
                        const orderIds = localStorage.getItem('orderIds');
                        if (orderIds == null) this.isNoOrder = true
                        const response = await APIService.post(`order/get-order-info-by-list-order-code`, JSON.parse(orderIds));
                        this.orderInfo = response.data.data;
                    }
                    this.isNoOrder = this.orderInfo.length == 0 ? true : false


                }
                catch (error) {
                    console.error('Error fetching order info:', error.message);
                }
            },
            async fetchStatusStep() {
                try {
                    const resVNPay = await APIService.get(`datacategory/get-list-by-parent-code/STATUS_VNPAY`);
                    const resCOD = await APIService.get(`datacategory/get-list-by-parent-code/STATUS_COD`);
                    this.activeKeyCOD = resCOD.data.data;
                    this.activeKeyVNPay = resVNPay.data.data;
                    // Cập nhật currentSteps sau khi fetch dữ liệu
                    for (const order of this.orderInfo) {
                        this.updateCurrentStep(order);
                    }
                }
                catch (error) {
                    console.error('Error fetching order info:', error.message);
                }
            },
            updateCurrentStep(order) {
                const steps = this.getStepList(order.paymentMethodName);

                const currentStep = steps.findIndex(
                    (item) => item.id == order.status
                );

                this.currentSteps[order.status] = currentStep

            },
            getStepList(paymentMethodName) {
                if (paymentMethodName == 'VNPay') {
                    return this.activeKeyVNPay;
                }
                return this.activeKeyCOD;
            }





        },
        computed: {
            formattedBirthDay() {
                return this.account.birthDay ? dayjs(this.account.birthDay).format('DD/MM/YYYY') : '';
            },
        },
        async beforeMount() {

        },
        async mounted() {
            await this.fetchAllOrderInfo();
            await this.fetchStatusStep();


            const email = localStorage.getItem('userEmail')

            if (email != null) {
                const serverResponse = await APIService.get(`account/get-user-by-email/${email}`)
                this.account = {
                    ...serverResponse.data.data,


                };

                this.id = serverResponse.data.data.id;
                this.avatarBase64 = serverResponse.data.data.avatarBase64;
                this.avatarContentType = serverResponse.data.data.avatarContentType;
                if (serverResponse.data.data.lockoutEnd && dayjs(serverResponse.data.data.lockoutEnd) > dayjs()) {
                    this.checkLockOut = true;
                } else {
                    this.checkLockOut = false;
                }
                this.checkEmailConfirmed = serverResponse.data.data.emailConfirmed;
            }








            // Kiểm tra nếu có avatar và chuyển đổi thành fileList
            if (this.avatarBase64) {
                const byteCharacters = atob(this.avatarBase64);
                const byteNumbers = new Array(byteCharacters.length);
                for (let i = 0; i < byteCharacters.length; i++) { // Sửa vòng lặp for
                    byteNumbers[i] = byteCharacters.charCodeAt(i);
                }
                const byteArray = new Uint8Array(byteNumbers);
                const blob = new Blob([byteArray], { type: this.avatarContentType });
                const file = new File([blob], 'avatar', { type: this.avatarContentType });

                this.fileList = [
                    {
                        uid: '-1', // ID duy nhất
                        name: 'avatar', // Tên tệp
                        status: 'done', // Trạng thái tải lên thành công
                        url: URL.createObjectURL(file), // Hoặc đường dẫn tạm thời khác
                        originFileObj: file // Đối tượng File để upload
                    },
                ];
            } else {
                this.fileList = [];
            }



        },



        data() {
            const account = reactive({
                fullName: '',
                phoneNumber: '',
                email: '',
                gender: '',
                nameGender: '',
                birthDay: '',
                address: '',
                avatar: '',
                avatarBase64: '',
                avatarContentType: '',

                provinceId: '',
                districtId: '',
                wardId: '',
            });
            const activeKey = ref('');
            const customStyle = 'background: #f7f7f7;border-radius: 4px;margin-bottom: 24px;border: 0;overflow: hidden';

            return {
                activeKey,
                customStyle,
                account,
                fileList: [],
                avatarBase64: null,
                avatarContentType: null,
                listGender: [],
                isChangeAvatar: false,
                apiUrl: '',
                id: '',
                provinceOptions: [],
                selectedProvince: '',
                districtOptions: [],
                selectedDistrict: '',
                wardOptions: [],
                selectedWard: '',
                addressChange: '',
                orderInfo: [],
                activeKeyCOD: [],
                activeKeyVNPay: [],
                currentSteps: [],
                isNoOrder: false,
                searchOrder: '',

            }
        },//end data
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
</style>