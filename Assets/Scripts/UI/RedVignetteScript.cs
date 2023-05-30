using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class RedVignetteScript : MonoBehaviour
    {
        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        void Start()
        {
            image.color = ColorUtils.ColorWithAlpha(image.color, 0f);
        }

    
        void FixedUpdate()
        {
            Color color = image.color;

            if (color.a > 0)
            {
                color.a -= Time.fixedDeltaTime;
                image.color = color;   
            }
        }

        public void ApplyVignette(float alphaValue = 0.8f)
        {
            image.color = ColorUtils.ColorWithAlpha(image.color, alphaValue);
        }
    }
}
