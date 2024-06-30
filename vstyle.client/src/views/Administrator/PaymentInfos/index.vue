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
                    <span style="color: white; font-size: 16px; margin-bottom: auto;display: block;">Lịch sử thanh
                        toán</span>
                </template>
                <template #extra>
                    <a-breadcrumb separator=">" style="margin-right:30px;">
                        <a-breadcrumb-item href="">
                            <font-awesome-icon icon="fa-solid fa-house" style="color: #fff;" />
                            <span style="color: #fff;"> Trang quản trị</span>
                        </a-breadcrumb-item>
                        <a-breadcrumb-item href="">

                            <font-awesome-icon icon="fa-brands fa-cc-mastercard" style="color: #fff;" />
                            <span style="color: #fff;"> Thanh toán</span>
                        </a-breadcrumb-item>


                    </a-breadcrumb>
                </template>
            </a-page-header>
        </a-col>
    </a-row>
    <transition name="route" mode="out-in" appear>
        <a-row>
            <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
                <a-card :bordered="true" title="Danh sách thông tin thanh toán" style="margin: 30px;">
                    <template #extra>
                        <a-range-picker style="width: 100%" @change="onChangeRangeTime"
                            :placeholder="['Từ ngày', 'Đến ngày']" v-model:value="formSearch.rangeTime_Filter"
                            format="YYYY-MM-DD" />
                    </template>

                    <a-row>
                        <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
                            <a-table bordered :columns="tableColumns" :dataSource="dataSourceTable" :pagination="false"
                                :loading="loadingTable" :scroll="{ x: 1000 }"
                                :row-class-name="(_record, index) => (index % 2 === 1 ? 'table-striped' : null)"
                                @change="handleTableChange">
                                <template #bodyCell="{ column, record }">
                                    <template v-if="column.key == 'orderCode'">
                                        <router-link style="color: #c21f24" :to="{
                                            name: 'DetailOrder',
                                            params: { id: record.orderId },
                                        }">
                                            {{ record.orderCode }}</router-link>
                                    </template>
                                    <template v-if="column.key === 'paymentDate'">
                                        {{ formatCreatedDate(record.paymentDate) }}
                                    </template>
                                    <template v-if="column.key === 'paymentMethodId'">
                                        {{ getPaymentName(record.paymentMethodId) }}
                                    </template>
                                    <template v-if="column.key == 'totalAmount'">
                                        <span>{{ fomartPrice(record.totalAmount) }}</span>
                                    </template>
                                    <template v-if="column.key === 'paymentStatusId'">
                                        <a-tag :color="getColor(getPaymentStatusName(record.paymentStatusId))">{{
                                            getPaymentStatusName(record.paymentStatusId) }} </a-tag>
                                    </template>

                                </template>
                            </a-table>
                        </a-col>
                    </a-row>
                    <a-row>
                        <a-col :span="24">
                            <a-pagination v-if="pagination.total > 0" style="margin-top: 20px; text-align: center"
                                v-model:current="pagination.current" :pageSize="pagination.pageSize"
                                :total="pagination.total" :showSizeChanger="false"
                                :show-total="(total) => `Tổng ${total} bản ghi`" @change="handlePaginationChange" />
                        </a-col>
                    </a-row>
                </a-card>
            </a-col>
        </a-row>
    </transition>

</template>
<script>
    import dayjs from "dayjs";
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

        },

        data() {
            return {
                tableColumns: [
                    {
                        title: "Mã đơn hàng",
                        dataIndex: "orderCode",
                        key: "orderCode",
                        width: "15%",
                        sorter: true,
                        showSorterTooltip: false
                    },
                    {
                        title: "Tên khách hàng",
                        dataIndex: "customerName",
                        key: "customerName",
                        width: "15%",
                        sorter: true,
                        showSorterTooltip: false
                    },
                    {
                        title: "Phương thức thanh toán",
                        dataIndex: "paymentMethodId",
                        key: "paymentMethodId",
                        width: "30%",
                    },
                    {
                        title: "Trạng thái thanh toán",
                        dataIndex: "paymentStatusId",
                        key: "paymentStatusId",
                        width: "30%",
                    },
                    {
                        title: "Tổng tiền",
                        dataIndex: "totalAmount",
                        key: "totalAmount",
                        width: "30%",
                    },
                    {
                        title: "Thời gian",
                        dataIndex: "paymentDate",
                        key: "paymentDate",
                        width: "30%",
                    },




                ],
                listPaymentMethod: [],
                listPaymentStatus: [],
                dataSourceTable: [],
                loadingTable: false,
                pagination: {
                    current: 1,
                    pageSize: 10,
                    total: 0,
                },
                formSearch: {
                    rangeTime_Filter: [],
                    sortQuery: "",
                },
            };
        },
        async mounted() {
            const selectedMenu = inject("selectedMenu");
            const changeSelectedMenu = inject("changeSelectedMenu");
            if (this.$route.name === "PaymentInfoHome") {
                changeSelectedMenu("PaymentInfo");
            }
            this.fetchData(this.pagination.current, this.pagination.pageSize);
            await this.fetchPaymentMethod();
            await this.fetchPaymentStatus();
        }, //end mounted
        methods: {
            onChangeRangeTime() {
                this.formSearch.startDate_Filter = this.formSearch.rangeTime_Filter[0],
                    this.formSearch.endDate_Filter = this.formSearch.rangeTime_Filter[1]
                this.fetchData(1, this.pagination.pageSize, this.formSearch);
            },
            fomartPrice(price) {
                return price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.');
            },
            getColor(statusName) {
                if (statusName != null) {
                    if (statusName.includes("Chờ")) {
                        return "yellow"
                    }
                    if (statusName.includes("hủy")) {
                        return "red"
                    }
                    return "green"
                }

            },
            formatCreatedDate(date) {
                return dayjs(date).format("HH:mm:ss DD/MM/YYYY ");
            },
            async fetchPaymentMethod() {
                const res = await APIService.get("datacategory/get-list-by-parent-code/PAYMENT_METHOD");
                this.listPaymentMethod = res.data.data;
            },
            async fetchPaymentStatus() {
                const res = await APIService.get("datacategory/get-list-by-parent-code/PAYMENT_STATUS");
                this.listPaymentStatus = res.data.data;
            },
            getPaymentName(id) {

                const item = this.listPaymentMethod.find(item => item.id === id)
                if (item) {
                    return item.name
                }
                return null
            },
            getPaymentStatusName(id) {

                const item = this.listPaymentStatus.find(item => item.id === id)
                if (item) {
                    return item.name
                }
                return null
            },
            goBack() {
                this.$router.go(-1); // Điều hướng trở lại trang trước
            },
            openModalCreate() {
                this.$refs.modalCreate.showModal();
            },

            handlePaginationChange() {
                this.formSearch.startDate_Filter = this.formSearch.rangeTime_Filter[0],
                    this.formSearch.endDate_Filter = this.formSearch.rangeTime_Filter[1]
                this.fetchData(this.pagination.current, this.pagination.pageSize, this.formSearch);
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
                        "paymentinfo/get-data-by-page",
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