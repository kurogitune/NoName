using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillDataSet")]
public class SkillData : ScriptableObject
{
    [Header("スキル番号")]
    public int SkillNo;
    [Header("スキル名")]
    public string SkillName;
    [Header("スキルゲージ消費量")]
    public float AbilityConsumption;
    [Header("効果時間")]
    public float EffectTime;
    [Header("スキル対象 True:自身以外 False:自身")]
    public bool SkillTarget;//True:自身以外 False:自身
    [Header("スキルタイプ")]
    public SkillType Type;
    [Header("スキル使用可能者")]
    public SkillAvailable Available;
    [Header("スキル射程")]
    public float Range;

    [Header("攻撃力")]
    public float AttackP;
    [Header("回復量")]
    public float RecoveRyamount;
}

public enum SkillType//スキル種類
{
    NoSelct,
    Attack,//攻撃スキル系
    Defense,//防御スキル系
    Support,//サポート系
    Special//特殊系
}

public enum SkillAvailable//スキル使用可能者
{
    Weak,//弱者
    Strongman//強者
}
