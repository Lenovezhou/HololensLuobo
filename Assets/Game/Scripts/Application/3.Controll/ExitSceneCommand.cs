using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class ExitSceneCommand:Controller
{
    public override void Execute(object data)
    {
        Game.Instance._ObjectPool.UnspawnAll();

        RoundModel rm = GetModel<RoundModel>();
        GameModel gm = GetModel<GameModel>();

        rm.StopRound();
        gm.IsPlaying = true;
        rm.IsPlaying = true;

        Game.Instance.SetGameTimeScale(1);

    }
}
