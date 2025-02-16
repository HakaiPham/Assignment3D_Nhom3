using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class RefillBox : MonoBehaviour
{
   
   public int itemID;
   public GameObject item;
   public int amount;
   public string name;

   private void Start()
   {
      name = "Box of" + item.name;
   }
}
