using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPanel : MonoBehaviour {

    private SpawnIcon[] m_Icons;
    private SpriteRenderer image;

    #region 方法
    public void Show(GameModel gm, Vector3 createPosition, bool upSide)
    {
        transform.position = createPosition;
        for (int i = 0; i < m_Icons.Length; i++)
        {
            TowerInfo info = Game.Instance._StaticDate.GetTowerInfo(i);
            m_Icons[i].Load(gm, info, createPosition, upSide, Hide);
        }
        image.enabled = true;
    }

    public void Hide()
    {
        Find();
        for (int i = 0; i < m_Icons.Length; i++)
        {
            m_Icons[i].Hide();
        }
        image.enabled = false;
    }

    void Find()
    {
        if (null == m_Icons)
        {
            m_Icons = GetComponentsInChildren<SpawnIcon>();
        }
        if (null == image)
        {
            image = GetComponent<SpriteRenderer>();
        }
    }

    #endregion
}
