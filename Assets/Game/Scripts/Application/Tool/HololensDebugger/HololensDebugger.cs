using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HololensDebugger : Singleton<HololensDebugger> {

    private TextMesh debugger;


    void Start () {
        debugger = transform.Find("FPSText").GetComponent<TextMesh>();	
	}

    public void WriteInHololensScene(string message)
    {
        debugger.text = message + "\r\n";
    }

    public void SaveLog(string message)
    {
        debugger.text += message + "\r\n";
    }


}
