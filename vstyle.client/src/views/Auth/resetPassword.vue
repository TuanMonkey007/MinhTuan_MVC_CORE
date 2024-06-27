<template>
  
  <a-row>
        <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <a-page-header style="
         border: 1px solid rgb(235, 237, 240);
          background-color: #c22026;
          height: fit-content;
        ">

                <template #title>
                    <a-breadcrumb separator=">" style="margin-left:30px;">
                        <a-breadcrumb-item href="">

                            <span style="color: #fff;"> Trang chủ</span>
                        </a-breadcrumb-item>

                        <a-breadcrumb-item class="active">

                            <span style="color: #fff;"> Đặt lại mật khẩu</span>
                        </a-breadcrumb-item>

                    </a-breadcrumb>
                </template>

            </a-page-header>
        </a-col>
    </a-row>
    <div>
      <a-row justify="center"> 
        <a-col :xs="24" :sm="24" :md="10" :lg="10" :xl="10">
          <a-card style="display: grid;
    justify-content: center;
    align-items: center;">
    
    <a-form style="    padding: 10px;
    
   " v-bind="layout" :model="formReset" name="normal_login" class="login-form" @finish="onFinish"
      @finishFailed="onFinishFailed">

      <a-form-item label="Mật khẩu" name="Password" :rules="[{ required: true, message: 'Vui lòng nhập mật khẩu!' },
      { min: 8, message: 'Tôi thiểu 8 ký tự' },
      { max: 25, message: 'Tối đa 25 ký tự' },
      {
        pattern: /^(?=.*[0-9])[a-zA-Z0-9!#$%&()*+,\-./:;<=>?@[\\\]^_`{|}~]+$/,
        message: 'Tối thiểu 1 số'
      },

      {
        pattern: /^(?=.*[a-z])[a-zA-Z0-9!#$%&()*+,\-./:;<=>?@[\\\]^_`{|}~]+$/,
        message: 'Tối thiểu 1 chữ thường'
      },

      {
        pattern: /^(?=.*[A-Z])[a-zA-Z0-9!#$%&()*+,\-./:;<=>?@[\\\]^_`{|}~]+$/,
        message: 'Tối thiểu 1 chữ hoa'
      }
        ,
      {
        pattern: /^(?=.*[!#$%&()*+,\-./:;<=>?@[\\\]^_`{|}~])[a-zA-Z0-9!#$%&()*+,\-./:;<=>?@[\\\]^_`{|}~]+$/,
        message: 'Ít nhất 1 ký tự đặc biệt'
      }


      ]">
        <a-input-password v-model:value="formReset.Password">
          <template #prefix>
            <LockOutlined class="site-form-item-icon" />
          </template>
        </a-input-password>
      </a-form-item>
      <a-form-item label="Nhập lại mật khẩu" name="ConfirmPassword"
        :rules="[{ required: true, message: 'Vui lòng nhập lại mật khẩu!' }, { validator: checkConfirmPassword }]">
        <a-input-password v-model:value="formReset.ConfirmPassword">
          <template #prefix>
            <LockOutlined class="site-form-item-icon" />
          </template>
        </a-input-password>
      </a-form-item>
      <a-button style="background-color: #c22026;color: #fff" :disabled="disableBtnReset" type="primary" html-type="submit" class="login-form-button">
        Hoàn thành
      </a-button>
    </a-form>
  </a-card >

          </a-col>  
      </a-row>
  
</div>
</template>

<script>
  import { defineComponent, reactive, computed } from 'vue';
  import { UserOutlined, LockOutlined } from '@ant-design/icons-vue';
  import { message, notification } from 'ant-design-vue';
  import APIService from '@/helpers/APIService';
  export default {
    components: {
      LockOutlined,
      UserOutlined
    },


    methods: {
      async onFinish() {
        notification.info({
          message: 'Thống báo',
          description:
            'Đang xử lý....',
            key: 'loadingKey',
            duration: 0
        })
        try {
          const encodedToken = encodeURIComponent(this.token);
          const resetPasswordViewModel = {
            Password: this.formReset.Password,
            Token: this.token,
            Email: this.email
          };
          console.log(encodedToken);

          const response = await APIService.post("account/reset-password", resetPasswordViewModel);
          console.log(response.data)
          if(response.data.data !="") {
            notification.success({
              message: 'Thống báo',
              description:
                'Thay đổi mật khẩu thành công',
                key: 'loadingKey',
              
            })
            this.$router.push({ name: 'CustomerHome' });
          }else{
            notification.error({
            message: 'Thất bại',
            description:
              response.data.message,
              key: 'loadingKey',
             
          })
            
          }
          
        } catch {
          notification.error({
            message: 'Thất bại',
            description:
              'Đã có lỗi xảy ra',
              key: 'loadingKey',
            
          })
          
        }
      }
    },//end of methods
    setup() {
      const layout = {
        labelCol: {
          span: 10,
        },
        wrapperCol: {
          span: 14,
        },
      };
      const formReset = reactive({
        Password: '',
        ConfirmPassword: '',
      });

      const disableBtnReset = computed(() => {
        return !formReset.Password || !formReset.ConfirmPassword;
      });
      const checkConfirmPassword = (rule, value, callback) => {
        if (value !== formReset.Password) {
          callback(new Error('Mật khẩu không khớp'));
        } else {
          callback();
        }
      };
      return {
        formReset,
        disableBtnReset,
        layout,
        checkConfirmPassword
      };
    },//end of setup
    props: ['token','email'],

    mounted() {
      console.log("Token1: ",this.token); // Sử dụng token tại đây
      console.log("Token2: ", encodeURIComponent(this.token)); // Sử dụng email tại đây
      console.log("Email là: ", this.email); // Sử dụng email tại đây
    }

  }
</script>
<style scoped>


  .login-form-forgot {
    float: right;
  }

  .ant-col-rtl .login-form-forgot {
    float: left;
  }

  .login-form-button {
    width: 100%;
  }
  .ant-page-header {
        padding: 0px;
        /* Giảm padding */
        margin-bottom: 8px;
        /* Giảm margin dưới */
    }

    .ant-page-header {
        height: 35px;
        /* Tự động điều chỉnh chiều cao */
    }

    :deep(.ant-breadcrumb-separator) {
        color: #ffffff86;
        /* Đặt màu chữ cho breadcrumb */
    }

    :deep(.ant-breadcrumb-link a) {
        color: #ffffff86;
        /* Đặt màu chữ cho breadcrumb */
    }

    .active {
        color: #e60b0b;
        /* Đặt màu chữ cho breadcrumb */
    }

    .ant-page-header-heading-title {
        line-height: normal;
        /* Đặt màu chữ cho tiêu đề */
        display: flex;
        /* Hiển thị theo chiều ngang */
        color: #ffffff86;
    }
</style>
