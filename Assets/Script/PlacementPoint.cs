using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementPoint : MonoBehaviour
{
    public bool isInBox;

    private void Start()
    {
        //isInBox = true;
    }

    /*void Update(){
        if(isInBox){
            Debug.Log("Found in box!");
            gameObject.SetActive(true);
        } /*else {
            Debug.Log("Not in box!");
            gameObject.SetActive(false);
        }#1#
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CanPickup")){
            isInBox = true;
        }
    }

  
    void OnTriggerExit(Collider other){
        if(other.CompareTag("CanPickup")){
            isInBox = false;
        }
    }
}
