using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class CheckOutCounter : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform checkOutPoint;
    private Queue<GameObject> customerQueue = new Queue<GameObject>();//hàng đợi khách hàng
    public bool isCheckoutBusy = false;//Trạng thái quầy thanh toán
    public float queueSpacing = 1.5f;// Khoảng cách của các khách hàng trong hàng đợi
    public int _ToTalMoney = 0; // Tổng tiền cần thanh toán
    public TextMeshProUGUI _TotalMoneyText;
    public int _TienNhan = 0;
    public TextMeshProUGUI _TienNhanText;
    public int _TienThua = 0;
    public TextMeshProUGUI _TienThuaText;
    int tienthuahientai = 0;
    public PlayerCurrentMoney _playerMoney;
    public TextMeshProUGUI warringText;
    public RayCastCheckOut rayCastCheckOutManager;
    void Start()
    {
        _TotalMoneyText.text = "" + _ToTalMoney + "$";
        _TienNhanText.text = "" + _TienNhan + "$";
        _TienThuaText.text = "" + _TienThua + "$";
    }

    // Update is called once per frame
    void Update()
    {
    }
    public int ChekSoLuongTongTien()
    {
        return _ToTalMoney;
    }
    public void TongTien(int money)
    {
        _ToTalMoney += money;
        _TotalMoneyText.text = "" + _ToTalMoney + "$";
    }
    public int TienNhan()
    {
        _TienNhan = GetRandomPayment(_ToTalMoney);
        _TienNhanText.text = "" + _TienNhan + "$";
        return _TienNhan;
    }
    public int TienThua()
    {
        _TienThua = _TienNhan - _ToTalMoney;
        tienthuahientai = _TienThua;
         _TienThuaText.text = "" + _TienThua+"$";
        return _TienThua;
    }
    public int TienThoi(int money)
    {
        _TienThua -= money;
        _TienThuaText.text = "" + _TienThua+"$";
        return _TienThua;

    }
    public void ResetCurrentStayCheckOut()
    {
        _ToTalMoney = 0;
        _TotalMoneyText.text = "" + _ToTalMoney + "$";
        _TienNhan = 0;
        _TienNhanText.text = "" + _TienNhan + "$";
        _TienThua = 0;
        _TienThuaText.text = "" + _TienThua + "$";
    }
    public void ReturnMoney(GameObject obj, int money) // hủy việc sinh tiền nếu thối nhầm
    {
        StartCoroutine(ReturnCurrentMoney(obj, money));
    }
    IEnumerator ReturnCurrentMoney(GameObject obj,int money) 
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.T));
        Destroy(obj);
        UpdateTienThua();
        rayCastCheckOutManager.ResetLaiMoneyCount();
        // Kiểm tra lại giá trị tiền thừa thực tế
        if (_TienThua >= money)
        {
            _TienThua -= money;
        }
        else
        {
            _TienThua = tienthuahientai ; // Đảm bảo không bị lỗi trừ quá số tiền hiện có
        }

        _TienThuaText.text = _TienThua + "$";
    }
    public bool CheckPlayerCurrentMoney(int currentTienThua)
    {
        int currentMoney = _playerMoney.UpdateCurrentMoneY();
        Debug.Log($"[CheckTienThuaCoroutine] TienThua: {currentTienThua}, PlayerMoney: {currentMoney}");
        if (currentMoney < -currentTienThua)
        {
            warringText.gameObject.SetActive(true);
            StartCoroutine(TextManager(warringText.gameObject));
            return false;//Trả về false khi không đủ tiền hàng
        }
        return true; // Trả về true khi đủ tiền hàng
    }
    public int UpdateTienThua()
    {
        Debug.Log($"[UpdateTienThua] Current TienThua: {_TienThua}");
        return _TienThua;
    }
    public void CongTienToPlayer(int money)
    {
        _playerMoney.CongTien(money);
    }
    public void TruTienToPlayer(int money)
    {
        _playerMoney.TruTien(money);
    }
    // Hàm random số tiền khách đưa
    public int GetRandomPayment(int totalPrice)
    {
        int[] possiblePayments = { totalPrice, totalPrice + 5, totalPrice + 10, totalPrice + 20, totalPrice + 50, totalPrice + 100 };
        return possiblePayments[Random.Range(0, possiblePayments.Length)];
    }
    public void AddCustomerToQueue(GameObject customer)
    {
        if (!customerQueue.Contains(customer)) // Đảm bảo khách hàng không bị thêm trùng lặp
        {
            customerQueue.Enqueue(customer);
            UpdateQueuePosition();
        }
    }
    public void UpdateQueuePosition()
    {
        int index = 0;//Xử lý khách hàng đang có trong hàng đợi
        foreach (GameObject customer in customerQueue)
        {
            Vector3 targetPosition = checkOutPoint.position - new Vector3 (0, 0, index *queueSpacing);
            customer.GetComponent<CustomerControll>().MoveTo(targetPosition);
            index++;
        }
    }
    public void ProcessQueue()
    {
        if(customerQueue.Count > 0)
        {
            isCheckoutBusy = true;
            GameObject currentCustomer = customerQueue.Dequeue();//Lấy khách hàng đầu tiên'
            currentCustomer.GetComponent<CustomerControll>().GetCheckOutPostition(checkOutPoint.position);
            UpdateQueuePosition();
        }
    }
    public void CheckOutCompleted()
    {
        isCheckoutBusy = false; // Quầy thanh toán trống
        if(customerQueue.Count > 0)
        {
            ProcessQueue(); // Xử lý khách hàng tiếp theo

        }
    }
    public Transform CheckoutPoint()
    {
        return checkOutPoint;
    }
    public bool CheckOutIsBusy()
    {
        return isCheckoutBusy;
    }
    IEnumerator TextManager(GameObject obj)
    {
        yield return new WaitForSeconds(0.5f);
        obj.gameObject.SetActive(false);
    }
}
