using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public enum VignetteColor
    {
        Red,
        Blue
    }
    public class VignetteScript : MonoBehaviour
    {
        private Image image;

        public Sprite RedVignette;
        public Sprite BlueVignette;

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

        public void ApplyVignette(VignetteColor color, float alphaValue = 0.8f)
        {
            switch (color)
            {
                case VignetteColor.Red:
                    image.sprite = RedVignette;
                    break;
                case VignetteColor.Blue:
                    image.sprite = BlueVignette;
                    break;
            }
            image.color = ColorUtils.ColorWithAlpha(image.color, alphaValue);
        }
    }
}
