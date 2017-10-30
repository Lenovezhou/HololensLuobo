using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(ObjectPool))]
[RequireComponent(typeof(Sound))]
[RequireComponent(typeof(StaticData))]
public class Game : ApplicationBase
{
    #region 全局字段
    //全局访问功能
    public override ObjectPool _ObjectPool
    {
        get
        {
            return ObjectPool.Instance;
        }
    }


    public override Sound _Sound
    {
        get
        {
            return Sound.Instance;
        }
    }


    public override StaticData _StaticDate
    {
        get
        {
            return StaticData.Instance;
        }
    }

    [HideInInspector]
    public Sound Sound = null;//声音控制
    [HideInInspector]
    public StaticData StaticData = null;//静态数据

    [HideInInspector]
    public GameObject HololensCamera;//maincamera
    [HideInInspector]
    public GameObject Cursor;//光标

#endregion
    //全局方法
    public override void LoadScene(int level)
    {
        //---退出旧场景
        //事件参数
        SceneArgs e = new SceneArgs();
        e.SceneIndex = SceneManager.GetActiveScene().buildIndex;

        //发布事件
        SendEvent(Consts.E_ExitScene, e);

        //---加载新场景
        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }

    void OnLevelWasLoaded(int level)
    {
        Debug.Log("OnLevelWasLoaded:" + level);

        //事件参数
        SceneArgs e = new SceneArgs();
        e.SceneIndex = level;
        //发布事件
        SendEvent(Consts.E_EnterScene, e);
    }

    //游戏入口
    void Start()
    {
        //确保Game对象一直存在
        Object.DontDestroyOnLoad(this.gameObject);

        //全局单例赋值
        HololensCamera = Camera.main.gameObject;
        Cursor = GameObject.Find("InteractiveMeshCursor");

        Object.DontDestroyOnLoad(HololensCamera);
        Object.DontDestroyOnLoad(Cursor);

        //注册启动命令
        RegisterController(Consts.E_StartUp, typeof(StartUpCommand));

        //启动游戏
        SendEvent(Consts.E_StartUp);
    }
}
