using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLight : MonoBehaviour
{
    public float timeDayNight = 1f; // Thời gian quy định
    public Light light;
    public Gradient gradient;
    public float rotationSpeed; // Tốc độ quay light

    public GameObject objectToToggle; // GameObject cần bật/tắt
    public float dawnAngle = 25f; // Góc bình minh
    public float duskAngle = 160f; // Góc hoàng hôn

    void Start()
    {
        rotationSpeed = 360f / (timeDayNight * 60f);
    }

    void Update()
    {
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);

        if (light != null && gradient != null)
        {
            float time = Mathf.PingPong(Time.time / (timeDayNight * 30f), 1f);
            light.color = gradient.Evaluate(time);
        }

        // Bật tắt object dựa trên góc quay
        if (objectToToggle != null)
        {
            float currentAngle = transform.rotation.eulerAngles.x;
            if (currentAngle > 180f)
            {
                currentAngle -= 360f; // Chuyển đổi góc từ 0-360 thành -180 đến 180
            }

            // Bật object khi trong khoảng từ duskAngle đến dawnAngle (tối)
            bool isNight = currentAngle > duskAngle || currentAngle < dawnAngle;
            objectToToggle.SetActive(isNight);
        }
    }
}
