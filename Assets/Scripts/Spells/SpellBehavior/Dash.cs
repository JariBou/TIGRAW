using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public Spell spell;

    private Vector2 TestVec;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 moveVector = Player.instance.GetMoveVector();
        
        // Problem when dashing towards positive x
        // Player's body velocity set to 0 when pressing spacebar somehow?? and only affecting towards positive x
        Debug.LogWarning($"moveVector={moveVector}");
        TestVec = new Vector2(moveVector.x, moveVector.y) * spell.dashDistance * Time.fixedDeltaTime;
        Debug.LogWarning($"testVec={TestVec}");
        Player.instance.ApplyForce(TestVec);
        Invoke("EndDash", 0.25f);
    }

    void EndDash()
    {
        Player.instance.isDashing = false;
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        //Player.instance.SetVelocity(new Vector2(20, 0));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.instance.GetPosition();
    }
}
