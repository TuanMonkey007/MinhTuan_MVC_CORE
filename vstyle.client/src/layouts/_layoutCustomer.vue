<template lang="">
  <a-layout>
    <a-layout-header
      ref="header"
      :style="{
        position: 'fixed',
        zIndex: 100,
        width: '100%',
        display: 'flex',
        justifyContent: 'space-around',
        alignItems: 'center',
        boxShadow: 'rgba(87, 104, 110, 0.5) 2px 0px 9px 0px',
        backgroundColor: 'white',
        fontStyle: 'italic',
        fontSize: '20px',
        fontWeight: 'Medium',
      }"
    >
      <img
        src="@/assets/VStyleLogo_Option1.png"
        alt="VStyle"
        style="width: 70px; height: 70px"
      />
      <div>
        <a-menu
          v-model:selectedKeys="selectedKeys"
          mode="horizontal"
          :style="{ lineHeight: '64px' }"
        >
         
            <a-menu-item key="1"> <router-link to="/">
              Trang chủ  </router-link></a-menu-item>
         
         
            <a-menu-item key="2"><router-link to="/fdfdf">Sản phẩm</router-link></a-menu-item>
         
        
            <a-menu-item key="3">  <router-link to="/sdfsdf">Khuyến mãi  </router-link></a-menu-item>
        
         
            <a-menu-item key="5"> <router-link to="/">Giới thiệu  </router-link></a-menu-item>
        
       
            <a-menu-item key="6">   <router-link to="/sdfdf">Liên hệ   </router-link></a-menu-item>
       
        </a-menu>
      </div>

      <div style="width: 80px; display: flex; justify-content: space-between">
        <a-tooltip title="Tìm kiếm" placement="bottomRight" color="#de2e21">
          <a class="ant-dropdown-link" @click.prevent style="color: #383838">
            <search-outlined :style="{ fontSize: '20px' }" />
          </a>
        </a-tooltip>
        <a-tooltip title="Giỏ hàng" placement="bottomRight" color="#de2e21">
          <a class="ant-dropdown-link" @click.prevent style="color: #383838">
            <shopping-cart-outlined :style="{ fontSize: '20px' }" />
          </a>
        </a-tooltip>
        <a-tooltip
          title="Đăng nhập/Đăng ký"
          placement="bottomRight"
          color="#de2e21"
        >
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
      </div>
      <!-- End profile -->
    </a-layout-header>

    <a-layout-content
      :style="{
        padding: '0 50px',
        marginTop: '64px',
        justifyContent: 'center',
        minHeight: '1000px',
      }"
    >
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
  SearchOutlined,
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
      EventBus.emit("setLoading", true);
      try {
        setTimeout(() => {
          EventBus.emit("setLoading", false);
        }, 2000);
        localStorage.removeItem("accessToken");
        localStorage.removeItem("role");
        localStorage.removeItem("userName");
        this.isLoggedIn = false;
        this.$router.push({ name: "Login" });
        message.success("Đăng xuất thành công!");
      } catch {
        message.error("Đăng xuất thất bại!");
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


</style>
