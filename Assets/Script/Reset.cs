using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour, IInteraction
{
    public Scanner Scanner;
    public void Interact()
    {
        Debug.Log("Interacting with object:" + gameObject.name);
        Scanner.TotalPrice = 0;
        Scanner.TotalPriceText.text = "";
    }
}
