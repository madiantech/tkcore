const path = require('path');

module.exports = {
  dev: {

    // 资源路径配置
    assetsSubDirectory: 'static',
    assetsPublicPath: '/',

    // 代理配置
    proxyTable: {},

    // 开发服务器设置
    host: '127.0.0.1',
    port: 8080,
    autoOpenBrowser: true,
    errorOverlay: true,
    notifyOnErrors: true,
    poll: true

  },
  build: {

    // 打包后首页配置
    index: path.resolve(__dirname, '../dist/index.html'),

    // 打包资源根目录配置
    assetsRoot: path.resolve(__dirname, '../dist'),

    // 打包资源路径配置
    assetsSubDirectory: 'static',
    assetsPublicPath: './'

  }
}
