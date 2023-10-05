using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public GameObject lifeImage;
    public Sprite[] blackLifeImage = new Sprite[5];
    public Sprite[] redLifeImage   = new Sprite[5];

    public void SetHP()
    {
        if (PlayerController.hp > 0)
        {
            if (PlayerController.canUseRed)
            {
                lifeImage.GetComponent<Image>().sprite = redLifeImage[PlayerController.hp-1];
            }
            else
            {
                lifeImage.GetComponent<Image>().sprite = blackLifeImage[PlayerController.hp-1];
            }
        }
    }
}
