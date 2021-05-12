using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour//銃管理
{
    [Header("連射")]
    public bool Rapidfire;
    [Header("連射間隔")]
    public float Burstinterval;
    [Header("使用弾薬")]
    public GameObject Bullet;
    [Header("攻撃位置")]
    public Transform BulletPoz;
    [Header("弾速")]
    public float Bulletspeed;

    bool Attck;//攻撃位置
    float BurstintervalT;//攻撃間隔
    GameObject LookObj;
    // Start is called before the first frame update
    void Start()
    {
        Attck = false;
        BurstintervalT = Burstinterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Attck) return;

        BurstintervalT += Time.deltaTime;
        if (BurstintervalT > Burstinterval)//連射処理
        {
            GameObject g = Instantiate(Bullet);
            g.transform.position = BulletPoz.position;
            g.GetComponent<Rigidbody>().AddForce(BulletPoz.transform.forward * Bulletspeed * Time.deltaTime);
         //   if (LookObj != null) g.GetComponent<Bullet>().HomingDataIN(LookObj);
            BurstintervalT = 0;
        }

        if (!Rapidfire) KeyUP();//連射武器では無い場合
    }

    public void Shot(GameObject Obj)//砲撃処理
    {
        Attck = true;
        LookObj = Obj;
    }

    public void KeyUP()
    {
        Attck = false;
        BurstintervalT = Burstinterval;
    }
}
