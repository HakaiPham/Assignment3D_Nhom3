using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCurrentMoney : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI currentMoneyText;
    int currentMoney = 1000;
    void Start()
    {
        currentMoneyText.text = "" + currentMoney + "$";
        moneyText.text = "+" + currentMoney + "$";
        animator.SetTrigger("CongTien");
        StartCoroutine(ObjectManager());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CongTien(int money)
    {
        moneyText.gameObject.SetActive(true);
        currentMoney += money;
        currentMoneyText.text = "" + currentMoney + "$";
        moneyText.text = "+" + money + "$";
        animator.SetTrigger("CongTien");
        StartCoroutine(ObjectManager());
    }
    public void TruTien(int money)
    {
        moneyText.gameObject.SetActive(true);
        currentMoney += money;
        currentMoneyText.text = "" + currentMoney + "$";
        moneyText.text = "-" + money + "$";
        animator.SetTrigger("TruTien");
        StartCoroutine(ObjectManager());
    }
    IEnumerator ObjectManager()
    {
        yield return new WaitForSeconds(0.5f);
        moneyText.gameObject.SetActive(false);
    }
    public int UpdateCurrentMoneY()
    {
        Debug.Log($"[UpdateCurrentMoneY] Player Money Updated: {currentMoney}");
        return currentMoney;
    }
}
