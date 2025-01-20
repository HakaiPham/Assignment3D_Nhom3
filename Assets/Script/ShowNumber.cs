using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowNumber : MonoBehaviour
{
    public float dollar;
    public TextMeshPro BoxText;


    public void AddIn(float value)
    {
        dollar += value;
        MathF.Round(dollar,2);
        BoxText.text = "$" + dollar.ToString();
    }
}
