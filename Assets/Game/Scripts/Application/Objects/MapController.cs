using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

 #region 字段
    [SerializeField]
    SpriteRenderer background;

#endregion

    public void LoadLevel(Level level)
    {
        string path = "Res/Maps/" + level.Background;
        string pathfinal = path.Split('.')[0];
        Sprite s = Resources.Load<Sprite>(path);
        Sprite ss = Instantiate(s);
        background.sprite = ss;
    }
}
