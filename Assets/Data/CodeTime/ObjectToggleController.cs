using System;
using UnityEngine;

public class ObjectToggleController : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToToggle; // Vật thể cần bật/tắt

    [SerializeField, Range(0, 24)]
    private float toggleOnHour = 18f; // Giờ bật, mặc định 18:00

    [SerializeField, Range(0, 24)]
    private float toggleOffHour = 6f; // Giờ tắt, mặc định 06:00

    private TimeController timeController;

    void Start()
    {
        timeController = FindObjectOfType<TimeController>();
    }

    void Update()
    {
        if (timeController != null)
        {
            ToggleObjectByTime(timeController.GetCurrentTime());
        }
    }

    private void ToggleObjectByTime(DateTime currentTime)
    {
        TimeSpan toggleOnTime = TimeSpan.FromHours(toggleOnHour);
        TimeSpan toggleOffTime = TimeSpan.FromHours(toggleOffHour);
        TimeSpan currentTimeSpan = currentTime.TimeOfDay;

        if (IsTimeBetween(currentTimeSpan, toggleOnTime, toggleOffTime))
        {
            objectToToggle.SetActive(true); // Bật vật thể
        }
        else
        {
            objectToToggle.SetActive(false); // Tắt vật thể
        }
    }

    private bool IsTimeBetween(TimeSpan currentTime, TimeSpan startTime, TimeSpan endTime)
    {
        // Trường hợp thời gian qua đêm (offTime < onTime)
        if (startTime < endTime)
        {
            return currentTime >= startTime && currentTime <= endTime;
        }
        else
        {
            return currentTime >= startTime || currentTime <= endTime;
        }
    }
}
