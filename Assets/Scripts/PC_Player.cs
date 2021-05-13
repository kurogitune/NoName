using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Exception;
public class PC_Player : MonoBehaviour//PCでの操作するプレイヤー
{
    [Header("カメラオブジェクト位置")]
    public Vector3 CameraPoz;
    [Header("カメラ角度")]
    public Quaternion CameraRoot;

    [Header("移動速度")]
    public float MoveSpeed;
    [Header("ブースト移動速度")]
    public float BoothSpeed;
    [Header("ジャンプ力")]
    public float JunpP;
    [Header("カメラ回転測度")]
    public float CameraRootSpeed;

    [Header("攻撃物")]
    public GameObject GunObj;

    [Header("体力")]
    public int MaxHP;
    [Header("ブーストゲージ")]
    public int MaxBoothGage;
    [Header("能力使用専用ゲージ")]
    public int MaxAbilityGage;

    [Header("ブーストゲージ消費量")]
    public float BoothConsumption;
    [Header("ブーストゲームジャンプ消費量")]
    public float BoothJunpConsumption;
    [Header("ブーストゲージ自動回復量")]
    public float BoothRecovery;
    [Header("能力ゲージ消費量")]//スキルにより消費量は変化
    public float AbilityConsumption;
    [Header("能力ゲージ自動回復量")]
    public float AbilityRecovery;
    
    [Header("地面Ray")]
    public LayerMask Mask1;
    [Header("地面判定長さ")]
    public float RayRang;

    GameObject CameraObj;//カメラ
    Rigidbody Rig;
    Key_Data Key;//キーコード
    Gun GunSy;//銃の管理システム
    SkillData[] SkillList;//選択スキル 
    KeyCode Skillchange = KeyCode.V;//スキル切り替え
    KeyCode SkillUse = KeyCode.Q;//能力使用
    KeyCode LookOnKey = KeyCode.Tab;//ロックオン
    GameSystem GameSy;
    float X, Z;//移動用
    float MousX, MousY;//マウスでの視点回転用
    float Speed;//移動速度
    int HP;//体力
    float BoothGage;//ブーストゲージ
    float AbilityGauge;//能力用ゲージ
    int SkillNo;//スキル番号
    float GageRecoveryTime;//回復時間管理用
    bool LookON;//敵をロックしているか
    GameObject LookOnObj;//ロックオンしているオブジェクト
    void Start()
    {
        //以下初期値代入
        SkillNo = 1;
        HP = MaxHP;
        BoothGage = MaxBoothGage;
        AbilityGauge = MaxAbilityGage;
        //ここまで
        GameSy = GameObject.Find("System").GetComponent<GameSystem>();
        GameSy.SkillChange(SkillNo);
        Key = KeyDataSystem.Lord();//キーのデータを取得
        CameraObj = ObjDataGet.MainCameraObj();//カメラオブジェクトを取得
        Rig = GetComponent<Rigidbody>();
        CameraObj.transform.parent = transform;//プレイヤーオブジェクトと親子関係を作成
        CameraObj.transform.position = transform.position + CameraPoz;//カメラ座標を指定
        CameraObj.transform.rotation = CameraRoot;//カメラ回転率を代入
        GunSy = GunObj.GetComponent<Gun>();//銃管理
    }

    void Update()
    {
        //以下ゲージ関係回復処理
        GageRecoveryTime += Time.deltaTime;

        if (GageRecoveryTime>0.5f)//回復間隔設定
        {
            GageRecoveryTime = 0;
            if (BoothGage < MaxBoothGage)//ブーストゲージ
            {
                BoothGage += BoothRecovery;
                if (BoothGage > MaxBoothGage) BoothGage = MaxBoothGage;
            }

            if (AbilityGauge < MaxAbilityGage)//スキルゲージ
            {
                AbilityGauge += AbilityRecovery;
                if (AbilityGauge > MaxAbilityGage) AbilityGauge = MaxAbilityGage;
            }
        }
        //ここまで

        //以下移動用
        X = MoveKey.Horizontal();
        Z = MoveKey.Vertical();

        if (X != 0 & Input.GetKeyDown(Key.Run_Key)&BoothGage>BoothConsumption || Z != 0 & Input.GetKeyDown(Key.Run_Key) & BoothGage > BoothConsumption) //ブースト発動処理
        {
            BoothGage -= BoothConsumption;
            Speed = MoveSpeed * BoothSpeed;
        } 
        else Speed=MoveSpeed;
        transform.position += (transform.forward*Z+transform.right*X) * Speed * Time.deltaTime;
        //ここまで

        //以下攻撃処理
        if (Input.GetKeyDown(Key.Attac_Key)) GunSy.Shot();
        else if (Input.GetKeyUp(Key.Attac_Key)) GunSy.KeyUP();
        //ここまで

        //以下ジャンプ処理
        if (Input.GetKeyDown(Key.Junp_Key)&BoothGage>BoothJunpConsumption)//ジャンプ処理
        {
            BoothGage -= BoothJunpConsumption;
            Rig.AddForce(new Vector3(0,JunpP,0),ForceMode.Impulse);
        }
        //ここまで

        //以下スキル関連処理
        if (Input.GetKeyDown(Skillchange))//スキル切り替え
        {
            SkillNo++;
            if (SkillNo > SkillList.Length) SkillNo = 1;
            GameSy.SkillChange(SkillNo);
        }

        if (Input.GetKeyDown(SkillUse)&AbilityGauge>AbilityConsumption)//スキル使用
        {
            AbilityGauge -= AbilityConsumption;
        }
        //ここまで

        //以下視点管理

        if (Input.GetKeyDown(LookOnKey))
        {
            LookON = !LookON;
            if (!LookON)
            {
                LookOnObj = null;
                transform.rotation=new Quaternion(0,transform.rotation.y,0,1);
            }
            else LookOnObj = GameObject.Find("VR_Player");
        }

        if (LookON)
        {
            transform.LookAt(LookOnObj.transform.position);
        }
        else
        {
            MousX = MoveKey.MousHorizontal();
            MousY = MoveKey.MousVertical();
            transform.Rotate(new Vector3(0,MousX,0)*Time.deltaTime* CameraRootSpeed);
        }
        //ここまで


        //以下UI表示管理にデータを代入
        GameSy.HPGageIN(MaxHP,HP);
        GameSy.BoothGageIN(MaxBoothGage,(int)BoothGage);
        GameSy.AbilityhGageIN(MaxAbilityGage,(int)AbilityGauge);
    　　//ここまで
    }
}
