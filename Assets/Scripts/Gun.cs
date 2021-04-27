using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
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
        if (BurstintervalT > Burstinterval)
        {
            GameObject g = Instantiate(Bullet);
            g.transform.position = BulletPoz.position;
            g.GetComponent<Rigidbody>().AddForce(BulletPoz.transform.forward * Bulletspeed * Time.deltaTime);
            BurstintervalT = 0;
        }

        if (!Rapidfire) KeyUP();
    }

    public void Shot()
    {
        Attck = true;
    }

    public void KeyUP()
    {
        Attck = false;
        BurstintervalT = Burstinterval;
    }
}
