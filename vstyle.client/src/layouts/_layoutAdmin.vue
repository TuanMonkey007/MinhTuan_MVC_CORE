<template lang="">
  <a-layout style="min-height: 100vh">
    <a-layout-sider
      v-model:collapsed="collapsed"
      theme="dark"
      collapsible
      :width="250"
      breakpoint="xxl"
      collapsed-width="0"
      :trigger="null"
    >
      <a-menu v-model:selectedKeys="selectedMenu" theme="dark" mode="inline">
        <a-menu-item key="Dashboard">
          <bar-chart-outlined />
          <span>Trang chủ</span>
        </a-menu-item>
        <a-menu-item key="Product">
          <skin-outlined />
          <span>Sản phẩm</span>
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
          <a-menu-item key="Account"
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
      <a-layout-header style="background: #fff; padding-left: 10px">
        <a-row justify="space-between" style="width:100%">
          <a-col :span="2">
            <menu-unfold-outlined
              :style="{ fontSize: '24px' }"
              v-if="collapsed"
              class="trigger"
              @click="() => (collapsed = !collapsed)"
            />
            <menu-fold-outlined
              :style="{ fontSize: '24px' }"
              v-else
              class="trigger"
              @click="() => (collapsed = !collapsed)"
            />
          </a-col>
          <!-- Profile -->
          <a-col  :xs="13" :sm="10" :md="6" :lg="6" :xl="4" >
            <a-dropdown>
              <a class="ant-dropdown-link" style="color: black">
                <a-avatar></a-avatar>
                {{ userName }} <caret-down-outlined />
              </a>
              <template #overlay>
                <a-menu style="margin-top:20px">
                  <a-menu-item>
                    <UserOutlined />
                    <a href="javascript:;"> Trang cá nhân</a>
                  </a-menu-item>
                  <a-menu-item>
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
import { message } from "ant-design-vue";
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
    };
  },
  methods: {
    logOut() {
      try {
        localStorage.removeItem("accessToken");
        localStorage.removeItem("role");
        localStorage.removeItem("userName");
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
