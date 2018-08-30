# 
2018年8月30日14:56:54 新建项目
介绍：SDKDemo游戏接入安卓联运渠道sdk的demo工程，目前已接入
		聚合SDK：TypeSDK(http://www.typesdk.com/)
		单独SDK:曜灵116SDK，UC,HW,OPPO 等

文件夹介绍：
	1 SDKDemo\PingTaiPeiZhi\PlatSDK\ 下为各个sdk需要用到的资源文件，替换或者新增sdk时在这里添加
	2 SDKDemo\Assets\QiuSDK\Sciripts\ 下为各个sdk的c#代码部分
	3 SDKDemo\Assets\QiuSDK\Editor\ 为当前项目需要用到的Ediotr编辑器脚本，里面有平台资源文件拷贝以及打包配置设置

使用介绍：
	1 整个demo分为sdk调用逻辑
		1）聚合sdk的调用逻辑，在U3DTypeSDK.cs脚本里（此脚本是在聚合SDK：TypeSDK的提供的框架上修改了一些）
		2）单独sdk的调用逻辑，在SDKManager.cs脚本里，此为整合了所有单独sdk的调用
	
	2 sdk的调用方法在demo.unity场景的Main.cs脚本
		
	3 打包注意：
		1 要导TypeSDK母包时，要先点击菜单栏qSDK/Copy SDK/Android/TypeSDK  导入相应资源，然后查看player Setting 设置是否正确，
			默认在SDKBuildMain.cs脚本里设置了一些，确认无误后在Export
		
		2 要打其他平台的单独包时，同样在菜单栏qSDK下导入对应平台的资源，确认设置无误后build，也可以直接菜单栏qSDK/Build 对应
			平台的sdk，此步骤会自动导入资源并且填入设置，最好打包到指定平台，需要自己在SDKBuildMain.cs修改设置
		
	4 注意项：
		曜灵116SDK 因为是一个不常见的sdk，因此没有整合SDKManager.cs框架中，而是放在U3DTypeSDK.cs里，调用方法使用U3DTypeSDK脚本，
		打包方式跟打单独包一样