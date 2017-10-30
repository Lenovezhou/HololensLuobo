using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UILost : View
{
    public override string Name
    {
        get
        {
            return Consts.V_Lost;
        }
    }

    public override void HandleEvent(string eventName, object data)
    {
    }

    public void Show()
    {
        this.gameObject.SetActive(true);

        RoundModel rm = GetModel<RoundModel>();
        //UpdateRoundInfo(rm.RoundIndex + 1, rm.RoundTotal);
    }


}
