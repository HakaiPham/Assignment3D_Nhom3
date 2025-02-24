using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField]
    private float timeMultiplier;

    [SerializeField]
    private float startHour;

    [SerializeField]
    private TextMeshProUGUI timeText;

    [SerializeField]
    private Light sunLight;

    [SerializeField]
    private float sunriseHour;

    [SerializeField]
    private float sunsetHour;

    [SerializeField]
    private Color dayAmbientLight;

    [SerializeField]
    private Color nightAmbientLight;

    [SerializeField]
    private AnimationCurve lightChangeCurve;

    [SerializeField]
    private float maxSunLightIntensity;

    [SerializeField]
    private Light moonLight;

    [SerializeField]
    private float maxMoonLightIntensity;

    private DateTime currentTime;

    private TimeSpan sunriseTime;

    private TimeSpan sunsetTime;

    [SerializeField]
    private GameObject summaryMenu; // UI tổng kết ngày

    private int currentDay = 1; // Ngày bắt đầu từ 1
    [SerializeField]
    private TextMeshProUGUI dayText; // Text hiển thị ngày

    [SerializeField]
    private GameObject KhachHang; // Đối tượng khách hàng sẽ bị tắt lúc 20:00

    bool CheckCanSpawn = true;
    void Start()
    {
        currentDay = PlayerPrefs.GetInt("Date", 1);
        if (dayText != null)
        {
            dayText.text = $"Day {currentDay}";
        }
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);

        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeOfDay();
        RotateSun();
        UpdateLightSettings();
        if (currentTime.Hour >= 14 && currentTime.Minute >= 0)
        {
            CheckCanSpawn = false;

        }
        else if (currentTime.Hour >= 6 && currentTime.Minute >= 0)
        {
            CheckCanSpawn = true;

        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            currentDay++; // Tăng ngày lên 1
            PlayerPrefs.SetInt("Date", currentDay);
            PlayerPrefs.Save();
            currentTime = DateTime.Now.Date + TimeSpan.FromHours(6); // Đặt thời gian về 6:00 sáng
            summaryMenu.SetActive(false);
            Cursor.visible = false; // Ẩn con trỏ chuột
            Cursor.lockState = CursorLockMode.Locked; // Khóa con trỏ vào giữa màn hình
                                                      //Time.timeScale = 1; // Tiếp tục thời gian


            // Cập nhật lại UI ngay lập tức
            if (dayText != null)
            {
                dayText.text = $"Day {currentDay}";
            }
        }
    }

    private void UpdateTimeOfDay()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);
        if(timeText != null )
        {
            timeText.text = currentTime.ToString("HH:mm");
        }

        // Cập nhật text ngày
        if (dayText != null)
        {
            dayText.text = $"Day {currentDay}";
        }

        // Kiểm tra nếu qua 00:00 thì ngày +1
        if (currentTime.Hour == 0 && currentTime.Minute == 0)
        {
            currentDay++;
        }

        // Kiểm tra nếu đến 21:00 thì bật menu tổng kết
        if ((currentTime.Hour >= 21|| currentTime.Hour >= 1 && currentTime.Hour < 6) 
            && currentTime.Minute >= 0 && Input.GetKeyDown(KeyCode.U))
        {
            summaryMenu.SetActive(true);

            Cursor.visible = true; // Hiện con trỏ chuột
            Cursor.lockState = CursorLockMode.None; // Cho phép di chuyển chuột tự do
        }

        //// Kiểm tra nếu đến 20:00 thì tắt GameObject "KhachHang"
        //if (currentTime.Hour == 14 && currentTime.Minute == 0)
        //{
        //    if (KhachHang != null)
        //    {
        //        KhachHang.SetActive(false);
        //    }
        //}

        //// Bật lại KhachHang lúc 6:00 sáng
        //if (currentTime.Hour == 7 && currentTime.Minute == 0)
        //{
        //    if (KhachHang != null)
        //    {
        //        KhachHang.SetActive(true);
        //    }

        //}
    }

   
    public void SkipToMorning() // dung de skip ngay
    {
        currentDay++; // Tăng ngày lên 1
        PlayerPrefs.SetInt("Date", currentDay);
        PlayerPrefs.Save();
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(6); // Đặt thời gian về 6:00 sáng
        summaryMenu.SetActive(false);
        Cursor.visible = false; // Ẩn con trỏ chuột
        Cursor.lockState = CursorLockMode.Locked; // Khóa con trỏ vào giữa màn hình
        //Time.timeScale = 1; // Tiếp tục thời gian

        
        // Cập nhật lại UI ngay lập tức
        if (dayText != null)
        {
            dayText.text = $"Day {currentDay}";
        }
    }
    private void RotateSun()
    {
        float sunLightRotation;

        if(currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime) 
        {
            TimeSpan sunriseTosunsetDuration = CalculateTimeDifference(sunriseTime,sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseTosunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
        }
        else
        {
            TimeSpan sunsetTosunriseDration = CalculateTimeDifference(sunsetTime, sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference (sunsetTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetTosunriseDration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);    
        }

        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }

    private void UpdateLightSettings() // off
    {
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        sunLight.intensity = Mathf.Lerp(0, maxSunLightIntensity, lightChangeCurve.Evaluate(dotProduct));
        moonLight.intensity = Mathf.Lerp(maxMoonLightIntensity, 0 , lightChangeCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct));
    }
    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        if(difference.TotalSeconds < 0 )
        {
            difference += TimeSpan.FromHours(24);
        }
        return difference;
    }

    public DateTime GetCurrentTime()
    {
        return currentTime;
    }

    public int GetCurrentDay()
    {
        return currentDay;
    }
    public bool CheckCanSpawnCustomer()
    {
        return CheckCanSpawn;
    }
}
