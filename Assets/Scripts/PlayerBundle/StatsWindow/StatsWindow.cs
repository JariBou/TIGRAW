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
            _player = Player.instance;
            _canvas.enabled = true;

            healthText.text = _player.health.ToString(CultureInfo.InvariantCulture);
            atkMultiplierText.text = _player.atkMultiplier.ToString(CultureInfo.InvariantCulture);
            armorText.text = _player.armor.ToString();
        }
    
        public void Disable()
        {
            _canvas.enabled = false;
        }
    }
}
