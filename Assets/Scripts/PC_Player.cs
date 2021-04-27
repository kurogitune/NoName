using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Exception;
public class PC_Player : MonoBehaviour
{
    [Header("カメラオブジェクト位置")]
    public Vector3 CameraPoz;
    [Header("カメラ角度")]
    public Quaternion CameraRoot;

    [Header("移動速度")]
    public float MoveSpeed;
    [Header("ブースト移動速度")]
    public float BoothSpeed;

    [Header("攻撃物")]
    public GameObject GunObj;

    [Header("体力")]
    public float MaxHP;
    [Header("ブーストゲージ")]
    public float MaxBoothGage;
    [Header("能力使用専用ゲージ")]
    public float MaxAbilityGauge;

    GameObject CameraObj;//カメラ
    Key_Data Key;//キーコード
    Gun GunSy;//銃の管理システム
    float X, Z;//移動用
    float Speed;//移動速度
    float HP;//体力
    float BoothGage;//ブーストゲージ
    float AbilityGauge;//能力用ゲージ
    void Start()
    {
        Key = KeyDataSystem.Lord();//キーのデータを取得
        CameraObj = ObjDataGet.MainCameraObj();
        CameraObj.transform.parent = transform;
        CameraObj.transform.position = transform.position + CameraPoz;
        CameraObj.transform.rotation = CameraRoot;
        GunSy = GunObj.GetComponent<Gun>();
    }

    void Update()
    {
        X = MoveKey.Horizontal();
        Z = MoveKey.Vertical();

        if (X != 0&Input.GetKeyDown(Key.Run_Key) || Z != 0 & Input.GetKeyDown(Key.Run_Key)) Speed=MoveSpeed*BoothSpeed;
        else Speed=MoveSpeed;

        if (Input.GetKeyDown(Key.Attac_Key)) GunSy.Shot();
        else if (Input.GetKeyUp(Key.Attac_Key)) GunSy.KeyUP();

        transform.position += new Vector3(X,0,Z)*Speed*Time.deltaTime;
    }
}
