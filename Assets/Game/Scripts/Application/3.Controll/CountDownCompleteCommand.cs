using System;
using System.Collections.Generic;

public class CountDownCompleteCommand : Controller
{
    public override void Execute(object data)
    {
        RoundModel rModel = GetModel<RoundModel>();
        rModel.StartRound();
    }
}
