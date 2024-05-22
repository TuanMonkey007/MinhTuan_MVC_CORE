<template>
  <a-page-header
    style="border: 1px solid rgb(235, 237, 240); height: max-content; background-color: #fff; margin-bottom: 16px; align-items: center;"
    title="Quản lý tài khoản" @back="goBack">
    <template #extra>
      <a-breadcrumb>
        <a-breadcrumb-item href="">
          <font-awesome-icon icon="fa-solid fa-house" />
        </a-breadcrumb-item>
        <a-breadcrumb-item href="">
          <font-awesome-icon :icon="['fas', 'gear']" />
          <span> Hệ thống</span>
        </a-breadcrumb-item>

        <a-breadcrumb-item>
          <font-awesome-icon :icon="['fas', 'user']" />
          <span> Tài khoản người dùng</span>
        </a-breadcrumb-item>
      </a-breadcrumb>
    </template>
  </a-page-header>
  <transition name="route" mode="out-in" appear>

    <a-card :bordered="true" title="Danh sách tài khoản" style=" margin: 30px; min-height: 50vh;">
      <template #extra>
        <a-button @click="openModalAdd" type="primary">Thêm mới</a-button>
      </template>
      <a-table :columns="tableColumns" :dataSource="dataSourceTable" :pagination="false" :loading="loadingTable"
        @change="handleTableChange">
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'action'">
            <a-space>
              <a-tooltip title="Xem chi tiết" placement="leftTop">

                <a-button type="link" shape="circle" @click="openModalDetail(record.id)">
                  <template #icon><font-awesome-icon :icon="['fas', 'circle-info']" style="color: #74C0FC;" /></template>
                </a-button>
              </a-tooltip>

              <a-tooltip title="Chỉnh sửa">
                <a-button type="link" shape="circle" @click="openModalDetail(record.id)">
                  <template #icon><font-awesome-icon icon="fa-solid fa-pen-to-square"
                      style="color: #FFD43B;" /></template>
                </a-button>
              </a-tooltip>

              <a-popconfirm title="Xác nhận xóa?" ok-text="Xóa" cancel-text="Hủy" @confirm="deleteObj(record.id)">
                <a-tooltip title="Xóa" placement="rightBottom">
                  <a-button type="link" shape="circle">
                    <template #icon><font-awesome-icon icon="fa-solid fa-trash-alt"
                        style="color: #ff4d4f;" /></template>
                  </a-button>
                </a-tooltip>
              </a-popconfirm>
            </a-space>
          </template>

          <template v-if="column.key === 'status'">
            <a-space style="justify-content: center;">

              <font-awesome-icon :icon="['fas', 'circle']"
                :style="{ color: getLockoutIconColor(record.lockoutEnd) }" />
              
            </a-space>

          </template>

        </template>
      </a-table>
      <a-pagination style="margin-top: 20px; text-align: center;" v-model:current="pagination.current"
        v-model:pageSize="pagination.pageSize" :total="pagination.total" :showSizeChanger="false"
        :show-total="total => `Tổng ${total} bản ghi`" @change="handlePaginationChange" />
    </a-card>


  </transition>
</template>
<script>
  import { Pagination } from 'ant-design-vue';
  import APIService from '@/helpers/APIService';
  import { inject } from 'vue';
  export default {
    components: {
      APagination: Pagination
    },
    data() {
      return {
        tableColumns: [
        {
            title: 'Trạng thái',
            dataIndex: 'lockoutEnd',
            key: 'status',
            width: '10%',
          },
        {
            title: 'Avatar',
            dataIndex: 'avatar',
            key: 'avatar',
            width: '15%',
            sorter: false,
          },
          {
            title: 'Họ và tên',
            dataIndex: 'fullName',
            key: 'fullName',
            width: '15%',
            sorter: false,
          },
          {
            title: 'Giới tính',
            dataIndex: 'gender',
            key: 'gender',
            width: '15%',
            sorter: false,
          },
          {
            title: 'Email',
            dataIndex: 'email',
            key: 'email',
            width: '15%',
            sorter: false,
          },
          {
            title: 'Số điện thoại',
            dataIndex: 'phoneNumber',
            key: 'phone',
            width: '15%',
          },
         

          {
            title: 'Hành động',
            dataIndex: 'action',
            key: 'action',
            width: '10%',

          },
        ],
        dataSourceTable: [],
        loadingTable: false,
        pagination: {
          current: 1,
          pageSize: 10,
          total: 0
        },
        searchParams: {
          PageIndex: 1,
          PageSize: 10,
        },


      }
    },//end data

    mounted() {
      const selectedMenu = inject('selectedMenu');
      const changeSelectedMenu = inject('changeSelectedMenu');
      if (this.$route.name === 'AccountHome') {
        changeSelectedMenu('Account');
      }
      this.fetchData();

    },//end mounted
    methods: {
      goBack() {
        this.$router.go(-1); // Điều hướng trở lại trang trước
      },
      handlePaginationChange(current, pageSize) {
        this.searchParams.PageIndex = current;
        this.searchParams.PageSize = pageSize;
        this.fetchData();
      },
      async fetchData() {
        this.loadingTable = true;
        try {
          const response = await APIService.post('/account/get-all-user', {
            pageIndex: 1,
            pageSize: 10,

          });
          this.dataSourceTable = response.data.data.items;
          this.pagination.total = response.data.data.totalCount;
          console.log(this.dataSourceTable);
        } catch (error) {
          console.log(error);
        }
        this.loadingTable = false;
      },
      getLockoutIconColor(lockoutEnd) {
        const now = new Date();
        const lockoutEndDate = new Date(lockoutEnd);
        const remainingLockout = lockoutEndDate - now;

        if (remainingLockout > 10 * 60 * 1000) { // Hơn 10 phút
          return '#fe0101'; // Màu đỏ
        } else if (remainingLockout > 0) { // Dưới 10 phút
          return '#ffa500'; // Màu cam
        } else { // Đã hết thời gian khóa
          return '#06c646'; // Màu xanh
        }
      },
    },//end methods
  }
</script>

<style scoped></style>