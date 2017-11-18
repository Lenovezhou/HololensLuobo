using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UICountDown : View
{
    public override string Name
    {
        get
        {
            return Consts.V_CountDown;
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
                SceneArgs sa = data as SceneArgs;
                if (sa.SceneIndex == 3)
                {
                    ShowOrHide(true);
                    StartCoroutine(StartCountDown());
                    StartCoroutine(TurnAround());
                }
                break;
            default:
                break;
        }
    }


    #region 字段
    public Image Count;
    public GameObject turnaround;
#endregion

    #region 属性
    public Sprite[] Numbers;

    #endregion

    #region 帮助方法

    IEnumerator TurnAround()
    {
        while (true)
        {
            turnaround.transform.Rotate(Vector3.forward, Time.deltaTime * 360);
            yield return new WaitForFixedUpdate();
        }
    }


    IEnumerator StartCountDown()
    {
        int index = 3;
        while (index > 0)
        {
            Count.overrideSprite = Numbers[index -1];
            index--;
            yield return new WaitForSeconds(1);

            GameModel gm = GetModel<GameModel>();
            while (!gm.IsPlaying)
            {
                yield return new WaitForFixedUpdate();
            }

            if (index <= 0)
                break;
        }
        SendEvent(Consts.E_CountDownComplete);
        ShowOrHide(false);
        
    }

    void ShowOrHide(bool isshow)
    {
        gameObject.SetActive(isshow);
    }


    #endregion

}