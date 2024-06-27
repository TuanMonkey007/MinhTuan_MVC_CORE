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
          <span style="color: white; font-size: 16px; margin-bottom: auto;display: block;">Quản lý đơn hàng</span>
        </template>
        <template #extra>
          <a-breadcrumb separator=">" style="margin-right:30px;">
            <a-breadcrumb-item href="">
              <font-awesome-icon icon="fa-solid fa-house" style="color: #fff;" />
              <span style="color: #fff;"> Trang quản trị</span>
            </a-breadcrumb-item>

            <a-breadcrumb-item href="">

              <font-awesome-icon icon="fa-solid fa-cart-shopping" style="color: #fff;" />
              <span style="color: #fff;"> Đơn hàng</span>
            </a-breadcrumb-item>

          </a-breadcrumb>
        </template>
      </a-page-header>
    </a-col>
  </a-row>
  <transition name="route" mode="out-in" appear>
    <a-row>
      <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
        <a-card :bordered="true" title="Danh sách đơn hàng" style="margin: 30px;">
          
          
          <a-row style="margin-bottom: 20px" :gutter="24">
              <a-col :span="24">
                <a-collapse v-model:activateKey="activateKey" :accordion="true" :bordered="false"
                  style="background-color:rgb(206 213 225)">
                  <a-collapse-panel key="1" :show-arrow="false" ref="searchCollapse">
                    <template #header>
                      <a-row>
                        <a-col :span="24">
                          <a-space>
                            <font-awesome-icon icon="fa-solid fa-filter" />
                            <span>Bộ lọc</span>
                          </a-space>
                        </a-col>
                      </a-row>
                    </template>
                    <a-row style="margin-bottom: 20px" :gutter="24">

                      <a-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
                        <a-input v-model:value="formSearch.orderCode_Filter" @keyup.enter="SearchData"
                          placeholder="Mã đơn hàng">
                        </a-input>
                      </a-col>
                      <a-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
                        <a-range-picker style="width: 100%" :placeholder="['Từ ngày','Đến ngày']" v-model:value="formSearch.rangeTime_Filter" format="YYYY-MM-DD"
                        />
                      </a-col>
                    </a-row>
                 
                    <a-row style="justify-content: contents;margin-top: 10px">
                      <a-col :span="24" style="justify-items: center; display: grid;">
                        <a-button type="primary" @click="SearchData" class="btnSearch">
                          <font-awesome-icon icon="fa-solid fa-search" />
                          Tìm kiếm
                        </a-button>
                      </a-col>
                    </a-row>
                  </a-collapse-panel>
                </a-collapse>
              </a-col>
            </a-row>


          <a-row>
            <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
              <a-table bordered :columns="tableColumns" :dataSource="dataSourceTable" :pagination="false"
                :loading="loadingTable" :scroll="{ x: 1000 }"
                :row-class-name="(_record, index) => (index % 2 === 1 ? 'table-striped' : null)"
                @change="handleTableChange">
                <template #bodyCell="{ column, record }">
                  <template v-if="column.key == 'code'">
                    <router-link style="color: #c21f24" :to="{
                          name: 'DetailOrder',
                          params: { id: record.id },
                        }">
                      {{ record.code }}</router-link>
                    </template> 
                  <template v-if="column.key == 'status'">
                    <a-tag :color="getColor(record.isCancelled,record.statusName)">
                      {{ record.isCancelled == true ? 'Đơn huỷ' : record.statusName }}
                    </a-tag>
                    </template> 
                  <template v-if="column.key == 'totalAmount'">
                    <span>{{ fomartPrice(record.totalAmount) }}</span>
                    </template> 
                  <template v-if="column.key === 'createdDate'">
                    {{ formatCreatedDate(record.createdDate) }}
                  </template>
                  <template v-if="column.key === 'action'">
                    <a-space>
                      <a-tooltip title="Chi tiết" placement="leftTop">
                        <router-link :to="{
                          name: 'DetailOrder',
                          params: { id: record.id },
                        }">
                          <a-button type="link" shape="circle">
                            <font-awesome-icon :icon="['fas', 'info']" style="color: #01a214" />
                            <template #icon></template>
                          </a-button>
                        </router-link>
                      </a-tooltip>
                      <a-tooltip title="Hủy đơn" placement="leftTop">
                        
                          <a-button type="link" @click="openModalCancelOrder(record.id, record.statusName, record.isCancelled)" shape="circle">
                            <font-awesome-icon icon="fa-solid fa-ban" style="color: #0253de;" />

                            <template #icon></template>
                          </a-button>
                      </a-tooltip>
                      <a-tooltip title="Cập nhật trạng thái" placement="leftTop">
                        
                        <a-button type="link" @click="openModalUpdateStatus(record.id,record.paymentMethodName, record.isCancelled)" shape="circle">
                          <font-awesome-icon icon="fa-solid fa-pen-to-square"
                          style="color: #ffd43b" />

                         
                        </a-button>
                    </a-tooltip>


                      <a-popconfirm title="Xác nhận xóa?" ok-text="Xóa" cancel-text="Hủy"
                        @confirm="deleteObj(record.id, record.isCancelled)">
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
  <ModalCancel @updateSuccess="fetchData(pagination.current, pagination.pageSize)" ref="modalCancel"/>
  <ModalUpdateStatus @updateSuccess="fetchData(pagination.current, pagination.pageSize)" ref="modalUpdateStatus"/>
</template>
<script>
  import dayjs from "dayjs";
  import ModalCancel from "./ModalCancel.vue";
  import ModalUpdateStatus from "./ModalUpdateStatus.vue";
 
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
      ModalCancel,
      ModalUpdateStatus
    
    },

    data() {
      return {
        tableColumns: [
          {
            title: "Mã",
            dataIndex: "code",
            key: "code",
            width: "15%",
            sorter: true,
            showSorterTooltip: false,
          },
          {
            title: "Người mua",
            dataIndex: "customerName",
            key: "customerName",
            width: "15%",
            sorter: true,
            showSorterTooltip: false,
          },
          {
            title: "Địa chỉ",
            dataIndex: "shippingAddress",
            key: "shippingAddress",
            width: "15%",
            sorter: true,
            showSorterTooltip: false,
          },
          {
            title: "Trạng thái",
            dataIndex: "status",
            key: "status",
            width: "15%",
         
          },
          {
            title: "Tổng tiền",
            dataIndex: "totalAmount",
            key: "totalAmount",
            width: "10%",
            sorter: true,
            showSorterTooltip: false,
          },
          {
            title: "Ngày đặt",
            dataIndex: "createdDate",
            key: "createdDate",
            width: "10%",
            sorter: true,
            showSorterTooltip: false,
          },

          {
            title: "Hành động",
            dataIndex: "action",
            key: "action",
            width: "5%",
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
          orderCode_Filter: "",
          rangeTime_Filter: [],
          startTime_Filter: "",
          endTime_Filter: "",
          sortQuery: "",
        },
      };
    },
    mounted() {
      const selectedMenu = inject("selectedMenu");
      const changeSelectedMenu = inject("changeSelectedMenu");
      if (this.$route.name === "OrderHome") {
        changeSelectedMenu("Order");
      }
      this.fetchData(this.pagination.current, this.pagination.pageSize);
    }, //end mounted
    methods: {
      openModalUpdateStatus(id,statusName,isCancelled) {
        if(isCancelled == true){
          notification.warning({
            message: "Không thể hành động",
            description: "Đơn hàng đã bị hủy",
          })
          return
        }
        this.$refs.modalUpdateStatus.showModal(id,statusName);
      },
      getColor(isCancelled,statusName) {
       if(isCancelled == true){
         return "red"
       }
       if(statusName.includes("Chờ xác nhận")){
         return "yellow"
       }
       return "green"
      },
      openModalCancelOrder(id,statusName,isCancelled) {
        if(isCancelled == true){
          notification.warning({
            message: "Không thể hành động",
            description: "Đơn hàng đã bị hủy",
          })
          return
        }
        if(statusName.includes("Đang vận chuyển")  || statusName.includes("Đang giao hàng")){ 
          console.log(statusName)
          notification.warning({
            message: "Thể hành động",
            description: "Đơn hàng không thể hủy trong thời gian này",
          })
          return
        }
        this.$refs.modalCancel.showModal(id);
      },
      fomartPrice(price) {
                return price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.');
      },
      goBack() {
        this.$router.go(-1); // Điều hướng trở lại trang trước
      },
      openModalCreate() {
        this.$refs.modalCreate.showModal();
      },
      async deleteObj(id,isCancelled) {
        try {
          if(isCancelled != true){
          notification.warning({
            message: "Không thể hành động",
            description: "Đơn hàng không thể xóa",
          })
          return
        }
          const response = await APIService.delete(`order/soft-delete/${id}`);
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
        this.formSearch.startTime_Filter=this.formSearch.rangeTime_Filter[0]
        this.formSearch.endTime_Filter=this.formSearch.rangeTime_Filter[1]
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
          sortQuery: this.formSearch.sortQuery,
        };

        try {
          const response = await APIService.post(
            "order/get-data-by-page",
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

      formatCreatedDate(date) {
        return dayjs(date).format("HH:mm:ss DD/MM/YYYY ");
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
    },//end methods

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
