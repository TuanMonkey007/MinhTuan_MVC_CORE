<template>
     <a-row>
    <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
      <a-page-header
        style="
          border: 1px solid rgb(235, 237, 240);
          height: min-content;
          background-color: #fff;
          margin-bottom: 16px;
        "
        title="Quản lý sản phẩm"
       
      >
        <template #extra>
          <a-breadcrumb separator=">">
            <a-breadcrumb-item href="">
              <font-awesome-icon icon="fa-solid fa-house" />
              <span> Trang quản trị</span>
            </a-breadcrumb-item>
            <a-breadcrumb-item href="">
                <font-awesome-icon :icon="['fas', 'shirt']" />
              <span> Sản phẩm</span>
            </a-breadcrumb-item>
            
          </a-breadcrumb>
        </template>
</a-page-header>
</a-col>
</a-row>
<transition name="route" mode="out-in" appear>
  <a-row>
    <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
      <a-card :bordered="true" title="Danh sách sản phẩm" style="margin: 30px; height: 600px">
        <template #extra>
        <a-button @click="openModalCreate" type="primary"
          ><font-awesome-icon icon="fa-solid fa-plus" /> Thêm mới</a-button
        >
      </template>

        <a-row style="margin-bottom: 20px" :gutter="24">
          
          <a-col :span="12">
            <a-input v-model:value="formSearch.code_Filter"  @keyup.enter="SearchData" placeholder="Mã sản phẩm">
            </a-input>
          </a-col>
          <a-col :span="12">
            <a-input v-model:value="formSearch.name_Filter"  @keyup.enter="SearchData" placeholder="Tên sản phẩm">
            </a-input>
          </a-col>
        </a-row>
        <a-row>
          <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <a-table :columns="tableColumns" :dataSource="dataSourceTable" :pagination="false" :loading="loadingTable"
            :scroll="{ x: 1000 }"   :row-class-name="(_record, index) => (index % 2 === 1 ? 'table-striped' : null)"
              @change="handleTableChange">
              <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'action'">
            <a-space>
              

              <a-tooltip title="Chỉnh sửa">
                <a-button type="link" shape="circle" @click="openModalUpdate(record.id)">
                  <template #icon><font-awesome-icon
                      icon="fa-solid fa-pen-to-square"
                      style="color: #ffd43b"
                  /></template>
                </a-button>
              </a-tooltip>

              <a-popconfirm title="Xác nhận xóa?" ok-text="Xóa" cancel-text="Hủy" @confirm="deleteObj(record.id)">
                <a-tooltip title="Xóa" placement="rightBottom">
                  <a-button type="link" shape="circle">
                    <template #icon><font-awesome-icon
                        icon="fa-solid fa-trash-alt"
                        style="color: #ff4d4f"
                    /></template>
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
<ModalCreate @addSuccess="fetchData(pagination.current, pagination.pageSize)" ref="modalCreate" />
</template>
<script>
    import ModalCreate from "@/views/Administrator/Products/ModalCreate.vue";
  import { Modal, Pagination, message } from "ant-design-vue";
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
      ModalCreate

    },

    data() {
      return {
        tableColumns: [
        {
            title: "Hình ảnh",
            dataIndex: "imageThumbnail",
            key: "imageThumbnail",
            width: "20%",
          },
          {
            title: "Mã sản phẩm",
            dataIndex: "code",
            key: "code",
            width: "15%",
            sorter: false,
          },
          {
            title: "Tên sản phẩm",
            dataIndex: "name",
            key: "name",
            width: "15%",
            sorter: false,
          },
          {
            title: "Giá tiền",
            dataIndex: "price",
            key: "price",
            width: "10%",
          },
          {
            title: "Số lượng",
            dataIndex: "stockQuantity",
            key: "stockQuantity",
            width: "10%",
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
          pageSize: 5,
          total: 0,
        },
        formSearch: {
          code_Filter: "",
          name_Filter: "",
        },
      };
    },
    mounted() {
      const selectedMenu = inject("selectedMenu");
      const changeSelectedMenu = inject("changeSelectedMenu");
      if (this.$route.name === "ProductHome") {
        changeSelectedMenu("Product");
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
          const response = await APIService.delete(`product/soft-delete/${id}`);
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
      async fetchData(pageIndex, pageSize, params) {
        this.loadingTable = true;
        var searchParam = {
          ...params,
          pageIndex: pageIndex,
          pageSize: pageSize,
        };

        try {
          const response = await APIService.post(
            "/product/get-data-by-page",
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
    },
  };
</script>
<style scoped>
      .btnSearch:hover {
    background-color: rgb(229 127 123);
    color: white;
  }

  :deep(.table-striped) td {
    background-color: #fafafa;
  }

  /* Ẩn chữ trên màn hình nhỏ hơn hoặc bằng 768px */
  @media (max-width: 767px) {
    .ant-breadcrumb-link>span {
      display: none;
    }
  }
</style>