using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBoard : View
{
    public override string Name
    {
        get
        {
            return Consts.V_Board;
        }
    }

    public override void HandleEvent(string eventName, object data)
    {
    }
} 
