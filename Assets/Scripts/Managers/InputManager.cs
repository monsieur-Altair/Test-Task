using System;
using Exceptions;
using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private float mouseSensitivity;
        [SerializeField] private GameObject player;
        private Transform _playerTransform;
        private Player _player;
        private float _xRotation;
        private const float MAXRotationAngle=90.0f; 
        private const float MINRotationAngle=-90.0f;
        
        private Camera _mainCamera;
        private Transform _cameraTransform;

        private CharacterController _characterController;
        [SerializeField] private float velocity;
        
        private WeaponManager _weaponManager;
        
        private const string PistolKey = "1";
        private const string ShotgunKey = "2";
        private const string AssaultRifleKey = "3";
        private const string CooldownKey = "r";

        private int _pistolIndex;
        private int _shotgunIndex;
        private int _assaultRifleIndex;
        
        private void Start()
        {
            GetPlayerInformation();
            GetCameraInformation();
            GetWeaponInformation();
        }

        // Update is called once per frame
        private void Update()
        {
            SwitchingWeapons();
            Firing();
            CheckCooldown();
            MovePlayer();
            MoveCamera();
        }

        private void CheckCooldown()
        {
            if (Input.GetKeyDown(CooldownKey)&&_player.CurrentWeapon.IsOnCooldown==false)
            {
                _player.StartCooldown();
            }
        }
        
        private void Firing()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _player.Fire();
            }
        }

        private void SwitchingWeapons()
        {
            CheckKeySwitchWeapon(PistolKey,_pistolIndex);
            CheckKeySwitchWeapon(ShotgunKey,_shotgunIndex);
            CheckKeySwitchWeapon(AssaultRifleKey,_assaultRifleIndex);
        }

        private void CheckKeySwitchWeapon(string keyName, int weaponIndex)
        {
            if (Input.GetKeyDown(keyName) && _weaponManager.CurrentIndex != weaponIndex)
            {
                _weaponManager.SwitchWeapon(weaponIndex);
            }
        }
        
        private void MoveCamera()
        {
            var mouseX = Input.GetAxis("Mouse X")*mouseSensitivity*Time.deltaTime;
            var mouseY = Input.GetAxis("Mouse Y")*mouseSensitivity*Time.deltaTime/1.5f;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, MINRotationAngle, MAXRotationAngle);
        
            //transform.localRotation=Quaternion.Euler(_xRotation,0,0);
            _cameraTransform.localEulerAngles = Vector3.right * _xRotation;
            _playerTransform.Rotate(Vector3.up*mouseX);
        }

        private void MovePlayer()
        {
            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");
            var moveDirection = Vector3.forward * z + Vector3.right * x;
            moveDirection = _playerTransform.TransformDirection(moveDirection);
            _characterController.Move(moveDirection * Time.deltaTime * velocity);
        }

        private void GetPlayerInformation()
        {
            if (player == null)
            {
                throw new GameException("cannot get players game object");
            }
            _playerTransform = player.transform;
            _player = player.GetComponent<Player>();
            if (_player == null)
            {
                throw new GameException("cannot get players component");
            }
            
            _characterController = player.GetComponent<CharacterController>();
            if (_characterController == null)
            {
                throw new GameException("cannot get character controller component");
            }
        }

        private void GetCameraInformation()
        {
            _mainCamera=Camera.main;
            if (_mainCamera == null)
            {
                throw new GameException("cannot get main camera");
            }
            _cameraTransform = _mainCamera.transform;
            
            _xRotation = 0.0f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void GetWeaponInformation()
        {
            _weaponManager=WeaponManager.Instance;
            _pistolIndex = (int) Weapons.Type.Pistol;
            _shotgunIndex = (int) Weapons.Type.Shotgun;
            _assaultRifleIndex = (int) Weapons.Type.AssaultRifle;
        }
    }
}