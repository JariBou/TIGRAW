using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpStatueScript : Interactable
{
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        if (canvas.enabled)
        {
            canvas.enabled = false;
            return;
        }
        canvas.enabled = true;
        PlayerUpgrade[] upgradesScripts = canvas.GetComponentsInChildren<PlayerUpgrade>();
        foreach (PlayerUpgrade script in upgradesScripts)
        {
            script.Refresh();
        }
    }
}
