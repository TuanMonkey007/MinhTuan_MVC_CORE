<template>
  <a-layout>
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
      <a-menu mode="inline" selectable="true" forceSubMenuRender="true" v-model:selectedKeys="selectedMenu"
        style="height: max-content;border: none;font-size:16px">
        <a-menu-item key="1"> <router-link to="/">
            Trang chủ </router-link></a-menu-item>
        <a-menu-item key="2">
          <router-link :to="{ name: 'ProductList', params: { gender: 'Nữ' } }">
            Nữ
          </router-link>
        </a-menu-item>
        <a-menu-item key="3">
          <router-link :to="{ name: 'ProductList', params: { gender: 'Nam' } }">
            Nam
          </router-link>
        </a-menu-item>

        <a-menu-item key="5"> <router-link :to="{ name: 'About' }">Chính sách</router-link></a-menu-item>
        <a-menu-item key="7"> <router-link :to="{ name: 'Contact' }">Liên hệ </router-link></a-menu-item>
        <a-sub-menu forceSubMenuRender="true">
          <template #title>
            <span>

              <span>Tin tức</span>
            </span>
          </template>
          <a-menu-item :key="6" style="justify-content: flex-start">

            <router-link :to="{ name: 'BlogCategory', params: { categoryCode: 'TIN-TUC-SU-KIEN' } }"> Sự kiện thời
              trang</router-link>
          </a-menu-item>
          <a-menu-item :key="8">
            <router-link :to="{ name: 'BlogCategory', params: { categoryCode: 'XU-HUONG-THOI-TRANG' } }"> Xu hướng
              thời trang</router-link>
          </a-menu-item>
          <a-menu-item :key="9">
            <router-link :to="{ name: 'BlogCategory', params: { categoryCode: 'TU-VAN-PHONG-CACH' } }"> Tư vấn
              phong cách</router-link>
          </a-menu-item>
        </a-sub-menu>

      </a-menu>

    </a-drawer>
    <a-layout-header ref="header" :style="{
      position: 'fixed',
      zIndex: 100,
      width: '100%',
      alignItems: 'center',
      boxShadow: 'rgba(87, 104, 110, 0.5) 2px 0px 9px 0px',
      backgroundColor: 'white',

      fontSize: '20px',
      fontWeight: 'Medium',
    }">
      <a-row justify="space-around" style="align-items: center;margin: 0px;padding: 0px;width: 100%;">
        <a-col :xs="4" :sm="4" :md="4" :lg="0" :xl="0" :xxl="0">
          <font-awesome-icon @click="showDrawer" :icon="['fas', 'bars']" style=" font-size:24px;" />
        </a-col>
        <a-col :xs="6" :sm="6" :md="6" :lg="2" :xl="2" :xxl="2">
          <router-link :to="{ name: 'CustomerHome' }">
            <img src="@/assets/VStyleLogo_Option1.png" alt="VStyle"
              style="width: 64px; height: 64px; margin: 0px;padding: 0px;display: block" />
          </router-link>

        </a-col>
        <a-col :xs="0" :sm="0" :md="0" :lg="16" :xl="16" :xxl="16" style="justify-content: flex-start; display: flex;">
          <a-row style="width: 100%;">
            <a-col :xs="0" :lg="24" :xl="24" :xxl="24" style="width: 100%">
              <a-menu mode="horizontal" selectable="true" forceSubMenuRender="true" v-model:selectedKeys="selectedMenu"
                style="height: max-content;border: none;font-size:16px">
                <a-menu-item key="1"> <router-link to="/">
                    Trang chủ </router-link></a-menu-item>
                <a-menu-item key="2">
                  <router-link :to="{ name: 'ProductList', params: { gender: 'Nữ' } }">
                    Nữ
                  </router-link>
                </a-menu-item>
                <a-menu-item key="3">
                  <router-link :to="{ name: 'ProductList', params: { gender: 'Nam' } }">
                    Nam
                  </router-link>
                </a-menu-item>

                <a-menu-item key="5"> <router-link :to="{ name: 'About' }">Chính sách</router-link></a-menu-item>
                <a-menu-item key="7"> <router-link :to="{ name: 'Contact' }">Liên hệ </router-link></a-menu-item>

                <a-menu-item key="6">
                  <a-sub-menu forceSubMenuRender="true">
                    <template #title>
                      <span>

                        <span>Tin tức</span>
                      </span>
                    </template>
                    <a-menu-item :key="6" style="justify-content: flex-start">

                      <router-link :to="{ name: 'BlogCategory', params: { categoryCode: 'TIN-TUC-SU-KIEN' } }"> Sự kiện
                        thời
                        trang</router-link>
                    </a-menu-item>
                    <a-menu-item :key="8">
                      <router-link :to="{ name: 'BlogCategory', params: { categoryCode: 'XU-HUONG-THOI-TRANG' } }"> Xu
                        hướng
                        thời trang</router-link>
                    </a-menu-item>
                    <a-menu-item :key="9">
                      <router-link :to="{ name: 'BlogCategory', params: { categoryCode: 'TU-VAN-PHONG-CACH' } }"> Tư vấn
                        phong cách</router-link>
                    </a-menu-item>
                  </a-sub-menu>
                  <!-- <a-dropdown>
                    <span class="dropdown-text">Tin tức</span>
                    <template #overlay>
                      <a-menu style="margin-top: 10px">

                        <a-menu-item style="justify-content: flex-start">

                          <router-link :to="{ name: 'BlogCategory', params: { categoryCode: 'TIN-TUC-SU-KIEN' } }"> Sự
                            kiện thời trang</router-link>
                        </a-menu-item>
                        <a-menu-item>
                          <router-link :to="{ name: 'BlogCategory', params: { categoryCode: 'XU-HUONG-THOI-TRANG' } }">
                            Xu hướng thời trang</router-link>
                        </a-menu-item>
                        <a-menu-item>
                          <router-link :to="{ name: 'BlogCategory', params: { categoryCode: 'TU-VAN-PHONG-CACH' } }"> Tư
                            vấn phong cách</router-link>
                        </a-menu-item>




                      </a-menu>
                    </template>
                  </a-dropdown> -->
                </a-menu-item>
              </a-menu>
            </a-col>
          </a-row>

        </a-col>
        <a-col :xs="14" :sm="14" :md="14" :lg="6" :xl="6" :xxl="6">
          <a-row justify="end">
            <a-col :span="18">
              <a-tooltip title="Tìm kiếm" placement="bottomRight" color="#de2e21">
                <a class="ant-dropdown-link" @click.prevent style="color: #383838">

                  <a-input @keyup.enter="handleSearch" style="max-width: 100%;height: auto" :bordered="false"
                    placeholder="Tìm kiếm">
                    <template #suffix>
                      <a-upload :before-upload="beforeUpload" @change="handleChangeImageSearch"
                        accept=".jpg,.jpeg,.png,.bmp,.webp" :action="apiUrl" :fileList="fileListImageSearch"
                        :showUploadList="false" multiple="false" listType="none">
                        <CameraOutlined @click="handleSearchCamera" :style="{ fontSize: '20px' }" />
                      </a-upload>

                    </template>
                  </a-input>



                </a>


              </a-tooltip>
            </a-col>
            <a-col :span="3">
              <a-tooltip title="Giỏ hàng" placement="bottomRight" color="#de2e21">
                <a class="ant-dropdown-link" @click.prevent style="color: #383838">
                  <router-link :to="{ name: 'Cart' }" style="color: #383838">
                    <shopping-cart-outlined :style="{ fontSize: '20px' }" />
                  </router-link>

                </a>
              </a-tooltip>
            </a-col>
            <a-col :span="3">


              <a-dropdown v-if="isLoggedIn">
                <a class="ant-dropdown-link" @click.prevent style="color: #383838">
                  <UserOutlined :style="{ fontSize: '20px' }" />
                </a>
                <template #overlay>
                  <a-menu style="margin-top: 10px">
                    <a-menu-item>
                      <UserOutlined :style="{ fontSize: '20px' }" />
                      <router-link :to="{ name: 'CustomerProfile' }"> Trang cá nhân</router-link>
                    </a-menu-item>
                    <a-menu-item>
                      <font-awesome-icon icon="fa-solid fa-boxes-stacked" :style="{ fontSize: '20px' }" />
                      <router-link :to="{ name: 'TrackingOrder' }"> Tra cứu đơn hàng</router-link>
                    </a-menu-item>

                    <a-menu-item v-if="role != 'CUSTOMER'">
                      <font-awesome-icon icon="fa-solid fa-wrench" :style="{ fontSize: '20px' }" />
                      <router-link :to="{ name: 'AdminHome' }"> Quản trị</router-link>
                    </a-menu-item>
                    <a-menu-item @click="logout">
                      <logout-outlined :style="{ fontSize: '20px' }" />
                      <a @click="logout"> Đăng xuất</a>
                    </a-menu-item>

                  </a-menu>
                </template>
              </a-dropdown>
              <a-dropdown v-else>
                <a class="ant-dropdown-link" @click.prevent style="color: #383838">
                  <UserOutlined :style="{ fontSize: '20px' }" />
                </a>
                <template #overlay>
                  <a-menu style="margin-top: 10px">
                    <a-menu-item @click="openModalLogin">
                      <UserOutlined :style="{ fontSize: '20px' }" />
                      Đăng nhập
                    </a-menu-item>
                    <a-menu-item>
                      <font-awesome-icon icon="fa-solid fa-boxes-stacked" :style="{ fontSize: '20px' }" />
                      <router-link :to="{ name: 'TrackingOrder' }"> Đơn hàng</router-link>
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

      marginTop: '64px',
      justifyContent: 'center',
      minHeight: '500px',
      backgroundColor: '#f5f5f5',

    }">
      <router-view></router-view>
    </a-layout-content>
    <a-layout-footer :style="{ textAlign: 'center', padding: '0px' }">
      <a-row style="border-top-left-radius: 20%; border-top-right-radius: 20%; ">
        <a-col :span="24">
          <a-card>

            <template #title>
              <a-row justify="start" style=" align-items: center">
                <a-col :span="8" style="justify-content: center; display:flex">
                  <font-awesome-icon :icon="['fab', 'facebook']"
                    style="color: #0659ea; font-size: 32px ;margin-right: 10px" />
                  <font-awesome-icon :icon="['fab', 'instagram']"
                    style="color: #E1306C; font-size: 32px;margin-right: 10px" />
                  <font-awesome-icon :icon="['fab', 'tiktok']" style=" font-size: 32px" />
                </a-col>
                <a-col :span="8">
                  <img src="@/assets/VStyleLogo_Option1.png" alt="VStyle" style="height: 80px" />

                </a-col>
                <a-col :span="8" style="justify-content: center; display:flex">
                  <img src="@/assets/image/VNpay.png" alt="VStyle" style="height: 32px;margin-right: 10px" />
                  <img src="@/assets/image/giaohangnhanh.png" alt="VStyle" style="height: 32px" />
                </a-col>

              </a-row>
            </template>
            <a-row>
              <a-col :xs="0" :sm="0" :md="8">
                <span style="font-size: 16px; font-weight: bold;">Liên hệ</span>
                <br>
                <font-awesome-icon :icon="['fas', 'phone-volume']" class="footer-link" />
                <a class="footer-link"> 034.346.3334</a>
                <br>
                <font-awesome-icon :icon="['fa-solid', 'fa-location-dot']" class="footer-link" />
                <a class="footer-link"> Số 175 Tây Sơn, Đống Đa, Thành phố Hà Nội</a>
                <br>
                <font-awesome-icon :icon="['fas', 'inbox']" class="footer-link" />
                <a class="footer-link"> 034.346.3334 (Zalo)</a>
                <br>
                <font-awesome-icon :icon="['far', 'envelope']" class="footer-link" />
                <a class="footer-link"> cskh@vstyle.vn</a>
                <br>
                <font-awesome-icon :icon="['far', 'envelope']" class="footer-link" />
                <a class="footer-link"> dev.minhtuan07@gmail.com</a>
              </a-col>
              <a-col :xs="0" :sm="0" :md="8">
                <span style="font-size: 16px; font-weight: bold;">Công ty</span>
                <br>
                <a class="footer-link" href="/about"> Về chúng tôi</a>
                <br>
                <br>
                <a class="footer-link"> Tuyển dụng</a>
                <br>
                <br>
                <a class="footer-link"> Chuỗi cửa hàng</a>
              </a-col>
              <a-col :xs="0" :sm="0" :md="8">
                <span style="font-size: 16px; font-weight: bold;">Chính sách</span>
                <br>
                <a class="footer-link" href="/policy/payment-policy"> Chính sách thanh toán</a>
                <br>
                <a class="footer-link" href="/policy/warranty-policy"> Chính sách bảo hành, đổi trả</a>
                <br>
                <a class="footer-link" href="/policy/shipping-policy"> Chính sách giao, nhận hàng và kiếm hàng</a>
                <br>
                <a class="footer-link" href="/policy/privacy-policy"> Chính sách bảo mật thông tin</a>
                <br>
                <a class="footer-link" href="/policy/buying-policy"> Chính sách mua hàng</a>
              </a-col>

            </a-row>
            <a-row>
              <a-col :xs="24" :sm="24" :md="0">
                <a-collapse v-model:activeKey="activeKey" :bordered="false" accordion>
                  <a-collapse-panel key="1" :show-arrow="false">
                    <template #header>
                      <span style="font-size: 16px; font-weight: bold;">Liên hệ</span>

                    </template>
                    <font-awesome-icon :icon="['fas', 'phone-volume']" class="footer-link" />
                    <a class="footer-link"> 034.346.3334</a>
                    <br>
                    <font-awesome-icon :icon="['fa-solid', 'fa-location-dot']" class="footer-link" />
                    <a class="footer-link"> Số 175 Tây Sơn, Đống Đa, Thành phố Hà Nội</a>
                    <br>
                    <font-awesome-icon :icon="['fas', 'inbox']" class="footer-link" />
                    <a class="footer-link"> 034.346.3334 (Zalo)</a>
                    <br>
                    <font-awesome-icon :icon="['far', 'envelope']" class="footer-link" />
                    <a class="footer-link"> cskh@vstyle.vn</a>
                    <br>
                    <font-awesome-icon :icon="['far', 'envelope']" class="footer-link" />
                    <a class="footer-link"> dev.minhtuan07@gmail.com</a>
                  </a-collapse-panel>
                  <a-collapse-panel key="2" :show-arrow="false">
                    <template #header>
                      <span style="font-size: 16px; font-weight: bold;">Công ty</span>
                    </template>
                    <a class="footer-link" href="/about"> Về chúng tôi</a>
                    <br>
                    <br>
                    <a class="footer-link"> Tuyển dụng</a>
                    <br>
                    <br>
                    <a class="footer-link"> Chuỗi cửa hàng</a>
                  </a-collapse-panel>
                  <a-collapse-panel key="3" :show-arrow="false">
                    <template #header>
                      <span style="font-size: 16px; font-weight: bold;">Chính sách</span>
                    </template>

                    <a class="footer-link" href="/policy/payment-policy"> Chính sách thanh toán</a>
                    <br>
                    <a class="footer-link" href="/policy/warranty-policy"> Chính sách bảo hành, đổi trả</a>
                    <br>
                    <a class="footer-link" href="/policy/shipping-policy"> Chính sách giao, nhận hàng và kiếm hàng</a>
                    <br>
                    <a class="footer-link" href="/policy/privacy-policy"> Chính sách bảo mật thông tin</a>
                    <br>
                    <a class="footer-link" href="/policy/buying-policy"> Chính sách mua hàng</a>
                  </a-collapse-panel>
                </a-collapse>
              </a-col>
            </a-row>
          </a-card>

        </a-col>
      </a-row>
      <a-row>
        <a-col :span="24">
          <span style="font-size: 12px; font-style: italic;">Bản quyền thuộc về Công Ty TNHH Thời Trang Việt.</span>

        </a-col>
      </a-row>


    </a-layout-footer>
  </a-layout>
  <a-back-top class="btn-back-to-top" :visibilityHeight="100" style="background-color:#c21f24; left:5% ">

  </a-back-top>
  <!-- <a-float-button >
      <template #icon>
        <div   class="elfsight-app-74c558cc-6b15-4fef-beb5-958f8c4af58e"  data-elfsight-app-lazy 
        ></div>
 
      </template> 
     
    </a-float-button> -->
  <!-- <div class="elfsight-app-74c558cc-6b15-4fef-beb5-958f8c4af58e" data-elfsight-app-lazy
    style="width: 20px !important; height: 20px  !important"></div> -->
  <a-float-button-group trigger="hover" type="primary" :style="{ right: '25px' }">
    <template #icon>
      <font-awesome-icon icon="fa-solid fa-question" />
    </template>

    <a-float-button href="https://zalo.me/0343463334">
      <template #icon>

        <img src="@/assets/image/zalo.png" style="width: 20px; height: 20px; object-fit: contain" />
      </template>
    </a-float-button>
    <a-float-button href="https://www.facebook.com/profile.php?id=61560898449191">
      <template #icon>
        <font-awesome-icon icon="fa-brands fa-facebook-messenger" style="color: #0054e6;" />
      </template>
    </a-float-button>
    <a-float-button href="https://t.me/monkeywise07">
      <template #icon>
        <font-awesome-icon icon="fa-brands fa-telegram" style="color: #000000;" />
      </template>
    </a-float-button>
  </a-float-button-group>
  <ModalLogin ref="modalCreate"></ModalLogin>
</template>

<script>
  import weaviate from 'weaviate-ts-client';
  import ModalLogin from "@/views/Auth/login.vue"
  import EventBus from "@/helpers/EventBus.js";
  import { ref, onMounted, onUnmounted, reactive, beforeMount, computed, provide } from "vue";
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
    CameraOutlined,

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
      ModalLogin,
      CameraOutlined
    },


    beforeUnmount() {
      window.removeEventListener("scroll", this.handleScroll);
    },
    methods: {
      handleChangeImageSearch(info) {

        this.fileListImageSearch = [...info.fileList];

        if (info.file.status == 'done') {
          // Xử lý khi tải lên thành công
          const file = info.file.originFileObj; // Lấy đối tượng file gốc
          if (file) {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = async () => {
              const base64Image = reader.result.split(',')[1]; // Lấy phần base64 từ Data URL
              await this.searchImage(base64Image);
            };
            reader.onerror = (error) => {
              console.error('Error reading file:', error);
            };
          }
        } else if (info.file.status == 'error') {
          message.error(`${info.file.name} Lỗi .`);
          this.fileListImageSearch = [];
          info.file = null

        } else if (info.file.status == null) {
          this.fileListImageSearch = [];
          info.file = null

        }

      },
      async searchImage(base64Image) {
        try {

          const resImage = await this.client.graphql.get()
            .withClassName('ProductImage')
            .withFields(['image', 'path', '_additional { certainty}'])
            .withNearImage({ image: base64Image, certainty: 0.8 })
            .withLimit(5)
            .do();

          resImage.data.Get.ProductImage.sort((a, b) => b._additional.certainty - a._additional.certainty);
          var imagePaths = []; // Tạo một mảng trống để lưu trữ các imagePath

          if (resImage.data.Get.ProductImage && resImage.data.Get.ProductImage.length > 0) {
            resImage.data.Get.ProductImage.forEach((image, index) => {
              const imagePath = image?.path;
              const certainty = image?._additional?.certainty;


              if (imagePath && certainty !== undefined) {
                imagePaths.push(imagePath);

              } else {
                console.error(`Lỗi ảnh ${index + 1}: Đường dẫn hoặc độ chính xác hoặc điểm rerank không hợp lệ.`);
              }
            });

            localStorage.setItem('imagePaths', JSON.stringify(imagePaths)); // Lưu vào localStorage

            // console.log("imagepath ở layout" , this.imagePaths)
            // ...
            this.$router.push({
              name: 'ResultImageSearch',
              query: { imagePaths: JSON.stringify(imagePaths) }
            });
            // ...



          } else {
            console.error('Không tìm thấy hình ảnh sản phẩm nào.');
          }

        } catch (error) {
          console.error('Error querying Weaviate:', error);
        }

      },
      openModalLogin() {
        this.$refs.modalCreate.showModal();
      },
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
        this.role = localStorage.getItem("role");
        this.isLoggedIn = !!token;
      },
      logout() {

        try {

          localStorage.removeItem("accessToken");
          localStorage.removeItem("role");
          localStorage.removeItem("userName");
          localStorage.removeItem("jwt");
          localStorage.removeItem("userPhone");
          localStorage.removeItem("userEmail");
          localStorage.removeItem("userCartId");
          this.isLoggedIn = false;
          this.$router.push({ name: "CustomerHome" });
          notification.success({
            message: "Đăng xuất thành công!",
          });
        } catch {
          notification.error({
            message: "Đăng xuất thất bại!",
          });
        }
      },
      handleSearch(event) {
        const keyword = event.target.value.trim();
        if (keyword) {
          // Navigate to the search page with the keyword as a parameter
          this.$router.push({ name: 'KeywordSearch', params: { keyword } });
        }
      }
    },
    data() {
      return {
        isLoggedIn: false,
        role: null,
        apiUrl: '',
        fileListImageSearch: [],
        client: null,

      };
    },
    beforeMount() {
      this.checkLoginStatus();
    },
    mounted() {
      this.checkLoginStatus();
      window.addEventListener("scroll", this.handleScroll);
      this.apiUrl = process.env.VUE_APP_URL + 'Account/valid-upload';
      this.isSynchronizing = true
      //  this.client = weaviate.client({
      //   scheme: 'http',
      //   host: 'localhost:8080'
      // });
      //đay là cấu hình dev
      this.client = weaviate.client({
        scheme: 'http',
        host: 'localhost:8081/weaviate'
      });
      //Đây là cấu hình docker
    },
    setup() {

      const beforeUpload = file => {

        // Danh sách các phần mở rộng tệp được chấp nhận
        const allowedExtensions = ['jpg', 'jpeg', 'png', 'bmp', 'webp'];
        var isJpgOrPng = true;
        // Kiểm tra phần mở rộng file
        const extension = file.name.split('.').pop().toLowerCase();
        if (!allowedExtensions.includes(extension)) {
          notification.error({
            message: 'Định dạng ảnh không hợp lệ',
            description: 'Chọn định dạng ảnh với một phần mở rộng: ' + allowedExtensions.join(', ')
          })
          isJpgOrPng = false;
          return false;
        }

        const isLt2M = file.size / 1024 / 1024 < 10;
        if (!isLt2M) {
          notification.error({
            message: 'Tính tối đa 10MB',
            description: 'Tính tối đa 10MB'
          })
          return false;
        }

        return isJpgOrPng && isLt2M;
      };
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
      const selectedMenu = ref(["Dashboard"]);

      const changeSelectedMenu = (newKeys) => {
        selectedMenu.value = [newKeys];
      };

      // Cung cấp selectedMenu và changeSelectedMenu cho các component con
      provide("selectedMenu", selectedMenu);
      provide("changeSelectedMenu", changeSelectedMenu);




      return {
        beforeUpload,
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

  /* .logo {
    float: right;

    margin: 16px 24px 16px 0;
    background: rgba(255, 255, 255, 0.3);
  }

  .logo {
    float: right;
    margin: 16px 0 16px 24px;
  } */
  :deep() {
    font-family: 'Archivo', serif !important;
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

  :deep(.ant-float-btn-body .btn-back-to-top) {
    background-color: #e54f54;

  }

  .ant-float-btn-body:hover {
    background-color: #c21f24;
  }

  body,
  html {

    background-color: #ffffff;

  }

  .footer-link {
    color: #5f5d5d;
  }

  .footer-link:hover {
    color: #de2128;
  }
:deep(.ant-layout-header) {
  padding-inline: 10px !important;
}

</style>
