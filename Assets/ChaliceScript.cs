using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaliceScript : Interactable
{
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = false;
    }
        
    public override void Interact()
    {
        if (canvas.enabled)
        {
            canvas.enabled = false;
            return;
        }
        canvas.enabled = true;
    }

    protected override void OnFlagEvent(Flag flag)
    {
            
    }
}
