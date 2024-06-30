<template>
  <a-spin :spinning="isSynchronizing">
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
            <span style="color: white; font-size: 16px; margin-bottom: auto;display: block;">Quản lý sản phẩm</span>
          </template>
          <template #extra>
            <a-breadcrumb separator=">" style="margin-right:30px;">
              <a-breadcrumb-item href="">
                <font-awesome-icon icon="fa-solid fa-house" style="color: #fff;" />
                <span style="color: #fff;"> Trang quản trị</span>
              </a-breadcrumb-item>
              <a-breadcrumb-item href="">
                <font-awesome-icon :icon="['fas', 'shirt']" style="color: #fff;" />
                <span style="color: #fff;"> Sản phẩm</span>
              </a-breadcrumb-item>

            </a-breadcrumb>
          </template>
        </a-page-header>
      </a-col>
    </a-row>
    <transition name="route" mode="out-in" appear>
      <a-row>
        <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
          <a-card :bordered="true" title="Danh sách sản phẩm" style="margin: 30px;">
            <template #extra>


              <a-popconfirm placement="bottomLeft" title="Đồng bộ ảnh sẽ tốn nhiều thời gian đó" ok-text="Đồng bộ ngay"
                cancel-text="Để sau" @confirm="handleConfigSearchImage">
                <a-button class="btnSearch" style="margin-right: 10px" type="dash">
                  <font-awesome-icon :icon="['fas', 'arrows-rotate']" />
                </a-button>
              </a-popconfirm>
              <a-button @click="openModalCreate" class="btnSearch" type="primary"><font-awesome-icon
                  icon="fa-solid fa-plus" /> Thêm
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

                      <a-col :span="12">
                        <a-input v-model:value="formSearch.code_Filter" @keyup.enter="SearchData"
                          placeholder="Mã sản phẩm">
                        </a-input>
                      </a-col>
                      <a-col :span="12">
                        <a-input v-model:value="formSearch.name_Filter" @keyup.enter="SearchData"
                          placeholder="Tên sản phẩm">
                        </a-input>
                      </a-col>
                    </a-row>
                    <a-row :gutter="24">

                      <a-col :span="12">
                        <span>Chọn khoảng giá</span>
                        <a-slider style="max-width: 70%" v-model:value="formSearch.price_Filter" range :min="0"
                          :max="1000000" placeholder="Chọn khoảng giá" />
                      </a-col>
                      <a-col :span="12">

                        <a-select showSearch="true" optionFilterProp="label" style="min-width: 100%" v-model:value="formSearch.category_Filter" mode="multiple"
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
                <a-table :columns="tableColumns" :dataSource="dataSourceTable" :pagination="false"
                  :loading="loadingTable" :scroll="{ x: 1000 }"
                  :row-class-name="(_record, index) => (index % 2 === 1 ? 'table-striped' : null)"
                  @change="handleTableChange" bordered>
                  <template #bodyCell="{ column, record }">
                    <template v-if="column.key === 'categories'">
                      <a-tag v-for="item in record.listCategoryName" :key="item.id" color="default">{{ item }}</a-tag>
                    </template>


                    <template v-if="column.key === 'action'">
                      <a-space>
                        <a-tooltip title="Cấu hình mẫu mã" placement="leftTop">
                          <a-button type="link" shape="circle" @click="openModalVariant(record.id)">

                            <font-awesome-icon icon="fa-solid fa-wand-magic-sparkles" style="color: #033996;" />

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
                    <template v-else-if="column.key === 'thumbnail'">
                      <img v-if="record.thumbnailBase64"
                        :src="'data:' + record.thumbnailContentType + ';base64,' + record.thumbnailBase64"
                        style="max-width: 100px; max-height: 100px; object-fit: contain;" />
                      <a-tag color="yellow" v-else>Chọn cập nhật</a-tag>
                    </template>
                    <template v-if="column.key === 'price'">
                      <span style="color: red">{{ fomartPrice(record.price) }}&#8363;</span>
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
    <ModalVariant @addSuccess="fetchData(pagination.current, pagination.pageSize)" ref="modalVariant" />
    <ModalUpdate @updateSuccess="fetchData(pagination.current, pagination.pageSize)" ref="modalUpdate" />
  </a-spin>
</template>
<script>
  import weaviate from 'weaviate-ts-client';
  import ModalVariant from "@/views/Administrator/Products/ModalVariant.vue";
  import ModalUpdate from "@/views/Administrator/Products/ModalUpdate.vue";
  import ModalCreate from "@/views/Administrator/Products/ModalCreate.vue";
  import { Modal, Pagination, message, notification } from "ant-design-vue";
  import {
    SmileOutlined,
    DownOutlined,
    CompassOutlined,
  } from "@ant-design/icons-vue";
  import APIService from "@/helpers/APIService";
  import { inject, ref } from "vue";


  export default {
    components: {
      SmileOutlined,
      DownOutlined,
      Pagination,
      ModalCreate,
      ModalVariant,
      ModalUpdate

    },
    data() {
      return {
        isSynchronizing: false,
        categoryOptions: [],
        tableColumns: [
          {
            title: "Hình ảnh",
            dataIndex: "thumbnail",
            key: "thumbnail",
            width: "20%",
          },
          {
            title: "Mã sản phẩm",
            dataIndex: "code",
            key: "code",
            width: "15%",
            sorter: true,
            showSorterTooltip: false,
          },
          {
            title: "Tên sản phẩm",
            dataIndex: "name",
            key: "name",
            width: "15%",
            sorter: true,
            showSorterTooltip: false,
          },
          {
            title: "Giá tiền",
            dataIndex: "price",
            key: "price",
            width: "10%",
            sorter: true,
            showSorterTooltip: false,
          },
          {
            title: "Số lượng",
            dataIndex: "stockQuantity",
            key: "stockQuantity",
            width: "10%",
            sorter: true,
            showSorterTooltip: false,
          },
          {
            title: "Danh mục",
           
            key: "categories",
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
          price_Filter: [0, 1000000],
          category_Filter: [],
          sortQuery: "",
        },

      };
    },
    async mounted() {
      const selectedMenu = inject("selectedMenu");
      const changeSelectedMenu = inject("changeSelectedMenu");
      if (this.$route.name === "ProductHome") {
        changeSelectedMenu("Product");
      }
      const serverResponse = await APIService.get('datacategory/get-list-by-parent-code/SAN_PHAM')
      this.categoryOptions = serverResponse.data.data.map(item => {
        return {
          label: item.name,
          value: item.id
        }
      })
      this.fetchData(this.pagination.current, this.pagination.pageSize);
    }, //end mounted
    methods: {
      async handleConfigSearchImage() {
        this.isSynchronizing = true
        // const client = weaviate.client({
        //   scheme: 'http',
        //   host: 'localhost:8080'
        // });
        //đay là cấu hình dev
        const client = weaviate.client({
          scheme: 'http',
          host: 'localhost:8081/weaviate'
        });
        //Đây là cấu hình docker


        try {
          const schemaRes = await client.schema.getter().do();

          const classExists = schemaRes.classes.some(cls => cls.class === 'ProductImage');
          const schemaConfig = {
            class: 'ProductImage',
            vectorizer: 'img2vec-neural',
            vectorIndexType: 'hnsw',
            moduleConfig: {
              'img2vec-neural': {
                imageFields: ['image']
              },

            },
            properties: [
              { name: 'image', dataType: ['blob'] },
              { name: 'path', dataType: ['string'] }
            ]
          };
          if (!classExists) {
            await client.schema.classCreator().withClass(schemaConfig).do();

          } else {
            await client.schema.classDeleter().withClassName('ProductImage').do();
            await client.schema.classCreator().withClass(schemaConfig).do();
          }
          const response = await APIService.post('imagesearch/reload-image-to-model', {});

          if (response.data == "Đã load thành công dữ liệu.") {
            notification.success({
              message: 'Thành công',
              description: 'Đã load xong dữ liệu',
              duration: 1
            })
          }
        } catch (error) {
          notification.error({
            message: 'Không thể hành động',
            description: 'Cấu hình schema không thể thay đổi',
            duration: 1
          })
        } finally {
          this.isSynchronizing = false

        }

      },
      fomartPrice(price) {
        return price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.');
      },
      openCollapseSearch() {
        this.$refs.searchCollapse.click;
        console.log(this.$refs);
      },
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
            notification.success({
              message: "Thất bại",
              description: response.data.message,
            });
          }
        } catch (error) {
          notification.error({
            message: "Lỗi",
            description: error,
          });
        }
      },
      handlePaginationChange() {
        // this.fetchData(this.pagination.current, this.pagination.pageSize);
        this.SearchData()
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
          sortQuery: this.formSearch.sortQuery,
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
          notification.error({
            message: "Lỗi",
            description: error,
          });
        }
        this.loadingTable = false;
      }, //end fetchData
      openModalUpdate(id) {
        this.$refs.modalUpdate.showModal(id);
      },
      openModalVariant(id) {
        this.$refs.modalVariant.showModal(id);
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