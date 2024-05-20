<template lang="">
    <a-spin :spinning="isLoading" tip="Đang xử lý">
      <a-layout style="min-height: 100vh">
    <a-layout-sider v-model:collapsed="collapsed" collapsible :width="250">
      <div class="logo" />
      <a-menu v-model:selectedKeys="selectedKeys" theme="dark" mode="inline">
        <a-menu-item key="1">
          <bar-chart-outlined />
          <span>Trang chủ</span>
        </a-menu-item>
        <a-menu-item key="2">
          <skin-outlined />
          <span>Sản phẩm</span>
        </a-menu-item>
        <a-menu-item key="3">
          <shopping-cart-outlined />
          <span>Đơn hàng</span>
        </a-menu-item>
        <a-sub-menu key="sub1">
          <template #title>
            <span>
              <setting-outlined />
              <span>Hệ thống</span>
            </span>
          </template>
          <a-menu-item key="4">Danh mục dùng chung</a-menu-item>
          <a-menu-item key="5">Tài khoản người dùng</a-menu-item>
          
        </a-sub-menu>
        
      </a-menu>
    </a-layout-sider>
    <a-layout>
      <a-layout-header style="background: #fff; padding-left:10px;display: flex; justify-content: space-between;" >
        <div>
        <menu-unfold-outlined   :style="{fontSize: '24px'}" 
          v-if="collapsed"
          class="trigger"
          @click="() => (collapsed = !collapsed)"
        />
        <menu-fold-outlined   :style="{fontSize: '24px'}"   v-else class="trigger" @click="() => (collapsed = !collapsed)" />
        </div>
        <!-- Profile -->
       <div>
        <a-dropdown>

    <a class="ant-dropdown-link" @click.prevent style="color:black">
      <a-avatar></a-avatar>
      Họ và tên <caret-down-outlined />
     
    </a>
    <template #overlay >
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
       </div>
    
    
    <!-- End profile -->
      
      </a-layout-header>
      <a-layout-content style="margin: 20px;background-color:#ececec">
        <router-view></router-view>
      
      </a-layout-content>
      <a-layout-footer style="text-align: center">
        VStyle ©2024 Tạo bởi <a href="https://facebook.com/minhtuan.me">Minh Tuấn </a>
      </a-layout-footer>
    </a-layout>
  </a-layout>
  </a-spin>
</template>
<script>
import { ref, onMounted, onUnmounted } from 'vue';
import EventBus from '@/helpers/EventBus';
import {
   DesktopOutlined, UserOutlined, TeamOutlined, FileOutlined ,
  MenuUnfoldOutlined, MenuFoldOutlined,
  SettingOutlined,
  SkinOutlined,
  BarChartOutlined,
  ContainerOutlined,
  ShoppingCartOutlined,
  LogoutOutlined,
  CaretDownOutlined,
  
    } from "@ant-design/icons-vue";
import { message } from 'ant-design-vue';
export default {
  name: 'LayoutAdmin',
  components: {
   BarChartOutlined,
    DesktopOutlined,
    UserOutlined,
    TeamOutlined,
    FileOutlined,
    MenuUnfoldOutlined, MenuFoldOutlined,
    SettingOutlined,
    SkinOutlined,
    ShoppingCartOutlined,
    LogoutOutlined,
    CaretDownOutlined
        },
  setup() {
    const isLoading = ref(false);

    const handleLoading = (value) => {
      isLoading.value = value;
    };

    onMounted(() => {
      EventBus.on('setLoading', handleLoading);
    });

    onUnmounted(() => {
      EventBus.off('setLoading', handleLoading);
    });

    return {
      selectedKeys: ref(['1']),
        collapsed: ref(false),
      isLoading
    };
  },
  methods: {
    logOut() {
      EventBus.emit('setLoading', true);
      try{
        
        setTimeout(() => {
        EventBus.emit('setLoading', false);
         }, 2000);
        localStorage.removeItem('accessToken');
        localStorage.removeItem('role');
      localStorage.removeItem('userName');
        this.$router.push('/login');
        message.success('Đăng xuất thành công!');
      }catch{
        message.error('Đăng xuất thất bại!');
      }
      
     
      
      
    }
  }
};
</script>
<style lang="">
   
</style>