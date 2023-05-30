using System;
using UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public HeatDisplayScript DebugHeatDisplayScript;
    public OverdriveDisplayScript OverdriveDisplayScript;
    public RedVignetteScript RedVignetteScript;
    public bool DEBUG_OVERDRIVE;

    private void Start()
    {
        if (!DEBUG_OVERDRIVE)
        {
            DebugHeatDisplayScript.gameObject.SetActive(false);
        }
    }
}
