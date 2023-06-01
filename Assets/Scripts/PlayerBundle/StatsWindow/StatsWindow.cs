using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerBundle.StatsWindow
{
    public class StatsWindow : MonoBehaviour
    {
        private Canvas _canvas;


        public TMP_Text healthText;
        public TMP_Text atkMultiplierText;
        public TMP_Text moveSpeed;
        public TMP_Text recoveryRate;
        public TMP_Text bonusRecoveryRate;
        public TMP_Text maxHeat;
        

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

            healthText.text =$"{_player.health.ToString(CultureInfo.InvariantCulture)}/{_player.GetMaxHealth().ToString(CultureInfo.InvariantCulture)}";
            atkMultiplierText.text = Math.Round(_player.AtkMultiplier, 3).ToString(CultureInfo.InvariantCulture);
            moveSpeed.text = _player.movespeed.ToString(CultureInfo.InvariantCulture);
            recoveryRate.text = _player.heatManager.GetHeatRecoveryRate().ToString(CultureInfo.InvariantCulture);
            bonusRecoveryRate.text = _player.heatManager.GetBonusHeatRecoveryRate().ToString(CultureInfo.InvariantCulture);
            maxHeat.text = _player.heatManager.GetMaxHeat().ToString(CultureInfo.InvariantCulture);
        }

        private void FixedUpdate()
        {
            if (!_canvas.enabled){return;}
            moveSpeed.text = _player.movespeed.ToString(CultureInfo.InvariantCulture); // Move speed can dinamically change
            healthText.text =$"{_player.health.ToString(CultureInfo.InvariantCulture)}/{_player.GetMaxHealth().ToString(CultureInfo.InvariantCulture)}";
            atkMultiplierText.text = Math.Round(_player.AtkMultiplier, 3).ToString(CultureInfo.InvariantCulture);
            recoveryRate.text = _player.heatManager.GetHeatRecoveryRate().ToString(CultureInfo.InvariantCulture);
            bonusRecoveryRate.text = _player.heatManager.GetBonusHeatRecoveryRate().ToString(CultureInfo.InvariantCulture);
        }

        public void Disable()
        {
            _canvas.enabled = false;
        }
    }
}
