const { defineConfig } = require('@vue/cli-service')

module.exports = defineConfig({
  transpileDependencies: true,
  devServer: {
    hot: true, // Kích hoạt Hot Module Replacement (HMR)
    liveReload: true, // Kích hoạt Live Reloading,
    port: process.env.VUE_APP_PORT,
    client: {
      overlay: {
        runtimeErrors: false
      }
    }
  },
  publicPath: '/',


  // css: {
  //   loaderOptions: {
  //     less: {
  //       lessOptions: {
  //         modifyVars: {
  //           'primary-color': '#1DA57A', // For example, change the primary color
  //         },
  //         javascriptEnabled: true,
  //       },
  //     },
  //   },
  // },

  
})

