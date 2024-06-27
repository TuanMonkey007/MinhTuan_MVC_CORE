<template>
    <div>
        <a-modal v-model:open="open" title="CẬP NHẬT MÃ GIẢM GIÁ" :footer="null">
            <a-form ref="formRef" :model="voucher" layout="vertical" style="width: 100%;">
                <a-row :gutter="70">
                    <a-col :xs="24" :sm="24" :md="12" :lg="12" :xl="12">
                        <a-form-item ref="code" label="Mã giảm giá" name="code" :rules="[
                            {
                                required: true,
                                message: 'Mã giảm giá là bắt buộc',
                                trigger: 'change',
                            },
                            {
                                min: 5,
                                max: 20,
                                message: 'Độ dài từ 5-20',
                                trigger: 'blur',
                            },
                        ]">
                            <a-input v-model:value="voucher.code" @input="formatInput" />
                        </a-form-item>
                    </a-col>
                    <a-col :xs="24" :sm="24" :md="12" :lg="12" :xl="12">
                        <a-form-item ref="quantity" label="Số lượng" name="quantity">
                            <a-input-number v-model:value="voucher.quantity"></a-input-number>
                        </a-form-item>
                    </a-col>
                </a-row>
                <a-row>
                    <a-col :span="24">
                        <a-form-item ref="description" label="Khoảng thời gian">
                            <a-range-picker :placeholder="['Ngày bắt đầu','Ngày kết thúc']" show-time v-model:value="rangeTime" format="YYYY-MM-DD HH:mm:ss"
                                @change="handleChangeRangeTime" />
                        </a-form-item>
                    </a-col>
                </a-row>
                <a-row>
                    <a-col :span="24"><a-form-item ref="quantity" label="Đơn hàng tối thiểu"
                            name="minimumPurchaseAmount">
                            <a-input-number v-model:value="voucher.minimumPurchaseAmount"></a-input-number>
                        </a-form-item></a-col>
                </a-row>
                <a-row>
                    <a-col :span="24">
                        <a-tabs v-model:activeKey="activeKey">
                            <a-tab-pane key="1" force-render>
                                <template #tab>
                                    <span>
                                        <font-awesome-icon icon="fa-solid fa-tags" style="color: #059e57;" />
                                        Giảm trực tiếp
                                    </span>
                                </template>
                                <a-form-item ref="quantity" label="Số tiền giảm" name="discountAmount">
                                    <a-input-number v-model:value="voucher.discountAmount"></a-input-number>
                                </a-form-item>
                            </a-tab-pane>
                            <a-tab-pane key="0">
                                <template #tab>
                                    <span>
                                        <font-awesome-icon icon="fa-solid fa-percent" style="color: #f57c0a;" />
                                        Giảm theo phần trăm
                                    </span>
                                </template>
                                <a-row :gutter="70">
                                    <a-col :xs="24" :sm="24" :md="8" :lg="8" :xl="8"><a-form-item ref="quantity"
                                            label="Giảm tối đa" name="maxValue">
                                            <a-input-number v-model:value="voucher.maxValue"></a-input-number>
                                        </a-form-item></a-col>
                                    <a-col :xs="24" :sm="24" :md="16" :lg="16" :xl="16">
                            
                                        <a-form-item ref="quantity" label="Tỷ lệ giảm" name="discountPercent">
                                            <a-row>
                                                <a-col :span="12">
                                                    <a-slider v-model:value="voucher.discountPercent" :min="1" :max="100" />
                                                </a-col>
                                                <a-col :span="4">
                                                    <a-input-number v-model:value="voucher.discountPercent" :min="1" :max="100"
                                                        style="margin-left: 16px" />
                                                </a-col>
                                            </a-row>
                                        </a-form-item>
                                    </a-col>

                                </a-row>


                            </a-tab-pane>
                        </a-tabs>
                    </a-col>
                </a-row>



                <a-divider></a-divider>
                <a-form-item>
                    <a-button @click="handleSubmitAsync"  :loading="isLoading"
                        type="primary" style="margin-right: 20px;" class="login-form-button">
                        Hoàn thành
                    </a-button>
                    <a-button type="normal" @click="closeModal" style="margin-right: 20px;" class="login-form-button">
                        Đóng
                    </a-button>


                </a-form-item>
            </a-form>

        </a-modal>
    </div>
</template>
<script>
    import { computed } from "vue";
    import APIService from "@/helpers/APIService"
    import { ref, reactive } from "vue";
    import { message, notification } from "ant-design-vue";
    import dayjs from "dayjs";
    export default {
     
        data() {

            const voucher = reactive({
                code: "",
                minimumPurchaseAmount: 0,
                discountAmount: 0,
                discountPercent: 100,
                maxValue: 0,
                type: 0,
                timeStart: null,
                timeEnd: null,
                quantity: 0,
            })

            return {
                id: undefined,
                voucher,
                isLoading: false,

                labelCol: {
                    span: 6,
                },
                wrapperCol: {
                    span: 14,
                },
                rangeTime: [],
                open: false,
                activeKey: "0",

            }
        },

        methods: {
            handleChangeRangeTime(value) {
                this.voucher.timeStart = value[0].toISOString()
                this.voucher.timeEnd = value[1].toISOString()

                
            },
            formatInput(event) {
                // Chuyển đổi thành chữ in hoa
                let inputValue = event.target.value.toUpperCase();
                // Loại bỏ khoảng trắng
                inputValue = inputValue.replace(/\s+/g, '');
                inputValue = inputValue.replace(/[^A-Z0-9_-]/g, '');
                this.voucher.code = inputValue;
            },
            async showModal(id) {
                this.open = true
                const serverResponse = await APIService.get(`voucher/${id}`)
                this.voucher = {
                    ...serverResponse.data.data,
                    timeStart: dayjs(serverResponse.data.data.timeStart),
                    timeEnd: dayjs(serverResponse.data.data.timeEnd)
                }
                this.rangeTime = [dayjs(serverResponse.data.data.timeStart), dayjs(serverResponse.data.data.timeEnd)]


                this.id = id
                console.log(this.voucher)

            },
            closeModal() {
                this.open = false

                this.$refs.formRef.resetFields();
            },
           
            async handleSubmitAsync() {
                this.$refs.formRef.validate().then(async () => {
                    this.isLoading = true
                    this.voucher.type = this.activeKey
                    const serverResponse = await APIService.put(`voucher/update/${this.id}`, this.voucher)
                    if (serverResponse.data.message == "Cập nhật thành công") {
                        notification.success({
                            message: 'Thông báo',
                            description: 'Cập nhật thành công'
                        })
                        this.closeModal()
                    } else {
                        notification.error({
                            message: 'Thông báo',
                            description: 'Cập nhật thất bại'
                        })
                    }
                    this.isLoading = false
                    this.$emit('updateSuccess')
                }).catch(error => {
                    console.log('error', error);
                }).finally(() => {
                    this.isLoading = false;
                })
            }

        }
    }

</script>