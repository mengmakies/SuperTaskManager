###功能简介
Windows服务每天在后台定时采集商品价格，浏览商品时，可以实时显示价格走势图
>插件下载地址：http://www.c3dn.net/_share/chrome-plugin-huamao.crx

>Windows服务定时采集

<img src="http://git.oschina.net/markies/chrome-plugin-huamao/raw/master/screenshot3.jpg" width = "80%" height = "auto" alt="图片名称" align=center />

<img src="http://git.oschina.net/markies/chrome-plugin-huamao/raw/master/screenshot4.jpg" width = "45%" height = "auto" alt="图片名称" align=center />
<img src="http://git.oschina.net/markies/chrome-plugin-huamao/raw/master/screenshot5.jpg" width = "45%" height = "auto" alt="图片名称" align=center /> 

----

>chrome插件显示采集的价格走势图
>https://github.com/mengmakies/chrome-plugin-huamao

<img src="http://git.oschina.net/markies/chrome-plugin-huamao/raw/master/screenshot1.jpg" width = "80%" height = "auto" alt="图片名称" align=center /> &nbsp;
<img src="http://git.oschina.net/markies/chrome-plugin-huamao/raw/master/screenshot2.jpg" width = "80%" height = "auto" alt="图片名称" align=center />

###技术要点
1. HtmlAgilityPack；
2. Leancloud后端云；
3. Quartz任务管理；
4. cron表达式；

###注意
>本采集系统支持两种采集方式：
>1. 用户打开商品列表页面时，立即采集当前页面的商品价格数据；
>2. 服务器后台运行windows服务程序，每天凌晨00:10定时采集所有商品列表页面；


>- 如有任何问题，请加本人QQ：364223587！

###更新日志（平均每2个月同步更新环信官方SDK）
>2016.12....  敬请期待...
>2016.11.29  定时采集商品价格数据
