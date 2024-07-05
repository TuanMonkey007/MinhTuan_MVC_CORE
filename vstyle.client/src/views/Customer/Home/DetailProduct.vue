<template>
    <a-row>
        <a-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <a-page-header style="
         border: 1px solid rgb(235, 237, 240);
          background-color: #c22026;
          height: fit-content;
        ">

                <template #title>
                    <a-breadcrumb separator=">" style="margin-left:30px;">
                        <a-breadcrumb-item href="">

                            <span style="color: #fff;"> Trang chủ</span>
                        </a-breadcrumb-item>

                        <a-breadcrumb-item class="active">

                            <span style="color: #fff;"> {{ this.productInfo.name }}</span>
                        </a-breadcrumb-item>

                    </a-breadcrumb>
                </template>

            </a-page-header>
        </a-col>
    </a-row>
    <div v-if="isLoading">
        <a-skeleton :loading="isLoading" :paragraph="{ rows: 20 }" />
    </div>
    <a-row v-if="!isLoading" style="align-items: top" :gutter="20" justify="center">
        <a-col :xs="0" :sm="0" :md="0" :lg="3" :xl="3" :xxl="3" style="margin-left: 30px">
            <swiper v-if="mainImages.length > 0" @swiper="setThumbsSwiper" :slidesPerView="4" :loop="true"
                :watchSlidesProgress="true" :modules="modules" class="mySwiper" direction="vertical" freeMode="true">
                <swiper-slide v-for="image in mainImages" :key="image.id"
                    style="height:auto;width: 60%;">
                    <a-image :src="'data:' + image.contentType + ';base64,' + image.base64" :alt="image.path"
                        style="border-radius: 5%;object-fit: contain;" :preview="false" />
                </swiper-slide>
            </swiper>
        </a-col>
        <a-col :xs="24" :sm="24" :md="24" :lg="8" :xl="8" :xxl="8">

            <swiper v-if="mainImages.length > 0" :loop="false" :navigation="true" :thumbs="{ swiper: thumbsSwiper }"
                :spaceBetween="10" :modules="modules" class="mySwiper2" :autoplay="{
                    delay: 2500,
                    disableOnInteraction: false,
                }" style="border-radius: 5%">
                <swiper-slide v-for="image in mainImages" :key="image.id">
                    <a-image :src="'data:' + image.contentType + ';base64,' + image.base64" :alt="image.path"
                        style="border-radius: 5%;object-fit: contain; height: 100%;" :preview="false" />
                </swiper-slide>
            </swiper>

        </a-col>

        <a-col :xs="24" :sm="24" :md="24" :lg="8" :xl="8" :xxl="8">
            <a-row style="border: 0.5px solid;border-radius: 10px;padding: 30px;margin: 10px">
                <a-col :span="24">
                    <h3 style="font-size: 24px; font-weight: 600;">{{ this.productInfo.name }}</h3>
                    <h4
                        style="font-size: 14px;font-weight: 400;line-height: 22px;letter-spacing: 0;text-align: left;color: #807878;">
                        Mã sản phẩm: {{ this.productInfo.code }}</h4>
                    <p class="price-product" style="font-size: 24px; font-weight: 600; line-height: 36px;"> {{
                        fomartPrice(this.productPriceChange) }}&#8363;</p>

                </a-col>

                <a-col :span="24">
                    <h4>Chọn màu: <span style="font-weight: 500;"> {{ getSelectedColorLabel() }}</span></h4>

                    <a-radio-group v-model:value="selectedColor" :options="productColorOptions" button-style="solid"
                        optionType="button" @change="handleColorChange">
                    </a-radio-group>
                </a-col>
                <a-col :span="24">
                   
                      
                            <h4>Chọn size: <span style="font-weight: 500;"> {{ getSelectedSizeLabel() }}
                                </span></h4>

                            <a-radio-group v-model:value="selectedSize" :options="productSizeOptions"
                                button-style="solid" optionType="button" @change="handleSizeChange">
                            </a-radio-group>
                            <a-tooltip color="#de2e21"  :title="'Chọn lại'" placement="right">
                            <a-button style="margin-left: 10px" @click="handleResetSizeColor"> <font-awesome-icon
                                    :icon="['fas', 'arrows-rotate']" /></a-button>
                                    </a-tooltip>    
                </a-col>
                <a-col :span="24">
                    <a-row :gutter="[16,16]">
                <a-col :span="12" style="margin-top: 10px; margin-bottom: -10px">
                    <span>Số lượng: <span style="font-weight: 500;"> {{ this.quantity }} </span></span>
                </a-col>
                <a-col :span="12" style="display: flex; align-items: flex-end;">
                            
                        </a-col>
                <a-col :xs="24" :sm="24" :md="5" :lg="5" :xl="5">
                    <a-input-number v-model:value="quantity" min="1" max="10" size="small" :controls="false"
                        class="inputQuantity">
                        <template #addonBefore>
                            <a-button size="small" type="text" @click="decreaseQuantity">-</a-button>
                        </template>
                        <template #addonAfter>
                            <a-button size="small" type="text" @click="increaseQuantity">+</a-button>
                        </template>
                    </a-input-number>
                </a-col>

            </a-row>
                    </a-col>    
            </a-row>
          
            <a-row justify="center" :gutter="[16, 16]" style="margin-top: 20px;">
            
                <a-col :xs="12" :sm="12" :md="12" :lg="12" :xl="12">
                    <a-button @click="handleAddToCart" class="btn-add-to-cart">
                        <font-awesome-icon icon="fa-solid fa-cart-plus" style="color: #ad2b1f;" />
                    </a-button>
                </a-col>
                <a-col :xs="12" :sm="12" :md="12" :lg="12" :xl="12">
                    <a-button type="primary" @click="handleBuyNow" class="btn-buy-now">Mua ngay</a-button>
                </a-col>
            </a-row>
            <a-row justify="flex-start" :gutter="[16, 16]" style="margin-top: 20px;">
                <a-col :span="8">
                    <a-row>
                        <a-col :xs="12" :sm="12" :md="6" :lg="6" :xl="6">
                            <svg width="34" height="34" viewBox="0 0 34 34" fill="none">
                                <path
                                    d="M15.8847 2.7252C16.3896 1.88349 17.6095 1.88349 18.1144 2.7252L22.0161 9.22969C22.1978 9.53269 22.4951 9.74865 22.8394 9.82787L30.2312 11.5286C31.1878 11.7487 31.5647 12.9088 30.9202 13.6491L25.9398 19.3699C25.7078 19.6363 25.5942 19.9858 25.6253 20.3377L26.292 27.8933C26.3783 28.8711 25.3914 29.5881 24.4882 29.2039L17.5084 26.235C17.1833 26.0967 16.8158 26.0967 16.4907 26.235L9.51091 29.2039C8.6077 29.5881 7.62083 28.8711 7.7071 27.8933L8.37381 20.3377C8.40486 19.9858 8.29132 19.6363 8.05932 19.3699L3.07887 13.6491C2.43438 12.9088 2.81133 11.7487 3.76787 11.5286L11.1597 9.82787C11.504 9.74865 11.8013 9.53269 11.983 9.22969L15.8847 2.7252Z"
                                    fill="#DC3A35"></path>
                                <path
                                    d="M8.39783 2.33688C8.49924 2.08763 8.85217 2.08763 8.95358 2.33688L9.58545 3.88984C9.61593 3.96475 9.67536 4.02418 9.75027 4.05466L11.3032 4.68652C11.5525 4.78794 11.5525 5.14087 11.3032 5.24228L9.75027 5.87415C9.67536 5.90462 9.61593 5.96405 9.58545 6.03896L8.95358 7.59192C8.85217 7.84117 8.49924 7.84117 8.39783 7.59192L7.76596 6.03896C7.73548 5.96405 7.67605 5.90462 7.60114 5.87415L6.04819 5.24228C5.79894 5.14087 5.79894 4.78794 6.04819 4.68652L7.60114 4.05466C7.67605 4.02418 7.73548 3.96475 7.76596 3.88984L8.39783 2.33688Z"
                                    fill="#E95056"></path>
                                <path
                                    d="M28.3134 22.9994C28.4148 22.7501 28.7678 22.7501 28.8692 22.9994L29.1041 23.5768C29.1346 23.6517 29.194 23.7111 29.2689 23.7416L29.8463 23.9765C30.0956 24.0779 30.0956 24.4308 29.8463 24.5323L29.2689 24.7672C29.194 24.7977 29.1346 24.8571 29.1041 24.932L28.8692 25.5094C28.7678 25.7586 28.4148 25.7586 28.3134 25.5094L28.0785 24.932C28.048 24.8571 27.9886 24.7977 27.9137 24.7672L27.3363 24.5323C27.087 24.4308 27.087 24.0779 27.3363 23.9765L27.9137 23.7416C27.9886 23.7111 28.048 23.6517 28.0785 23.5768L28.3134 22.9994Z"
                                    fill="#E95056"></path>
                                <path
                                    d="M11.9391 20.4825C11.7336 20.4825 11.5634 20.4375 11.4286 20.3476C11.3001 20.2513 11.1942 20.1068 11.1107 19.9141L8.68327 14.5199C8.59336 14.3272 8.56768 14.157 8.60621 14.0093C8.64474 13.8552 8.72501 13.7364 8.84702 13.6529C8.96903 13.563 9.11673 13.5181 9.29012 13.5181C9.50846 13.5181 9.67543 13.5694 9.79102 13.6722C9.90661 13.7685 10.0029 13.913 10.08 14.1057L12.2088 19.0279H11.7272L13.856 14.096C13.9395 13.9034 14.039 13.7589 14.1546 13.6626C14.2702 13.5662 14.4307 13.5181 14.6362 13.5181C14.8096 13.5181 14.9509 13.563 15.0601 13.6529C15.1757 13.7364 15.2463 13.8552 15.272 14.0093C15.3041 14.157 15.2784 14.3272 15.1949 14.5199L12.7675 19.9141C12.6776 20.1068 12.5684 20.2513 12.44 20.3476C12.318 20.4375 12.151 20.4825 11.9391 20.4825Z"
                                    fill="white"></path>
                                <path
                                    d="M17.4447 20.4825C17.2007 20.4825 17.0145 20.415 16.886 20.2802C16.7576 20.1453 16.6934 19.9559 16.6934 19.7118V14.2887C16.6934 14.0446 16.7576 13.8552 16.886 13.7204C17.0145 13.5855 17.2007 13.5181 17.4447 13.5181C17.6823 13.5181 17.8654 13.5855 17.9938 13.7204C18.1222 13.8552 18.1865 14.0446 18.1865 14.2887V19.7118C18.1865 19.9559 18.1222 20.1453 17.9938 20.2802C17.8718 20.415 17.6888 20.4825 17.4447 20.4825Z"
                                    fill="white"></path>
                                <path
                                    d="M20.8824 20.4825C20.6384 20.4825 20.4521 20.415 20.3237 20.2802C20.1953 20.1453 20.131 19.9559 20.131 19.7118V14.3657C20.131 14.1153 20.1953 13.9258 20.3237 13.7974C20.4585 13.669 20.648 13.6048 20.892 13.6048H23.2327C23.9905 13.6048 24.5749 13.7974 24.9859 14.1827C25.3969 14.5616 25.6024 15.0914 25.6024 15.7721C25.6024 16.4528 25.3969 16.9858 24.9859 17.3711C24.5749 17.7564 23.9905 17.9491 23.2327 17.9491H21.6241V19.7118C21.6241 19.9559 21.5599 20.1453 21.4314 20.2802C21.3094 20.415 21.1264 20.4825 20.8824 20.4825ZM21.6241 16.7932H22.9823C23.3676 16.7932 23.6598 16.7065 23.8589 16.5331C24.0644 16.3597 24.1671 16.106 24.1671 15.7721C24.1671 15.4382 24.0644 15.1877 23.8589 15.0208C23.6598 14.8538 23.3676 14.7703 22.9823 14.7703H21.6241V16.7932Z"
                                    fill="white"></path>
                            </svg>
                        </a-col>
                        <a-col :xs="12" :sm="12" :md="18" :lg="18" :xl="18">
                            <span style="font-weight: 600">Miễn phí vận chuyển</span>
                            <br>
                            <span>Đơn hàng từ 299k</span>
                        </a-col>
                    </a-row>
                </a-col>
                <a-col :span="8">
                    <a-row>
                        <a-col :xs="12" :sm="12" :md="6" :lg="6" :xl="6">
                            <svg width="32" height="22" viewBox="0 0 32 22" fill="none">
                                <rect y="0.0908203" width="23.4464" height="18.7229" rx="2" fill="#DC3A35"></rect>
                                <rect x="2.26172" y="1.89685" width="18.9217" height="9.62914" rx="2" fill="white">
                                </rect>
                                <path
                                    d="M28.3059 6.76025H23.7715C22.6669 6.76025 21.7715 7.65568 21.7715 8.76025V16.814C21.7715 17.9185 22.6669 18.814 23.7715 18.814H30.0001C31.1047 18.814 32.0001 17.9185 32.0001 16.814V12.2124C32.0001 11.9069 31.9301 11.6055 31.7955 11.3313L30.1013 7.87911C29.7652 7.19425 29.0688 6.76025 28.3059 6.76025Z"
                                    fill="#DC3A35"></path>
                                <ellipse cx="8.70855" cy="18.383" rx="3.55425" ry="3.52615" fill="#E95056"></ellipse>
                                <ellipse cx="23.6753" cy="18.383" rx="3.55425" ry="3.52615" fill="#E95056"></ellipse>
                                <ellipse rx="1.97212" ry="1.95652" transform="matrix(-1 0 0 1 8.70952 18.3825)"
                                    fill="white"></ellipse>
                                <ellipse rx="1.97212" ry="1.95652" transform="matrix(-1 0 0 1 23.6753 18.3825)"
                                    fill="white"></ellipse>
                                <path
                                    d="M6.3168 9.45164C5.73155 9.45164 5.23101 9.33705 4.81518 9.10786C4.39935 8.87866 4.07849 8.5578 3.85261 8.14525C3.63186 7.72762 3.52148 7.23613 3.52148 6.67079C3.52148 6.24806 3.58309 5.86862 3.7063 5.53248C3.83464 5.19124 4.01946 4.90093 4.26074 4.66155C4.50203 4.41708 4.79465 4.22864 5.13861 4.09621C5.4877 3.96379 5.88043 3.89758 6.3168 3.89758C6.57348 3.89758 6.83274 3.93069 7.09456 3.9969C7.36151 4.05802 7.59253 4.14715 7.78761 4.26429C7.91595 4.33559 8.00323 4.42472 8.04943 4.53168C8.09563 4.63863 8.10847 4.74559 8.08793 4.85254C8.07253 4.9595 8.02889 5.05118 7.95702 5.12757C7.89028 5.20397 7.80558 5.25236 7.7029 5.27273C7.60023 5.2931 7.48472 5.27018 7.35638 5.20397C7.20236 5.11229 7.04065 5.04608 6.87124 5.00534C6.70183 4.96459 6.52985 4.94422 6.3553 4.94422C6.01134 4.94422 5.72129 5.01298 5.48513 5.15049C5.25412 5.28291 5.07957 5.47645 4.96149 5.73111C4.84342 5.98577 4.78438 6.29899 4.78438 6.67079C4.78438 7.0375 4.84342 7.35073 4.96149 7.61048C5.07957 7.87022 5.25412 8.06886 5.48513 8.20637C5.72129 8.33879 6.01134 8.405 6.3553 8.405C6.51958 8.405 6.68642 8.38463 6.85584 8.34389C7.03038 8.30314 7.19723 8.23948 7.35638 8.15289C7.48985 8.08668 7.60793 8.06376 7.7106 8.08414C7.81841 8.09942 7.90568 8.14525 7.97242 8.22165C8.0443 8.29295 8.08793 8.37954 8.10333 8.4814C8.12387 8.58326 8.1136 8.68513 8.07253 8.78699C8.03146 8.88885 7.95445 8.97543 7.84151 9.04674C7.65157 9.17407 7.41798 9.27338 7.14076 9.34469C6.86354 9.41599 6.58888 9.45164 6.3168 9.45164Z"
                                    fill="#DC3A35"></path>
                                <path
                                    d="M11.2774 9.45164C10.7435 9.45164 10.2737 9.33705 9.86815 9.10786C9.46772 8.87357 9.15456 8.54761 8.92868 8.12997C8.70793 7.71234 8.59755 7.22594 8.59755 6.67079C8.59755 6.24806 8.65916 5.86862 8.78237 5.53248C8.91071 5.19124 9.09296 4.90093 9.32911 4.66155C9.56526 4.41708 9.84762 4.22864 10.1762 4.09621C10.5099 3.96379 10.8769 3.89758 11.2774 3.89758C11.8215 3.89758 12.2938 4.01218 12.6943 4.24137C13.0947 4.47056 13.4053 4.79397 13.626 5.21161C13.8519 5.62415 13.9649 6.108 13.9649 6.66315C13.9649 7.08588 13.9007 7.46787 13.7723 7.80911C13.644 8.15035 13.4618 8.4432 13.2256 8.68767C12.9895 8.93214 12.7071 9.12059 12.3785 9.25301C12.05 9.38543 11.6829 9.45164 11.2774 9.45164ZM11.2774 8.4432C11.5802 8.4432 11.8369 8.37444 12.0474 8.23693C12.263 8.09432 12.4273 7.8906 12.5403 7.62575C12.6583 7.35582 12.7174 7.0375 12.7174 6.67079C12.7174 6.11055 12.5916 5.67763 12.34 5.37204C12.0885 5.06136 11.7343 4.90602 11.2774 4.90602C10.9796 4.90602 10.7229 4.97478 10.5073 5.11229C10.2917 5.24981 10.1274 5.45099 10.0145 5.71583C9.90152 5.98067 9.84505 6.29899 9.84505 6.67079C9.84505 7.22594 9.97082 7.66141 10.2224 7.97718C10.4739 8.28786 10.8256 8.4432 11.2774 8.4432Z"
                                    fill="#DC3A35"></path>
                                <path
                                    d="M15.5119 9.36761C15.3117 9.36761 15.1577 9.31413 15.0499 9.20717C14.9421 9.10022 14.8882 8.94742 14.8882 8.74879V4.60044C14.8882 4.4018 14.9421 4.24901 15.0499 4.14205C15.1577 4.0351 15.3117 3.98162 15.5119 3.98162H17.0135C17.9427 3.98162 18.6589 4.2159 19.162 4.68447C19.6702 5.14795 19.9243 5.81005 19.9243 6.67079C19.9243 7.10371 19.8576 7.48824 19.7241 7.82439C19.5958 8.15544 19.4084 8.43556 19.162 8.66475C18.9156 8.89394 18.6127 9.06966 18.2533 9.19189C17.8939 9.30903 17.4807 9.36761 17.0135 9.36761H15.5119ZM16.0817 8.39736H16.9442C17.2368 8.39736 17.4909 8.36171 17.7066 8.29041C17.9222 8.2191 18.1019 8.11215 18.2456 7.96954C18.3893 7.82693 18.4972 7.64867 18.569 7.43476C18.6409 7.21576 18.6768 6.9611 18.6768 6.67079C18.6768 6.09018 18.5305 5.65981 18.2379 5.37968C17.9504 5.09447 17.5192 4.95186 16.9442 4.95186H16.0817V8.39736Z"
                                    fill="#DC3A35"></path>
                                <path
                                    d="M27.9175 8.23315H23.8938C23.7281 8.23315 23.5938 8.36747 23.5938 8.53315V11.2256C23.5938 11.3912 23.7281 11.5256 23.8938 11.5256H30.178C30.3253 11.5256 30.4221 11.372 30.3582 11.2393C29.9813 10.4565 29.371 9.19554 29.1472 8.75801C28.8933 8.26176 28.2654 8.23302 27.9175 8.23315Z"
                                    fill="white"></path>
                                <rect x="27.2295" y="12.7872" width="3.26564" height="1.10113" rx="0.550567"
                                    fill="white">
                                </rect>
                            </svg>
                        </a-col>
                        <a-col :xs="12" :sm="12" :md="18" :lg="18" :xl="18">
                            <span style="font-weight: 600">Giao hàng nhanh</span>
                            <br>
                            <span>Từ 2-4 ngày</span>
                        </a-col>
                    </a-row>
                </a-col>
                <a-col :span="8">
                    <a-row>
                        <a-col :xs="12" :sm="12" :md="6" :lg="6" :xl="6">
                            <svg width="24" height="26" viewBox="0 0 24 26" fill="none">
                                <rect x="0.365234" y="0.307007" width="23.2705" height="25.386" rx="3" fill="#DC3A35">
                                </rect>
                                <path fill-rule="evenodd" clip-rule="evenodd"
                                    d="M6.4346 10.6109L2.95298 14.4092C2.6838 14.7028 2.6838 15.179 2.95298 15.4726L6.4346 19.2707C6.70378 19.5644 7.14022 19.5644 7.4094 19.2707C7.67859 18.9771 7.67859 18.501 7.4094 18.2073L5.10447 15.6928H17.7432V14.1889H5.10447L7.4094 11.6743C7.67859 11.3806 7.67859 10.9045 7.4094 10.6109C7.14022 10.3172 6.70378 10.3172 6.4346 10.6109Z"
                                    fill="white"></path>
                                <path
                                    d="M17.7432 15.6928V14.1889C19.6822 14.1889 21.2491 15.5919 21.249 17.6162C21.249 18.6159 20.3088 20.6508 17.7432 20.6508C17.7432 20.6508 13.8059 20.6508 12.6414 20.6508C11.4769 20.6508 11.4974 19.1946 12.6414 19.1946C13.7854 19.1946 17.7432 19.1946 17.7432 19.1946C19.7719 19.1946 20.7336 16.1042 17.7432 15.6928Z"
                                    fill="white"></path>
                                <rect x="8.82715" y="0.307007" width="6.3465" height="5.76955" rx="1.3" fill="white">
                                </rect>
                            </svg>
                        </a-col>
                        <a-col :xs="12" :sm="12" :md="18" :lg="18" :xl="18">
                            <span style="font-weight: 600">Đổi trả linh hoạt</span>
                            <br>
                            <span>Trong vòng 7 ngày</span>
                        </a-col>
                    </a-row>
                </a-col>
            </a-row>
            <a-divider></a-divider>




        </a-col>
    </a-row>
    <!-- Thông tin cchi tiết -->
    <a-row justify="center" style="margin-top: 20px">
        <a-col :span=24>
            <h3 style="font-size: 24px; display: flex; font-weight: 600; align-items: center; justify-content: center;">
                Thông tin sản
                phẩm
            </h3>
        </a-col>
        <a-col :xs="24" :sm="24" :md="24" :lg="16" :xl="16">

            <div class="scrollable-container" v-html="this.productInfo.description"></div>
        </a-col>
    </a-row>
    <!-- End Thông tin chi tiết -->

    <!-- Sản phẩm liên quan -->
    <a-row justify="center" style="margin-top: 20px">
        <a-col :span="24">
            <h3 style="font-size: 24px; display: flex; font-weight: 600; align-items: center; justify-content: center;">
                Sản phẩm liên quan
            </h3>
        </a-col>
        <a-col :xs="24" :sm="24" :md="22" :lg="22" :xl="22"   >
            <swiper :loop="true"  :modules="modules"  :slidesPerView="'auto'"
    :centeredSlides="false"
  
    :breakpoints="swiperBreakpoints"
    >
                <!-- :breakpoints="swiperBreakpoints" > -->
                <swiper-slide v-for="item in productRelative" :key="item.id">
                    <router-link :to="{ name: 'ProductDetail', params: { code: item.code, id: item.id } }">
                        <a-card hoverable style="width: 190px;margin: 20px" :bordered="false">
                            <template #cover>
                                <div style="width: 100%;">
                                    <img alt="example" class="image-product"
                                        :src="'data:' + item.thumbnailContentType + ';base64,' + item.thumbnailBase64" />
                                </div>
                            </template>
                            <a-card-meta>
                                <template #title>
                                    <span style="font-size: 16px; align-items: center">{{ item.name }}</span>
                                </template>
                                <template #description>
                                    <span style="font-size: 16px; align-items: center">{{ item.code }}</span>
                                    <br>
                                    <span class="price-product">{{ fomartPrice(item.price) }}&#8363;</span>
                                </template>
                            </a-card-meta>
                        </a-card>
                    </router-link>
                </swiper-slide>
            </swiper>
        </a-col>
        <a-col v-for="i in 6"  :key="i" :xs="10" :sm="10" :md="10" :lg="4" :xl="4" style="justify-content: center;">
                    <a-skeleton v-if="isLoading" :loading="isLoadingRelative" active :paragraph="{ rows: 4 }" />
        </a-col>
    </a-row>

</template>
<script>
    // import Swiper core and required modules
    import { Navigation, Pagination, Scrollbar, A11y, Thumbs } from 'swiper/modules';

    // Import Swiper Vue.js components
    import { Swiper, SwiperSlide } from 'swiper/vue';

    // Import Swiper styles
    import 'swiper/css';
    import 'swiper/css/navigation';
    import 'swiper/css/pagination';
    import 'swiper/css/scrollbar';
    import 'swiper/css/a11y';
    import 'swiper/css/thumbs';
    import 'swiper/css/free-mode';

    import { defineComponent, onMounted, ref, reactive } from 'vue';
    import { useRoute } from 'vue-router';
    import APIService from '@/helpers/APIService';
    import { LeftCircleOutlined, RightCircleOutlined } from '@ant-design/icons-vue';
    import { message, notification } from 'ant-design-vue';
    export default defineComponent({
        components: {
            LeftCircleOutlined,
            RightCircleOutlined,
            Swiper,
            SwiperSlide,


        },
        watch: {
            '$route.params.id'(newId) {
                // this.gender = newGender;
                // Thực hiện lại logic tìm kiếm sản phẩm với this.gender
                window.location.reload();
            }
        },
        

        data() {
            return {
                isLoadingRelative: false,
                isLoading:true,
                id: null,
                mainImages: [],
                productVariant: [],
                availableColors: [], // Mảng lưu trữ các màu khả dụng
                availableSizes: [], // Mảng lưu trữ các size khả dụng
                selectedColor: '',
                selectedSize: '',
                productColorOptions: [],
                productSizeOptions: [],
                quantity: 1,
                productInfo: {},
                productPriceChange: 0,
                thumbsSwiper: ref(null),
                productRelative: [],
                modules: [Navigation, Pagination, Scrollbar, Thumbs],
                swiperBreakpoints: {
                    320: {
                        slidesPerView: 2,
                        spaceBetween: 0
                    },
                    640: {
                        slidesPerView: 2,
                        spaceBetween: 0
                    },
                    768: {
                        slidesPerView: 4,
                        spaceBetween: 40
                    },
                    1024: {
                        slidesPerView: 6,
                        spaceBetween: 10
                    }
                },

            };
        },
        async mounted() {
            this.id = this.$route.params.id;
            await this.fetchProductRelative();

            await this.fetchProductImage();
            await this.fetchProductVariant();
            await this.fetchProductColor();
            await this.fetchProductSize();
            await this.fetchProductInfo();



        },
        methods: {
            setThumbsSwiper(swiper) {
                this.thumbsSwiper = swiper;
            },
            async fetchProductRelative() {
                try {
                    this.isLoadingRelative = true;
                    const response = await APIService.get(`product/get-relative-product-by-id/${this.id}`);
                    this.productRelative = response.data.data.items;
                    
                } catch (error) {
                    console.error('Error fetching product info:', error);
                }finally {
                    this.isLoadingRelative = false;
                }
            },
            async fetchProductInfo() {
                try {
                    this.isLoading = true
                    const response = await APIService.get(`product/${this.id}`);
                    this.productInfo = response.data.data.items[0];
                    this.productPriceChange = this.productInfo.price;
                    document.title = this.productInfo.name

                } catch (error) {
                    console.error('Error fetching product info:', error);
                }
                finally {
                    this.isLoading = false
                }
            },
            async fetchProductImage() {
                try {

                    const response = await APIService.get(`product/get-all-product-image-by-id/${this.id}`);
                    this.mainImages = response.data.data;

                } catch (error) {
                    console.error('Error fetching banner images:', error);
                }
            },
            async fetchProductVariant() {
                try {
                    const response = await APIService.get(`product/get-product-variant/${this.id}`);
                    this.productVariant = response.data.data;
                } catch (error) {
                    console.error('Error fetching product variant:', error);
                }
            },
            async fetchProductColor() {
                try {
                    const response = await APIService.get(`product/get-all-color-of-product-by-id/${this.id}`);

                    this.productColorOptions = response.data.data.map(color => ({
                        label: color.name,
                        value: color.id,
                    }));
                } catch (error) {
                    console.error('Error fetching product color:', error);
                }
            },
            async fetchProductSize() {
                try {
                    const response = await APIService.get(`product/get-all-size-of-product-by-id/${this.id}`);

                    this.productSizeOptions = response.data.data.map(size => ({
                        label: size.name,
                        value: size.id,
                    }));

                } catch (error) {
                    console.error('Error fetching product size:', error);
                }
            },//end fetchProductSize
            handleColorChange(colorId) {
                this.selectedColor = colorId.target.value;
                this.updateAvailableSizes();
            },

            handleSizeChange(sizeId) {
                this.selectedSize = sizeId.target.value;
                this.updateAvailableColors();
            },

            updateAvailableSizes() {
                const availableSizes = this.productVariant
                    .filter(variant => variant.colorId === this.selectedColor && variant.stockQuantity > 0)
                    .map(variant => variant.sizeId);

                this.productSizeOptions = this.productSizeOptions.map(sizeOption => ({
                    ...sizeOption,
                    disabled: !availableSizes.includes(sizeOption.value),
                }));
                const variant = this.productVariant.find(
                    v => v.colorId === this.selectedColor && v.sizeId === this.selectedSize
                );

                // Cập nhật productPriceChange nếu tìm thấy variant
                if (variant) {
                    this.productPriceChange = variant.price;
                } else {
                    this.productPriceChange = this.productInfo.price; // Hoặc giá trị mặc định khác
                }
            },

            updateAvailableColors() {
                const availableColors = this.productVariant
                    .filter(variant => variant.sizeId === this.selectedSize && variant.stockQuantity > 0)
                    .map(variant => variant.colorId);

                this.productColorOptions = this.productColorOptions.map(colorOption => ({
                    ...colorOption,
                    disabled: !availableColors.includes(colorOption.value),
                }));
                const variant = this.productVariant.find(
                    v => v.colorId === this.selectedColor && v.sizeId === this.selectedSize
                );

                // Cập nhật productPriceChange nếu tìm thấy variant
                if (variant) {
                    this.productPriceChange = variant.price;
                } else {
                    this.productPriceChange = this.productInfo.price; // Hoặc giá trị mặc định khác
                }
            },
            handleResetSizeColor() {
                this.selectedColor = '';
                this.selectedSize = '';
                this.productSizeOptions = this.productSizeOptions.map(sizeOption => ({
                    ...sizeOption,
                    disabled: false,
                }));
                this.productColorOptions = this.productColorOptions.map(colorOption => ({
                    ...colorOption,
                    disabled: false,
                }));
                this.productPriceChange = this.productInfo.price;
            },
            getSelectedColorLabel() {
                const selectedOption = this.productColorOptions.find(option => option.value === this.selectedColor);
                return selectedOption ? selectedOption.label : ''; // Nếu không tìm thấy option, trả về chuỗi rỗng
            },
            getSelectedSizeLabel() {
                const selectedOption = this.productSizeOptions.find(option => option.value === this.selectedSize);
                return selectedOption ? selectedOption.label : ''; // Nếu không tìm thấy option, trả về chuỗi rỗng
            },
            decreaseQuantity() {
                if (this.quantity > 1) {
                    this.quantity--;
                }
            },
            increaseQuantity() {
                if (this.quantity < 10) {
                    this.quantity++;
                }
            },
            async handleAddToCart() {
                if (!this.selectedColor || !this.selectedSize) {
                    notification.error({
                        message: 'Lỗi',
                        description: 'Vui lòng chọn màu và size',
                    });
                    return;
                }
                const variant = this.productVariant.find(
                    v => v.colorId === this.selectedColor && v.sizeId === this.selectedSize
                );
                if (!variant) {
                    notification.error({
                        message: 'Lỗi',
                        description: 'Không tìm thấy sản phẩm phù hợp',
                    });
                    return;
                }
                try {
                    if (localStorage.getItem('userCartId')) {
                        const response = await APIService.post('cart/add-to-cart', {
                            productVariantId: variant.id,
                            cartId: localStorage.getItem('userCartId'),
                            quantity: this.quantity,
                        });
                        localStorage.setItem('userCartId', response.data.message);
                    }
                    else {

                        const response = await APIService.post('cart/add-to-cart', {
                            productVariantId: variant.id,
                            cartId: localStorage.getItem('cartId'),
                            quantity: this.quantity,
                        });
                        localStorage.setItem('cartId', response.data.message);
                    }



                    notification.success({
                        message: 'Thành công',
                        description: 'Thêm sản phẩm vào giỏ hàng thành công',
                    });
                }
                catch {
                    notification.error({
                        message: 'Lỗi',
                        description: 'Có lỗi xảy ra khi thêm sản phẩm vào giỏ hàng',
                    });
                    localStorage.removeItem('cartId');
                }


            },
            async handleBuyNow() {
                if (!this.selectedColor || !this.selectedSize) {
                    notification.error({
                        message: 'Lỗi',
                        description: 'Vui lòng chọn màu và size',
                    });
                    return;
                }
                const variant = this.productVariant.find(
                    v => v.colorId === this.selectedColor && v.sizeId === this.selectedSize
                );
                if (!variant) {
                    notification.error({
                        message: 'Lỗi',
                        description: 'Không tìm thấy sản phẩm phù hợp',
                    });
                    return;
                }
                this.$router.push({
                    name: 'BuyNow',
                    params: {
                        productVariantId: variant.id, // ID biến thể sản phẩm
                        quantity: this.quantity,
                        price: this.productPriceChange
                    }
                });


            },

            fomartPrice(price) {
                return price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.');
            },
        },//end methods




    });
</script>
<style scoped>
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
        color: #ffffff86;
        /* Đặt màu chữ cho breadcrumb */
    }

    :deep(.ant-breadcrumb-link a) {
        color: #ffffff86;
        /* Đặt màu chữ cho breadcrumb */
    }

    .active {
        color: #e60b0b;
        /* Đặt màu chữ cho breadcrumb */
    }

    .ant-page-header-heading-title {
        line-height: normal;
        /* Đặt màu chữ cho tiêu đề */
        display: flex;
        /* Hiển thị theo chiều ngang */
        color: #ffffff86;
    }


    :deep(.ant-input-number-input) {
        width: 50px !important;
    }

    /* Trong file CSS của component hoặc file global */
    /* Đảm bảo bạn sử dụng :deep() để xuyên qua Shadow DOM của a-input-number */

    /* Tùy chỉnh border-radius cho nút đầu tiên và nút cuối cùng */

    :deep(.ant-input-number-group-addon) {
        border-top-right-radius: 20px;
        border-bottom-right-radius: 20px;
        border-top-left-radius: 20px;
        border-bottom-left-radius: 20px;
    }


    .btn-add-to-cart {
        width: 100%;
        background-color: #fff;
        border: 1px solid #b11d1b;
        box-shadow: none;
        border-radius: 15px;
        display: inline-block;
        box-sizing: border-box;
        min-width: 100px;
        /* position: relative; */
        overflow: visible;
        transition: .5s linear;
    }

    .mySwiper2 {
        width: 100%;
        height: auto;

    }

    .mySwiper {
        width: 100%;
        height: 700px;
    }



    :deep(.swiper-button-prev) {
        color: #b11d1b !important;
    }

    :deep(.swiper-button-next) {
        color: #b11d1b !important;
    }

    :deep(.swiper-pagination-bullet-active) {
        background-color: #b11d1b !important;
    }

    .scrollable-container {
        max-height: 200px;
        /* Adjust this value based on your design */
        overflow-y: auto;

        padding: 10px;
        border: none
    }

    /* Hide scrollbar for WebKit-based browsers (Chrome, Safari) */
    .scrollable-container::-webkit-scrollbar {
        display: none;
    }

    /* Hide scrollbar for Internet Explorer, Edge */
    .scrollable-container {
        -ms-overflow-style: none;
        /* IE and Edge */
        scrollbar-width: none;
        /* Firefox */
    }


</style>