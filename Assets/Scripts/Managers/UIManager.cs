using System.Collections;
using Characters;
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
        private BaseCharacter _character;

        private void Start()
        {
            _character = player.GetComponent<BaseCharacter>();
            if (_character == null)
            {
                throw new GameException("cannot get player component");
            }
            cooldownText.gameObject.SetActive(false);
            _character.Cooldown += DisplayCooldownText;
        }

        private void LateUpdate()
        {
            DisplayHp();
            DisplayBulletCount();
        }

        private void DisplayHp()
        {
            hpText.text = "HP: " + (int)_character.Hp;
        }

        private void DisplayBulletCount()
        {
            bulletText.text = _character.CurrentWeapon.CurrentBulletCount + "/" +_character.CurrentWeapon.Clip;
        }

        private void DisplayCooldownText()
        {
            var time = _character.CurrentWeapon.Cooldown;
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