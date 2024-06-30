<template>
  <a-row>
    <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
      <a-page-header style="
         border: 1px solid rgb(235, 237, 240);
          background-color: #c21f24;
        " @back="goBack">
        <template #backIcon>
          <font-awesome-icon :icon="['fas', 'backward']" style="color: white; margin-left: 30px" />
        </template>
        <template #title>
          <span style="color: white; font-size: 16px; margin-bottom: auto;display: block;">Quản lý danh mục</span>
        </template>
        <template #extra>
          <a-breadcrumb separator=">" style="margin-right:30px;">
            <a-breadcrumb-item href="">
              <font-awesome-icon icon="fa-solid fa-house" style="color: #fff;" />
              <span style="color: #fff;"> Trang quản trị</span>
            </a-breadcrumb-item>
            <a-breadcrumb-item href="">
              <font-awesome-icon :icon="['fas', 'gear']" style="color: #fff;" />
              <span style="color: #fff;"> Hệ thống</span>
            </a-breadcrumb-item>
            <a-breadcrumb-item href="">
              <font-awesome-icon :icon="['fas', 'fa-list']" style="color: #fff;" />
              <span style="color: #fff;"> Danh mục</span>
            </a-breadcrumb-item>

          </a-breadcrumb>
        </template>
      </a-page-header>
    </a-col>
  </a-row>
  <transition name="route" mode="out-in" appear>
    <a-row>
      <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
        <a-card :bordered="true" title="Danh sách danh mục" style="margin: 30px;">
          <template #extra>
            <a-button @click="openModalCreate" type="primary"><font-awesome-icon icon="fa-solid fa-plus" /> Thêm
              mới</a-button>
          </template>

          <a-row style="margin-bottom: 20px" :gutter="24">

            <a-col :span="12">
              <a-input v-model:value="formSearch.code_Filter" @keyup.enter="SearchData" placeholder="Mã danh mục">
              </a-input>
            </a-col>
            <a-col :span="12">
              <a-input v-model:value="formSearch.name_Filter" @keyup.enter="SearchData" placeholder="Tên danh mục">
              </a-input>
            </a-col>
          </a-row>
          <a-row>
            <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
              <a-table bordered :columns="tableColumns" :dataSource="dataSourceTable" :pagination="false"
                :loading="loadingTable" :scroll="{ x: 1000 }"
                :row-class-name="(_record, index) => (index % 2 === 1 ? 'table-striped' : null)"
                @change="handleTableChange">
                <template #bodyCell="{ column, record }">
                  <template v-if="column.key === 'action'">
                    <a-space>
                      <a-tooltip title="Cấu hình" placement="leftTop">
                        <a-button type="link" shape="circle">
                          <router-link :to="{
                            name: 'DataCategoryHome',
                            params: { id: record.id },
                          }">
                            <font-awesome-icon icon="fa-solid fa-list-check" style="color: #01a214" />
                          </router-link>
                          <template #icon></template>
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
                </template>
              </a-table>
            </a-col>
          </a-row>
          <a-row>
            <a-col :span="24">
              <a-pagination v-if="pagination.total > 0" style="margin-top: 20px; text-align: center"
                v-model:current="pagination.current" :pageSize="pagination.pageSize" :total="pagination.total"
                :showSizeChanger="false" :show-total="(total) => `Tổng ${total} bản ghi`"
                @change="handlePaginationChange" />
            </a-col>
          </a-row>
        </a-card>
      </a-col>
    </a-row>
  </transition>
  <ModalCreate @addSuccess="fetchDataAfterCreated()" ref="modalCreate" />
  <ModalUpdate @updateSuccess="fetchData(pagination.current, pagination.pageSize)" ref="modalUpdate" />
</template>
<script>
  import ModalCreate from "./ModalCreate.vue";
  import ModalUpdate from "./ModalUpdate.vue";
  import { Modal, Pagination, message, notification } from "ant-design-vue";
  import {
    SmileOutlined,
    DownOutlined,
    CompassOutlined,
  } from "@ant-design/icons-vue";
  import APIService from "@/helpers/APIService";
  import { inject } from "vue";
  export default {
    components: {
      SmileOutlined,
      DownOutlined,
      Pagination,
      ModalCreate,
      ModalUpdate,
    },

    data() {
      return {
        tableColumns: [
          {
            title: "Mã danh mục",
            dataIndex: "code",
            key: "code",
            width: "15%",
            sorter: true,
            showSorterTooltip: false
          },
          {
            title: "Tên danh mục",
            dataIndex: "name",
            key: "name",
            width: "15%",
            sorter: true,
            showSorterTooltip: false
          },
          {
            title: "Mô tả",
            dataIndex: "description",
            key: "description",
            width: "30%",
          },

          {
            title: "Hành động",
            dataIndex: "action",
            key: "action",
            width: "10%",
          },
        ],
        dataSourceTable: [],
        loadingTable: false,
        pagination: {
          current: 1,
          pageSize: 10,
          total: 0,
        },
        formSearch: {
          code_Filter: "",
          name_Filter: "",
          sortQuery: "",
        },
      };
    },
    mounted() {
      const selectedMenu = inject("selectedMenu");
      const changeSelectedMenu = inject("changeSelectedMenu");
      if (this.$route.name === "CategoryHome") {
        changeSelectedMenu("Category");
      }
      this.fetchData(this.pagination.current, this.pagination.pageSize);
    }, //end mounted
    methods: {
      goBack() {
        this.$router.go(-1); // Điều hướng trở lại trang trước
      },
      openModalCreate() {
        this.$refs.modalCreate.showModal();
      },
      async deleteObj(id) {
        try {
          const response = await APIService.delete(`category/soft-delete/${id}`);
          if (response.data.message == "Xóa thành công") {
            notification.success({
              message: "Thành công",
              description: response.data.message,
            });
            if (this.dataSourceTable.length === 1) {
              // Nếu chỉ còn 1 bản ghi (bản ghi vừa bị xóa), quay lại trang trước
              this.pagination.current = Math.max(1, this.pagination.current - 1);
            }

            this.fetchData(this.pagination.current, this.pagination.pageSize); // Tải lại dữ liệu
          } else {
            notification.error({
              message: "Thất bại",
              description: response.data.message,
            });
          }
        } catch (error) {
          notification.error({
            message: "Thất bại",
            description: "Xóa không thành công",
          });
        }
      },
      handlePaginationChange() {
        this.fetchData(this.pagination.current, this.pagination.pageSize);
      },
      SearchData() {
        this.fetchData(
          this.pagination.current,
          this.pagination.pageSize,
          this.formSearch
        );
      },
      async fetchDataAfterCreated() {
        this.loadingTable = true;
        var searchParam = {
     
          pageIndex: 1,
          pageSize: this.pagination.pageSize,
          sortQuery: "createdDate desc",
        };

        try {
          const response = await APIService.post(
            "/category/get-data-by-page",
            searchParam
          );
          this.dataSourceTable = response.data.data.items;
          this.pagination.total = response.data.data.totalCount;
          this.pagination.current = response.data.data.pageIndex;
          this.pagination.pageSize = response.data.data.pageSize;
        } catch (error) {
          message.error(error);
        }
        this.loadingTable = false;
      },
      async fetchData(pageIndex, pageSize, params) {
        this.loadingTable = true;
        var searchParam = {
          ...params,
          pageIndex: pageIndex,
          pageSize: pageSize,
          sortQuery: this.formSearch.sortQuery,
        };

        try {
          const response = await APIService.post(
            "/category/get-data-by-page",
            searchParam
          );
          this.dataSourceTable = response.data.data.items;
          this.pagination.total = response.data.data.totalCount;
          this.pagination.current = response.data.data.pageIndex;
          this.pagination.pageSize = response.data.data.pageSize;
        } catch (error) {
          message.error(error);
        }
        this.loadingTable = false;
      }, //end fetchData
      openModalUpdate(id) {
        this.$refs.modalUpdate.showModal(id);
      },
      handleTableChange(pagination, filters, sorter) {
        if (sorter.field && sorter.order) {
          this.formSearch.sortQuery = `${sorter.field} ${sorter.order === 'ascend' ? 'asc' : 'desc'}`;
        } else {
          // Nếu không có sắp xếp, đặt sortQuery về rỗng
          this.formSearch.sortQuery = '';
        }
        this.fetchData(pagination.current, pagination.pageSize);
      },


    },
  };
</script>
<style scoped>
  .btnSearch:hover {
    background-color: rgb(229 127 123);
    color: white;
  }



  /* Ẩn chữ trên màn hình nhỏ hơn hoặc bằng 768px */
  @media (max-width: 767px) {
    .ant-breadcrumb-link>span {
      display: none;
    }
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
    color: #fff;
    /* Đặt màu chữ cho breadcrumb */
  }

  .ant-page-header-heading-title {
    line-height: normal;
    /* Đặt màu chữ cho tiêu đề */
    display: flex;
    /* Hiển thị theo chiều ngang */
  }
</style>
