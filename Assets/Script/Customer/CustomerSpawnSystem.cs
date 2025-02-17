using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerSpawnSystem : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int _CurrentQuanityCustomer = 0;
    [SerializeField] private int _MaxCustomer;
    public GameObject[] Customer;
    public Transform spawnPosition0;
    public Transform spawnPosition1;
    void Start()
    {
        StartCoroutine(SpawnCustomer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Khách hàng có lượng khách nhất định được vào mua hàng
    //Nếu  khách hàng chưa thanh toán thì số lượng sẽ không bị trừ đi
    //Chỉ bị trừ đi khi khách hàng thanh toán xong
    IEnumerator SpawnCustomer()
    {
        while (true)  // Luôn chạy
        {
            if (_CurrentQuanityCustomer < _MaxCustomer)  // Chỉ spawn khi chưa đủ số lượng
            {
                int spawnPosition = SpawnPosition();
                int random = Random.Range(0, Customer.Length);
                if (spawnPosition == 0)
                {
                    GameObject customer = Instantiate(Customer[random], spawnPosition0.position, Quaternion.identity);
                }
                else
                {
                    GameObject customer1 = Instantiate(Customer[random], spawnPosition1.position, Quaternion.identity);
                }
                _CurrentQuanityCustomer++;
            }

            yield return new WaitForSeconds(Random.Range(2, 6)); // Chờ rồi kiểm tra lại
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Customer")
        {
            bool checkCustomerReturn = other.GetComponent<CustomerControll>().CheckCustomerReturnOriginal();
            if (checkCustomerReturn)
            {
                _CurrentQuanityCustomer--;
            }
        }
    }
    public int SpawnPosition()
    {
        int random = Random.Range(0,2);
        if(random == 0)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

}
