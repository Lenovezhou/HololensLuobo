using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class StartLevelCommand : Controller
{
    public override void Execute(object data)
    {
        StartLevelArgs s = data as StartLevelArgs;

        //第一步
        GameModel gm = GetModel<GameModel>();
        gm.StartLevel(s.LevelIndex);

        //第二步
        RoundModel rm = GetModel<RoundModel>();
        rm.LoadLevel(gm.PlayLevel);
        rm.IsPlaying = true;
        
        //进入游戏
        Game.Instance.LoadScene(3);

    }
}
