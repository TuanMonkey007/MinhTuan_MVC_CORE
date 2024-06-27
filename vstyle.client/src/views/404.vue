<template>
	<a-result status="404" title="404" sub-title="Xin lỗi, Trang bạn tìm không tồn tại">
		<template #extra>
        <router-link  class="btn-buy-now"  style="border-radius: 20px; display: inline-flex;align-items: center;justify-content: center;background-color: #c12227; height: 40px;" :to="{ name: 'CustomerHome' }">Trở về trang chủ</router-link>
      </template>
	</a-result>
</template>