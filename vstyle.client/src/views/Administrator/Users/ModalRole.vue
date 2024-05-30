<template>
  <div>
    <a-modal
      v-model:open="open"
      title="Phân quyền tài khoản"
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
                <a-col :span="24" v-for="role in allRoles" :key="role.code">
                  <a-checkbox :value="role.code">{{ role.name }}</a-checkbox>
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
import { message } from "ant-design-vue";
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
      allRoles: [],
      selectedRoles: ref([]),
    };
  },

  methods: {
    async showModal(id) {
      const responseRole = await APIService.get(
        "datacategory/get-list-by-parent-code/PHAN_QUYEN"
      );
      this.allRoles = responseRole.data.data;
      this.id = id;
      const response = await APIService.get(`account/get-roles/${id}`);
      this.selectedRoles = response.data.data;
      this.open = true;
    },
    closeModal() {
      this.open = false;
      this.selectedRoles = [];
      this.$refs.formRef.resetFields();
    },

    // async handleSubmitAsync() {
    //   message.loading({
    //     content: "Đang xử lý...",
    //     key: "loadingKey",
    //     duration: 0,
    //   });
    //   this.isLoading = true;
    //   console.log(this.selectedRoles);
    // }, //end handleSubmitAsync
    async handleSubmitAsync() {
      this.isLoading = true;
      message.loading({
        content: "Đang xử lý...",
        key: "loadingKey",
        duration: 0,
      });
      try {
        // Gửi yêu cầu cập nhật quyền người dùng lên server
        const response = await APIService.put(
          `account/set-role/${this.id}`,
          this.selectedRoles
        );

        if (response.data.status === "Success") {
          message.success({
            content: response.data.message,
            key: "loadingKey",
            duration: 2,
          });
          this.closeModal();
        } else {
          message.error({
            content: response.data.message,
            key: "loadingKey",
            duration: 2,
          });
        }
      } catch (error) {
        console.error("Lỗi khi cập nhật quyền:", error);
     
      } finally {
        this.isLoading = false;
      }
    }, //end handleSubmitAsync
  }, //end methods
};
</script>
