using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RayCastCheckOut : MonoBehaviour
{
    // Start is called before the first frame update
    public float rayCastDistance = 10f; // Khoảng cách Raycast
    public GameObject _1USD;
    public GameObject _5USD;
    public GameObject _10USD;
    public GameObject _20USD;
    public GameObject _50USD;
    public GameObject _100USD;
    public Transform vitrisinhtien;
    public CheckOutCounter checkOutManager;
    private List<GameObject> _MoneyList = new List<GameObject>();
    private int moneyCount = 0; // Biến đếm số tiền đã sinh ra
    private float moneyOffset = 0.01f; // Khoảng cách giữa các tờ tiền
    public PlayerCurrentMoney playerMoneyManager;
    public TextMeshPro moneytext;
    AudioSource audioSource;
    public AudioClip _SoundOpenWoodBox;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        RayCastSystem();
        //KiemTraTienThuaHienTai();
    }
    public void RayCastSystem()
    {
        if (Input.GetMouseButtonDown(0)) // Click chuột trái
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("CanPickup")) // Kiểm tra nếu trúng Item
                {
                    ItemStore item = hit.collider.GetComponent<ItemStore>(); // Lấy script ItemStore


                    if (item != null)
                    {
                        // Tìm CustomerControll của Item (từ cha của nó)
                        CustomerControll customer = item.GetComponentInParent<CustomerControll>();

                        if (customer != null)
                        {
                            customer.CheckOutItem(item.gameObject, item.price); // Gọi hàm thanh toán
                        }
                    }
                }
                else if (hit.collider.CompareTag("NapBan"))
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(_SoundOpenWoodBox);
                    }
                    NapBan napBan = hit.collider.GetComponent<NapBan>();
                    if (napBan != null)
                    {
                        napBan.MoveTo();
                    }
                }
                else if (hit.collider.CompareTag("1USD")&&checkOutManager.ChekSoLuongTongTien()!=0)
                {
                    checkOutManager.TienThoi(1);
                    checkOutManager.UpdateTienThua();
                    Vector3 spawnPosition = vitrisinhtien.position + new Vector3(0, moneyCount * moneyOffset, 0);
                    GameObject usd = Instantiate(_1USD, spawnPosition, Quaternion.Euler(90,0,0));
                    checkOutManager.ReturnMoney(usd, 1);
                    if(usd != null)
                    {
                        _MoneyList.Add(usd);
                    }
                    moneyCount++;
                }
                else if (hit.collider.CompareTag("5USD") && checkOutManager.ChekSoLuongTongTien() != 0)
                {
                   
                    checkOutManager.TienThoi(5);
                    checkOutManager.UpdateTienThua();
                    Vector3 spawnPosition = vitrisinhtien.position + new Vector3(0, moneyCount * moneyOffset, 0);
                    GameObject usd = Instantiate(_5USD, spawnPosition, Quaternion.Euler(90, 0, 0));
                    checkOutManager.ReturnMoney(usd, 5);
                    if (usd != null)
                    {
                        _MoneyList.Add(usd);
                    }
                    moneyCount++;
                }
                else if (hit.collider.CompareTag("10USD") && checkOutManager.ChekSoLuongTongTien() != 0)
                {
                    checkOutManager.TienThoi(10);
                    checkOutManager.UpdateTienThua();
                    Vector3 spawnPosition = vitrisinhtien.position + new Vector3(0, moneyCount * moneyOffset, 0);
                    GameObject usd = Instantiate(_10USD, spawnPosition, Quaternion.Euler(90, 0, 0));
                    checkOutManager.ReturnMoney(usd, 10);
                    if (usd != null)
                    {
                        _MoneyList.Add(usd);
                    }
                    moneyCount++;
                }
                else if (hit.collider.CompareTag("20USD") && checkOutManager.ChekSoLuongTongTien() != 0)
                {
                    checkOutManager.TienThoi(20);
                    checkOutManager.UpdateTienThua();
                    Vector3 spawnPosition = vitrisinhtien.position + new Vector3(0, moneyCount * moneyOffset, 0);
                    GameObject usd = Instantiate(_20USD, spawnPosition, Quaternion.Euler(90, 0, 0));
                    checkOutManager.ReturnMoney(usd, 20);
                    if (usd != null)
                    {
                        _MoneyList.Add(usd);
                    }
                    moneyCount++;
                }
                else if (hit.collider.CompareTag("50USD") && checkOutManager.ChekSoLuongTongTien() != 0)
                {
                    checkOutManager.TienThoi(50);
                    checkOutManager.UpdateTienThua();
                    Vector3 spawnPosition = vitrisinhtien.position + new Vector3(0, moneyCount * moneyOffset, 0);
                    GameObject usd = Instantiate(_50USD, spawnPosition, Quaternion.Euler(90, 0, 0));
                    checkOutManager.ReturnMoney(usd, 50);
                    if (usd != null)
                    {
                        _MoneyList.Add(usd);
                    }
                    moneyCount++;
                }
                else if (hit.collider.CompareTag("100USD") && checkOutManager.ChekSoLuongTongTien() != 0)
                {
                    checkOutManager.TienThoi(100);
                    checkOutManager.UpdateTienThua();
                    Vector3 spawnPosition = vitrisinhtien.position + new Vector3(0, moneyCount * moneyOffset, 0);
                    GameObject usd = Instantiate(_100USD, spawnPosition, Quaternion.Euler(90, 0, 0));
                    checkOutManager.ReturnMoney(usd, 100);
                    if (usd != null)
                    {
                        _MoneyList.Add(usd);
                    }
                    moneyCount++;
                }
            }
        }
    }
    public void KiemTraTienThuaHienTai()
    {
        int checkMoney = checkOutManager.UpdateTienThua();
        if (checkMoney <= 0)
        {
            foreach(GameObject usd in _MoneyList)
            {
                Destroy(usd);
            }
            _MoneyList.Clear();
            moneyCount = 0;
        }
    }
    public int ResetLaiMoneyCount()
    {
        moneyCount = 0;
        return moneyCount;
    }
}
