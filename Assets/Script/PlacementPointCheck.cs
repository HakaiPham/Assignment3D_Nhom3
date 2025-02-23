using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlacementPointCheck : MonoBehaviour, IInteraction
{
    public List<GameObject> placementPoints;
    [HideInInspector]public GameObject item;
    public void Interact()
    {
        Debug.Log("Interacting with object:" + gameObject.name);
        foreach (GameObject point in placementPoints)
        {
            if(point.GetComponent<PlacementPoint>().isInBox == false)
            {
                /*point.SetActive(true);*/
                point.GetComponent<PlacementPoint>().isInBox = true;
               
                Instantiate(item, point.transform.position, point.transform.rotation);
                return;
            }
        }
    }
}
