<template>
  <div class="container" style="display: grid;
    
    justify-content: center;
    margin-top: 25vh;">

    <a-form style="    padding: 70px;
    border: 1px solid #ccc;
    border-radius: 5px;" v-bind="layout" :model="formReset" name="normal_login" class="login-form" @finish="onFinish"
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
      <a-button :disabled="disableBtnReset" type="primary" html-type="submit" class="login-form-button">
        Hoàn thành
      </a-button>
    </a-form>
  </div>
</template>

<script>
  import { defineComponent, reactive, computed } from 'vue';
  import { UserOutlined, LockOutlined } from '@ant-design/icons-vue';
  import { message } from 'ant-design-vue';
  import APIService from '@/helpers/APIService';
  export default {
    components: {
      LockOutlined,
      UserOutlined
    },


    methods: {
      async onFinish() {
        message.loading({ content: 'Đang xử lý...', key: "LoadingKey", duration: 0 });
        try {
          const encodedToken = encodeURIComponent(this.token);
          const resetPasswordViewModel = {
            Password: this.formReset.Password,
            Token: this.token,
            Email: this.email
          };
          console.log(encodedToken);

          const response = await APIService.post("account/reset-password", resetPasswordViewModel);
          if(response.data.data =="") {
            message.error({ content: response.data.message, key: "LoadingKey", duration: 2 });
            this.$router.push({ name: 'Login' });
          }else{
            message.success({ content: response.data.message, key: "LoadingKey", duration: 2 });
            this.$router.push({ name: 'Login' });
          }
          
        } catch {
          message.error({ content: 'Đã xảy ra lỗi', key: "LoadingKey", duration: 2 });
          
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
</style>
