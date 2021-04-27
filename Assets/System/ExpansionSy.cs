using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System;
using System.Text;
using UnityEngine.EventSystems;

namespace Exception//自作拡張?
{
    //以下音関連
    public static class AudioSystem //音を流すシステム
    {
        static AudioSource BGMAudio, SEAudio, VoiceAoudio;//BGM再生用　SE再生用 声再生用
        static GameObject BGM, SE, VOICE;
        public static void AudioSourceIN()
        {
            if (BGMAudio != null || SEAudio != null || VoiceAoudio != null) return;
            BGM = new GameObject("BGMAudio");
            SE = new GameObject("SEAudio");
            VOICE = new GameObject("VOICEAudio");

            BGM.AddComponent<AudioSource>();
            SE.AddComponent<AudioSource>();
            VOICE.AddComponent<AudioSource>();
            BGM.hideFlags = HideFlags.HideAndDontSave;//Hierarchyに表示しなくさせるが存在はしている
            SE.hideFlags = HideFlags.HideAndDontSave;//Hierarchyに表示しなくさせるが存在はしている
            VOICE.hideFlags = HideFlags.HideAndDontSave;//Hierarchyに表示しなくさせるが存在はしている

            BGMAudio = BGM.GetComponent<AudioSource>();
            SEAudio = SE.GetComponent<AudioSource>();
            VoiceAoudio = VOICE.GetComponent<AudioSource>();

            Option_Data OPdata = OptionDataSystem.Lord();//オプションデータ取得
            BGMAudio.volume = OPdata.BGMvolume;
            SEAudio.volume = OPdata.SEvolume;
            VoiceAoudio.volume = OPdata.Voicevolume;

            BGMAudio.playOnAwake = SEAudio.playOnAwake = VoiceAoudio.playOnAwake = false;
        }//再生機を作成

        public static void BGMVolumeIN(float volume) { BGMAudio.volume = volume; } //BGM音量変更

        public static void BGMPlaye(AudioClip BGM, bool Loop) { BGMAudio.clip = BGM; BGMAudio.loop = Loop; BGMAudio.Play(); }//BGM再生

        public static void BGMPause() { BGMAudio.Pause(); }//BGMを一時停止
        public static void BGMUnPause() { BGMAudio.UnPause(); }//BGMを一時停止終了

        public static void BGMStop() { BGMAudio.Stop(); }//BGM停止

        public static void SEVolumeIN(float volume) { SEAudio.volume = volume; }//SE音量変更

        public static void SEPlaye(AudioClip Se) { SEAudio.PlayOneShot(Se); }//SE再生

        public static void VoiceVolume(float volume) { VoiceAoudio.volume = volume; }//ボイスの音量変更

        public static void VoicePlaye(AudioClip Se) { VoiceAoudio.PlayOneShot(Se); }//ボイス再生
        public static void VoicePause() { VoiceAoudio.Pause(); }//ボイスを一時停止
        public static void VoiceUnPause() { VoiceAoudio.UnPause(); }//ボイスを一時停止終了

        public static void AllVolume(float BGMv, float SEv, float voicev) { BGMAudio.volume = BGMv; SEAudio.volume = SEv; VoiceAoudio.volume = voicev; }//BMG,SE,Voice音量同時変更
        public static void AudioSourceDes()
        { BGMAudio = null; SEAudio = null; VoiceAoudio = null; BGMAudio.playOnAwake = SEAudio.playOnAwake = VoiceAoudio.playOnAwake = true; }
    }
    //ここまで


    //以下通信関連
    public class Savar_Data//クライン受付サーバーのIPアドレスポート番号
    {
        public string IP = "";
        public int Port = 0;
    }

    public class SaverDataSystem//IPアドレスポート番号ロードセーブシステム
    {
        static string FileName = "/Data";
        static string FileDataName = "/IP_Data.json";
        public static void Save(Savar_Data data)//IPアドレスセーブ
        {
            try
            {
                StreamWriter strw;
                string jos = JsonUtility.ToJson(data);
                strw = new StreamWriter(new FileStream(Application.dataPath + FileName+ FileDataName, FileMode.Create));//ファイルがある場合上書き
                strw.Write(jos);
                strw.Flush();
                strw.Close();
            }
            catch
            {

                if (!Directory.Exists(Application.dataPath + FileName))//フォルダーがある確認 ない場合フォルダー作成
                {
                    Debug.Log("保存先のフォルダーがないためフォルダーを作成します");
                    Directory.CreateDirectory(Application.dataPath + FileName);//フォルダー作成
                }
                Debug.Log("データを作成");
                StreamWriter strw;
                string jos = JsonUtility.ToJson(data);
                strw = new StreamWriter(new FileStream(Application.dataPath + FileName + FileDataName, FileMode.OpenOrCreate));
                strw.Write(jos);
                strw.Flush();
                strw.Close();
            }
        }

        public static Savar_Data Lord()//IPアドレスロード
        {
            try
            {
                StreamReader str;
                str = new StreamReader(new FileStream(Application.dataPath + FileName + FileDataName, FileMode.Open));
                string json = str.ReadToEnd();
                str.Close();
                return JsonUtility.FromJson<Savar_Data>(json);
            }
            catch
            {
                Savar_Data data = new Savar_Data();
                Save(data);

                StreamReader str;
                str = new StreamReader(new FileStream(Application.dataPath + FileName + FileDataName, FileMode.Open));
                string json = str.ReadToEnd();
                str.Close();
                return JsonUtility.FromJson<Savar_Data>(json);
            }
        }
    }

    public class Tuusin//自作通信システム
    {
        static Encoding ecn = Encoding.UTF8;//文字コード指定 
        static NetworkStream nst;//サーバーデータ
        static UdpClient souisinudp;//ゲームサーバールームデータ
        static UdpClient zyusinudp;//受信用

        public static void nstIN(NetworkStream ns)//サーバーデータを取得
        {
            nst = ns;
        }

        public static void tuusinKill()//通信エラーの場合サーバーデータを削除
        {
            nst = null;
            souisinudp = null;
        }

        public static void TCPKILL()
        {
            nst = null;
        }

        public static void UDPIN(UdpClient ud, UdpClient zudp)//UDPデータを代入
        {
            souisinudp = ud;
            zyusinudp = zudp;
        }

        public static void TCPsosin(string s)//データ送信処理
        {
            byte[] bun = ecn.GetBytes(s + '\n');//byteデータ作
            nst.Write(bun, 0, bun.Length);//送信
        }

        public static string TCPzyusi()//受信処理　タイムアウトあり
        {
            string s = "";
            nst.WriteTimeout = 1000;//受信タイムアウト設定　１秒
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] resByte = new byte[256];
                    int resSize = 0;//受信した文字数

                    do
                    {
                        resSize = nst.Read(resByte, 0, resByte.Length);//文字数記録
                        if (resSize == 0) break;

                        ms.Write(resByte, 0, resSize);//受信データ蓄積　文字　？　文字数
                    }
                    while (nst.DataAvailable || resByte[resSize - 1] != '\n');//読み取り可能データがあるか、データの最後が\nではない場合は受信を継続

                    string resMsg = ecn.GetString(ms.GetBuffer(), 0, (int)ms.Length);//受信データを文字列に変換
                    resMsg = resMsg.TrimEnd('\n');//文字最後の\nを消す
                    s = resMsg;
                }
            }
            catch
            {
                s = "11";
                Debug.Log("データなし");
            }
            return s;
        }

        public static string TCPzyusinTime_NO()//データ受信でタイムアウトなし版
        {
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] resByte = new byte[256];
                int resSize = 0;//受信した文字数

                do
                {
                    resSize = nst.Read(resByte, 0, resByte.Length);//文字数記録
                    if (resSize == 0) break;

                    ms.Write(resByte, 0, resSize);//受信データ蓄積　文字　？　文字数
                }
                while (nst.DataAvailable || resByte[resSize - 1] != '\n');//読み取り可能データがあるか、データの最後が\nではない場合は受信を継続

                string resMsg = ecn.GetString(ms.GetBuffer(), 0, (int)ms.Length);//受信データを文字列に変換
                resMsg = resMsg.TrimEnd('\n');//文字最後の\nを消す
                return resMsg;
            }
        }

        public static void UDPsousin(string s)//UDPでのデータ送信
        {
            byte[] bun = ecn.GetBytes(s);//送信データ作成
            souisinudp.Send(bun, bun.Length);//送信
        }

        public static string UDPzyusin()//UDPでの受信
        {
            IPEndPoint ep = null;
            byte[] zusin = zyusinudp.Receive(ref ep);
            return ecn.GetString(zusin);
        }
    }
    //ここまで

    public class ObjDataGet  //以下指定のタグのオブジェクトを全て取得させる処理
    {
        public static GameObject PlayerObjGet()//プレイヤーのタグが付いたオブジェクトを取得
        {
            GameObject RetumObj = null;
            foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
            {
                if (obj.tag == "Player")
                {
                    RetumObj = obj;
                }
            }
            return RetumObj;
        }

        public static GameObject MainCameraObj()//メインカメラのタグが付いたオブジェクトを取得
        {
            GameObject RetumObj = null;
            foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
            {
                if (obj.tag == "MainCamera")
                {
                    RetumObj = obj;
                }
            }
            return RetumObj;
        }

        public static GameObject SelctTagObj(string tag)//選択したタグの付いたオブジェクト取得（ただし指定タグは一つしか使用していない物に使う事を推奨）
        {
            GameObject RetumObj = null;
            foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
            {
                if (obj.tag == tag)
                {
                    RetumObj = obj;
                }
            }
            return RetumObj;
        }

        public static List<GameObject> Tag_All_obj(string tag)//指定したタグの付いたオブジェクトリストを取得
        {
            List<GameObject> objdata = new List<GameObject>();
            List<GameObject> returnobjdata = new List<GameObject>();
            foreach (GameObject boj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
            {
                if (boj.tag == tag)
                {
                    objdata.Add(boj);
                }
            }
            for (int i = objdata.Count - 1; i > -1; i--)
            {
                returnobjdata.Add(objdata[i]);
            }
            return returnobjdata;
        }
    }
    //ここまで


    //以下操作キー操作コントローラーボタン設定関連
    public class Key_Data//初期操作キーデータ
    {
        //以下キー入力での操作
        public KeyCode Junp_Key = KeyCode.Space;//ジャンプキー
        public KeyCode Run_Key = KeyCode.LeftShift;//走るキー
        public KeyCode Attac_Key = KeyCode.Mouse0;//攻撃キー 
        public KeyCode LookON_Key = KeyCode.Tab;//ロックオンキー
        public KeyCode LookOFF_Key = KeyCode.Q;//ロックオン解除キー
        public KeyCode Itemuse_Key = KeyCode.E;//アイテム使用キー

        //以下コントローラーでの入力
        public KeyCode Junp_Con = KeyCode.Joystick1Button0;//ジャンプボタン
        public KeyCode Run_Con = KeyCode.Joystick1Button8;//走るボタン
        public KeyCode Attack_Con = KeyCode.Joystick1Button1;//攻撃ボタン
        public KeyCode LookON_Con = KeyCode.Joystick1Button9;//ロックオンボタン
        public KeyCode LookOFF_Con = KeyCode.Joystick1Button3;//ロックオン解除ボタン
        public KeyCode Itemuse_Con = KeyCode.Joystick1Button2;//アイテム使用ボタン
    }
    public class KeyDataSystem//操作キーデータロードセーブ
    {
        static string FileName = "/Data";
        static string FileDataName = "/KeyData.json";
        public static void Save(Key_Data data)//オプションセーブ
        {
            try
            {
                StreamWriter strw;
                string jos = JsonUtility.ToJson(data);
                strw = new StreamWriter(new FileStream(Application.dataPath + FileName+FileDataName, FileMode.Create));//ファイルがある場合上書き
                strw.Write(jos);
                strw.Flush();
                strw.Close();
            }
            catch
            {

                if (!Directory.Exists(Application.dataPath + FileName))//フォルダーがある確認 ない場合フォルダー作成
                {
                    Debug.Log("保存先のフォルダーがないためフォルダーを作成します");
                    Directory.CreateDirectory(Application.dataPath + FileName);//フォルダー作成
                }
                Debug.Log("データを作成");
                StreamWriter strw;
                string jos = JsonUtility.ToJson(data);
                strw = new StreamWriter(new FileStream(Application.dataPath + FileName + FileDataName, FileMode.OpenOrCreate));
                strw.Write(jos);
                strw.Flush();
                strw.Close();
            }
        }

        public static Key_Data Lord()//オプションロード
        {
            try
            {
                StreamReader str;
                str = new StreamReader(new FileStream(Application.dataPath + FileName + FileDataName, FileMode.Open));
                string json = str.ReadToEnd();
                str.Close();
                return JsonUtility.FromJson<Key_Data>(json);
            }
            catch
            {
                Key_Data data = new Key_Data();
                Save(data);

                StreamReader str;
                str = new StreamReader(new FileStream(Application.dataPath + FileName + FileDataName, FileMode.Open));
                string json = str.ReadToEnd();
                str.Close();
                return JsonUtility.FromJson<Key_Data>(json);
            }
        }
    }

    public class MoveKey//移動のキーを返す
    {
        static string Hol = "Horizontal", Ver = "Vertical";
        static string MousHol = "Mouse X", MousVer = "Mouse Y";
        
        public static bool Horizontal_Right(){ bool b = false; if (Input.GetAxisRaw(Hol) > 0) b = true;  return b; }
        public static bool Horizonta_Left(){ bool b = false; if (Input.GetAxisRaw(Hol) < 0) b = true;  return b; }
        public static bool Vertical_Up(){ bool b = false; if (Input.GetAxisRaw(Ver) > 0) b = true;  return b; }
        public static bool Vertical_Down(){ bool b = false; if (Input.GetAxisRaw(Ver) < 0) b = true;  return b; }
        public static float Horizontal(){return Input.GetAxis(Hol);}
        public static float Vertical() {return Input.GetAxis(Ver);}

        //以下マウス座標関連
        public static bool Mous_Right() { bool b = false; if (Input.GetAxisRaw(MousHol) > 0) b = true; return b; }
        public static bool Mous_Left() { bool b = false; if (Input.GetAxisRaw(MousHol) < 0) b = true; return b; }
        public static bool Mous_Up() { bool b = false; if (Input.GetAxisRaw(MousVer) > 0) b = true; return b; }
        public static bool Mous_Down() { bool b = false; if (Input.GetAxisRaw(MousVer) < 0) b = true; return b; }
        public static float MousHorizontal() { return Input.GetAxis(MousHol); }
        public static float MousVertical() { return Input.GetAxis(MousVer); }
    }
    //ここまで


    //以下オプション設定関連
    public class Option_Data//設定データ
    {
        public float BGMvolume = 1;//BGM音量
        public float SEvolume = 1;//効果音音量
        public float Voicevolume = 1;//声音量
        public float RoteSpeed = 1;//声音量
        public int SelctoButton = 0;//選択ボタン 0:Xbox 1:PS4
    }

    public class OptionDataSystem//オプションロードセーブ
    {
        static string FileName = "/Data";
        static string FileDataName = "/Option_Data.json";
        public static void Save(Option_Data data)//オプションセーブ
        {
            try
            {
                StreamWriter strw;
                string jos = JsonUtility.ToJson(data);
                strw = new StreamWriter(new FileStream(Application.dataPath + FileName + FileDataName, FileMode.Create));//ファイルがある場合上書き
                strw.Write(jos);
                strw.Flush();
                strw.Close();
            }
            catch
            {

                if (!Directory.Exists(Application.dataPath + FileName))//フォルダーがある確認 ない場合フォルダー作成
                {
                    Debug.Log("保存先のフォルダーがないためフォルダーを作成します");
                    Directory.CreateDirectory(Application.dataPath + FileName);//フォルダー作成
                }
                Debug.Log("データを作成");
                StreamWriter strw;
                string jos = JsonUtility.ToJson(data);
                strw = new StreamWriter(new FileStream(Application.dataPath + FileName + FileDataName, FileMode.OpenOrCreate));
                strw.Write(jos);
                strw.Flush();
                strw.Close();
            }
        }

        public static Option_Data Lord()//オプションロード
        {
            try
            {
                StreamReader str;
                str = new StreamReader(new FileStream(Application.dataPath + FileName + FileDataName, FileMode.Open));
                string json = str.ReadToEnd();
                str.Close();
                return JsonUtility.FromJson<Option_Data>(json);
            }
            catch
            {
                Option_Data data = new Option_Data();
                Save(data);

                StreamReader str;
                str = new StreamReader(new FileStream(Application.dataPath + FileName + FileDataName, FileMode.Open));
                string json = str.ReadToEnd();
                str.Close();
                return JsonUtility.FromJson<Option_Data>(json);
            }
        }
    }
    public static class KeySelcotChange//選択キーの入れ替え
    {
        public static void KeyChange(int i)//決定キーの入れ替え　0：XBox標準 1:PS4系
        {
            StandaloneInputModule UI_InputSy = GameObject.Find("EventSystem").GetComponent<StandaloneInputModule>();

            switch (i)
            {
                case 0://標準
                    UI_InputSy.submitButton = "Submit";
                    UI_InputSy.cancelButton = "Cancel";
                    break;

                case 1://PS4
                    UI_InputSy.submitButton = "Submit_C";
                    UI_InputSy.cancelButton = "Cancel_C";
                    break;
            }
        }
    }
    //ここまで


    public static class KeyNameLog//入力されたボタンの名前を取得
    {
        public static string KyeName()
        {
            string retuns = "";
            if (Input.anyKeyDown)
            {
                foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(code))
                    {
                        // 入力されたキー名を表示
                        retuns = code.ToString();
                    }
                }
            }
            else retuns = "NotPushKey"; 
            return retuns;
        }
    }
}
