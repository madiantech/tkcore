const path = require('path')
const config = require('./config');
const utils = require('./utils')
const webpack = require('webpack')
const merge = require('webpack-merge')
const common = require('./webpack.common')
const HtmlWebpackPlugin = require('html-webpack-plugin')
const VueLoaderPlugin = require('vue-loader/lib/plugin')
const MiniCSSExtractPlugin = require('mini-css-extract-plugin')
const OptimizeCSSAssetsWebpackPlugin = require('optimize-css-assets-webpack-plugin')
// const UglifyJsPlugin = require('uglifyjs-webpack-plugin');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');

module.exports = merge(common, {
  mode: "production",
  // devtool: "#source-map",
  performance: {
    hints: "warning",
    maxAssetSize: 30000000, // 单位 byte
    maxEntrypointSize: 50000000, // 单位 byte
    assetFilter: (assetFilter) => {
      return assetFilter.endsWith('.css') || assetFilter.endsWith('.js')
    }
  },
  optimization: {
    // splitChunks: {
    //   name: true,
    //   cacheGroups: {
    //     commons: { // 抽离自己写的公共代码
    //       chunks: 'async', // async针对异步加载的chunk做切割，initial针对初始chunk，all针对所有chunk。
    //       name: "common", // 打包后的文件名，任意命名
    //       minChunks: 2, // 最小引用2次
    //       minSize: 30000 // 只要超出30000字节就生成一个新包
    //     },
    //     vendor: {
    //       test: /[\\/]node_modules[\\/]/, // 指定是node_modules下的第三方包
    //       chunks: 'initial',
    //       name: 'vendor', // 打包后的文件名，任意命名
    //       priority: 20 // 设置优先级，防止和自定义的公共代码提取时被覆盖，不进行打包
    //     },
    //     libs: {
    //       name: "chunk-libs",
    //       test: /[\\/]node_modules[\\/]/,
    //       priority: 10,
    //       chunks: 'initial' // 只打包初始时依赖的第三方
    //     },
    //     elementUI: {
    //       name: "chunk-elementUI",
    //       priority: 30, // 权重要大于 libs 和 app 不然会被打包进 libs 或者 app
    //       test: /[\\/]node_modules[\\/]element-ui[\\/]/,
    //       name: "elementUi"
    //     }
    //   }
    // },
    minimizer: [
      // new UglifyJsPlugin({
      //   exclude: /\.min\.js$/,
      //   cache: true,
      //   parallel: true, // 开启并行压缩，充分利用CPU
      //   sourceMap: false,
      //   extractComments: false, // 移出注释
      //   uglifyOptions: {
      //     compress: true // 压缩
      //   }
      // }),
      new OptimizeCSSAssetsWebpackPlugin({
        assetNameRegExp: /\.css$/g,
        cssProcessor: require('cssnano'),
        cssProcessorPluginOptions: {
          preset: [
            'default',
            {
              discardComments: {
                removeAll: true
              },
              normalizeUnicode: false
            }
          ]
        },
        canPrint: false
      }) // 压缩css 优化代码
    ]
    // runtimeChunk: {
    //   name: "manifest"
    // }
  },
  plugins: [
    new webpack.DefinePlugin({
      PRODUCTION: JSON.stringify(true),
      VERSION: JSON.stringify('[hash:5]'),
      BROWSER_SUPPORTS_HTML5: true,
      TWO: '1+1',
      'typeof window': JSON.stringify('object'),
      'process.env': {
        NODE_ENV: JSON.stringify(process.env.NODE_ENV)
      }
    }),
    new CleanWebpackPlugin({
      dry: false,
      cleanOnceBeforeBuildPatterns: config.build.assetsRoot,
      dangerouslyAllowCleanPatternsOutsideProject: true
    }),
    new HtmlWebpackPlugin({
      template: path.resolve(__dirname, '../index.html'),
      filename: 'index.html',
      inject: true,
      hash: true,
      minify: {
        removeComments: true, //移除HTML中的注释
        collapseWhitespace: true, //折叠空白区域 也就是压缩代码
        removeAttributeQuotes: true, //去除属性引用
      }
    }),
    new VueLoaderPlugin(),
    new MiniCSSExtractPlugin({
      // filename: utils.assetsPath('styles/index.[hash:5].css')
      filename: utils.assetsPath('styles/app.css')
    })
  ]
})
