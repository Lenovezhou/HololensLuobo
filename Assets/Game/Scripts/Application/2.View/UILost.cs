using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

class UILost : View
{


    #region 字段

    public Button ReStart;

    #endregion

    #region 属性
    public override string Name
    {
        get
        {
            return Consts.V_Lost;
        }
    }
    #endregion 
    public override void HandleEvent(string eventName, object data)
    {
    }

    public void Show()
    {
        this.gameObject.SetActive(true);

        RoundModel rm = GetModel<RoundModel>();
        //UpdateRoundInfo(rm.RoundIndex + 1, rm.RoundTotal);
    }

    public void Restart()
    {
        GameModel gm = GetModel<GameModel>();


        StartLevelArgs e = new StartLevelArgs()
        {
            LevelIndex = gm.GameProgress
        };

        SendEvent(Consts.E_StartLevel, e);
    }

}
