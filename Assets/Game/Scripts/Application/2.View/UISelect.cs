using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelect : View {
    public override string Name
    {
        get
        {
            return Consts.V_Select;
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
                if (e.SceneIndex == 2)
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
    int m_SelectedIndex = -1;
    #endregion



    #region Button回调
    public void StartBut()
    {
        //测试
        m_SelectedIndex = 1;
        StartLevelArgs e = new StartLevelArgs()
        {
            LevelIndex = m_SelectedIndex
        };

        SendEvent(Consts.E_StartLevel, e);
    }
#endregion

}
