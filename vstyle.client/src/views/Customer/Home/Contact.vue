<template>
    <a-row justify="center" :gutter="30" style="margin-top: 20px">
        <a-col :xs="20" :sm="20" :md="20" :lg="10" :xl="10">
            <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3724.6329237392833!2d105.82215010952005!3d21.007346580555915!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x3135ac8109765ba5%3A0xd84740ece05680ee!2zVHLGsOG7nW5nIMSQ4bqhaSBo4buNYyBUaOG7p3kgbOG7o2k!5e0!3m2!1sen!2sus!4v1719133712063!5m2!1sen!2sus" width="600" height="450" style="border:0;max-width: 100%;" allowfullscreen="" loading="lazy" referrerpolicy="no-referrer-when-downgrade"></iframe>       </a-col>
        <a-col :xs="20" :sm="20" :md="20" :lg="10" :xl="10">
            <h3 style="font-size: 24px; display: flex; font-weight: 500; align-items: center; justify-content: center;">THÔNG TIN LIÊN HỆ</h3>
            
            <a-row style="margin-top: 20px;align-items: center;">
                <a-col :span="2">
                    <font-awesome-icon icon="fa-solid fa-phone" style="font-size: 20px;"/>
                </a-col>
                <a-col :span="20" style="align-items: center;">
                   <span tyle="font-size: 20px;" v-html="this.contact.phone">
                   </span>
                   </a-col> 
            </a-row>
            <a-row style="margin-top: 20px;align-items: center;">
                <a-col :span="2">
                    <font-awesome-icon icon="fa-solid fa-envelope"  style="font-size: 20px;"/>
                </a-col>
                <a-col :span="20" style="align-items: center;">
                   <span tyle="font-size: 20px;" v-html="this.contact.email">
                   </span>
                   </a-col> 
            </a-row>
            <a-row style="margin-top: 20px;align-items: center;">
                <a-col :span="2">
                    <font-awesome-icon icon="fa-solid fa-location-dot" style="font-size: 20px;" />
                </a-col>
                <a-col :span="20" style="align-items: center;">
                   <span tyle="font-size: 20px;" v-html="this.contact.address">
                   </span>
                   </a-col> 
            </a-row>
        
</a-col>
    </a-row>
    
</template>
<script>
import APIService from '@/helpers/APIService';
    export default {
        data() {
            
            return {
                contact:{
                    phone:'',
                    email:'',
                    address:''
                },
                googleMap:''
            }
        },
        async mounted() {
           
                 await this.fetchContact();
                    await this.fetchGoogleEmbed()
        },
         methods:{
            async fetchContact(){
                const response = await APIService.get("datacategory/get-data-by-code-and-parent-code/SDT/LIEN_HE");
                    this.contact.phone = response.data.data.description;
                   
                    const response2 = await APIService.get("datacategory/get-data-by-code-and-parent-code/EMAIL/LIEN_HE");
                    this.contact.email = response2.data.data.description;
                   
                    const response3 = await APIService.get("datacategory/get-data-by-code-and-parent-code/DIA_CHI/LIEN_HE");
                    this.contact.address = response3.data.data.description;
                   
            },
            async fetchGoogleEmbed(){
                const res = await APIService.post("datacategory/get-data-by-page", {
                        pageIndex: 1,
                        pageSize: 1,
                        code_Filter: "GOOGLEMAP_EMBED"
                })
                console.log(res)
                this.googleMap = res.data.data.items[0].description
            }
        }

    }
</script>
<style lang="">

</style>