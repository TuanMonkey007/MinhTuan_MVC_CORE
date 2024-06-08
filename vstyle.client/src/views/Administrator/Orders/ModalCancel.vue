<template>
    <div>
      <a-modal
        v-model:open="open"
        title="Hủy đơn hàng"
        :footer="null"
        width="400px"
        style="top: 20px"
      >
        <a-divider></a-divider>
        <a-form>
          <a-row>
            <a-col :span="24">
              <a-checkbox-group v-model:value="selectedRoles">
                <a-row>
                  <a-col :span="24" v-for="role in allReason" :key="role.code">
                    <a-checkbox :value="role.name">{{ role.name }}</a-checkbox>
                  </a-col>
                  
                </a-row>
              </a-checkbox-group>
             
              </a-col >
          </a-row>
          <a-divider></a-divider>
          <a-row>
            <a-col :span="24" style="justify-content: center">
              <a-form-item>
                <a-button
                  @click="handleSubmitAsync"
                  :loading="isLoading"
                  type="primary"
                  style="margin-right: 20px"
                  class="login-form-button"
                >
                  Hoàn thành
                </a-button>
                <a-button
                  @click="closeModal"
                  style="margin-right: 20px"
                  class="login-form-button"
                >
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
  
    methods: {
      async showModal(id) {
        const responseRole = await APIService.get(
          "datacategory/get-list-by-parent-code/LY_DO_HUY_DONHANG"
        );
        this.allReason = responseRole.data.data;
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
          const response = await APIService.put(
            `order/cancel/${this.id}`,
            {
                reasonCancel : this.selectedRoles.join(', ')
            }
          );
  
          if (response.data.status === "success") {
            notification.success({
              message: "Thành công",
              description: "Hủy đơn hàng thành công",
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
  