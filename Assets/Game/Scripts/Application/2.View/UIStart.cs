using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStart : View
{
    public override string Name
    {
        get
        {
            return Consts.V_Start;
        }
    }

    public override void RegisterEvents()
    {
        AttentionEvents.Add(Consts.E_EnterScene);
    }


    public override void HandleEvent(string eventName, object data)
    {
        switch (eventName)
        {
            case Consts.E_EnterScene:
                SceneArgs e = data as SceneArgs;
                if (e.SceneIndex == 1)
                {
                    GetComponent<Canvas>().worldCamera = Camera.main;
                    GetComponent<Canvas>().enabled = true;
                }
                break;
            default:
                break;
        }
    }


    #region 字段
    #endregion

    #region Button 回调
    public void AdventureBut()
    {
        Game.Instance.LoadScene(2);
    }
#endregion


}
