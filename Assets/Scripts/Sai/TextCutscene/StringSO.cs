using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StringSO", menuName = "ScriptableObjects/StringSO")]
public class StringSO : ScriptableObject
{
    [TextArea(3, 10)]
    public string value;
}
