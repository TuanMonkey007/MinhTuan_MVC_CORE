<template lang="">

    <a-layout>
      <a-layout-header :style="{ position: 'fixed', zIndex: 100, width: '100%', display: 'flex', justifyContent: 'space-around', alignItems: 'center', boxShadow: '0px 2px 4px 0 rgba(87, 104, 110, 0.5)' }">
  
        <img src="@/assets/VStyleLogo_Option1.png" alt="VStyle" style="width: 70px; height: 70px;">
        <div>
          <a-menu
            v-model:selectedKeys="selectedKeys"
            theme="dark"
            mode="horizontal"
            :style="{ lineHeight: '64px' }"
          >
            <a-menu-item key="1">Trang chủ</a-menu-item>
            <a-menu-item key="2">Sản phẩm</a-menu-item>
            <a-menu-item key="3">Khuyến mãi</a-menu-item>
            <a-menu-item key="5">Giới thiệu</a-menu-item>
            <a-menu-item key="6">Liên hệ</a-menu-item>
          </a-menu>
        </div>
  
        <div style="width:80px;display:flex;justify-content:space-between">
          <a class="ant-dropdown-link" @click.prevent style="color:#fff">
            <search-outlined :style="{fontSize: '20px'}"  />
          </a>
          <a class="ant-dropdown-link" @click.prevent style="color:#fff">
            <shopping-cart-outlined :style="{fontSize: '20px'}"  />
          </a>
          <a v-if="!isLoggedIn" class="ant-dropdown-link" @click.prevent style="color:#fff">
              <router-link to="/login"  style="color:#fff"> <UserOutlined :style="{ fontSize: '20px' }" /></router-link>
            </a>
          <a-dropdown v-if="isLoggedIn">
            <a class="ant-dropdown-link" @click.prevent style="color:#fff">
              <UserOutlined :style="{ fontSize: '20px' }" />
            </a>
            <template #overlay>
              <a-menu style="margin-top:10px">
                <a-menu-item >
                  <UserOutlined :style="{ fontSize: '20px' }" />
                  <router-link to="/profile">  Trang cá nhân</router-link>
                </a-menu-item>
                <a-menu-item @click="logout">
                  <logout-outlined :style="{ fontSize: '20px' }" />
                  <a @click="logout">  Đăng xuất</a>
                </a-menu-item>
              </a-menu>
            </template>
          </a-dropdown>
        </div>
        <!-- End profile -->
      </a-layout-header>

      <a-layout-content :style="{ padding: '0 50px', marginTop: '64px', justifyContent:'center' , minHeight:'1000px' }">
        <router-view></router-view>
      </a-layout-content>
      <a-layout-footer :style="{ textAlign: 'center' }">
        VStyle ©2024 Tạo bởi <a href="https://facebook.com/minhtuan.me">Minh Tuấn </a>
      </a-layout-footer>
    </a-layout>

</template>

<script>
  import EventBus from "@/helpers/EventBus.js";
  import { ref, onMounted, onUnmounted, reactive, beforeMount } from "vue";
  import {
    MenuFoldOutlined,
    MenuUnfoldOutlined,
    VideoCameraOutlined,
    UploadOutlined,
    UserOutlined,
    LogoutOutlined,
    LoginOutlined,
    CaretDownOutlined,
    ShoppingCartOutlined,
    SearchOutlined
  } from "@ant-design/icons-vue";
  import { message } from "ant-design-vue";

  export default {
    name: "LayoutCustomer",
    components: {
      UserOutlined,
      MenuFoldOutlined,
      MenuUnfoldOutlined,
      VideoCameraOutlined,
      UploadOutlined,
      LogoutOutlined,
      LoginOutlined,
      CaretDownOutlined,  
      ShoppingCartOutlined,
      SearchOutlined
    },
    methods: {
      handleCollapse(collapsed) {
        this.collapsed = collapsed;
      },
      checkLoginStatus() {
        const token = localStorage.getItem('accessToken');
        this.isLoggedIn = !!token;
       
      },
      logout() {
        EventBus.emit('setLoading', true);
        try {
          setTimeout(() => {
            EventBus.emit('setLoading', false);
          }, 2000);
          localStorage.removeItem('accessToken');
          localStorage.removeItem('role');
          localStorage.removeItem('userName');
          this.isLoggedIn = false;
          this.$router.push({ name: 'Login' });
          message.success('Đăng xuất thành công!');
        } catch {
          message.error('Đăng xuất thất bại!');
        }
      },
    },
    data() {
      return {
        isLoggedIn: false,
      };
    },
    beforeMount() {
      this.checkLoginStatus();
    },
    mounted() {
      this.checkLoginStatus();
    },
    setup() {
      const isLoading = ref(false);
      const handleLoading = (value) => {
        isLoading.value = value;
      };

      onMounted(() => {
        EventBus.on("setLoading", handleLoading);
      });

      onUnmounted(() => {
        EventBus.off("setLoading", handleLoading);
      });

      return {
        selectedKeys: ref(['2']),
        isLoading,
      };
    },
  };
</script>

<style scoped>
  .logo {
    float: right;
   
    margin: 16px 24px 16px 0;
    background: rgba(255, 255, 255, 0.3);
  }
  .logo {
    float: right;
    margin: 16px 0 16px 24px;
  }
</style>
