<template lang="">
 
    <a-layout-header style="background: #fff; padding: 0">
        <MenuUnfoldOutlined
          v-if="collapsed"
          class="trigger"
          @click="() => (collapsed = !collapsed)"
        />
        <MenuFoldOutlined v-else class="trigger" @click="() => (collapsed = !collapsed)" />
      </a-layout-header>

</template>
<script>
import { ref } from 'vue';
import { MenuFoldOutlined, MenuUnfoldOutlined } from '@ant-design/icons-vue';

    export default {
        components: {
            MenuFoldOutlined,
            MenuUnfoldOutlined
        },
        setup() {
            return {
                selectedKeys: ref(['1']),
                collapsed: ref(false)
            }
        }
    }
</script>

<style>

</style>