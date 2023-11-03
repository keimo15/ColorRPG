using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public GameObject lifeImage;
    public Sprite[] blackLifeImage = new Sprite[5];
    public Sprite[] redLifeImage   = new Sprite[5];
    [SerializeField] PlayerButtle player;

    // HP に応じて lifeImage を変える
    public void SetHP()
    {
        if (player.hp > 0)
        {
            if (GameManager.instance.canUseRed)
            {
                lifeImage.GetComponent<Image>().sprite = redLifeImage[player.hp-1];
            }
            else
            {
                lifeImage.GetComponent<Image>().sprite = blackLifeImage[player.hp-1];
            }
        } 
        else
        {
            lifeImage.SetActive(false);
        }
    }
}
