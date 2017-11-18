using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour,IPointerClickHandler, IInputClickHandler
{

    private void OnMouseDown()
    {
        Debug.Log(gameObject.name);
    }

    public void Clickbut()
    {
        Debug.Log("clickbut----->>>>me");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick" + gameObject.name);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown" + gameObject.name);
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("OnInputClicked" + gameObject.name);
    }
}
