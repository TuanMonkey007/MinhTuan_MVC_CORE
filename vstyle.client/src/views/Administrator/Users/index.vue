<template>
  <a-row>
    <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
      <a-page-header style="
          border: 1px solid rgb(235, 237, 240);
          height: max-content;
          background-color: #fff;
          margin-bottom: 16px;
          align-items: center;
        " title="Quản lý tài khoản" @back="goBack">
        <template #extra>
          <a-breadcrumb separator=">">
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
    </a-col>
  </a-row>

  <transition name="route" mode="out-in" appear>
    <a-row>
      <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
        <a-card :bordered="true" title="Danh sách tài khoản" style="margin: 30px; min-height: 50vh">
          <template #extra>
            <a-button @click="openModalAdd" type="primary"><font-awesome-icon icon="fa-solid fa-plus" /> Thêm
              mới</a-button>
          </template>
          <a-row>
            <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
              <a-table :columns="tableColumns" :dataSource="dataSourceTable" :pagination="false" :loading="loadingTable"
                :scroll="{ x: 1000 }" :row-class-name="(_record, index) => (index % 2 === 1 ? 'table-striped' : null)
                  " @change="handleTableChange">
                <template #bodyCell="{ column, record }">
                  <template v-if="column.key === 'action'">
                    <a-space>
                      <a-tooltip title="Xem chi tiết" placement="leftTop">
                        <a-button type="link" shape="circle">
                          <template #icon><font-awesome-icon :icon="['fas', 'circle-info']"
                              style="color: #74c0fc" /></template>
                        </a-button>
                      </a-tooltip>

                      <a-tooltip title="Chỉnh sửa">
                        <a-button type="link" shape="circle" @click="openModalUpdate(record.id)">
                          <template #icon><font-awesome-icon icon="fa-solid fa-pen-to-square"
                              style="color: #ffd43b" /></template>
                        </a-button>
                      </a-tooltip>

                      <a-popconfirm title="Xác nhận xóa?" ok-text="Xóa" cancel-text="Hủy"
                        @confirm="deleteObj(record.id)">
                        <a-tooltip title="Xóa" placement="rightBottom">
                          <a-button type="link" shape="circle">
                            <template #icon><font-awesome-icon icon="fa-solid fa-trash-alt"
                                style="color: #ff4d4f" /></template>
                          </a-button>
                        </a-tooltip>
                      </a-popconfirm>
                    </a-space>
                  </template>

                  <template v-if="column.key === 'status'">
                    <a-space style="justify-content: center;">
                      <a-tooltip v-if="getLockoutIconColor(record.lockoutEnd, record.emailConfirmed) == '#e2e4e9'"
                        title="Chưa xác nhận" placement="right"> <font-awesome-icon :icon="['fas', 'circle']"
                          :style="{ color: '#e2e4e9' }" /></a-tooltip>
                      <a-tooltip v-if="getLockoutIconColor(record.lockoutEnd, record.emailConfirmed) == '#06c646'"
                        title="Đã xác thực" placement="right">
                        <font-awesome-icon :icon="['fas', 'circle']" :style="{ color: '#06c646' }" /></a-tooltip>
                      <a-tooltip v-if="getLockoutIconColor(record.lockoutEnd, record.emailConfirmed) == '#ffa500'"
                        title="Tạm khóa" placement="right">
                        <font-awesome-icon :icon="['fas', 'circle']" :style="{ color: '#ffa500' }" /></a-tooltip>
                      <a-tooltip v-if="getLockoutIconColor(record.lockoutEnd, record.emailConfirmed) == '#fe0101'"
                        title="Khóa dài hạn" placement="right">
                        <font-awesome-icon :icon="['fas', 'circle']" :style="{ color: '#fe0101' }" /></a-tooltip>
                    </a-space>

                  </template>

                 
                </template>
              </a-table>
            </a-col>
          </a-row>
          <a-row>
            <a-col :span="24">
              <a-pagination style="margin-top: 20px; text-align: center" v-model:current="pagination.current"
                v-model:pageSize="pagination.pageSize" :total="pagination.total" :showSizeChanger="false"
                :show-total="(total) => `Tổng ${total} bản ghi`" @change="handlePaginationChange" />
            </a-col>
          </a-row>
        </a-card>
      </a-col>
    </a-row>
  </transition>

  <ModalCreate @addSuccess="fetchData(pagination.current, pagination.pageSize)" ref="modalCreate" />
  <ModalUpdate @updateSuccess="fetchData(pagination.current, pagination.pageSize)" ref="modalUpdate" />
</template>
<script>
  import { message } from "ant-design-vue";
  import ModalCreate from "./ModalCreate.vue";
  import ModalUpdate from "./ModalUpdate.vue";
  import { Pagination } from "ant-design-vue";
  import APIService from "@/helpers/APIService";
  import { inject } from "vue";
  export default {
    components: {
      ModalCreate,
      ModalUpdate,
      APagination: Pagination,
    },
    data() {
      return {
        tableColumns: [
          {
            title: "Trạng thái",
            dataIndex: "lockoutEnd",
            key: "status",
            width: "10%",
          },
          {
            title: "Avatar",
            dataIndex: "avatar",
            key: "avatar",
            width: "15%",
            sorter: false,
          },
          {
            title: "Họ và tên",
            dataIndex: "fullName",
            key: "fullName",
            width: "15%",
            sorter: false,
          },
          {
            title: "Giới tính",
            dataIndex: "gender",
            key: "gender",
            width: "15%",
            sorter: false,
          },
          {
            title: "Email",
            dataIndex: "email",
            key: "email",
            width: "15%",
            sorter: false,
          },
          {
            title: "Số điện thoại",
            dataIndex: "phoneNumber",
            key: "phone",
            width: "15%",
          },

          {
            title: "Hành động",
            dataIndex: "action",
            key: "action",

            width: "15%",
          },
        ],
        dataSourceTable: [],
        loadingTable: false,
        pagination: {
          current: 1,
          pageSize: 5,
          total: 0,
        },
        searchParams: {
          PageIndex: 1,
          PageSize: 5,
        },
      };
    }, //end data

    mounted() {
      const selectedMenu = inject("selectedMenu");
      const changeSelectedMenu = inject("changeSelectedMenu");
      if (this.$route.name === "AccountHome") {
        changeSelectedMenu("Account");
      }
      this.fetchData();
    }, //end mounted
    methods: {
      goBack() {
        this.$router.go(-1); // Điều hướng trở lại trang trước
      },
      changeStatus(id) {
        console.log("changeStatus");
      },
      handlePaginationChange(current, pageSize) {
        this.pagination.current = current;
        this.pagination.pageSize = pageSize;
        this.fetchData(this.pagination.current, this.pagination.pageSize);
      },
      async fetchData(current, pageSize) {
        this.loadingTable = true;
        try {
          const response = await APIService.post("/account/get-all-user", {
            PageIndex: current,
            PageSize: pageSize,
          });
          this.dataSourceTable = response.data.data.items;
          this.pagination.total = response.data.data.totalCount;
          console.log(this.dataSourceTable);
        } catch (error) {
          console.log(error);
        }
        this.loadingTable = false;
      },
      getLockoutIconColor(lockoutEnd, emailConfirmed) {
        const now = new Date();
        console.log(emailConfirmed);
        const lockoutEndDate = new Date(lockoutEnd);
        const remainingLockout = lockoutEndDate - now;

        if (remainingLockout > 10 * 60 * 1000) {
          // Hơn 10 phút
          return "#fe0101"; // Màu đỏ
        } else if (remainingLockout > 0) {
          // Dưới 10 phút
          return "#ffa500"; // Màu cam
        } else {
          // Đã hết thời gian khóa
          if (emailConfirmed == false) {
            return "#e2e4e9"; // Màu xám
          }
          return "#06c646"; // Màu xanh
        }
      }, //end getLockoutIconColor
      
      openModalAdd() {
        this.$refs.modalCreate.showModal();
      },
      openModalUpdate(id) {
        this.$refs.modalUpdate.showModal(id);
      },
      async deleteObj(id) {
        try {
          const response = await APIService.delete(`account/delete/${id}`);

          if (response.data.message == "Xóa thành công") {
            message.success("Xóa thành công");
            if (this.dataSourceTable.length === 1) {
              // Nếu chỉ còn 1 bản ghi (bản ghi vừa bị xóa), quay lại trang trước
              this.pagination.current = Math.max(1, this.pagination.current - 1);
            }

            this.fetchData(this.pagination.current, this.pagination.pageSize); // Tải lại dữ liệu
          } else {
            message.error("Xóa thất bại");
          }
        } catch (error) {
          message.error("Xóa thất bại");
        }
      },
    }, //end methods
  };
</script>

<style scoped>
  :deep(.table-striped) td {
    background-color: #ededed;
  }

  /* Ẩn chữ trên màn hình nhỏ hơn hoặc bằng 768px */
  @media (max-width: 767px) {
    .ant-breadcrumb-link>span {
      display: none;
    }
  }
</style>
