using System;
using System.Collections;
using Exceptions;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Text hpText;
        [SerializeField] private Text cooldownText;
        [SerializeField] private Text bulletText;
        [SerializeField] private GameObject player;
        private Player _player;

        private void Start()
        {
            _player = player.GetComponent<Player>();
            if (_player == null)
            {
                throw new GameException("cannot get player component");
            }
            cooldownText.gameObject.SetActive(false);
            _player.Cooldown += DisplayCooldownText;
        }

        private void LateUpdate()
        {
            DisplayHp();
            DisplayBulletCount();
        }

        private void DisplayHp()
        {
            hpText.text = "HP: " +_player.Hp;
        }

        private void DisplayBulletCount()
        {
            bulletText.text = _player.CurrentWeapon.CurrentBulletCount + "/" +_player.CurrentWeapon.Clip;
        }

        private void DisplayCooldownText()
        {
            var time = _player.CurrentWeapon.Cooldown;
            StartCoroutine(DisplaySeconds(time));
        }

        private IEnumerator DisplaySeconds(float time)
        {
            cooldownText.gameObject.SetActive(true);
            while (time > 0.0f)
            {
                cooldownText.text=time.ToString("N2") + " sec";
                time -= Time.deltaTime;
                yield return null;
            }
            cooldownText.gameObject.SetActive(false);
        }
    }
}