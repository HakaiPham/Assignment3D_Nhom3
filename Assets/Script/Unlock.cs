using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Unlock : MonoBehaviour
{
    [SerializeField] private GameObject lockedObject;
    [SerializeField] private TextMeshPro lockedText;
    [SerializeField] private GameObject Textpack;
    public int unlockamount;
    public int levelneed;
    
    // Start is called before the first frame update
    void Start()
    {
        lockedText.text = "You need " + unlockamount + " to unlock this item" + " and level " + levelneed;
        lockedText.color = Color.white;
        lockedObject.SetActive(false);
    }
    
    public void OnTriggerEnter(Collider other)
    {
        
        Textpack.SetActive(true);
        lockedText.text = "You need " + unlockamount + " to unlock this item" + " and level " + levelneed;
        lockedText.color = Color.white;
        if (other.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {
            var a = FindFirstObjectByType(typeof(PlayerCurrentMoney));
            Debug.Log("Purchasing object:" + gameObject.name);
            if (a.GetComponent<PlayerCurrentMoney>().UpdateCurrentMoneY() >= unlockamount && a.GetComponent<PlayerCurrentMoney>().UpdateLevel() >= levelneed)
            {
                a.GetComponent<PlayerCurrentMoney>().TruTien(-unlockamount);
                lockedObject.SetActive(true);
                Textpack.SetActive(false);
                gameObject.SetActive(false);
            }
            else
            {
                lockedText.text = "Not enough";
                lockedText.color = Color.red;
            }
        }
        
    }
    
    public void OnTriggerStay(Collider other)
    {
        
        Textpack.SetActive(true);
        if (other.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {
            var a = FindFirstObjectByType(typeof(PlayerCurrentMoney));
            Debug.Log("Purchasing object:" + gameObject.name);
            if (a.GetComponent<PlayerCurrentMoney>().UpdateCurrentMoneY() >= unlockamount && a.GetComponent<PlayerCurrentMoney>().UpdateLevel() >= levelneed)
            {
                a.GetComponent<PlayerCurrentMoney>().TruTien(-unlockamount);
                lockedObject.SetActive(true);
                Textpack.SetActive(false);
                gameObject.SetActive(false);
            }
            else
            {
                lockedText.text = "Not enough money";
                lockedText.color = Color.red;
            }
        }
        
    }
    
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Textpack.SetActive(false);
        }
    }
    
}
