using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Const;

public class ScoreController : MonoBehaviour
{
    public Player player;
    private void OnTriggerEnter2D(Collider2D col)
    {
        Element bullet = col.gameObject.GetComponent<Element>();
        if(bullet == null )return;
        if (bullet.side == SIDE.ENEMY || bullet.side == SIDE.BOSS)
        {
            player?.OnScore?.Invoke(10);
        }
    }
}
