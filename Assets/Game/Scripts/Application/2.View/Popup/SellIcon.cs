using HoloToolkit.Unity.InputModule;
using System;
using UnityEngine;


public class SellIcon : MonoBehaviour, IInputClickHandler
{
    Tower m_Tower;
    SpriteRenderer selfsprite;
    BoxCollider selfbox;

    void Awake()
    {
        selfsprite = GetComponent<SpriteRenderer>();
        selfbox = GetComponent<BoxCollider>();
    }

    public void Load(Tower tower)
    {
        selfsprite.enabled = true;
        selfbox.enabled = true;
        m_Tower = tower;
    }

    //编辑器状态下
    public void OnMouseDown()
    {
#if !NETFX_CORE
        m_Tower.Tile.Data = null;

        SellTowerArgs e = new SellTowerArgs()
        {
            tower = m_Tower
            , towergameobj = m_Tower.gameObject
            , gold = m_Tower.BasePrice * m_Tower.Level / 2
        };
        SendMessageUpwards("SellTower", e, SendMessageOptions.DontRequireReceiver);
#endif
    }

    //Hololens状态下
    public void OnInputClicked(InputClickedEventData eventData)
    {
#if NETFX_CORE
        m_Tower.Tile.Data = null;

        SellTowerArgs e = new SellTowerArgs()
        {
            tower = m_Tower
            , towergameobj = m_Tower.gameObject
            , gold = m_Tower.BasePrice * m_Tower.Level / 2
        };
        SendMessageUpwards("SellTower", e, SendMessageOptions.DontRequireReceiver);
#endif
    }



}
