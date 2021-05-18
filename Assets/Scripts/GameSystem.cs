using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameSystem : MonoBehaviour//ゲーム管理(通信処理＆制限時間管理＆BGM管理)
{
    [Header("制限時間 分:秒")]
    public int Minutes;
    public float Seconds;
    [Header("BGMリスト")]
    public BGMList BGMLis;
    [Header("SEリスト")]
    public SEList SELis;
    [Header("使用機体リスト")]
    public GameObject[] PlayerModelLis;
    [Header("初期出現位置")]
    public GameObject[] PlayerINPoz;

    [Header("開始カウントダウン")]
    public int Max_CountDownTime;

    [Header("ステータスUI")]
    public GameObject StatusUI;
    [Header("スキルUI")]
    public GameObject SkillUI;
    [Header("制限時間UI")]
    public GameObject TimeCountUI;
    [Header("開始カウントダウンUI")]
    public GameObject CountDownUI;

    [Header("体力ゲージ")]
    public Text HPGage;
    [Header("ブーストゲージ")]
    public Text BoothGage;
    [Header("能力用ゲージ")]
    public Text AbilityhGage;
    [Header("制限時間")]
    public Text TimeText;
    [Header("カウントダウン")]
    public Text CountDownText;

    [Header("skill表示オブジェクト")]
    public GameObject[] SkillObj;
    [Header("非選択カラー")]
    public Color NotSelctColor;
    [Header("選択カラー")]
    public Color SelctColor;

    public  bool RoomMaster;//部屋の主か
    public bool EveryoneReady;//全員準備完了か
    bool GameStart;//ゲームを開始しているか
    bool GamePlay;//ゲーム中か
    int TimeCount_Minutes;//制限時間分
    float TimeCount_Seconds;//制限時間秒
    float CountDownTime;//初期カウントダウン
    int PlayerNo=1;//プレイヤー番号
    int PlayerModelNo=1;//機体番号
    // Start is called before the first frame update
    void Start()
    {
        TimeCount_Seconds = Seconds;
        TimeCount_Minutes = Minutes;
        CountDownTime = Max_CountDownTime;
        TimeCountIN(TimeCount_Minutes, TimeCount_Seconds);
        CounDownUIIN(CountDownTime.ToString("0"), false);

        GameObject PlayerModel = Instantiate(PlayerModelLis[PlayerNo-1]);
        PlayerModel.transform.position = PlayerINPoz[PlayerNo - 1].transform.position;
        PlayerModel.transform.rotation = PlayerINPoz[PlayerNo - 1].transform.rotation;
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
                    CounDownUIIN(CountDownTime.ToString("0"),false);
                    if (CountDownTime<0)
                    {
                        CounDownUIIN("GO!!",true);
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

        TimeCountIN(TimeCount_Minutes,TimeCount_Seconds);//UI管理に制限時間を代入
    }

    public bool GamePlayOUT()//プレイ中かを出力
    {
        return GamePlay;
    }

    public void SkillChange(int SkillNo)//スキルUI切り替え
    {
        for (int i = 0; i < SkillObj.Length; i++)//スキルUI選択移動処理
        {
            if (i == SkillNo - 1) SkillObj[i].transform.transform.Find("BackImage").GetComponent<Image>().color = SelctColor;
            else SkillObj[i].transform.transform.Find("BackImage").GetComponent<Image>().color = NotSelctColor;
        }
    }

    public void TimeCountIN(int Minutes, float Seconds)//制限時間表示
    {
        TimeText.text = string.Format("{0}:{1}", Minutes, Seconds.ToString("00"));
    }

    public void HPGageIN(int MaxHP, int CurrentHP)//体力表示
    {
        HPGage.text = string.Format("Hp {0}/{1}", CurrentHP, MaxHP);
    }

    public void BoothGageIN(int MaxBooth, int CurrentBooth)//ブーストゲージ
    {
        BoothGage.text = string.Format("Booth {0}/{1}", CurrentBooth, MaxBooth);
    }

    public void AbilityhGageIN(int MaxAbilityh, int CurrentAbilityh)//能力ゲージ表示
    {
        AbilityhGage.text = string.Format("Abilityh {0}/{1}", CurrentAbilityh, MaxAbilityh);
    }

    public void CounDownUIIN(string Count, bool CountEnd)//カウントダウン表示
    {
        CountDownText.text = Count;
        if (CountEnd) Invoke("CoutUIFalse", 2f);
    }

    void CoutUIFalse()//カウントダウンUI非表示
    {
        CountDownUI.SetActive(false);
    }
}
