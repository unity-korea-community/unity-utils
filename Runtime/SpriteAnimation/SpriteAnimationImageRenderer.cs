using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimationImageRenderer : MonoBehaviour, ISpriteAnimationRenderer
{
    [SerializeField]
    SpriteAnimation _spriteAnimation = null;
    [SerializeField]
    Image _image = null;

    private void Awake()
    {
        if (_image == null)
        {
            _image = GetComponent<Image>();
        }

        if (_spriteAnimation == null)
        {
            _spriteAnimation = GetComponent<SpriteAnimation>();
        }

        _spriteAnimation.AddRenderer(this);
    }

    public void SetSpriteAnimation(SpriteAnimation spriteAnimation)
    {
        _spriteAnimation = spriteAnimation;
    }

    public void UpdateSprite(Sprite sprite)
    {
        _image.sprite = sprite;
    }
}