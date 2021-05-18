using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class Skill_Editor : EditorWindow//スキルエディタ
{
    static string AssetFileName = "/AssetObj/SkillAsst";//武器アセットオブジェクトの保存先フォルフダー
    static string AssetListFileName = "/AssetObj/AsstList";
    static string WeaponListDataPath = "/AssetObj/AsstList/SkillDataList.asset";//リストの保存先のパス
    static SkillList SkillLis = null;
    [MenuItem("Editor/WeapoEditor %#Z")]
    static void Open()
    {
        if (!Directory.Exists(Application.dataPath + "/AssetObj"))//フォルダー確認
        {
            Debug.Log("AssetObjフォルダー作成");
            Directory.CreateDirectory(Application.dataPath + "/AssetObj");//ない場合作成
            AssetDatabase.Refresh();//Unityのファイル表示を更新  
        }

        if (!Directory.Exists(Application.dataPath + AssetFileName))
        {
            Directory.CreateDirectory(Application.dataPath + AssetFileName);
            Debug.Log("WeapoAssetフォルダー作成");
            AssetDatabase.Refresh();//Unityのファイル表示を更新  
        }

        if (!Directory.Exists(Application.dataPath + AssetListFileName))
        {
            Directory.CreateDirectory(Application.dataPath + AssetListFileName);
            Debug.Log("AsstListフォルダー作成");
            AssetDatabase.Refresh();//Unityのファイル表示を更新  
        }

        if (!File.Exists(Application.dataPath + WeaponListDataPath))
        {
            AssetDatabase.CreateAsset(new SkillList(), "Assets" + WeaponListDataPath);//ない場合作成
            SkillLis = (SkillList)AssetDatabase.LoadAssetAtPath("Assets" + WeaponListDataPath, typeof(SkillList));//作成したファイルをロード
        }
        else
        {
            SkillLis = (SkillList)AssetDatabase.LoadAssetAtPath("Assets" + WeaponListDataPath, typeof(SkillList));//ある場合データファイルロード
        }
        GetWindow<Skill_Editor>("SkillEditor"); // タイトル名を指定
    }


    //こいつらは処理用
    int ItemNo, SelctitemNo = -1;//スキル番号　　選択アイテム番号　セレクト番号

    //以下に変更物のデータ変数を定義
    string SkillName = "";//スキル名前
    int SkillNo;//スキル番号
    float AbilityConsumption;//スキルゲージ消費量
    float EffectTime;//効果時間
    bool SkillTarget;//True:自身以外 False:自身
    SkillType Type;//スキルタイプ
    SkillAvailable Available;//スキル使用可能者
    float Range;//スキルタイプ射程
    float AttackUP_P;//攻撃UP倍率
    float RecoveRyamount;//回復量
    //ここまで

    //左表示関連
    SkillData[]SkillDataList = new SkillData[0];//データ
    Vector2 LehtBox = Vector2.zero;
    Vector2 RigitBox = Vector2.zero;
    private void OnGUI()
    {
        //以下武器データ取得処理
        DirectoryInfo dir = new DirectoryInfo("Assets" + AssetFileName);//ファイルパス
        FileInfo[] fild = dir.GetFiles("*.asset");//こいつでファイル名を取得
        if (SkillDataList.Length != fild.Length)
        {
            SkillDataList = new SkillData[fild.Length];
            for (int i = 0; i < fild.Length; i++)
            {
                string path = dir + "/" + fild[i].Name;
                string WeaponName = fild[i].Name.Substring(0, fild[i].Name.Length - 6);
                SkillDataList[i] = (SkillData)AssetDatabase.LoadAssetAtPath(path, typeof(SkillData));
            }
        }
        //ここまで

        using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
        {
            //以下データ選択表示処理
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                if (SkillDataList.Length != 0)
                {
                    EditorGUILayout.LabelField("スキル選択");
                    EditorGUILayout.LabelField("現在のスキル数 : " + SkillDataList.Length);

                    LehtBox = EditorGUILayout.BeginScrollView(LehtBox, GUI.skin.box);//選択枠
                    {
                        for (int i = 0; i < SkillDataList.Length; i++)
                        {
                            if (GUILayout.Button(SkillDataList[i].SkillName))
                            {
                                if (i != ItemNo) ItemNo = i;
                            }
                            GUI.backgroundColor = GUI.color;
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
                else
                {
                    EditorGUILayout.LabelField("NoData");
                    SkillName = "";
                    SkillNo = 0;
                }
            }
            //ここまで

            //以下データ変更処理
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))//ここから縦並び
            {
                RigitBox = EditorGUILayout.BeginScrollView(RigitBox, GUI.skin.box);//選択枠
                {
                    EditorGUI.BeginDisabledGroup(SkillDataList.Length == 0);//こいつで囲んだボタンをおせなくする
                    if (SkillDataList.Length != 0) EditorGUILayout.LabelField("スキルデータ変更 : " + SkillDataList[ItemNo].SkillName);
                    else EditorGUILayout.LabelField("スキルデータ変更");
                    //以下に変更処理を書く

                    //ここまで
                }

                EditorGUI.EndDisabledGroup();//ここまで

                if (SkillDataList.Length != 0)//代入先がある場合
                {
                    if (ItemNo != SelctitemNo)//代入先が変更されたら
                    {
                        SelctitemNo = ItemNo;
                        DataReset();
                    }

                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        bool Data_change = true;

                        if (Data_change) Data_change = false;//データが更新されたか

                        EditorGUI.BeginDisabledGroup(Data_change);//こいつで囲んだボタンをおせなくする
                        if (GUILayout.Button("Reset"))
                        {
                            DataReset();
                        }

                        if (GUILayout.Button("Save"))
                        {
                            //データ保存

                            EditorUtility.SetDirty(SkillDataList[SelctitemNo]);//指定したScriptObject変更を記録
                            AssetDatabase.SaveAssets();//ScriptObjectをセーブする
                            DataReset();
                        }
                        EditorGUI.EndDisabledGroup();//ここまで
                    }
                }
                EditorGUILayout.EndScrollView();

            }//ここまで
        }

        var e = Event.current;
        switch (e.type)
        {
            case EventType.ContextClick://右クリックメニュー
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("スキルを追加"), false, () => { File_Set(); });
                menu.ShowAsContext();
                break;
        }
    }

    void DataReset()//一時保存データ初期化
    {
    }

    void File_Set()//ファイル追加処理
    {
        string DefName = "NewWeponData.asset";//初期値の名前
        DefName = Path.GetFileNameWithoutExtension(AssetDatabase.GenerateUniqueAssetPath(Path.Combine("Assets" + AssetFileName, DefName)));//同じ名前のものがあるかを判定
        var pas = EditorUtility.SaveFilePanelInProject("スキルを追加", DefName, "asset", "", "Assets" + AssetFileName);
        if (!string.IsNullOrEmpty(pas))//保存処理
        {
            string[] name1 = pas.Split('/');
            string WeaponName = name1[name1.Length - 1].Substring(0, name1[name1.Length - 1].Length - 6);
            SkillData Savedata = new SkillData();
            Savedata.SkillName = WeaponName;//ファイル名を代入
            Savedata.SkillNo = SkillDataList.Length + 1;

            AssetDatabase.CreateAsset(Savedata, pas);
            AssetDatabase.Refresh();
        }
    }

    private void OnDestroy()
    {
       SkillLis.PCPLayer_SkillLis =SkillDataList;//スキルリストに作成したスキルリストを保存
    }
}
