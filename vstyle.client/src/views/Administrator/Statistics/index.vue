<template>
  <div style="min-height: 100vh">
    <transition name="route" mode="out-in" appear>
      <a-row style="justify-content: space-evenly;">
        <a-col :xs="20" :md="10" :lg="5" :xl="5" style="margin: 15px;">
          <a-card :loading="isLoadingCard" style="box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1), 0 5px 10px rgba(0, 0, 0, 0.08);">
            <a-row :gutter="20" justify="space-around">
              <a-col :span="6"
                style="display: flex; align-items: center; justify-content: center;background: #ffc107;border-radius: 10px;">
                <ClockCircleOutlined style="color: #1f2d3d; font-size: 50px;object-fit: contain" />
              </a-col>
              <a-col :span="18">
                <a-statistic :value="this.countNumberOrderWaitingConfirm" :precision="0"
                  :value-style="{ color: '#db3027' }" style="margin-right: 50px">
                  <template #title>
                    <span style="font-size: 18px; font-weight: bold; color: #1f2d3d">Đợi duyệt</span>
                  </template>
                </a-statistic>
              </a-col>
            </a-row>

          </a-card>
        </a-col>
        <a-col :xs="20" :md="10" :lg="5" :xl="5" style="margin: 15px;">
          <a-card :loading="isLoadingCard" style="box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1), 0 5px 10px rgba(0, 0, 0, 0.08);">
            <a-row :gutter="20" justify="space-around">
              <a-col :span="6"
                style="display: flex; align-items: center; justify-content: center; background: #28a645;border-radius: 10px;">

                <ShoppingOutlined style="color: #1f2d3d; font-size: 50px" />
              </a-col>
              <a-col :span="18">
                <a-statistic :value="this.countNumberOrderToday" :precision="0" :value-style="{ color: '#db3027' }"
                  style="margin-right: 50px">
                  <template #title>
                    <span style="font-size: 18px; font-weight: bold; color: #1f2d3d">Đơn hàng</span>
                  </template>
                </a-statistic>
              </a-col>
            </a-row>

          </a-card>
        </a-col>

        <a-col :xs="20" :md="10" :lg="5" :xl="5" style="margin: 15px;">
          <a-card :loading="isLoadingCard" style="box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1), 0 5px 10px rgba(0, 0, 0, 0.08);">
            <a-row :gutter="20" justify="space-around">
              <a-col :span="6"
                style="display: flex; align-items: center; justify-content: center; background: #16a2b8;border-radius: 10px;">

                <DollarCircleOutlined style="color: #1f2d3d; font-size: 50px" />
              </a-col>
              <a-col :span="18">
                <a-statistic :value="this.totalRevenueToday" :precision="0" :value-style="{ color: '#db3027' }"
                  style="margin-right: 50px">
                  <template #title>
                    <span style="font-size: 18px; font-weight: bold; color: #1f2d3d">Doanh thu</span>
                  </template>
                </a-statistic>
              </a-col>
            </a-row>

          </a-card>
        </a-col>

        <a-col :xs="20" :md="10" :lg="5" :xl="5" style="margin: 15px;">
          <a-card :loading="isLoadingCard" style="box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1), 0 5px 10px rgba(0, 0, 0, 0.08);">
            <a-row :gutter="20" justify="space-around">
              <a-col :span="6"
                style="display: flex; align-items: center; justify-content: center; background: #f2bbc1;border-radius: 10px;">

                <UserAddOutlined style="color: #1f2d3d; font-size: 50px;object-fit: contain" />
              </a-col>
              <a-col :span="18">
                <a-statistic :value="this.countUserRegisterToday" :precision="0" :value-style="{ color: '#db3027' }"
                  style="margin-right: 50px">
                  <template #title>
                    <span style="font-size: 18px; font-weight: bold; color: #1f2d3d">Người mới</span>
                  </template>
                  <template #suffix>
                    <span style="font-size: 18px; font-weight: bold; color: #db3027">| {{ this.totalUser }}</span>
                  </template>
                </a-statistic>
              </a-col>
            </a-row>

          </a-card>
        </a-col>





      </a-row>
    </transition>


    <transition name="route" mode="in-out" appear>
      <a-row :gutter="20" style="margin: 20px;">
        <a-col :xs="24" :sm="24" :md="24" :lg="16" :xl="16">
          <a-card :loading="isLoadingMixChart">
            <template #title>Doanh thu và đơn hàng</template>
            <template #extra>
              <a-range-picker :placeholder="['Ngày bắt đầu','Ngày kết thúc']" v-model:value="rangeTimeMixChart" format="YYYY-MM-DD"
              @change="handleChangeRangeTimeMixChart" />
            </template>  
            <apexchart ref="chartRef" type="line" height="350" :options="mixChartOptions" :series="mixSeries">
            </apexchart>

          </a-card>
        </a-col>
        <a-col :xs="24" :sm="24" :md="24" :lg="8" :xl="8">
          <a-card :loading="isLoadingDonutChart">
           
            <template #extra>
              <a-range-picker :placeholder="['Ngày bắt đầu','Ngày kết thúc']" v-model:value="rangeTimeDonutChart" format="YYYY-MM-DD"
              @change="handleChangeRangeTimeDonutChart" />
            </template>  
            <a-result status="warning" title="Không có dữ liệu" v-if="this.isNoDataDonutChart"></a-result> 
            <apexchart v-else type="donut" ref="donutRef" :options="donutChartOptions" :series="donutSeries"></apexchart>
            
          </a-card>
        </a-col>
      </a-row>
    </transition>

    <!-- Table sản phẩm bán chạy -->
    <transition name="route" mode="in-out" appear>
      <a-row justify="center">
        <a-col :xs="22" :sm="22" :md="16" :lg="16" :xl="16" >
          <a-card  title="Top sản phẩm bán chạy">
            <template #extra>
              <a-range-picker :placeholder="['Ngày bắt đầu','Ngày kết thúc']" v-model:value="rangeTimeTopSelling" format="YYYY-MM-DD"
              @change="handleChangeRangeTimeTopSelling" />
            </template>  
            <a-table :columns="columns" :data-source="dataTableTopSelling" :pagination="false" :row-key="record => record.id"
              :scroll="{ x: 800, y: 500 }">
              <template #bodyCell="{ column, record }">
                <template v-if="column.dataIndex === 'thumbnail'">
                  <img v-if="record.thumbnailBase64" :src="'data:' + record.thumbnailContentType + ';base64,' + record.thumbnailBase64"
                    style="max-width: 100px; max-height: 100px; object-fit: contain;" />
                  <a-tag color="red" v-else>Chưa cập nhật</a-tag>
                
                  </template> 
                <template v-if="column.dataIndex === 'productPrice'">
                  <span style="color: red">{{ fomartPrice(record.productPrice) }}&#8363;</span>
                  </template> 
                </template> 
            </a-table>
          </a-card>
        </a-col>
      </a-row>

    </transition>
    <!-- End table sản phẩm bán chạy -->
  </div>
</template>
<script>
  import dayjs from 'dayjs';
  import APIService from '@/helpers/APIService';
  import { ArrowUpOutlined, ArrowDownOutlined, DollarCircleOutlined, ClockCircleOutlined, ShoppingOutlined, UserAddOutlined } from '@ant-design/icons-vue';
  import { defineComponent, ref } from 'vue';
  import VueApexCharts from 'vue-apexcharts';
  export default defineComponent({
    components: {
      ArrowUpOutlined,
      ArrowDownOutlined,
      DollarCircleOutlined,
      ClockCircleOutlined,
      ShoppingOutlined,
      UserAddOutlined
    },
    data() {
      return {

        isNoDataDonutChart: false,
        isLoadingCard: false,
        isLoadingDonutChart: false,
        isLoadingMixChart: false,
        rangeTimeMixChart:[],
        rangeTimeDonutChart:[],
        rangeTimeTopSelling:[],
        countNumberOrderToday: 0,
        countNumberOrderYesterday: 0,
        countNumberOrderWaitingConfirm: 0,
        totalRevenueToday: 0,
        countUserRegisterToday: 0,
        totalUser: 0,
        calculateGrowthRate: (a, b) => {
          return (a - b) / b * 100
        },
        donutSeries: [],
        donutChartOptions: {
          chart: {
            type: 'donut',
          },
          title: {
            text: 'Doanh thu theo giới tính',
            align: 'left',
            position: 'bottom', // Đặt tiêu đề ở dưới cùng
            margin: 10,
            offsetX: 0,
            offsetY: 0,
            floating: false,
            style: {
              fontSize: '16px',
              fontWeight: 'bold',
              fontFamily: 'Archirvo, Helvetica, Arial, sans-serif',
              color: '#263238'
            },
          },
          labels: [],
          responsive: [{
            breakpoint: 480,
            options: {
              chart: {
                width: 300
              },
              legend: {
                position: 'bottom'
              }
            }
          }]
        },

        mixSeries: [{
          name: 'Doanh thu',
          type: 'column',
          data: []
        }, {
          name: 'Số đơn hàng',
          type: 'line',
          data: []
        }],
        mixChartOptions: {
          colors: ['#00215E', '#FC4100'],
          responsive: [
            {
              breakpoint: 480, // Kích thước màn hình tối đa
              options: {
                chart: {
                  width: 300 // Chiều rộng mới của biểu đồ
                },
                legend: {
                  position: 'bottom' // Vị trí mới của chú giải
                }
              }
            }
          ],
          chart: {
            height: 400,
            type: 'line',
            toolbar: {
              tools: {
                download: true,
                selection: true,
                zoom: true,
                zoomin: true,
                zoomout: true,
                pan: true,
                reset: true
              },

            }
          },
          stroke: {
            width: [0, 4]
          },
          
          
          dataLabels: {
            enabled: true,
            enabledOnSeries: [1]
          },
          labels: [],
          xaxis: {
            type: 'category' // Quan trọng!
          },
          yaxis: [
            {
              title: {
                text: 'Doanh thu',
              },

            },
            {
              opposite: true,
              title: {
                text: 'Số đơn hàng'
              }
            }]
        },
        dataTableTopSelling: [],
        columns: [
          {
            title: 'Hình ảnh',
            dataIndex: 'thumbnail',
            key: 'thumbnail',
            width: '15%',
          },
          {
            title: 'Tên sản phẩm',
            dataIndex: 'productName',
            key: 'productName',
            width: '30%',
          },
          {
            title: 'Giá tiền',
            dataIndex: 'productPrice',
            key: 'productPrice',
            width: '10%',
          },
          {
            title: 'Số lượng đã bán',
            dataIndex: 'totalQuantitySold',
            key: 'totalQuantitySold',
            width: '10%',
          },

        ]

      };
    },
    async mounted() {
    this.isLoadingCard = true;

    try {
        await Promise.all([
            this.fetchCountNumberOrderToday(),
            this.fetchCountNumberOrderYesterday(),
            this.fetchCountNumberOrderWaitingConfirm(),
            this.fetchRevenueToday(),
            this.fetchCountUser(),
            this.fetchRevenueForMixChart(),
            this.fetchRevenueForDonutChart(),
            this.fetchDataTableTopSelling()
        ]);
    } catch (error) {
        console.error('Error fetching data:', error);
    }

    this.isLoadingCard = false;
},

    methods: {
     async handleChangeRangeTimeTopSelling(){
      try{
          const response = await APIService.get(`order/get-top-product-selling-day-to-day/${this.rangeTimeTopSelling[0]}/${this.rangeTimeTopSelling[1]}`);
          this.dataTableTopSelling = response.data.data
        }catch(error){
          console.log(error);
        }
      },
      async handleChangeRangeTimeDonutChart(){
        this.isLoadingDonutChart= true
        try {
          const response = await APIService.get(`order/get-revenue-of-category-day-to-day/${this.rangeTimeDonutChart[0]}/${this.rangeTimeDonutChart[1]}`);

          this.donutSeries = response.data.data.map(item => {
            return item.revenue
          })
          if(this.donutSeries.length > 0){
            this.isNoDataDonutChart = false
          }
          else{
            this.isNoDataDonutChart = true
          }
          this.donutChartOptions.labels = response.data.data.map(item => {
            return item.categoryName.toString()
          })
        
          this.$refs.donutRef.updateOptions({
            labels: this.donutChartOptions.labels
          });
          
        } catch (error) {
          console.log(error);
        }finally{
          this.isLoadingDonutChart = false
        }
      },
     async handleChangeRangeTimeMixChart(){
        this.isLoadingMixChart= true
      try {
          const response = await APIService.get(`order/get-revenue-day-to-day/${this.rangeTimeMixChart[0]}/${this.rangeTimeMixChart[1]}`);
          this.mixChartOptions.labels = response.data.data.map(item => {
            return dayjs(item.label_Day).format('DD-MM-YYYY')
          })
          this.mixSeries[0].data = response.data.data.map(item => {
            return item.revenue
          })
          this.mixSeries[1].data = response.data.data.map(item => {
            return item.countOrder
          })

          this.$refs.chartRef.updateOptions({
            labels: this.mixChartOptions.labels
          });
        } catch (error) {
          console.log(error);
        }finally{
          this.isLoadingMixChart = false
        }
      },
      async fetchCountNumberOrderToday() {
        try {
          const response = await APIService.get('order/count-order-today');
          this.countNumberOrderToday = response.data.data;
        } catch (error) {
          console.log(error);
        }
      },
      async fetchCountNumberOrderYesterday() {
        try {
          const response = await APIService.get('order/count-order-yesterday');
          this.countNumberOrderYesterday = response.data.data;
        } catch (error) {
          console.log(error);
        }
      },
      async fetchCountNumberOrderWaitingConfirm() {
        try {
          const response = await APIService.get('order/count-order-waiting-confirm');
          this.countNumberOrderWaitingConfirm = response.data.data;
        } catch (error) {
          console.log(error);
        }
      },
      async fetchRevenueToday() {
        try {
          const response = await APIService.get('order/get-revenue-today');
          this.totalRevenueToday = response.data.data;
        } catch (error) {
          console.log(error);
        }
      },
      async fetchCountUser() {
        try {
          const response1 = await APIService.get('account/count-user-regis-today');
          this.countUserRegisterToday = response1.data.data;
          const response2 = await APIService.get('account/count-total-user');
          this.totalUser = response2.data.data;
        } catch (error) {
          console.log(error);
        }
      },
      async fetchRevenueForMixChart() {
        this.isLoadingMixChart =true
        try {
          const response = await APIService.get('order/get-revenue-of-month');
          this.mixChartOptions.labels = response.data.data.map(item => {
            return dayjs(item.label_Day).format('DD-MM-YYYY')
          })
          this.mixSeries[0].data = response.data.data.map(item => {
            return item.revenue
          })
          this.mixSeries[1].data = response.data.data.map(item => {
            return item.countOrder
          })

          this.$refs.chartRef.updateOptions({
            labels: this.mixChartOptions.labels
          });



        } catch (error) {
          console.log(error);
        }finally{
          this.isLoadingMixChart = false
        }
      },

      async fetchRevenueForDonutChart() {
        this.isLoadingDonutChart = true
        try {
          const response = await APIService.get('order/get-revenue-of-category');

          this.donutSeries = response.data.data.map(item => {
            return item.revenue
          })
          this.donutChartOptions.labels = response.data.data.map(item => {
            return item.categoryName.toString()
          })
          if(this.donutSeries.length > 0){
            this.isNoDataDonutChart = false
          }
          else{
            this.isNoDataDonutChart = true
          }
          this.$refs.donutRef.updateOptions({
            labels: this.donutChartOptions.labels
          });

        } catch (error) {
          console.log(error);
        }finally{
          this.isLoadingDonutChart = false
        }
      },

      async fetchDataTableTopSelling(){
        try{
          const response = await APIService.get('order/get-top-product-selling');
          this.dataTableTopSelling = response.data.data
        }catch(error){
          console.log(error);
        }
      },
      fomartPrice(price) {
                return price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.');
      },




    },//end methods





  });


</script>