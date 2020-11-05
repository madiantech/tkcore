# TKCore 是什么

[TKCore](http://www.tkcore.net/) 是一套基于.Net Core 开发的用于快速构筑管理后台的**开发框架**。随着需求复杂度的由低到高：

- 标准的增删改查模块，你只需要配置 XML 文件便可实现。目前除了常见的单表、主从表、树状等功能外、系统预置了多达 10 余种配置模型，支持实现更为复杂的功能。

- 标准模块基础上需要部分自定义的模块，框架支持以代码插件的形式对配置进行补充来实现功能。

- 完全自定义的模块，支持以插件 DLL 形式扩展进来。

框架内置了常见的数据验证、显示格式化、和丰富的控件，这些都可以通过修改 XML 配置来切换效果。TKCore不但可以在开发阶段大幅提升开发效率，在系统运维阶段，可灵活应对客户的需求变更，从而提升系统整体的品质。



[开发文档传送入口](http://www.tkcore.net/)



## 理念

**不重复造轮子**

项目开发中经常会有相同的功能写了一遍又一遍的情况。TKCore 框架的模块化、插件化开发方式，会促使你去逐步积累并优化插件，最终变成 可通过XML配置实现。



## 特性

- **跨平台**

  基于.net core开发，适用于 Windows、macOS 和 Linux，支持Docker容器快速部署。

- **非侵入**

  框架以中间件形式引入，它不会对现有工程产生影响

- **安全**

  框架内置登录权限、操作权限、数据权限、功能权限机制，防止SQL注入，脚本注入，保护系统的安全。

- **配置灵活**

  不仅可实现单表、树状增删改查功能，一主多从等复杂功能也不在话下。就算是完全定制的页面也可以自定义轻松化解。配置代替编码，从此让你对需求变更“和善”起来。

- **可扩展**

  插件化的开发模式设计让你随时插拔你的模块功能。

- **效率至上**

  自带配置文件生成工具，只需简单配置，即可快速实现增删改查功能，从而节省大量时间。框架通过缓存、优化处理的方式提升运行效率。

  

## 起步

官方指南假设你是.Net Core 的开发者，已了解关于 .Net Core、C# 、 PowerDesigner 和数据库等相关知识。如果你刚开始学习后台开发，掌握好基础知识再来吧！



TKCore Web 应用程序 = Web应用程序 + TKCore的DLL和固有资源。我们推荐第一次接触TKCore的伙伴通过 ToolkitSuite 工具，创建 TKCore Web 应用程序，具体可[点击这里]([http://www.tkcore.net/toolkitsuite/#%E5%BA%94%E7%94%A8%E7%94%9F%E6%88%90%E5%B7%A5%E5%85%B7](http://www.tkcore.net/toolkitsuite/#应用生成工具))了解。



## 目录结构

```
- 项目根目录
 - wwwroot
  + toolkitcss
  + toolkitjs
  - favicon.ico
 - Xml
  + Data
  + Module
  + razor
  + razortemplate
  + schema
  - Application.xml
  - Default.xml
 - appsettings.json
 - Program.cs
 - Startup.cs
```

目录结构其实是原 Asp.NET Core Web 应用的基础上，加上了 TKCore 框架的特有的文件和文件夹。特有内容如下：

- toolkitcss - 框架需要的 css 样式
- toolkitjs - 框架需要的 js 库
- Data - 存放 DataXml 的根目录。DataXml 简单讲就是对表的数据进行描述，或者说是表的元数据。
- Module - 存放 ModuleXml 的根目录。ModuleXml 简单讲就是功能定义，通过该文件可以运行相应的功能模块。
- razor - 存放自定义 razor 页面的目录
- razortemplate - 框架的 razor 页面模板
- schema - 框架预置的 Xml 配置文件的格式定义。如果 Xml 编辑软件（如 XMLSpy）支持 schema，那么会大大增加 Xml 的编辑数据的速度，降低 Xml 的编辑难度和各种不必要的人为错误。
- Application.xml - 项目的配置文件
- Default.xml - 项目的默认值配置文件



## 准备好了吗

我们刚才简单介绍了 TKCore 框架——本教程的其余部分将更加详细地涵盖这些功能以及其它高级功能，所以请务必读完整个[教程](http://www.tkcore.net/)！

我们将遵循由浅入深的原则，逐步带你了解 TKCore 的功能：

**入门篇** - 将带你了解只需要配置、完全不需要编写代码就可实现的功能

**初级篇** - 配置为主、代码为辅。

**中级篇** - 进一步介绍 TKCore 的代码功能，带你了解其运作原理。

**高级篇** - 深度了解 TKCore 框架，按照自己需求对功能，对框架进行定制。



