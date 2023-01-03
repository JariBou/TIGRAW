using PlayerBundle;
using TMPro;
using UnityEngine;

public class DisplayScript : MonoBehaviour
{

    public GameObject player;
    private Player _script;
    private TMP_Text _textMesh;
    // Start is called before the first frame update
    void Start()
    {
        _script = player.GetComponent<Player>();
        _textMesh = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        string text = _script.GetHeat().ToString();
        _textMesh.SetText($"HEAT: {text}");
    }
}
