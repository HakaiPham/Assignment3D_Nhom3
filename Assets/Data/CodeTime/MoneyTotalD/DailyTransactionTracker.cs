using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DailyTransactionTracker : MonoBehaviour
{
    public TimeController timeController; // Đối tượng quản lý thời gian
    public CheckOutCounter checkOutCounter; // Quầy thanh toán

    public int totalIncome = 0; // Tổng tiền cộng trong ngày
    public int totalExpense = 0; // Tổng tiền trừ trong ngày
    public int netEarnings = 0; // Thu nhập thực tế = tổng cộng - tổng trừ

    public TextMeshProUGUI incomeText; // UI hiển thị tiền cộng
    public TextMeshProUGUI expenseText; // UI hiển thị tiền trừ
    public TextMeshProUGUI netEarningsText; // UI hiển thị tổng thu nhập

    private int lastRecordedDay = -1; // Ngày cuối cùng được ghi nhận

    void Start()
    {
        UpdateUI();
        if (timeController != null)
        {
            lastRecordedDay = timeController.GetCurrentDay();
        }
    }

    void Update()
    {
        // Kiểm tra nếu ngày mới bắt đầu, reset dữ liệu
        if (timeController != null && timeController.GetCurrentDay() != lastRecordedDay)
        {
            ResetDailyTransactions();
            lastRecordedDay = timeController.GetCurrentDay();
        }
    }

    /// <summary>
    /// Cộng tiền vào thu nhập
    /// </summary>
    public void AddIncome(int amount)
    {
        totalIncome += amount;
        netEarnings = totalIncome - totalExpense;
        UpdateUI();
    }

    /// <summary>
    /// Trừ tiền khỏi tổng thu nhập
    /// </summary>
    public void AddExpense(int amount)
    {
        totalExpense += Mathf.Abs(amount); // Đảm bảo amount luôn là số dương
        netEarnings = totalIncome - totalExpense;
        UpdateUI();
    }

    /// <summary>
    /// Cập nhật hiển thị trên UI
    /// </summary>
    void UpdateUI()
    {
        if (incomeText != null) incomeText.text = "Tiền cộng: " + totalIncome + "$";
        if (expenseText != null) expenseText.text = "Tiền trừ: " + totalExpense + "$";
        if (netEarningsText != null) netEarningsText.text = "Tổng thu nhập: " + netEarnings + "$";
    }

    /// <summary>
    /// Reset dữ liệu khi sang ngày mới
    /// </summary>
    public void ResetDailyTransactions()
    {
        totalIncome = 0;
        totalExpense = 0;
        netEarnings = 0;
        UpdateUI();
    }
}