
using Exceptions;
using UnityEngine;



public class MouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private GameObject player;
    private Transform _playerTransform;
    private float _xRotation;
    private const float MAXRotationAngle=90.0f; 
    private const float MINRotationAngle=-90.0f; 
    void Start()
    {
        if (player == null)
        {
            throw new GameException("cannot get player");
        }
        _playerTransform = player.transform;
        if (_playerTransform == null)
        {
            throw new GameException("cannot get player transform component");
        }

        _xRotation = 0.0f;
        Cursor.lockState = CursorLockMode.Locked;
    }

    

    // Update is called once per frame
    void Update()
    {
        var mouseX = Input.GetAxis("Mouse X")*mouseSensitivity*Time.deltaTime;
        var mouseY = Input.GetAxis("Mouse Y")*mouseSensitivity*Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, MINRotationAngle, MAXRotationAngle);
        
        //transform.localRotation=Quaternion.Euler(_xRotation,0,0);
        transform.localEulerAngles = Vector3.right * _xRotation;
        _playerTransform.Rotate(Vector3.up*mouseX);
    }
}
