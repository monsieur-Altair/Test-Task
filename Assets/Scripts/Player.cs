using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public int Hp { get; private set; }
    public Weapons.BaseWeapon CurrentWeapon { get; private set; }

    public event Action Cooldown;

    private void Start()
    {
        Hp = 100;
    }

    public void SetWeapon(Weapons.BaseWeapon weapon)
    {
        CurrentWeapon = weapon;
    }

    public void Fire()
    {
        CurrentWeapon.StartFiring();
    }

    public void StartCooldown()
    {
        OnCooldown();
    }

    private void OnCooldown()
    {
        Cooldown?.Invoke();
    }
}