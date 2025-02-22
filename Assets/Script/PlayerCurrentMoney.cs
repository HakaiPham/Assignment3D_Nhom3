using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCurrentMoney : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI currentMoneyText;
    public TextMeshProUGUI levelText;
    public Slider ExpSlider;
    int currentMoney = 1000;
    int level = 1;
    float exp = 0;
    float maxExp = 100;
    void Start()
    {
        currentMoneyText.text = "" + currentMoney + "$";
        moneyText.text = "+" + currentMoney + "$";
        animator.SetTrigger("CongTien");
        
        levelText.text = "Level " + level;
        ExpSlider.value = exp;
        ExpSlider.maxValue = maxExp;
        
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
        moneyText.text = "" + money + "$";
        animator.SetTrigger("TruTien");
        StartCoroutine(ObjectManager());
    }
    
    public void CongExp(int exp)
    {
        this.exp += exp;
        if (this.exp >= maxExp)
        {
            ExpSlider.value = 0;
            maxExp = maxExp * 1.1f;
            level++;
        }
        
        levelText.text = "Level " + level;
        ExpSlider.value = this.exp;
        ExpSlider.maxValue = maxExp;
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
    
    public int UpdateLevel()
    {
        Debug.Log($"[UpdateLevel] Player Level Updated: {level}");
        return level;
    }
}
