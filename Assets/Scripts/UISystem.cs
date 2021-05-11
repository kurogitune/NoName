using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class UISystem : MonoBehaviour//UI管理
{
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

    public void SkillChange(int SkillNo)//スキルUI切り替え
    {
        for (int i = 0; i < SkillObj.Length; i++)//スキルUI選択移動処理
        {
            if (i == SkillNo-1) SkillObj[i].transform.transform.Find("BackImage").GetComponent<Image>().color = SelctColor;
            else SkillObj[i].transform.transform.Find("BackImage").GetComponent<Image>().color = NotSelctColor;
        }
    }

    public void TimeCountIN(int Minutes,float Seconds)//制限時間表示
    {
        TimeText.text = string.Format("{0}:{1}",Minutes,Seconds.ToString("00"));
    }

    public void HPGageIN(int MaxHP,int CurrentHP)//体力表示
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

    public void CounDownUIIN(string Count,bool CountEnd)//カウントダウン表示
    {
        CountDownText.text = Count;
        if (CountEnd) Invoke("CoutUIFalse", 2f);
    }

    void CoutUIFalse()//カウントダウンUI非表示
    {
        CountDownUI.SetActive(false);
    } 
}
