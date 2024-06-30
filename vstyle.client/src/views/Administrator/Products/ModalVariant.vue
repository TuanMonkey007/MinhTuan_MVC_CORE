<template>
    <div>
        <a-modal v-model:open="open" title="THÊM KIỂU DÁNG SẢN PHẨM" :footer="null" width="600px" style="top: 20px">
            <a-divider style="margin-top: 0px"></a-divider>

            <a-form v-if="open" ref="formRef" name="dynamic_form_nest_item" :model="dynamicValidateForm" @finish="onFinish">
                <a-row v-for="(productVariant, index) in this.dynamicValidateForm.productVariants"
                    :key="productVariant.productId" :gutter="16">

                    <a-col :span="6">
                        <a-tooltip title="Kích cỡ">
                            <a-form-item :name="['productVariants', index, 'sizeId']" :rules="{
                                required: true,
                                message: 'Chọn Size',
                            }">
                                <!-- <a-input v-model:value="productVariant.size" placeholder="Kích cỡ" /> -->
                                <a-select :disabled="!productVariant.changeable" show-search optionFilterProp="label" v-model:value="productVariant.sizeId" placeholder="Kích cỡ"
                                    :options="sizeOptions" @select="handleSelectSize(index)">
                                </a-select>
                            </a-form-item>
                        </a-tooltip>

                    </a-col>
                    <a-col :span="6">
                        <a-tooltip title="Màu sắc">
                            <a-form-item :name="['productVariants', index, 'colorId']" :rules="{
                                required: true,
                                message: 'Chọn màu sắc',
                            }">

                                <a-select :disabled="!productVariant.changeable"  show-search optionFilterProp="label" v-model:value="productVariant.colorId" placeholder="Màu sắc"
                                    :options="colorOptions" @select="handleSelectColor(index)">

                                </a-select>
                            </a-form-item>
                        </a-tooltip>

                    </a-col>
                    <a-col :span="6">
                        <a-tooltip title="Giá tiền">
                            <a-form-item :name="['productVariants', index, 'price']" :rules="[{
                                required: true,
                                message: 'Nhập giá tiền',
                            }]">
                                <a-input-number v-model:value="productVariant.price" placeholder="Giá tiền" />
                            </a-form-item>
                        </a-tooltip>

                    </a-col>
                    <a-col :span="5">
                        <a-tooltip title="Số lượng">
                            <a-form-item :name="['productVariants', index, 'stockQuantity']" :rules="{
                                required: true,
                                message: 'Nhập số lượng',
                            }">
                                <a-input type="number" v-model:value="productVariant.stockQuantity"
                                    placeholder="Số lượng" />
                            </a-form-item>
                        </a-tooltip>

                    </a-col>
                    <a-col :span="1">
                        <MinusCircleOutlined v-if="productVariant.changeable == true" @click="removeproductVariant(productVariant)" />
                    </a-col>


                </a-row>
                <a-form-item>
                    <a-button type="dashed" block @click="addproductVariant">
                        <PlusOutlined />
                        Thêm kiểu dáng sản phẩm
                    </a-button>
                </a-form-item>
                <a-form-item>
                    <a-button type="primary" html-type="submit" :loading="isLoading">Hoàn thành</a-button>
                    <a-button @click="closeModal" style="margin-left: 20px;">
                        Đóng
                    </a-button>
                </a-form-item>
            </a-form>
        </a-modal>
    </div>
</template>
<script>
    import { computed, watch } from "vue";
    import APIService from "@/helpers/APIService";
    import { ref, reactive } from "vue";
    import { message, notification } from "ant-design-vue";
    import { MinusCircleOutlined, PlusOutlined } from "@ant-design/icons-vue";
    export default {
        components: {
            MinusCircleOutlined,
            PlusOutlined,
        },
        setup() {

            const formRef = ref();
            const dynamicValidateForm = reactive({
                productVariants: [],
            });
            const sizeOptions = ref([]);
            const colorOptions = ref([]);
            const removeproductVariant = (item) => {
                let index = dynamicValidateForm.productVariants.indexOf(item);
                if (index !== -1) {
                    dynamicValidateForm.productVariants.splice(index, 1);
                }
            };

            return {
                isLoading: false,
                labelCol: {
                    span: 6,
                },
                wrapperCol: {
                    span: 14,
                },
                formRef,
                dynamicValidateForm,
                removeproductVariant,
                sizeOptions,
                colorOptions,
            };
        }, // end setup
        data() {
            return {
                open: false,
                productId: null,
                defaultPrice: 0
            };
        },

       async mounted() {
             await  this.fetchColorAndSize();
          
            
        },
        watch: {
            'dynamicValidateForm.productVariants': {
                handler(newVariants) {
                    if (newVariants && newVariants.length > 0 && this.isModalOpen) {
                        newVariants.forEach((_, index) => {
                            this.validateUniqueCombination(index);
                        });
                    }
                },
                deep: true
            }
        },
        methods: {

            validateUniqueCombination(currentVariantIndex) {
                const currentVariant = this.dynamicValidateForm.productVariants[currentVariantIndex];

                // Kiểm tra nếu cả sizeId và colorId đều đã được chọn
                if (!currentVariant.sizeId || !currentVariant.colorId) {
                    return;
                }

                // Kiểm tra xem có productVariant nào khác có cùng sizeId và colorId
                const isDuplicate = this.dynamicValidateForm.productVariants.some((pv, index) => {
                    return index !== currentVariantIndex && pv.sizeId === currentVariant.sizeId && pv.colorId === currentVariant.colorId;
                });

                if (isDuplicate) {
                    notification.warning({
                        message: 'Thông báo',
                        description: 'Kích cỡ và màu sắc đã được chọn. Vui lòng chọn kích cỡ hoặc màu sắc khác.',
                        duration: 2
                    });

                    // Xóa giá trị sizeId và colorId của productVariant hiện tại
                    currentVariant.sizeId = null;
                    currentVariant.colorId = null;
                }
            },
            handleSelectSize(index) {
                this.validateUniqueCombination(index); // Kiểm tra khi chọn size
            },
            handleSelectColor(index) {
                this.validateUniqueCombination(index); // Kiểm tra khi chọn color
            },

            async onFinish() {
                this.isLoading = true;
                //message.loading({ message: 'Đang xử lý...', key: 'keyLoading', duration: 0 });
                notification.info({
                    message: 'Thông báo',
                    description: 'Đang xử lý...',
                    key: 'keyLoading',
                    duration: 0
                });
                // Chuẩn bị dữ liệu
                const listModel = this.dynamicValidateForm.productVariants.map((productVariant) => ({
                    id: productVariant.id,
                    sizeId: productVariant.sizeId,
                    colorId: productVariant.colorId,
                    price: productVariant.price,
                    stockQuantity: productVariant.stockQuantity,
                    productId: this.productId,
                }));
                try{

                
                // Gửi dữ liệu lên server
                const response = await APIService.put(`product/update-product-variant/${this.productId}`, { listModel: listModel })
                if(response.data.message == "Cập nhật thành công"){
                    notification.success({ message: response.data.message, key: 'keyLoading', duration: 2 });
                    this.closeModal()
                    this.$emit('addSuccess')
                }else{
                    notification.error({ message: response.data.message, key: 'keyLoading', duration: 2 });
                }}
                catch(error){
                    notification.error({ message: "Lỗi", key: 'keyLoading', duration: 2 });
                }
            },
            addproductVariant() {
                this.dynamicValidateForm.productVariants.push({
                    sizeId: null,
                    colorId: null,
                    price: this.defaultPrice,
                    stockQuantity: 0,
                    productId: this.productId,
                    changeable: true
                });

            },
            async fetchColorAndSize() {
                const serverResponse = await APIService.get('datacategory/get-list-by-parent-code/MAU_SAC')
                this.colorOptions = serverResponse.data.data.map(item => {
                    return {
                        label: item.name,
                        value: item.id
                    }
                })
                const serverResponse2 = await APIService.get('datacategory/get-list-by-parent-code/KICH_CO')
                this.sizeOptions = serverResponse2.data.data.map(item => {
                    return {
                        label: item.name,
                        value: item.id
                    }
                })
            },
            async fetchAllProductVariant() {
                const serverResponse = await APIService.get(`product/get-product-variant/${this.productId}`)
                this.dynamicValidateForm.productVariants = serverResponse.data.data
                console.log("danh sách size", this.dynamicValidateForm.productVariants)
                const res =  await APIService.get(`product/${this.productId}`);
                this.defaultPrice = res.data.data.items[0].price;
            },
            showModal(id) {
               
                this.productId = id
                this.fetchAllProductVariant()
                this.open = true;
            },
            closeModal() {
                this.open = false;
                this.dynamicValidateForm.productVariants = [];
                this.isLoading = false;
                // this.$refs.formRef.resetFields();
            },
        }, // end methods
    };
</script>
