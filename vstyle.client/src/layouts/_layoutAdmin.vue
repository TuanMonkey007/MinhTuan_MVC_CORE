<template lang="">
  <a-layout style="min-height: 100vh">
    <a-layout-sider
      v-model:collapsed="collapsed"
      theme="dark"
      collapsible
      :width="250"
      breakpoint="xl"
      collapsed-width="0"
      style="position: fixed; height: 100vh; top: 0; left: 0; z-index: 9999"
    >
      <a-menu v-model:selectedKeys="selectedMenu" theme="dark" mode="inline">
        <a-menu-item key="Dashboard">
          <bar-chart-outlined />
          <span>Trang chủ</span>
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
          <a-menu-item v-if="isAdmin" key="Account"
            ><router-link :to="{ name: 'AccountHome' }"
              >Tài khoản người dùng</router-link
            ></a-menu-item
          >
          <a-menu-item key="Category"
            ><router-link :to="{ name: 'CategoryHome' }"
              >Danh mục dùng chung</router-link
            ></a-menu-item
          >
        </a-sub-menu>
      </a-menu>
    </a-layout-sider>
    <a-layout>
      <a-layout-header style="background: #fff; padding: 0">
        <a-row justify="space-between" style="width: 100%">
          <a-col :span="2"></a-col>

          <a-col :span="22" style="justify-content: flex-end; display: flex">
            {{ userName }}
            <a-dropdown>
              <a
                class="ant-dropdown-link"
                style="color: black; margin-left: 5px"
              >
                <a-avatar
                  v-if="avatarBase64"
                  :src="'data:' + avatarContentType + ';base64,' + avatarBase64"
                  :size="32"
                ></a-avatar>
                <a-avatar v-else :size="32"></a-avatar>
              </a>
              <template #overlay>
                <a-menu style="margin-top: 20px; min-width: 150px">
                  <a-menu-item>
                    <UserOutlined />
                    <a href="javascript:;"> Trang cá nhân</a>
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
      <a-row>
        <a-col :span="24"
          ><a-layout-content style="background-color: #ececec">
            <router-view /> </a-layout-content
        ></a-col>
      </a-row>

      <a-layout-footer style="text-align: center">
        VStyle ©2024 Tạo bởi
        <a href="https://facebook.com/minhtuan.me">Minh Tuấn </a>
      </a-layout-footer>
    </a-layout>
  </a-layout>
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
import { message } from "ant-design-vue";
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
    const selectedMenu = ref(["Dashboard"]);
    const userName = localStorage.getItem("userName");
    const changeSelectedMenu = (newKeys) => {
      selectedMenu.value = [newKeys];
    };

    // Cung cấp selectedMenu và changeSelectedMenu cho các component con
    provide("selectedMenu", selectedMenu);
    provide("changeSelectedMenu", changeSelectedMenu);

    return {
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
        message.success("Đăng xuất thành công!");
      } catch {
        message.error("Đăng xuất thất bại!");
      }
    }, //end logOut
    updateSelectedMenu(newKeys) {
      // Đổi tên phương thức thành updateSelectedMenu
      this.selectedMenu = newKeys;
    }, //end updateSelectedMenu
  }, //end methods
};
</script>
<style></style>
