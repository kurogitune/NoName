using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SEList")]
public class SEList : ScriptableObject
{
    [Header("UIセレクトSE")]
    public AudioClip SelectSE;
    [Header("UI移動SE")]
    public AudioClip CursorMoveSE;
}
