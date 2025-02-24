using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlacementPointCheck : MonoBehaviour, IInteraction
{
    public List<GameObject> placementPoints;
    [HideInInspector]public GameObject item;
    private void Start()
    {
    }
    public void Interact()
    {
        Debug.Log("Interacting with object:" + gameObject.name);
        foreach (GameObject point in placementPoints)
        {
            if(point.GetComponent<PlacementPoint>().isInBox == false)
            {
                /*point.SetActive(true);*/
                point.GetComponent<PlacementPoint>().isInBox = true;
                GameObject items = Instantiate(item, point.transform.position, point.transform.rotation);
                items.transform.SetParent(point.transform);
                items.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                return;
            }
        }
    }
   
}
