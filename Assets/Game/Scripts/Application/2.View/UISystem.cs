using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISystem : View
{
    #region 事件
    public Button btnResume;
    public Button btnRestart;
    public Button btnSelect;
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_Sytem; }
    }
    #endregion

    #region 方法
    public void Show()
    {
        this.gameObject.SetActive(true);
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
    public override void RegisterEvents()
    {
        AttentionEvents.Add(Consts.E_UISystem);
        AttentionEvents.Add(Consts.E_EndLevel);
    }

    void SendResumGame()
    {
        PauseResumGameArgs pr = new PauseResumGameArgs { show = false, pausegame = false };
        SendEvent(Consts.E_UISystem, pr);
    }

    #endregion

    #region 事件回调
    public override void HandleEvent(string eventName, object data)
    {
        switch (eventName)
        {
            case Consts.E_UISystem:
                PauseResumGameArgs pr = data as PauseResumGameArgs;
                if (pr.show)
                {
                    Show();
                }
                else {
                    Hide();
                }
                break;
            case Consts.E_EndLevel:
                Hide();
                break;
            default:
                break;
        }

    }

    public void OnResumeClick()
    {
        SendResumGame();
    }

    public void OnRestartClick()
    {
        Game.Instance.LoadScene(3);
        //SendResumGame();
    }

    public void OnSelectClick()
    {
        Game.Instance.LoadScene(2);
        SendResumGame();
    }

   

    #endregion
}
