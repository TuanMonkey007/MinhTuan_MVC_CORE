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
                <a-card :bordered="true" style="margin:20px; min-height: 25vh">
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
                                    :size="100">
                                    
                                </a-avatar>
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
            <a-col :xs="24" :sm="24" :md="16" :lg="16" :xl="16">
                <a-card :bordered="true" style="margin:20px;min-height: 50vh">
                    <a-row>

                        <a-col :xs="24" :sm="24" :md="12" :lg="12" :xl="12">
                            <a-steps direction="vertical" v-model:current="currentStep"
                                :status="this.orderInfo.isCancelled ? 'error' : 'process'">
                                <a-step v-for="(status, index) in orderStatusList" :key="index" :title="status.name"
                                    :disabled="true" />
                            </a-steps>
                        </a-col>
                        <a-col :xs="24" :sm="24" :md="12" :lg="12" :xl="12">
                            <a-descriptions title="Thông tin đặt hàng">
                                <a-descriptions-item span="24" label="Họ và tên người nhận">{{ orderInfo.customerName
                                    }}</a-descriptions-item>
                                <a-descriptions-item span="24" label="Số điện thoại">{{ orderInfo.customerPhoneNumber
                                    }}</a-descriptions-item>
                                <a-descriptions-item span="24" label="Địa chỉ">{{ orderInfo.shippingAddress
                                    }}</a-descriptions-item>
                                <a-descriptions-item span="24" label="Ghi chú ">{{ orderInfo.customerNote
                                    }}</a-descriptions-item>

                                <a-descriptions-item span="24" label="Thời gian đặt">{{
                                    formatCreatedDate(orderInfo.createdDate) }}</a-descriptions-item>
                                <a-descriptions-item span="24" label="Phương thức thanh toán"><a-tag color="default">{{
                                    orderInfo.paymentMethodName
                                        }}</a-tag></a-descriptions-item>
                                <a-descriptions-item v-if="isVnpay" span="24" label="Trạng thái thanh toán">
                                    <a-tag color="green"> {{ this.orderInfo.paymentStatusName }}</a-tag>
                                </a-descriptions-item>
                                <a-descriptions-item v-if="this.orderInfo.voucherCode != null" span="24" label="Mã giảm giá">
                                    <a-tag color="green"> {{ this.orderInfo.voucherCode }}</a-tag>
                                </a-descriptions-item>
                            </a-descriptions>

                            <a-descriptions title="Thông tin vận chuyển">
                                <a-descriptions-item span="24" label="Đơn vị vận chuyển ">GHN</a-descriptions-item>
                                <a-descriptions-item span="24" label="Mã đơn">{{ orderInfo.code
                                    }}</a-descriptions-item><a-descriptions-item span="24" label="Trạng thái">
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
                        <a-col :span="24">
                            <span style="font-weight: bold; font-size: 18px;">Danh sách sản phẩm</span>
                            <a-table :columns="orderItemscolumns" :data-source="orderItemsTable" :pagination="false"
                                :scroll="{ x: 1000 }">
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
                        <a-col :span="24">
                            <a-row justify="end">

                                <a-col :xs="24" :sm="24" :md="8" :lg="8" :xl="8"
                                    style=" margin-top: 10px; display: flex; justify-content: space-between">
                                    <span style="font-weight: 600;font-size: 16px">TỔNG THANH
                                        TOÁN</span>
                                    <span style="font-weight: 600;color: #c12227; font-size: 20px;">
                                        {{ fomartPrice(this.orderInfo.totalAmount) }}&#8363;

                                    </span>
                                </a-col>
                            </a-row>
                            <a-row justify="end" style="margin-top: 10px">
                                <a-col :xs="12" :sm="12" :md="4" :lg="4" :xl="4"
                                    style="display: flex; justify-content: end">
                                    <a-button type="primary" v-if="userCancel" @click="handleCancelOrder"
                                        style="border-radius: 20px; width:fit-content; background-color: #ffffff; border-color: #dc3a35; height: 40px">
                                        <a style="color: #dc3a35; font-family: Archivo"> Hủy đơn hàng</a>
                                    </a-button>
                                </a-col>
                                <a-col v-if="this.showPayment == true" :xs="12" :sm="12" :md="4" :lg="4" :xl="4"
                                    style="display: flex; justify-content: end">
                                    <a-button type="primary" v-if="userCancel" @click="handleGetLinkPayment"
                                        style="border-radius: 20px; width:fit-content; background-color: #c12227; border-color: #dc3a35; height: 40px">
                                        <a style="color: #ffffff; font-family: Archivo"> Thanh toán</a>
                                    </a-button>
                                </a-col>
                            </a-row>


                        </a-col>
                    </a-row>
                </a-card>
            </a-col>
        </a-row>
    </transition>
    <ModalCancel @updateSuccess="handleCancelSuccess" ref="modalCancel"/>
 
</template>
<script>
import ModalCancel from '@/views/Customer/ModalCancel.vue';
    import { ref, reactive } from 'vue';
    import dayjs from 'dayjs';
    import { notification } from 'ant-design-vue';
    import APIService from '@/helpers/APIService';
    import axios from 'axios';
import { UserOutlined } from '@ant-design/icons-vue';

    export default {
        components: {
            ModalCancel,
        
        },
        methods: {
           async handleGetLinkPayment() {
                const id = this.$route.params.id;
                const response = await  APIService.get(`order/get-link-pay-vnpay/${id}`);
                window.location.href = response.data.message
            },
            handleCancelSuccess() {
                this.userCancel = false
            },
            handleCancelOrder(){
                this.$refs.modalCancel.showModal(this.orderInfo.id)
            },

            getColor(isCancelled) {
                return isCancelled ? "red" : "green";
            },
            getColorTag(isCancelled) {
                if (isCancelled == true) {
                    return 'red'
                }
                return 'green'
            },
            fomartPrice(price) {
                if (price != null) {
                    return price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.');
                }
                return 0

            },
            formatCreatedDate(date) {
                return dayjs(date).format("HH:mm:ss DD/MM/YYYY ");
            },
            async fetchOrderInfo() {
                const id = this.$route.params.id;
                const response = await APIService.get(`order/get-order-info-by-id/${id}`);
                this.orderInfo = response.data.data;
                if (this.orderInfo.paymentMethodName == 'VNPay') {
                    this.isVnpay = true
                    if(this.orderInfo.paymentStatusName =='Chờ thanh toán'){
                        this.showPayment = true
                    }
                }
                
                


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
            async fetchOrderItem() {

                const response = await APIService.get(`order/get-order-item-by-id/${this.orderInfo.id}`);

                this.orderItemsTable = response.data.data;
            },






        },
        computed: {
            formattedBirthDay() {
                return this.account.birthDay ? dayjs(this.account.birthDay).format('DD/MM/YYYY') : '';
            },
        },
        async mounted() {
            await this.fetchOrderInfo();
            await this.fetchListStatus();
            await this.fetchOrderItem();
            const email = localStorage.getItem('userEmail');
            if(email != null){
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
            
            if(this.orderInfo.isCancelled == true){
                this.userCancel = false
            }else if(this.orderInfo.statusName.includes("Chờ")){
                this.userCancel = true
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
                showPayment: '',
                isVnpay: false,
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
                orderStatusList: [],
                currentStep: 0,
                orderInfo: [],
                activeKeyCOD: [],
                activeKeyVNPay: [],
                currentSteps: [],
                listSteps: [],
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
                userCancel: false,

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