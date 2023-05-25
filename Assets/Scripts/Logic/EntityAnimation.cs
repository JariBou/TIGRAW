using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EntityAnimation : MonoBehaviour
{
    public SpriteRenderer renderer;

    public Sprite[] spriteArray;
    public int frameRate = 5;

    private int spriteIndex;

    Coroutine corotineAnim;

    bool IsDone;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        corotineAnim = StartCoroutine(PlayAnim());
    }

    IEnumerator PlayAnim()
    {
        float animSpeed = 1 / (float)frameRate;
        yield return new WaitForSeconds(animSpeed);
        if (spriteIndex >= spriteArray.Length)
        {
            Destroy(gameObject);
            yield break;
        }
        renderer.sprite = spriteArray[spriteIndex];
        spriteIndex += 1;
        if (IsDone == false)
            corotineAnim = StartCoroutine(PlayAnim());
    }
}