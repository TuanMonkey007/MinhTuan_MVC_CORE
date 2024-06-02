<template>
  <a-layout style="min-height: 100vh">
    <a-drawer v-model:open="visible" class="custom-class" style="color: red" :closable="false" placement="left"
    :width="{ xs: '90%', sm: '90%', md: '350px' }">
      <template #title>
        <a-row justify="space-between">
          <a-col :span="20">
            <router-link :to="{ name: 'CustomerHome' }">
              <img src="@/assets/VStyleLogo_Option1.png" alt="VStyle"
                style="width: 70px; height: 70px; margin-left: 50px" />
            </router-link>
          </a-col>
          <a-col :span="4">
            <a-button type="text" @click="visible = false">
              <font-awesome-icon :icon="['far', 'circle-xmark']" class="close-icon" />
            </a-button>
          </a-col>
        </a-row>
      </template>
      <a-menu v-model:selectedKeys="selectedMenu" mode="inline"
        style="border: none;padding-top: 10px;font-size: 16px;font-weight: 400">
        <a-menu-item key="Dashboard">
          <router-link :to="{ name: 'AdminHome' }">
            <bar-chart-outlined />
            <span>Trang chủ</span>
          </router-link>
        </a-menu-item>
        <a-menu-item key="Product">
          <router-link :to="{ name: 'ProductHome' }">
            <skin-outlined />
            <span>Sản phẩm</span>
          </router-link>
        </a-menu-item>
        <a-menu-item key="Cart">
          <shopping-cart-outlined />
          <span>Đơn hàng</span>
        </a-menu-item>
        <a-sub-menu key="Settings">
          <template #title>
            <span>
              <setting-outlined />
              <span>Hệ thống</span>
            </span>
          </template>
          <a-menu-item v-if="isAdmin" key="Account">
            <router-link :to="{ name: 'AccountHome' }">
              <font-awesome-icon icon="fa-regular fa-user" />
              <span>  Tài khoản</span>
            </router-link></a-menu-item>
          <a-menu-item key="Category"><router-link :to="{ name: 'CategoryHome' }">
            <font-awesome-icon :icon="['fas', 'layer-group']" />
            <span>  Danh mục</span>
          </router-link></a-menu-item>
              <a-menu-item key="Banner"><router-link :to="{ name: 'BannerHome' }">
                <font-awesome-icon :icon="['far', 'images']" />
                <span>  Banner</span>  
              </router-link></a-menu-item>
        </a-sub-menu>
      </a-menu>
    </a-drawer>
    <a-layout-header
      style="background-color: #fff; position: fixed; z-index: 1;width: 100%;margin-bottom: 64px;  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05), 0 4px 16px rgba(0, 0, 0, 0.1);">
      <a-row justify="space-between" style="width: 100%">
        <a-col :span="2">
          <font-awesome-icon @click="showDrawer" :icon="['fas', 'bars']" style="margin-left: 20px; font-size:24px;" />
        </a-col>
        <a-col :span="2">
          <router-link :to="{ name: 'CustomerHome' }">
            <img src="@/assets/VStyleLogo_Option1.png" alt="VStyle"
              style="width: 70px; height: 70px; margin-left: 50px" />
          </router-link>
        </a-col>
        <a-col :span="20" style="justify-content: flex-end; display: flex">
         
          
            <a-dropdown style="margin-bottom: 0px;">
                <a class="ant-dropdown-link" style="color: black; margin-left: 5px">
                
                  <a-avatar v-if="avatarBase64" :src="'data:' + avatarContentType + ';base64,' + avatarBase64"
                    :size="32"></a-avatar>
                  
                  <a-avatar v-else :size="32"></a-avatar>
               
                </a>
                <template #overlay>
                  <a-menu style=" min-width: 150px; margin-top: 0px">
                    <a-menu-item>
                      <span style="font-weight: bold; font-style: italic;"> {{ userName }} </span>
                    </a-menu-item>
                    <a-menu-item>
                      <RouterLink :to="{ name: 'AdminProfile' }">
                        <UserOutlined />
                        <a style="color: #1f1f1f;"> Trang cá nhân</a>

                      </RouterLink>
                    </a-menu-item>

                    <a-menu-item>
                      <a-divider style="margin: 0px 0px 5px; padding: 0px" />
                      <logout-outlined />
                      <a @click="logOut"> Đăng xuất</a>
                    </a-menu-item>
                  </a-menu>
                </template>
              </a-dropdown>
         



        </a-col>
        <!-- End profile -->
      </a-row>
    </a-layout-header>

    <a-layout-content style="margin-top: 64px">
      <a-layout>
        <a-row>
          <a-col :span="24">
            <a-layout-content style="background-color: #ececec">
              <a-row :gutter="0">

                <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
                  <router-view />
                  <a-layout-footer style="text-align: center">
                    VStyle ©2024 Tạo bởi
                    <a href="https://facebook.com/minhtuan.me">Minh Tuấn </a>
                  </a-layout-footer>
                </a-col>
              </a-row>

            </a-layout-content>
          </a-col>
        </a-row>
      </a-layout>



    </a-layout-content>

  </a-layout>
  <a-back-top :visibilityHeight="100" style="background-color:#c21f24 ">
  </a-back-top>
</template>
<script>
  import { ref, provide, onMounted, onUnmounted } from "vue";

  import {
    DesktopOutlined,
    UserOutlined,
    TeamOutlined,
    FileOutlined,
    MenuUnfoldOutlined,
    MenuFoldOutlined,
    SettingOutlined,
    SkinOutlined,
    BarChartOutlined,
    ContainerOutlined,
    ShoppingCartOutlined,
    LogoutOutlined,
    CaretDownOutlined,
  } from "@ant-design/icons-vue";
  import { Role } from "@/helpers/Constants.js";
  import { message, notification } from "ant-design-vue";
  import APIService from "@/helpers/APIService";
  export default {
    name: "LayoutAdmin",
    components: {
      BarChartOutlined,
      DesktopOutlined,
      UserOutlined,
      TeamOutlined,
      FileOutlined,
      MenuUnfoldOutlined,
      MenuFoldOutlined,
      SettingOutlined,
      SkinOutlined,
      ShoppingCartOutlined,
      LogoutOutlined,
      CaretDownOutlined,
    },
    setup() {
      const visible = ref(false);
      const showDrawer = () => {
        visible.value = true;
      };
      const selectedMenu = ref(["Dashboard"]);
      const userName = localStorage.getItem("userName");
      const changeSelectedMenu = (newKeys) => {
        selectedMenu.value = [newKeys];
      };

      // Cung cấp selectedMenu và changeSelectedMenu cho các component con
      provide("selectedMenu", selectedMenu);
      provide("changeSelectedMenu", changeSelectedMenu);

      return {
        visible,
        showDrawer,
        selectedMenu,
        collapsed: ref(false),
        userName,
        isAdmin: localStorage.getItem("role") === Role.ADMIN ? true : false,
      };
    },
    data() {
      return {
        avatarBase64: null,
        avatarContentType: null,
      };
    }, //end data
    async mounted() {
      try {
        const userPhone = localStorage.getItem("userPhone"); // Lấy userId từ localStorage
        const response = await APIService.get(
          `account/get-by-phone/${userPhone}`
        ); // Thay userId bằng ID người dùng
        this.userData = response.data.data;

        // Gán giá trị cho avatarBase64 và avatarContentType
        this.avatarBase64 = this.userData.avatarBase64;
        this.avatarContentType = this.userData.avatarContentType;
      } catch (error) {
        console.error(error);
        // Xử lý lỗi nếu có
      }
    }, //end mounted
    methods: {
      logOut() {
        try {
          localStorage.removeItem("accessToken");
          localStorage.removeItem("role");
          localStorage.removeItem("userName");
          localStorage.removeItem("userPhone");
          localStorage.removeItem("userEmail");
          this.$router.push("/login");
          this.$notification.success({
            message: "Đăng xuất thành công",
            description: "Hẹn gặp lại bạn!",
          });
        } catch {
          notification.error({
            message: "Đăng xuất thất bại",
            description: "Vui lòng thử lại sau!",
          });
        }
      }, //end logOut
      updateSelectedMenu(newKeys) {
        // Đổi tên phương thức thành updateSelectedMenu
        this.selectedMenu = newKeys;
      }, //end updateSelectedMenu
    }, //end methods
  };
</script>
<style scoped>
  :deep(.table-striped) td {
    background-color: #ededed;
  }


  .close-icon:hover {
    animation: spin 1s linear infinite;
    /* Chỉ áp dụng animation khi hover */
  }


  @keyframes spin {
    0% {
      transform: rotate(0deg);
    }

    100% {
      transform: rotate(360deg);
    }
  }

  :deep(.ant-float-btn-body) {
    background-color: #e54f54;
  }

  .ant-float-btn-body:hover {
    background-color: #c21f24;
  }
</style>
