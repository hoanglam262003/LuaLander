using Unity.Cinemachine;
using UnityEngine;

public class CinemachineZoomCamera : MonoBehaviour
{
    private const float NORMAL_ORTHOGRAPHIC_SIZE = 10f;
    private float zoomSpeed = 2f;
    public static CinemachineZoomCamera Instance { get; private set; }
    [SerializeField] private CinemachineCamera cinemachineCamera;
    private float targetOrthographicSize = 10f;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        cinemachineCamera.Lens.OrthographicSize = 
            Mathf.Lerp(cinemachineCamera.Lens.OrthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);
    }
    public void SetTargetOrthographicSize(float size)
    {
        targetOrthographicSize = size;
    }

    public void ResetToNormalOrthographicSize()
    {
        SetTargetOrthographicSize(NORMAL_ORTHOGRAPHIC_SIZE);
    }
}
