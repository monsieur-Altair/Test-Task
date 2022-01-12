using Characters;
using Exceptions;
using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private float mouseSensitivity;
        [SerializeField] private GameObject player;
        private Transform _playerTransform;
        private BaseCharacter _mainBaseCharacter;
        private float _xRotation;
        private const float MAXRotationAngle=90.0f; 
        private const float MINRotationAngle=-90.0f;
        private const float VerticalMouseSensitivity=1.5f;
        
        private Camera _mainCamera;
        private Transform _cameraTransform;

        private CharacterController _characterController;
        [SerializeField] private float velocity;
        
        private const string PistolKey = "1";
        private const string ShotgunKey = "2";
        private const string AssaultRifleKey = "3";
        private const string ElectricGunKey = "4";
        private const string CooldownKey = "r";

        private int _pistolIndex;
        private int _shotgunIndex;
        private int _assaultRifleIndex;
        private int _electricGunIndex;
        
        private void Start()
        {
            GetPlayerInformation();
            GetCameraInformation();
            GetWeaponInformation();
        }

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
            if (Input.GetKeyDown(CooldownKey)&&_mainBaseCharacter.CurrentWeapon.IsOnCooldown==false)
            {
                _mainBaseCharacter.StartCooldown();
            }
        }
        
        private void Firing()
        {
            if (Input.GetMouseButton(0))
            {
                _mainBaseCharacter.Shoot();
            }
        }

        private void SwitchingWeapons()
        {
            CheckKeySwitchWeapon(PistolKey,_pistolIndex);
            CheckKeySwitchWeapon(ShotgunKey,_shotgunIndex);
            CheckKeySwitchWeapon(AssaultRifleKey,_assaultRifleIndex);
            CheckKeySwitchWeapon(ElectricGunKey,_electricGunIndex);
        }

        private void CheckKeySwitchWeapon(string keyName, int weaponIndex)
        {
            if (Input.GetKeyDown(keyName) && 
                _mainBaseCharacter.CurrentIndex != weaponIndex && 
                _mainBaseCharacter.CurrentWeapon.IsOnCooldown == false)
            {
                WeaponManager.SwitchWeapon(weaponIndex, _mainBaseCharacter);
            }
        }
        
        private void MoveCamera()
        {
            var mouseX = Input.GetAxis("Mouse X")*mouseSensitivity*Time.deltaTime;
            var mouseY = Input.GetAxis("Mouse Y")*mouseSensitivity*Time.deltaTime/VerticalMouseSensitivity;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, MINRotationAngle, MAXRotationAngle);
        
            _cameraTransform.localEulerAngles = Vector3.right * _xRotation;
            _playerTransform.Rotate(Vector3.up*mouseX);
        }

        private void MovePlayer()
        {
            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");
            var moveDirection = _playerTransform.TransformDirection(Vector3.forward * z + Vector3.right * x);
            _characterController.Move(moveDirection * Time.deltaTime * velocity);
        }

        private void GetPlayerInformation()
        {
            if (player == null)
            {
                throw new GameException("cannot get players game object");
            }
            _playerTransform = player.transform;
            _mainBaseCharacter = player.GetComponent<BaseCharacter>();
            if (_mainBaseCharacter == null)
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
            _pistolIndex = (int) Weapons.Type.Pistol;
            _shotgunIndex = (int) Weapons.Type.Shotgun;
            _assaultRifleIndex = (int) Weapons.Type.AssaultRifle;
            _electricGunIndex = (int) Weapons.Type.ElectricGun;
        }
    }
}