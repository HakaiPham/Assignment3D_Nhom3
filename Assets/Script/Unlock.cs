using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Unlock : MonoBehaviour, IInteraction
{
    [SerializeField] private GameObject lockedObject;
    [SerializeField] private TextMeshPro lockedText;
    [SerializeField] private TextMeshPro NotText;
    [SerializeField] private GameObject Textpack;
    public int unlockamount;
    public int reciveamount;
    
    // Start is called before the first frame update
    void Start()
    {
        lockedText.text = "You need " + unlockamount + " to unlock this item";
        lockedObject.SetActive(true);
    }
    
    public void Interact()
    { 
        var a = FindFirstObjectByType(typeof(MoneyBag));
        Debug.Log("Purchasing object:" + gameObject.name);
        if (a.GetComponent<MoneyBag>().dollar >= unlockamount)
        {
            a.GetComponent<MoneyBag>().dollar -= unlockamount;
            lockedObject.SetActive(true);
            Textpack.SetActive(false);
        }
        else
        {
            NotText.text = "Not enough money";
            NotText.color = Color.red;
        }
    }
    
}
