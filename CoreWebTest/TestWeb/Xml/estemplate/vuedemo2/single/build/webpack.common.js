const path = require('path');
const config = require('./config');
const utils = require('./utils');
const webpack = require('webpack')
const MiniCSSExtractPlugin = require('mini-css-extract-plugin')

function resolve(dir) {
  return path.resolve(__dirname, '../', dir)
}

module.exports = {
  context: path.resolve(__dirname, '../'),
  entry: {
    "app": resolve('src/main.js')
  },
  output: {
    path: resolve('dist'),
    // filename: utils.assetsPath("js/[name].[hash:5].js"),
    filename: "bundle.js",
    // publicPath: process.env.NODE_ENV === 'production' ?
    //   config.build.assetsPublicPath : config.dev.assetsPublicPath
  },
  resolve: {
    extensions: ['.js', '.vue', '.json'],
    alias: {
      'vue$': 'vue/dist/vue.esm.js',
      '@': resolve('src')
    }
  },
  module: {
    rules: [{
        test: /\.html$/,
        use: ['html-loader']
      }, {
        test: /\.vue$/,
        use: ['vue-loader']
      }, {
        test: /\.js$/,
        loader: 'babel-loader',
        exclude: path.resolve(__dirname, '/node_modules'),
        include: path.resolve(__dirname, '/src'),
        options: {
          presets: ['env']
        }
      }, {
        test: /\.css$/,
        // use: [{
        //   loader: process.env.NODE_ENV === 'production' ?
        //     MiniCSSExtractPlugin.loader : 'vue-style-loader'
        // }, {
        //   loader: "css-loader"
        // }]
        use: ['style-loader', 'css-loader']
      }, {
        test: /\.(sass|scss)$/,
        // use: [{
        //   loader: process.env.NODE_ENV === 'production' ?
        //     MiniCSSExtractPlugin.loader : 'vue-style-loader'
        // }, {
        //   loader: "css-loader"
        // }, {
        //   loader: "sass-loader"
        // }]
        use: ['style-loader', 'css-loader', 'sass-loader']
      },
      {
        test: /\.svg$/,
        loader: 'svg-sprite-loader',
        include: [resolve('src/icons')],
        options: {
          symbolId: 'icon-[name]'
        }
      },
      {
        test: /\.(jpg|png|gif|bmp|jpeg)$/,
        loader: 'url-loader',
        options: {
          esModule: false, // 这里设置为false
          limit: 10000000,
          name: utils.assetsPath('images/[name]-[hash:5].[ext]')
        }
      }, {
        test: /\.(ttf|eot|woff|woff2)$/,
        loader: 'url-loader',
        options: {
          limit: 10000000,
          name: utils.assetsPath('font/[name].[ext]')
        }
      }
    ]
  },
  plugins: [
    // 这个插件千万不要用，因为虽然UI看的爽，但是程序调用却被这个插件阻塞了，导致无限等待
    //new webpack.ProgressPlugin()
  ]
}
