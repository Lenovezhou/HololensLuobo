using System;
using System.Collections.Generic;
using UnityEngine;

class PauseResumeController : Controller
{
    public override void Execute(object data)
    {
        PauseResumGameArgs pr = data as PauseResumGameArgs;

        GameModel gm = GetModel<GameModel>();
        RoundModel rm = GetModel<RoundModel>();

        //是否暂停游戏
        bool isgamepause = pr.pausegame;

        gm.IsPlaying = !isgamepause;
        rm.IsPlaying = !isgamepause;

        Game.Instance.SetGameTimeScale(gm.IsPlaying ? 1 : 0);
    }
}
