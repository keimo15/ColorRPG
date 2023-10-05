using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField] Slider slider;

    // HP バーの長さを変更する
    public void SetHPBar(float hp)
    {
        slider.value = hp;
    }
}