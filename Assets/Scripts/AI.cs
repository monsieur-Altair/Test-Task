using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI : MonoBehaviour
{
    private List<Player> _allEnemies = new List<Player>();
    [SerializeField] private GameObject enemiesLay;
    private void Start()
    {
        _allEnemies = enemiesLay.GetComponentsInChildren<Player>().ToList();
    }
    
    
}