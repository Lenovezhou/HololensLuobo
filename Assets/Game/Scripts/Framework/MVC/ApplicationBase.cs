using HoloToolkit.Unity;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class ApplicationBase : Singleton<ApplicationBase>
{
    //注册控制器
    protected void RegisterController(string eventName,Type controllerType)
    {
        MVC.RegisterController(eventName, controllerType);
    }

    protected void SendEvent(string eventName, object data = null)
    {
        MVC.SendEvent(eventName, data);
    }

    public abstract void SetGameTimeScale(int scale);

    public abstract void LoadScene(int index);

    public abstract ObjectPool _ObjectPool{get;}
    public abstract Sound _Sound { get; }
    public abstract StaticData _StaticDate { get; }
    public abstract void WriteInHololens(string message);
}