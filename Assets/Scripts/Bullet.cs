using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour//弾
{
    bool Homing;//追尾
    GameObject HomingObj;
    // Update is called once per frame
    void Update()
    {
        if (Homing)
        {
            transform.LookAt(HomingObj.transform.position);//ロックオンオブジェクトに回転
            return;
        }
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    public void HomingDataIN(GameObject Obj)
    {
        HomingObj = Obj;
        Homing = true;
    }
}
