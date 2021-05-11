using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BGMList")]
public class BGMList : ScriptableObject
{
    [Header("タイトルBGM")]
    public AudioClip TitleBGM;//タイトルBGM
    [Header("ゲームBGM")]
    public AudioClip GameBGM;//ゲームBGM
}
