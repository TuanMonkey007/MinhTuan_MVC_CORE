<template>
  <div>
    <a-modal v-model:open="open" title="CẬP NHẬT TRẠNG THÁI ĐƠN HÀNG" :footer="null" width="400px" style="top: 20px">
      <a-divider></a-divider>
      <a-form>
        <a-row>
          <a-col :span="24">

            <a-radio-group v-model:value="selectedStatus">
              <a-space direction="vertical">
                <a-radio v-for="item in listStatus" :key="item.id" :value="item.id" :disabled="getDisable(item.name)">
                  {{ item.name }}
                </a-radio>
              </a-space>
            </a-radio-group>


          </a-col>
        </a-row>
        <a-divider></a-divider>
        <a-row>
          <a-col :span="24" style="justify-content: center">
            <a-form-item>
              <a-button @click="handleSubmitAsync" :loading="isLoading" type="primary" style="margin-right: 20px"
                class="login-form-button">
                Hoàn thành
              </a-button>
              <a-button @click="closeModal" style="margin-right: 20px" class="login-form-button">
                Đóng
              </a-button>
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-modal>
  </div>
</template>
<script>
  import { message, notification } from "ant-design-vue";
  import APIService from "@/helpers/APIService";
  import { ref, reactive } from "vue";

  export default {
    setup() {
      return {
        open: ref(false),
        id: "",
        isLoading: ref(false),

        labelCol: {
          span: 6,
        },
        wrapperCol: {
          span: 14,
        },
        allReason: [],
        selectedRoles: ref([]),
      };
    },
    data() {
      return {
        selectedStatus: null,
        listStatus: [],
        paymentStatusName: null
      }
    },
    methods: {

      getDisable(name) {
        if (this.paymentStatusName == "Đã thanh toán") {
          if (name == "Chờ thanh toán")
            return true
        }
        return false
      },
      async showModal(id, statusName) {
        if (statusName.includes("VNPay") || !statusName.includes("COD")) {
          const response = await APIService.get('datacategory/get-list-by-parent-code/STATUS_VNPAY');
          this.listStatus = response.data.data;
        }
        else {
          const response = await APIService.get('datacategory/get-list-by-parent-code/STATUS_COD');
          this.listStatus = response.data.data;
        }
        const responseOrderInfor = await APIService.get(`order/get-order-info-by-id/${id}`);
        this.paymentStatusName = responseOrderInfor.data.data.paymentStatusName
        this.selectedStatus = responseOrderInfor.data.data.status
        console.log("Status name:",statusName)
        this.id = id;
        this.open = true;
      },
      closeModal() {
        this.open = false;
        this.selectedRoles = [];
        //this.$refs.formRef.resetFields();
      },
      async handleSubmitAsync() {
        this.isLoading = true;
        notification.info({
          message: "Đang xử lý...",
          key: "loadingKey",
          duration: 0,
        });
        try {
          // Gửi yêu cầu cập nhật quyền người dùng lên server
          const response = await APIService.get(`order/update-order-status/${this.id}/${this.selectedStatus}`);
          
          if (response.data.message === "Cập nhật thành công") {
            notification.success({
              message: "Thành công",
              description: "'Thay đổi trạng thái đơn hàng thành công'",
              key: "loadingKey",
              duration: 2,
            });
            this.closeModal();
            this.$emit("updateSuccess");
          } else {
            notification.error({
              message: "Thất bại",
              description: response.data.message,
              key: "loadingKey",
              duration: 2,
            });
          }
        } catch (error) {
          notification.error({
            message: "Thất bại",
            description: error.message,
            key: "loadingKey",
            duration: 2,
          });

        } finally {
          this.isLoading = false;
        }
      }, //end handleSubmitAsync
    }, //end methods
  };
</script>