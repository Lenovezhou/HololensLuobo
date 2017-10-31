using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class FielsFactory
{

    static public AbstractFielTool CreatFielTool()
    {
#if UNITY_EDITOR
        return new NormalFielTool();
#else
        return new UWPFielTool();
#endif
    }


}
