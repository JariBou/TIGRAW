using System.Globalization;
using PlayerBundle;
using TMPro;
using UnityEngine;

public class HeatDisplayScript : MonoBehaviour
{

    private Player _playerScript;
    public TMP_Text heatAmountText;
    public TMP_Text heatLevelText;
    public TMP_Text heatMaxText;
    // Start is called before the first frame update
    void Start()
    {
        _playerScript = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        string text = _playerScript.heatManager.GetHeat().ToString(CultureInfo.InvariantCulture);
        heatAmountText.SetText($"HEAT: {text}");
        heatLevelText.SetText($"Heat Level: {_playerScript.heatManager.GetHeatLevel()}");
        heatMaxText.SetText($"Max Heat: {_playerScript.heatManager.MaxHeat}");
    }
}
