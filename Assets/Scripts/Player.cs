using System;
using System.Collections;
using Exceptions;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Hp { get; private set; }
    public Weapons.BaseWeapon CurrentWeapon { get; private set; }
    private Material _currentMaterial;
    private static readonly int Color1 = Shader.PropertyToID("_Color");
    public event Action Cooldown;
    private bool _isAlive;
    

    private void Awake()
    {
        _currentMaterial = transform.GetChild(0).GetComponent<MeshRenderer>().material;
        if (_currentMaterial == null)
        {
            throw new GameException("cannot get character material");
        }
    }

    private void Start()
    {
        _isAlive = true;
        Hp = 100.0f;
    }

    public void SetWeapon(Weapons.BaseWeapon weapon)
    {
        CurrentWeapon = weapon;
    }

    public void Shoot()
    {
        CurrentWeapon.StartShooting();
    }

    public void StartCooldown()
    {
        OnCooldown();
    }

    private void OnCooldown()
    {
        Cooldown?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isAlive == false)
            return;
        
        var bullet = other.GetComponent<Weapons.Bullet>();
        if (bullet == null)
        {
            throw new GameException("cannot get bullet component");
        }
        
        DisplayHit(bullet.Damage);
        other.gameObject.SetActive(false);
    }

    private void DisplayHit(float damage)//change material color
    {
        Hp -= damage;
        var colorDamage = 2.0f * damage/100.0f;//depend on start color
        var currentColor = _currentMaterial.color;
        //Debug.Log("red ="+currentColor.r+" green ="+currentColor.g+" blue ="+currentColor.b);
        var additionalRed = 1.0f - currentColor.r - colorDamage;
        if (additionalRed > 0)
            currentColor.r += colorDamage;
        else
        {
            currentColor.r = 1.0f;
            currentColor.g -= colorDamage;
            if (currentColor.g < 0)
                currentColor.g = 0;
        }

        _currentMaterial.SetColor(Color1,currentColor);
        if (Hp <= 0)
            StartCoroutine(HideCharacter());
    }

    private IEnumerator HideCharacter()
    {
        _isAlive = false;
        yield return new WaitForSeconds(1.0f);
        gameObject.SetActive(false);
    }
}