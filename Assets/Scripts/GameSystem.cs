using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour//ゲーム管理(通信処理＆制限時間管理＆BGM管理)
{
    [Header("制限時間 分:秒")]
    public int Minutes;
    public float Seconds;
    [Header("BGM")]
    public AudioClip BGM;
    [Header("開始カウントダウン")]
    public int Max_CountDownTime;

    UISystem UISy;
    public  bool RoomMaster;//部屋の主か
    public bool EveryoneReady;//全員準備完了か
    bool GameStart;//ゲームを開始しているか
    bool GamePlay;//ゲーム中か
    int TimeCount_Minutes;//制限時間分
    float TimeCount_Seconds;//制限時間秒
    float CountDownTime;//初期カウントダウン
    // Start is called before the first frame update
    void Start()
    {
        UISy = GameObject.Find("UI").GetComponent<UISystem>();
        TimeCount_Seconds = Seconds;
        TimeCount_Minutes = Minutes;
        CountDownTime = Max_CountDownTime;
        UISy.TimeCountIN(TimeCount_Minutes, TimeCount_Seconds);
        UISy.CounDownUIIN(CountDownTime.ToString("0"), false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameStart)//ゲーム開始前処理
        {
            if (RoomMaster)//部屋主の場合
            {
                if (EveryoneReady)
                {
                    CountDownTime -= Time.deltaTime;
                    UISy.CounDownUIIN(CountDownTime.ToString("0"),false);
                    if (CountDownTime<0)
                    {
                        UISy.CounDownUIIN("GO!!",true);
                        GameStart = true ;
                    }
                }
            }
            else
            {

            }

            return;
        }

        if (RoomMaster)//部屋主の場合
        {
            TimeCount_Seconds -= Time.deltaTime;
            if (TimeCount_Seconds < 0)
            {
                TimeCount_Minutes--;
                TimeCount_Seconds = 59;
            }
        }
        else
        {

        }

        UISy.TimeCountIN(TimeCount_Minutes,TimeCount_Seconds);//UI管理に制限時間を代入
    }

    public bool GamePlayOUT()//プレイ中かを出力
    {
        return GamePlay;
    }
}
