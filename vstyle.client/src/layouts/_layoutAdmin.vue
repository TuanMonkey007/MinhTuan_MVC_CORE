<template>
  <a-layout style="min-height: 100vh">
    <a-drawer v-model:open="visible" :closable="false" placement="left"
      :width="{ xs: '90%', sm: '90%', md: '400px', xl: '500px', xxl: '500px' }">
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
      <a-menu forceSubMenuRender="true" v-model:selectedKeys="selectedMenu" mode="inline"
        style="border: none;padding-top: 10px;font-size: 16px;font-weight: 400">
        <a-menu-item key="Dashboard">
          <router-link :to="{ name: 'AdminHome' }">
            <font-awesome-icon :icon="['fas', 'chart-pie']" style="margin-right: 5px;"/>
            <span> Thống kê</span>
          </router-link>
        </a-menu-item>
        <a-menu-item key="Product">
          <router-link :to="{ name: 'ProductHome' }">
            <font-awesome-icon icon="fa-solid fa-shirt" style="margin-right: 5px;" />
            <span>Sản phẩm</span>
          </router-link>
        </a-menu-item>
        <a-menu-item key="Order">
          <router-link :to="{ name: 'OrderHome' }">
            <font-awesome-icon icon="fa-solid fa-cart-shopping" style="margin-right: 5px;" />
            <span> Đơn hàng</span>
          </router-link>
        </a-menu-item>
        <a-menu-item key="PaymentInfo">
                          <router-link :to="{ name: 'PaymentInfoHome' }">
                            <font-awesome-icon icon="fa-brands fa-cc-mastercard" style="margin-right: 5px;" />
                            <span> Thanh toán</span>
                          </router-link>
                        </a-menu-item>
        <a-menu-item key="Voucher">
          <router-link :to="{ name: 'VoucherHome' }">
            <font-awesome-icon icon="fa-solid fa-tags" style="margin-right: 5px;" />
            <span> Mã giảm giá</span>
          </router-link>
        </a-menu-item>
        <a-menu-item key="Article">
          <router-link :to="{ name: 'ArticleHome' }">
            <font-awesome-icon icon="fa-regular fa-newspaper" style="margin-right: 5px;" />
            <span> Bài viết</span>
          </router-link>
        </a-menu-item>
        <a-sub-menu v-if="isAdmin" key="Settings">
          <template #title>
            <span>
              <font-awesome-icon icon="fa-solid fa-gear" style="margin-right: 5px;" />
              <span>Hệ thống</span>
            </span>
          </template>
          <a-menu-item key="Account">
            <router-link :to="{ name: 'AccountHome' }">
              <font-awesome-icon icon="fa-regular fa-user" style="margin-right: 5px;" />
              <span> Tài khoản</span>
            </router-link></a-menu-item>
          <a-menu-item key="Category"><router-link :to="{ name: 'CategoryHome' }">
              <font-awesome-icon :icon="['fas', 'layer-group']" style="margin-right: 5px;" />
              <span> Danh mục</span>
            </router-link></a-menu-item>
          <a-menu-item key="Banner"><router-link :to="{ name: 'BannerHome' }">
              <font-awesome-icon :icon="['far', 'images']" style="margin-right: 5px;" />
              <span> Banner</span>
            </router-link></a-menu-item>
        </a-sub-menu>
      </a-menu>
    </a-drawer>
    <a-layout-header class="responsive-header"
      style="background-color: #2c3e50 ; position: sticky; top: 0; z-index: 99;width: 100%;     box-shadow: rgba(0, 0, 0, 0.05) 3px 20px 20px 20px, rgba(0, 0, 0, 0.1) 0px 4px 16px;">
      <a-row justify="space-between" style="width: 100%">
        <a-col :xs="12" :sm="12" :md="4" :lg="4" :xl="4">
          <a-row>
            <a-col :xs="12" :sm="12" :md="12" :lg="0" :xl="0" :xxl="0">
              <font-awesome-icon @click="showDrawer" :icon="['fas', 'bars']"
                style="margin-left: 10px; font-size:24px; color: slategray" />
            </a-col>
            <a-col :span="12"> <router-link :to="{ name: 'AdminHome' }">
                <img src="@/assets/VStyleLogo_Option1.png" alt="VStyle"
                  style="width: 70px; height: 70px; object-fit: contain" />
              </router-link></a-col>
          </a-row>
        </a-col>

        <a-col :span="2" style="justify-content: flex-end; display: flex">


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

    <a-layout-content>
      <a-layout>
        <a-row>
          <a-col :span="24">
            <a-layout-content style="background-color: #f5f5f5 ;overflow: hidden;">
              <a-row :gutter="0" >
                <a-col :xs="0" :sm="0" :md="0" :lg="4" class="menu-column" :xl="4"
                  style="background-color: #2c3e50 ;min-height: 100vh;">
                  <a-row justify="center">
                    <a-col :span="24" style="justify-content: center; display: flex">
                      <div style="padding: 10px 0px 10px 10px;">
                        <a-avatar v-if="avatarBase64" :src="'data:' + avatarContentType + ';base64,' + avatarBase64"
                          :size="80"></a-avatar>
                          <a-avatar v-else :size="80"></a-avatar>
                      </div>
                   
                    </a-col>
                    <a-col style="display: flex;align-items: center;justify-content: center;" :span="24">
                      <font-awesome-icon class="icon-user-hover" icon="fa-solid fa-user-gear" style="color: #000000;margin-right: 20px" @click="showModalUpdateProfile" />
                      <font-awesome-icon class="icon-user-hover" icon="fa-solid fa-right-from-bracket" style="color: #000000" @click="logOut"/>
                      
                        </a-col>
                    <a-col style="display: flex;align-items: center;justify-content: center;" :span="24">
                            <h3 style="color: rgb(249 233 233)">{{ this.userData.fullName }}</h3>
                          
                        </a-col>
                        <a-col style="display: flex;align-items: center;justify-content: center;" :span="24">
                           
                            <p style="color: rgb(249 233 233)">{{ this.userRole }}</p>
                        </a-col>
                    <a-col :span="24">
                      <a-menu forceSubMenuRender="true" v-model:selectedKeys="selectedMenu" mode="inline"
                        style="background-color: #2c3e50 ; color:#fff; border: none;padding-top: 10px;font-size: 16px;font-weight: 500">
                        <a-menu-item key="Dashboard">
                          <router-link :to="{ name: 'AdminHome' }">
                            <font-awesome-icon :icon="['fas', 'chart-pie']" style="margin-right: 5px;"/>
                            <span> Thống kê</span>
                          
                          </router-link>
                        </a-menu-item>
                        <a-menu-item key="Product">
                          <router-link :to="{ name: 'ProductHome' }">
                            <font-awesome-icon icon="fa-solid fa-shirt" style="margin-right: 5px;" />
                            <span>Sản phẩm</span>
                          </router-link>
                        </a-menu-item>
                        <a-menu-item key="Order">
                          <router-link :to="{ name: 'OrderHome' }">
                            <font-awesome-icon icon="fa-solid fa-cart-shopping" style="margin-right: 5px;" />
                            <span>Đơn hàng</span>
                          </router-link>
                        </a-menu-item>
                        <a-menu-item key="PaymentInfo">
                          <router-link :to="{ name: 'PaymentInfoHome' }">
                            <font-awesome-icon icon="fa-brands fa-cc-mastercard" style="margin-right: 5px;" />
                            <span> Thanh toán</span>
                          </router-link>
                        </a-menu-item>
                        <a-menu-item key="Voucher">
                          <router-link :to="{ name: 'VoucherHome' }">
                            <font-awesome-icon icon="fa-solid fa-tags" style="margin-right: 5px;" />
                            <span> Mã giảm giá</span>
                          </router-link>
                        </a-menu-item>
                        <a-menu-item key="Article">
                          <router-link :to="{ name: 'ArticleHome' }">
                            <font-awesome-icon icon="fa-regular fa-newspaper" style="margin-right: 5px;" />
                            <span> Bài viết</span>
                          </router-link>
                        </a-menu-item>
                        <a-sub-menu v-if="isAdmin" key="Settings">
                          <template #title>
                            <span>
                              <font-awesome-icon icon="fa-solid fa-gear" style="margin-right: 5px;" />
                              <span>Hệ thống</span>
                            </span>
                          </template>
                          <a-menu-item key="Account">
                            <router-link :to="{ name: 'AccountHome' }">
                              <font-awesome-icon icon="fa-regular fa-user" style="margin-right: 5px;" />
                              <span> Tài khoản</span>
                            </router-link></a-menu-item>
                          <a-menu-item key="Category"><router-link :to="{ name: 'CategoryHome' }">
                              <font-awesome-icon :icon="['fas', 'layer-group']" style="margin-right: 5px;" />
                              <span> Danh mục</span>
                            </router-link></a-menu-item>
                          <a-menu-item key="Banner"><router-link :to="{ name: 'BannerHome' }">
                              <font-awesome-icon :icon="['far', 'images']" style="margin-right: 5px;" />
                              <span> Banner</span>
                            </router-link></a-menu-item>
                        </a-sub-menu>
                      </a-menu>
                    </a-col>
                  </a-row>


                </a-col>
                <a-col :xs="0" :sm="0" :md="0" :lg="4" ></a-col>
                <a-col :xs="24" :sm="24" :md="24" :lg="20" :xl="20" style="min-height: 100vh;">
                  <div style="min-height: 100vh;">
                    <router-view />
                  </div>

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
  <a-back-top :visibilityHeight="100" style="background-color:#c21f24 " />
</template>
<script>
  import DashboardSidebar from "@/components/Layouts/DashboardSidebar.vue";
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
      DashboardSidebar,
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
        userData: {
          avatarBase64: null,
          avatarContentType: null,
          fullName: null,
        },
        avatarBase64: null,
        userRole:'',
        avatarContentType: null,
      };
    }, //end data
    async mounted() {
      try {
        if(localStorage.getItem("role") === Role.ADMIN) {
          this.userRole = "Quản trị viên";
        }else{
          this.userRole = "Nhân viên cửa hàng";
        }
        const userPhone = localStorage.getItem("userPhone"); // Lấy userId từ localStorage
        const response = await APIService.get(
          `account/get-by-phone/${userPhone}`
        ); // Thay userId bằng ID người dùng
        this.userData = response.data.data;
        console.log(this.userData);
        // Gán giá trị cho avatarBase64 và avatarContentType
        this.avatarBase64 = this.userData.avatarBase64;
        this.avatarContentType = this.userData.avatarContentType;
      } catch (error) {
        console.error(error);
        // Xử lý lỗi nếu có
      }
    }, //end mounted
    methods: {
      showModalUpdateProfile() {
          this.$router.push({ name: "AdminProfile" });
      },
      logOut() {
        try {
          localStorage.removeItem("accessToken");
          localStorage.removeItem("role");
          localStorage.removeItem("userName");
          localStorage.removeItem("userPhone");
          localStorage.removeItem("userEmail");
          localStorage.removeItem("userCartId");
          localStorage.removeItem("jwt");
          this.$router.push({ name: "CustomerHome" });
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

  :deep(.ant-menu-item-selected) {
    color: #f0f0f0 !important;
    background-color: #c21f24
  }

  .menu-column {
    background-color: #ffffff;
    /* Màu nền trắng để tách biệt */
    box-shadow: 0 4px 8px rgba(19, 73, 143, 0.4);
    /* Thêm bóng đổ */
    border-right: 1px solid #e0e0e0;
    /* Đường viền bên phải */
    padding: 10px;
    /* Khoảng cách bên trong */
    min-height: 100vh;
    /* Chiều cao tối thiểu để bao phủ toàn bộ chiều cao màn hình */
    position: fixed;
    /* border-top-right-radius: 20px;
    border-bottom-right-radius: 20px; */
    z-index: 999;
    max-height: 100vh
    /* Để giữ các phần tử con như logo hoặc menu ở đúng vị trí */
  }
  .icon-user-hover:hover {
    cursor: pointer;
    color: #c21f24 !important;
  }
  @media (min-width: 992px) {

    /* Định nghĩa cho màn hình lớn hơn 992px (lg) */
    .responsive-header {
      display: none;
    }
  }
</style>
