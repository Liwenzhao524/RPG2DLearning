using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

/// <summary>
/// Lua管理器
/// </summary>
public class LuaManager : BaseManager<LuaManager>
{
    private LuaEnv luaEnv;

    public LuaTable Global
    {
        get
        {
            return luaEnv.Global;
        }
    }

    public void Init()
    {
        if (luaEnv != null)
            return;
        luaEnv = new LuaEnv();

        luaEnv.AddLoader(MyCustomLoader);
        luaEnv.AddLoader(MyCustomABLoader);
    }

    private byte[] MyCustomLoader(ref string filePath)
    {
        //传入require执行的lua脚本文件名
        // Lua文件所在路径
        string path = Application.dataPath + "/Lua/" + filePath + ".lua";

        if (File.Exists(path))
        {
            return File.ReadAllBytes(path);
        }
        else
        {
            Debug.Log("MyCustomLoader重定向失败，文件名为" + filePath);
        }
        return null;
    }

    // 重定向AB包中Lua
    private byte[] MyCustomABLoader(ref string filePath)
    {
        TextAsset lua = ABManager.Instance.LoadRes<TextAsset>("lua", filePath + ".lua");
        if (lua != null)
            return lua.bytes;
        else
            Debug.Log("MyCustomABLoader重定向失败，文件名为：" + filePath);

        return null;
    }


    /// <summary>
    /// 传入lua文件名 执行lua脚本
    /// </summary>
    /// <param name="fileName"></param>
    public void DoLuaFile(string fileName)
    {
        string str = string.Format("require('{0}')", fileName);
        if (luaEnv == null)
        {
            Debug.Log("解析器为初始化");
            return;
        }
        luaEnv.DoString(str);
    }

    /// <summary>
    /// lua GC
    /// </summary>
    public void Tick()
    {
        if (luaEnv == null)
        {
            return;
        }
        luaEnv.Tick();
    }

    /// <summary>
    /// 销毁解析器
    /// </summary>
    public void Dispose()
    {
        if (luaEnv == null)
        {
            return;
        }
        luaEnv.Dispose();
        luaEnv = null;
    }
}
