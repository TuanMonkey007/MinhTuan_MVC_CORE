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
  publicPath:
  process.env.NODE_ENV === "production"
    ? "/MinhTuan_MVC_CORE/" // Thay tên repository của bạn trên muốn deploy
    : "/", // Nếu chạy ở chế độ development thì publicPath là '/'
})

