using UnityEngine;

public class SpriteAnimationSpriteRenderer : MonoBehaviour, ISpriteAnimationRenderer
{
    [SerializeField]
    SpriteAnimation _spriteAnimation = null;
    [SerializeField]
    SpriteRenderer _spriteRenderer = null;

    private void Awake()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
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
        _spriteRenderer.sprite = sprite;
    }
}