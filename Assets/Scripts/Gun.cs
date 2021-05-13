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
    public Transform Muzzle;
    [Header("弾速")]
    public float Bulletspeed;
    [Header("射程")]
    public float Rang;

    bool Attck;//攻撃位置
    float BurstintervalT;//攻撃間隔
    // Start is called before the first frame update
    void Start()
    {
        Attck = false;
        BurstintervalT = Burstinterval;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Muzzle.position,Muzzle.transform.forward*Rang);
        if (!Attck) return;

        BurstintervalT += Time.deltaTime;
        if (BurstintervalT > Burstinterval)//連射処理
        {
            GameObject g = Instantiate(Bullet);
            g.transform.position = Muzzle.position;
            g.GetComponent<Rigidbody>().AddForce(Muzzle.transform.forward * Bulletspeed * Time.deltaTime);
            BurstintervalT = 0;
            Destroy(g,2f);
        }

        if (!Rapidfire) KeyUP();//連射武器では無い場合
    }

    public void Shot()//砲撃処理
    {
        Attck = true;
    }

    public void KeyUP()
    {
        Attck = false;
        BurstintervalT = Burstinterval;
    }

    public Transform MuzzlPozOUT()//銃口の位置を出力
    {
        return Muzzle;
    }
}
