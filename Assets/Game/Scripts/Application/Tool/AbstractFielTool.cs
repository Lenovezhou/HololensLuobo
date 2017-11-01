using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using System.IO;
#endif

public abstract class AbstractFielTool
{

    public abstract void FillLevel(ref List<Level> levels);

#if UNITY_EDITOR

    public virtual List<FileInfo> GetLevelFiles() { return null; }

    public virtual void FillLevel(string path, ref Level l) { }

    public virtual void SaveLevel(string path,Level l) { }
#endif
}
