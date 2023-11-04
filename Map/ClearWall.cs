using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearWall : MonoBehaviour
{
    public Ability keyAbility;          // 透明な壁を無効化するトリガーとなる能力

    void Start()
    {
        gameObject.GetComponent<Collider2D>().enabled = true;

        switch (keyAbility)
        {
          case Ability.Jump:
            if (!GameManager.instance.canJump) return;
            break;
          case Ability.Walk:
            if (!GameManager.instance.canWalk) return;
            break;
          case Ability.Punch:
            if (!GameManager.instance.canPunch) return;
            break;
          default:
            return;
        }

        gameObject.GetComponent<Collider2D>().enabled = false;
    }
}
