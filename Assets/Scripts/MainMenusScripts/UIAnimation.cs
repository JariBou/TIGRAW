using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenusScripts
{
    public class UIAnimation : MonoBehaviour
    {
        public Image image;

        public Sprite[] spriteArray;
        public int frameRate = 5;

        private int spriteIndex;

        Coroutine corotineAnim;

        bool IsDone;

        private void Start()
        {
            image = GetComponent<Image>();
            corotineAnim = StartCoroutine(PlayAnim());
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
            if (IsDone == false)
                corotineAnim = StartCoroutine(PlayAnim());
        }
    }
}
