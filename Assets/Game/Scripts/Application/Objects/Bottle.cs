using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : Tower {
    Transform m_ShotPoint;

    protected override void Awake()
    {
        base.Awake();
        m_ShotPoint = transform.Find("ShotPoint");
    }

    public override void Shot(Monster monster)
    {
        base.Shot(monster);

        GameObject go = Game.Instance._ObjectPool.Spawn("BallBullet");
        BallBullet bullet = go.GetComponent<BallBullet>();
        bullet.transform.position = m_ShotPoint.position;
        bullet.Load(this.UseBulletID, this.Level, this.MapRect, monster);
    }
}
