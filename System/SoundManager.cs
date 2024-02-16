using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// サウンドの管理
public class SoundManager : MonoBehaviour
{
    // BGM のサウンドデータ
    public AudioClip bgmInMenu;
    public AudioClip bgmInTownBright;
    public AudioClip bgmInTownDark;
    public AudioClip bgmInButtle;
    public AudioClip bgmInDungeon;
    public AudioClip bgmInBoss;
    public AudioClip bgmInClear;

    // SE のサウンドデータ
    public AudioClip seJump;
    public AudioClip sePunch;
    public AudioClip seAttack;
    public AudioClip seDamage;
    public AudioClip seItem;
    public AudioClip seClick;

    public static SoundManager soundManager;            // 最初の SoundManager を保存する変数
    public static BGMType playingBGM = BGMType.None;    // 再生中の BGM

    private void Awake()
    {
        // BGM 再生
        if (soundManager == null)
        {
            soundManager = this;                        // static 変数に自分を保存する
            DontDestroyOnLoad(gameObject);              // シーンが変わってもゲームオブジェクトを破棄しない
        }
        else
        {
            Destroy(gameObject);                        // ゲームオブジェクトを破棄
        }
    }

    // BGM の設定と再生
    public void PlayBgm(BGMType type)
    {
        if (type != playingBGM)
        {
            playingBGM = type;
            AudioSource audio = GetComponent<AudioSource>();
            switch (type)
            {
                case BGMType.Menu:
                    audio.clip = bgmInMenu;
                    break;
                case BGMType.TownBright:
                    audio.clip = bgmInTownBright;
                    break;
                case BGMType.TownDark:
                    audio.clip = bgmInTownDark;
                    break;
                case BGMType.Buttle:
                    audio.clip = bgmInButtle;
                    break;
                case BGMType.Dungeon:
                    audio.clip = bgmInDungeon;
                    break;
                case BGMType.Boss:
                    audio.clip = bgmInBoss;
                    break;
                case BGMType.Clear:
                    audio.clip = bgmInClear;
                    break;
                default:
                    break;
            }
            audio.Play();
        }
    }

    // BGM の停止
    public void StopBGM()
    {
        GetComponent<AudioSource>().Stop();
        playingBGM = BGMType.None;
    }

    // SE の再生
    public void PlaySE(SEType type)
    {
        switch (type)
        {
            case SEType.Jump:
                GetComponent<AudioSource>().PlayOneShot(seJump);
                break;
            case SEType.Punch:
                GetComponent<AudioSource>().PlayOneShot(sePunch);
                break;
            case SEType.Attack:
                GetComponent<AudioSource>().PlayOneShot(seAttack);
                break;
            case SEType.Damage:
                GetComponent<AudioSource>().PlayOneShot(seDamage);
                break;
            case SEType.Item:
                GetComponent<AudioSource>().PlayOneShot(seItem);
                break;
            case SEType.Click:
                GetComponent<AudioSource>().PlayOneShot(seClick);
                break;
            default:
                break;
        }
    }
}
