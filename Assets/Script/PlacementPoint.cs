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

    void Update(){
        if(isInBox){
            Debug.Log("Found in box!");
        } else {
            Debug.Log("Not in box!");
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Goods")){
            isInBox = true;
        }
    }

    void OnTriggerStay(Collider other){
        if(other.CompareTag("Goods")){
            isInBox = true;
        }
        else
        {
            isInBox = false;
        }
    }
    void OnTriggerExit(Collider other){
        if(other.CompareTag("Goods")){
            isInBox = false;
        }
    }
}
