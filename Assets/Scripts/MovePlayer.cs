using System;
using Exceptions;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    private CharacterController _characterController;
    [SerializeField] private float velocity;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        if (_characterController == null)
        {
            throw new GameException("cannot get character controller component");
        }
    }

    void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        var moveDirection = Vector3.forward * z + Vector3.right * x;
        moveDirection = transform.TransformDirection(moveDirection);
        _characterController.Move(moveDirection * Time.deltaTime * velocity);

    }
}