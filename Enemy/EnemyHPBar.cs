using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// hp バーの長さを変更する
public class EnemyHPBar : MonoBehaviour
{
    [SerializeField] Slider slider;

    public void SetHPBar(float hp)
    {
        slider.value = hp;
    }
}