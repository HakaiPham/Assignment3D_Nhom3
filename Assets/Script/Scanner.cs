using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public List<GameObject> ScannedItems;
    public TextMeshPro TotalPriceText;
    public float TotalPrice;
    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
            if (other.CompareTag("Goods") && !ScannedItems.Contains(other.gameObject))
            {
                other.GetComponent<ItemData>();
                TotalPrice += other.GetComponent<ItemData>().price;
                ScannedItems.Add(other.gameObject);
            }
        TotalPriceText.text = "Total Price: " + "$" + TotalPrice;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Goods") && !ScannedItems.Contains(other.gameObject))
        {
            other.GetComponent<ItemData>();
            TotalPrice += other.GetComponent<ItemData>().price;
            ScannedItems.Add(other.gameObject);
        }
        TotalPriceText.text = "Total Price: " + "$" + TotalPrice;
    }
}
