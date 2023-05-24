using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenusScripts
{
    public class UIAnimation : MonoBehaviour
    {
        public Image image;

        public Sprite IdleSprite;
        public Sprite[] spriteArray;
        public int frameRate = 5;

        public bool StartAnimOnLoad = true;
        private int spriteIndex;
        private bool isRunning;

        Coroutine corotineAnim;

        bool IsDone;

        private void Start()
        {
            image = GetComponent<Image>();
            if (StartAnimOnLoad)
            {
                StartAnim();
            }
            else
            {
                if (IdleSprite)
                {
                    image.sprite = IdleSprite;
                }
            }
        }

        IEnumerator PlayAnim()
        {
            float animSpeed = 1 / (float)frameRate;
            yield return new WaitForSeconds(animSpeed);
            if (spriteIndex >= spriteArray.Length)
            {
                spriteIndex = 0;
            }
            image.sprite = spriteArray[spriteIndex];
            spriteIndex += 1;
            corotineAnim = StartCoroutine(PlayAnim());
        }

        public void StartAnim()
        {
            if (isRunning) return;
            isRunning = true;
            corotineAnim = StartCoroutine(PlayAnim());
        }

        public void StopAnim()
        {
            if (!isRunning) return;
            StopCoroutine(corotineAnim);
            isRunning = false;
            if (IdleSprite)
            {
                image.sprite = IdleSprite;
            }
        }

        public void Toggle()
        {
            switch (isRunning)
            {
                case true:
                    StopAnim();
                    break;
                case false:
                    StartAnim();
                    break;
            }
        }
    }
}
