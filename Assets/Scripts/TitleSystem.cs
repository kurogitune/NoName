using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Exception;
using UnityEngine.UI;

public class TitleSystem : MonoBehaviour
{
    [Header("BGMリスト")]
    public BGMList BGMLis;
    [Header("SEリスト")]
    public BGMList SELis;

    public Text initialUI;

    Savar_Data SaverD;//サーバーデータ
    // Start is called before the first frame update
    void Start()
    {
        AudioSystem.AudioSourceIN();
        SaverD = SaverDataSystem.Lord();
        AudioSystem.BGMPlaye(BGMLis.TitleBGM,true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            initialUI.gameObject.SetActive(false);
        }
    }
}
