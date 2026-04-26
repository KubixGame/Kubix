using UnityEngine;

namespace Kubix.Editor;

public sealed class EditorCameraController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 16f;
    [SerializeField] private float _lookSpeed = 4f;
    [SerializeField] private float _zoomSpeed = 120f;

    private float _yaw = 45f;
    private float _pitch = 35f;

    private void Start()
    {
        var euler = transform.rotation.eulerAngles;
        _yaw = euler.y;
        _pitch = euler.x;
    }

    private void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");
        var lift = 0f;

        if (Input.GetKey(KeyCode.E))
        {
            lift += 1f;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            lift -= 1f;
        }

        var move = (transform.forward * vertical + transform.right * horizontal + Vector3.up * lift).normalized;
        transform.position += move * (_moveSpeed * Time.deltaTime);

        if (Input.GetMouseButton(1))
        {
            _yaw += Input.GetAxis("Mouse X") * _lookSpeed;
            _pitch -= Input.GetAxis("Mouse Y") * _lookSpeed;
            _pitch = Mathf.Clamp(_pitch, 15f, 80f);
            transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        }

        var scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > 0.01f)
        {
            transform.position += transform.forward * (scroll * _zoomSpeed * Time.deltaTime);
        }
    }
}
