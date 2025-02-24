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
    int currentMoney = 500;
    int level = 1;
    float exp = 0;
    float maxExp = 100;

    void Start()
    {
        //PlayerPrefs.DeleteKey("CurrentMoney");
        currentMoney = PlayerPrefs.GetInt("CurrentMoney", 500);
        currentMoneyText.text = "" + currentMoney + "$";
        moneyText.text = "+" + currentMoney + "$";
        animator.SetTrigger("CongTien");
        level = PlayerPrefs.GetInt("CurrentLevel",1);
        levelText.text = "Level " + level;
        exp = PlayerPrefs.GetFloat("CurrentExp",0f);
        ExpSlider.value = exp;
        maxExp = PlayerPrefs.GetFloat("MaxExp",100f);
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
        PlayerPrefs.SetInt("CurrentMoney", currentMoney);
        PlayerPrefs.Save();
        currentMoneyText.text = "" + currentMoney + "$";
        moneyText.text = "+" + money + "$";
        animator.SetTrigger("CongTien");
        StartCoroutine(ObjectManager());
    }
    public void TruTien(int money)
    {
        moneyText.gameObject.SetActive(true);
        currentMoney += money;
        PlayerPrefs.SetInt("CurrentMoney", currentMoney);
        PlayerPrefs.Save();
        currentMoneyText.text = "" + currentMoney + "$";
        moneyText.text = "" + money + "$";
        animator.SetTrigger("TruTien");
        StartCoroutine(ObjectManager());
    }
    
    public void CongExp(int exp)
    {
        Debug.Log(">>>>");
        this.exp += exp;
        ExpSlider.value = this.exp;
        if (this.exp >= maxExp)
        {
            this.exp = 0; // ✅ Giữ lại phần exp dư khi lên cấp
            ExpSlider.value = this.exp;
            maxExp *= 1.1f;
            ExpSlider.maxValue = maxExp;
            PlayerPrefs.SetFloat("MaxExp", maxExp);
            level++;
            PlayerPrefs.SetInt("CurrentLevel", level);
            levelText.text = "Level " + level;
        }
        PlayerPrefs.SetFloat("CurrentExp", this.exp);
        PlayerPrefs.Save();
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
