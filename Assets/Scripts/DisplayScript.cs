using PlayerBundle;
using TMPro;
using UnityEngine;

public class DisplayScript : MonoBehaviour
{

    public GameObject player;
    private Player script;
    private TMP_Text textMesh;
    // Start is called before the first frame update
    void Start()
    {
        script = player.GetComponent<Player>();
        textMesh = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        string text = script.GetHeat().ToString();
        textMesh.SetText($"HEAT: {text}");
    }
}
