const { defineConfig } = require('@vue/cli-service')

module.exports = defineConfig({
  transpileDependencies: true,
  devServer: {
    hot: true, // Kích hoạt Hot Module Replacement (HMR)
    liveReload: true, // Kích hoạt Live Reloading,
    client: {
      overlay: {
        runtimeErrors: false
      }
    }
  },
  publicPath: '/',

  
})

