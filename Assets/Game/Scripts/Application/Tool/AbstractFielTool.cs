using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using System.IO;
#endif

public abstract class AbstractFielTool
{

    public abstract void FillLevel(Action<Level> callback);

    //加载图片
    public virtual IEnumerator LoadSprite(string url, SpriteRenderer render)
    {
        WWW www = new WWW(url);

        while (!www.isDone)
            yield return www;

        Texture2D texture = www.texture;
        Sprite sp = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
        render.sprite = sp;
    }

    public abstract void LoadImage(string url, Action<Sprite> callback);
#if UNITY_EDITOR

    public virtual List<FileInfo> GetLevelFiles() { return null; }

    public virtual void FillLevel(string path, Action<Level> callback) { }

    public virtual void SaveLevel(string path,Level l) { }
#endif
}
