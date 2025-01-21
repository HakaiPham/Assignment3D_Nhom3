using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheckOutCounter : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform checkOutPoint;
    private Queue<GameObject> customerQueue = new Queue<GameObject>();//hàng đợi khách hàng
    public bool isCheckoutBusy = false;//Trạng thái quầy thanh toán
    private int lastQueueCount = -1; // Biến lưu số lượng hàng đợi trước đó
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Tạo 1 biến xác định có người nào đang thanh toán không?
        //Nếu có thì người kia sẽ phải đợi cho đến khi được thanh toán xong
        //Biến sẽ thành true khi mà người chơi hoàn thanh xong việc thanh toán
        //Sau khi hoàn thành xong việc biến sẽ trở lại thành false
        if (customerQueue.Count != lastQueueCount)
        {
            Debug.Log("Số lượng khách hàng trong hàng chờ là: " + customerQueue.Count);
            lastQueueCount = customerQueue.Count; // Cập nhật số lượng hàng đợi hiện tại
        }
    }
    public void AddCustomerToQueue(GameObject customer)
    {
        if (!customerQueue.Contains(customer)) // Đảm bảo khách hàng không bị thêm trùng lặp
        {
            customerQueue.Enqueue(customer);
            Debug.Log("Thêm khách hàng: " + customer.name);
            ProcessQueue();
        }
    }
    public void ProcessQueue()
    {
        if(!isCheckoutBusy&&customerQueue.Count > 0)
        {
            isCheckoutBusy = true;
            GameObject currentCustomer = customerQueue.Dequeue();//Lấy khách hàng đầu tiên'
            currentCustomer.GetComponent<CustomerControll>().GetCheckOutPostition(checkOutPoint.position);
        }
    }
    public void CheckOutCompleted()
    {
        isCheckoutBusy = false; // Quầy thanh toán trống
        ProcessQueue(); // Xử lý khách hàng tiếp theo
    }
    public Transform CheckoutPoint()
    {
        return checkOutPoint;
    }
    public bool CheckOutIsBusy()
    {
        return isCheckoutBusy;
    }
}
