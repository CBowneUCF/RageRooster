using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;

    void Update()
    {
        Vector2 lookInput = Input.Camera;
        freeLookCamera.m_XAxis.Value += lookInput.x * Time.deltaTime * 300f;
        freeLookCamera.m_YAxis.Value += lookInput.y * Time.deltaTime * 2f;
    }
}