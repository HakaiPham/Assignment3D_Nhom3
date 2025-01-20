using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class AddDollar : MonoBehaviour, IInteraction
{
    public ShowNumber showNumber;
    public float value;
    public void Interact()
    {
        Debug.Log("Interacting with object:" + gameObject.name);
        showNumber.AddIn(value);
    }
}
