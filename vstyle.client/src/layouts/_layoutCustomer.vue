<template>
  <a-layout style="min-height: 100vh">
    <a-drawer v-model:visible="visible" class="custom-class" style="color: red" :closable="false" placement="left"
      :width="250">
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
      <a-menu v-model:selectedKeys="selectedKeys" mode="vertical" style="height: max-content;border: none">
            <a-menu-item key="1"> <router-link to="/">
                Trang chủ </router-link></a-menu-item>
            <a-menu-item key="2"><router-link to="/fdfdf">Sản phẩm</router-link></a-menu-item>
            <a-menu-item key="3"> <router-link to="/sdfsdf">Khuyến mãi </router-link></a-menu-item>
            <a-menu-item key="5"> <router-link to="/">Giới thiệu </router-link></a-menu-item>
            <a-menu-item key="6"> <router-link to="/sdfdf">Liên hệ </router-link></a-menu-item>
          </a-menu>
        
    </a-drawer>
    <a-layout-header ref="header" :style="{
      position: 'fixed',
      zIndex: 100,
      width: '100%',
      alignItems: 'center',
      boxShadow: 'rgba(87, 104, 110, 0.5) 2px 0px 9px 0px',
      backgroundColor: 'white',
      fontStyle: 'italic',
      fontSize: '20px',
      fontWeight: 'Medium',
    }">
      <a-row justify="space-around"  style="align-items: center;margin: 0px;padding: 0px;width: 100%;" >
        <a-col :xs="4" :sm="4" :md="4" :lg="0" :xl="0" :xxl="0" >
          <font-awesome-icon @click="showDrawer" :icon="['fas', 'bars']" style=" font-size:24px;" />
        </a-col>
        <a-col :xs="6" :sm="6" :md="6" :lg="6" :xl="6" :xxl="6">
          <router-link :to="{ name: 'CustomerHome' }">
            <img src="@/assets/VStyleLogo_Option1.png" alt="VStyle"
              style="width: 64px; height: 64px; margin: 0px;padding: 0px;display: block" />
          </router-link>

        </a-col>
        <a-col :xs="0" :sm="0" :md="0" :lg="13" :xl="13" :xxl="13"  style="justify-content: flex-start; display: flex">
          <a-menu v-model:selectedKeys="selectedKeys" mode="horizontal"  v-if="isLargeScreen"  style="height: max-content;border: none">
            <a-menu-item key="1"> <router-link to="/">
                Trang chủ </router-link></a-menu-item>
            <a-menu-item key="2"><router-link to="/fdfdf">Sản phẩm</router-link></a-menu-item>
            <a-menu-item key="3"> <router-link to="/sdfsdf">Khuyến mãi </router-link></a-menu-item>
            <a-menu-item key="5"> <router-link to="/">Giới thiệu </router-link></a-menu-item>
            <a-menu-item key="6"> <router-link to="/sdfdf">Liên hệ </router-link></a-menu-item>
          </a-menu>
        </a-col>
        <a-col  :xs="14" :sm="14" :md="14" :lg="3" :xl="3" :xxl="3">
          <a-row justify="end">
            <a-col :span="6">
              <a-tooltip title="Tìm kiếm" placement="bottomRight" color="#de2e21">
          <a class="ant-dropdown-link" @click.prevent style="color: #383838">
            <search-outlined :style="{ fontSize: '20px' }" />
          </a>
        </a-tooltip>
            </a-col>
            <a-col :span="6">
              <a-tooltip title="Giỏ hàng" placement="bottomRight" color="#de2e21">
          <a class="ant-dropdown-link" @click.prevent style="color: #383838">
            <shopping-cart-outlined :style="{ fontSize: '20px' }" />
          </a>
        </a-tooltip>
            </a-col>
            <a-col :span="6">
              <a-tooltip title="Đăng nhập/Đăng ký" placement="bottomRight" color="#de2e21">
          <a v-if="!isLoggedIn" class="ant-dropdown-link" @click.prevent>
            <router-link to="/login" style="color: #383838">
              <UserOutlined :style="{ fontSize: '20px' }" />
            </router-link>
          </a>
        </a-tooltip>

        <a-dropdown v-if="isLoggedIn">
          <a class="ant-dropdown-link" @click.prevent style="color: #383838">
            <UserOutlined :style="{ fontSize: '20px' }" />
          </a>
          <template #overlay>
            <a-menu style="margin-top: 10px">
              <a-menu-item>
                <UserOutlined :style="{ fontSize: '20px' }" />
                <router-link to="/profile"> Trang cá nhân</router-link>
              </a-menu-item>
              <a-menu-item @click="logout">
                <logout-outlined :style="{ fontSize: '20px' }" />
                <a @click="logout"> Đăng xuất</a>
              </a-menu-item>
            </a-menu>
          </template>
        </a-dropdown>
       
            </a-col>
          </a-row>
        </a-col>
      </a-row>




    
      <!-- End profile -->
    </a-layout-header>

    <a-layout-content :style="{
      padding: '0 50px',
      marginTop: '64px',
      justifyContent: 'center',
      minHeight: '1000px',
    }">
      <router-view></router-view>
    </a-layout-content>
    <a-layout-footer :style="{ textAlign: 'center' }">
      VStyle ©2024 Tạo bởi
      <a href="https://facebook.com/minhtuan.me">Minh Tuấn </a>
    </a-layout-footer>
  </a-layout>
</template>

<script>
  import EventBus from "@/helpers/EventBus.js";
  import { ref, onMounted, onUnmounted, reactive, beforeMount, computed } from "vue";
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
    SearchOutlined,
  } from "@ant-design/icons-vue";
  import { message, notification } from "ant-design-vue";

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
      SearchOutlined,
    },
    beforeUnmount() {
      window.removeEventListener("scroll", this.handleScroll);
    },
    methods: {
      handleScroll() {
        this.$nextTick(() => {
          // Sử dụng nextTick để đảm bảo header đã được render
          const header = this.$refs.header.$el;
          if (window.scrollY > 0) {
            header.style.boxShadow = "rgba(87, 104, 110, 0.5) 2px 0px 9px 0px";
          } else {
            header.style.boxShadow = "none";
          }
        });
      }, //end of handleScroll
      handleCollapse(collapsed) {
        this.collapsed = collapsed;
      },
      checkLoginStatus() {
        const token = localStorage.getItem("accessToken");
        this.isLoggedIn = !!token;
      },
      logout() {
       
        try {
          
          localStorage.removeItem("accessToken");
          localStorage.removeItem("role");
          localStorage.removeItem("userName");
          this.isLoggedIn = false;
          this.$router.push({ name: "Login" });
          notification.success({
            message: "Đăng xuất thành công!",
          });
        } catch {
          notification.error({
            message: "Đăng xuất thất bại!",
          });
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
      window.addEventListener("scroll", this.handleScroll);
    },
    setup() {
      const visible = ref(false);
      const showDrawer = () => {
        visible.value = true;
      };
      
      const isLoading = ref(false);
      const handleLoading = (value) => {
        isLoading.value = value;
      };
      const isLargeScreen = computed(() => {
      return window.innerWidth > 768; // Ví dụ: ẩn menu khi màn hình nhỏ hơn 768px
    });

      
      return {
        visible,
        showDrawer,
        isLargeScreen,
        selectedKeys: ref(["1"]),
        isLoading,
        colorToolTip: ref("#fff"),
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

</style>
