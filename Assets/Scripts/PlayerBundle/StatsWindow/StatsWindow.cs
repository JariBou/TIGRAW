using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace PlayerBundle.StatsWindow
{
    public class StatsWindow : MonoBehaviour
    {
        private Canvas _canvas;


        public TMP_Text healthText;
        public TMP_Text atkMultiplierText;
        public TMP_Text armorText;

        private Player _player;


        private void Start()
        {
            _canvas = GetComponent<Canvas>();
            _canvas.enabled = false;
        }

        public void Enable()
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();;
            _canvas.enabled = true;

            healthText.text =$"{_player.health.ToString(CultureInfo.InvariantCulture)}/{_player.MaxHealth.ToString(CultureInfo.InvariantCulture)}";
            atkMultiplierText.text = Math.Round(_player.AtkMultiplier, 3).ToString(CultureInfo.InvariantCulture);
            armorText.text = _player.baseArmor.ToString();
        }
    
        public void Disable()
        {
            _canvas.enabled = false;
        }
    }
}
