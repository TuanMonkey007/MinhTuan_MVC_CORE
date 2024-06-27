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
          <span style="color: white; font-size: 16px; margin-bottom: auto;display: block;">Quản lý bài viết</span>
        </template>
        <template #extra>
          <a-breadcrumb separator=">" style="margin-right:30px;">
            <a-breadcrumb-item href="">
              <font-awesome-icon icon="fa-solid fa-house" style="color: #fff;" />
              <span style="color: #fff;"> Trang quản trị</span>
            </a-breadcrumb-item>

            <a-breadcrumb-item href="">
              <font-awesome-icon :icon="['fas', 'fa-list']" style="color: #fff;" />
              <span style="color: #fff;"> Bài viết</span>
            </a-breadcrumb-item>

          </a-breadcrumb>
        </template>
      </a-page-header>
    </a-col>
  </a-row>
  <transition name="route" mode="out-in" appear>
    <a-row>
      <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
        <a-card :bordered="true" title="Danh sách bài viết" style="margin: 30px;">
          <template #extra>
            <a-button @click="openModalCreate" type="primary"><font-awesome-icon icon="fa-solid fa-plus" /> Thêm
              mới</a-button>
          </template>
          
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
                        <a-input v-model:value="formSearch.title_Filter" @keyup.enter="SearchData"
                          placeholder="Tiêu đề">
                        </a-input>
                      </a-col>
                      <a-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
                        <a-input v-model:value="formSearch.author_Filter" @keyup.enter="SearchData"
                          placeholder="Tên tác giả">
                        </a-input>
                      </a-col>
                    </a-row>
                    <a-row :gutter="24">

                      <a-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
                        <a-row :gutter="24" align="center" justify="space-around">
                          <a-col :xs="24" :sm="24" :md="24" :lg="8" :xl="8">

                            <a-select style="min-width: 100%" v-model:value="formSearch.status_Filter" :options="statusOptions" placeholder="Chọn trang thái" />

                              </a-col>
                            <a-col :xs="24" :sm="24" :md="24" :lg="16" :xl="16" style="justify-items: flex-end; display: grid;">
                              <a-range-picker style="width: 100%" :placeholder="['Từ ngày','Đến ngày']" v-model:value="formSearch.rangeTime_Filter" format="YYYY-MM-DD"
                              /></a-col>
                              
                        </a-row>
                       
                      </a-col>
                      <a-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12" >

                        <a-select :maxTagCount="2" showSearch="true" optionFilterProp="label" style="min-width: 100%" v-model:value="formSearch.category_Filter" mode="multiple"
                          :options="categoryOptions" placeholder="Chọn danh mục" />
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
                  <template v-if="column.key === 'thumbnail'">
                    <img v-if="record.thumbnailBase64"
                      :src="'data:' + record.thumbnailContentType + ';base64,' + record.thumbnailBase64"
                      style="max-width: 100px; max-height: 100px; object-fit: contain;" />
                    <a-tag color="yellow" v-else>Chọn cập nhật</a-tag>
                  </template>
                  <template v-if="column.key === 'publishDate'">
                    {{ formatCreatedDate(record.publishDate) }}
                  </template>
                  <template v-if="column.key === 'listCategoryName'">
                    <a-tag :color="randomColor()" style="margin-top: 5px;" v-for="item in record.listCategoryName" :key="item">{{ item }}</a-tag>
                  </template>
                  <template v-if="column.key === 'status'">
                    <a-tooltip v-if="record.status == true" title="Hiện" placement="right">
                      <font-awesome-icon :icon="['fas', 'check-circle']" :style="{ color: '#52c41a' }" /></a-tooltip>
                    <a-tooltip v-else title="Ẩn" placement="right"> <font-awesome-icon :icon="['fas', 'times-circle']"
                        :style="{ color: '#f5222d' }" /></a-tooltip>

                  </template>
                  <template v-if="column.key === 'action'">
                    <a-space>


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

</template>
<script>
  import dayjs from "dayjs";
  import {ref} from "vue";
  import { Pagination, message, notification } from "ant-design-vue";
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

    },

    data() {
      return {
        categoryOptions: [],
        statusOptions: [
            {
                value: null,
                label: "Tất cả"
            },
            {
                value: true,
                label: "Bài viêt hiển thị"
            },
            {
                value: false,
                label: "Bài viết ẩn"
            },

        ],
        tableColumns: [
          {
            title: "Trạng thái",
            dataIndex: "status",
            key: "status",
            width: "5%",
          },
          {
            title: "Thumbnail",
            dataIndex: "thumbnail",
            key: "thumbnail",
            width: "15%",
            sorter: false,

          },

          {
            title: "Tiêu đề",
            dataIndex: "title",
            key: "title",
            width: "15%",
            sorter: true,
            showSorterTooltip: false
          },
          {
            title: "Tóm tắt",
            dataIndex: "summary",
            key: "summary",
            width: "30%",
          },
          
          {
            title: "Danh mục",
            dataIndex: "listCategoryName",
            key: "listCategoryName",
            width: "20%",
          },
          {
            title: "Tác giả",
            dataIndex: "author",
            key: "author",
            width: "20%",
          },
          {
            title: "Ngày đăng",
            dataIndex: "publishDate",
            key: "publishDate",
            width: "20%",
            sorter: true,
            showSorterTooltip: false
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
          author_Filter: "",
          title_Filter: "",
          sortQuery: "",
          rangeTime_Filter: [],
          status_Filter: ref(null),
          category_Filter: [],
          startDate_Filter: "",
          endDate_Filter: "",
        },
      };
    },
  async  mounted() {
      const selectedMenu = inject("selectedMenu");
      const changeSelectedMenu = inject("changeSelectedMenu");
      if (this.$route.name === "ArticleHome") {
        changeSelectedMenu("Article");
      }
      const serverResponse = await APIService.get('datacategory/get-list-by-parent-code/BAI_VIET')
      this.categoryOptions = serverResponse.data.data.map(item => {
        return {
          label: item.name,
          value: item.id
        }
      })
      this.fetchData(this.pagination.current, this.pagination.pageSize);
    }, //end mounted
    methods: {
      randomColor(){
        const listColor  = ['green','purple','pink','blue','yellow','orange']
        console.log(listColor[Math.floor(Math.random()*listColor.length)])
        return listColor[Math.floor(Math.random()*listColor.length)]
      },
      formatCreatedDate(date) {
        return dayjs(date).format("HH:mm:ss DD/MM/YYYY ");
      },
      goBack() {
        this.$router.go(-1); // Điều hướng trở lại trang trước
      },
      openModalCreate() {
        this.$router.push({ name: "ArticleCreate" });
      },
      async deleteObj(id) {
        try {
          const response = await APIService.delete(`article/soft-delete/${id}`);
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
        this.formSearch.startDate_Filter=this.formSearch.rangeTime_Filter[0]
        this.formSearch.endDate_Filter=this.formSearch.rangeTime_Filter[1]
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
            "article/get-data-by-page",
            searchParam
          );
          this.dataSourceTable = response.data.data.items;
          this.pagination.total = response.data.data.totalCount;
          this.pagination.current = response.data.data.pageIndex;
          this.pagination.pageSize = response.data.data.pageSize;
        } catch (error) {
          notification.error({
            message: "Thất bại",
            description: error,
          })
        }
        this.loadingTable = false;
      }, //end fetchData
      openModalUpdate(id) {
        this.$router.push({ name: "ArticleUpdate", params: { id: id } });
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