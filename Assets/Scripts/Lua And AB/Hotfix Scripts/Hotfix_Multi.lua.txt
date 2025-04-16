print("*********多函数替换***********")

--lua当中 热补丁代码固定写法
--xlua.hotfix(类, "函数名", lua函数)

--xlua.hotfix(类, {函数名 = 函数, 函数名 = 函数....})
xlua.hotfix(CS.HotfixMain, {
	Update = function(self)
		print(os.time())
	end,
	Add = function(self, a, b )
		return a + b
	end,
	Speak = function(a)
		print(a)
	end
})

xlua.hotfix(CS.HotfixTest, {
	--构造函数 热补丁固定写法[".ctor"]！！！！
	--他们和别的函数不同 不是替换 是先调用原逻辑 再调用lua逻辑
	[".ctor"] = function()
		print("Lua热补丁构造函数")
	end,
	Speak = function(self,a)
		print("唐老狮说" .. a)
	end,
	--析构函数固定写法Finalize
	Finalize = function()
		
	end
})