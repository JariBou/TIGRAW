using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

public class UIController : MonoBehaviour
{
    public HeatDisplayScript DebugHeatDisplayScript;
    public OverdriveDisplayScript OverdriveDisplayScript;
    [FormerlySerializedAs("RedVignetteScript")] public VignetteScript vignetteScript;
    public bool DEBUG_OVERDRIVE;
    public List<GameObject> HeatMalusesDisplayList;

    private void Start()
    {
        if (!DEBUG_OVERDRIVE)
        {
            DebugHeatDisplayScript.gameObject.SetActive(false);
        }
    }
}
