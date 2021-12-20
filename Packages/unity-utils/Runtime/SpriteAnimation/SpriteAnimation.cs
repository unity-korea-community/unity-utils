using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public interface ISpriteAnimationRenderer
{
    void UpdateSprite(Sprite sprite);
}

[ExecuteInEditMode]
public class SpriteAnimation : MonoBehaviour
{
    public enum UpdateMode
    {
        GameTime,
        RealTime,
    }

    public IReadOnlyList<Sprite> AnimatedSprites => _animatedSprites;
    public float FrameCount => _frameCount;
    public bool IsLoop => _isLoop;

    [SerializeField]
    List<Sprite> _animatedSprites = new List<Sprite>();
    [SerializeField]
    float _playBeforeDelay = 0f;
    [SerializeField]
    float _frameCount = 1f;
    [SerializeField]
    UpdateMode _updateMode = UpdateMode.RealTime;
    [SerializeField]
    bool _isPlayOnEnable = false;
    [SerializeField]
    bool _isLoop = true;
    [SerializeField]
    bool _isDeactivate_WhenFinish = false;

    // NOTE unity event는 보통 인스펙터 최하단에 있기 때문에 여기에 배치
    public UnityEvent<SpriteAnimation> OnStartAnimation = new UnityEvent<SpriteAnimation>();
    public UnityEvent<int> OnChangeSpriteIndex = new UnityEvent<int>();
    public UnityEvent<SpriteAnimation> OnFinishAnimation = new UnityEvent<SpriteAnimation>();

    public bool IsPlaying { get; private set; }
    List<ISpriteAnimationRenderer> _renderers = new List<ISpriteAnimationRenderer>();
    Coroutine _playCoroutine;

    public static SpriteAnimation GerOrAddSpriteAnimationImage(GameObject target)
    {
        SpriteAnimation spriteAnimation = target.GetOrAddComponent<SpriteAnimation>();
        target.GetOrAddComponent<SpriteAnimationImageRenderer>().SetSpriteAnimation(spriteAnimation);

        return spriteAnimation;
    }

    public static SpriteAnimation GerOrAddSpriteAnimationSprite(GameObject target)
    {
        SpriteAnimation spriteAnimation = target.GetOrAddComponent<SpriteAnimation>();
        target.GetOrAddComponent<SpriteAnimationSpriteRenderer>().SetSpriteAnimation(spriteAnimation);

        return spriteAnimation;
    }

    public void AddRenderer(ISpriteAnimationRenderer renderer)
        => _renderers.Add(renderer);

    public void RemoveRenderer(ISpriteAnimationRenderer renderer)
        => _renderers.Remove(renderer);

    public void ClearRenderer()
        => _renderers.Clear();

    public void PlayEffect()
        => Play();

    public IEnumerator Play()
    {
        Stop();

        if (_animatedSprites.Count == 0)
        {
            Debug.LogError($"{name}({typeof(SpriteAnimation)}).{nameof(Play)}() fail, _animatedSprites.Count == 0", this);
            return null;
        }

        System.Func<float, object> getYield = (waitTime) => new WaitForSeconds(waitTime);
        switch (_updateMode)
        {
            case UpdateMode.RealTime:
                getYield = (waitTime) => new WaitForSecondsRealtime(waitTime);
                break;
        }

        UpdateRenderer(_animatedSprites.FirstOrDefault());
        IEnumerator routine = PlayCoroutine(getYield);
        _playCoroutine = StartCoroutine(routine);
        return routine;
    }

    public void Stop()
    {
        if (_playCoroutine != null)
        {
            StopCoroutine(_playCoroutine);
            _playCoroutine = null;
        }

        IsPlaying = false;
    }

    public void SetAnimatedSprite(params Sprite[] sprites)
    {
        _animatedSprites.Clear();
        _animatedSprites.AddRange(sprites);
    }

    public void SetPlayBeforeDelay(float delay) => _playBeforeDelay = delay;
    public void SetFrameCount(float frameCount) => _frameCount = frameCount;
    public void SetIsLoop(bool isLoop) => _isLoop = isLoop;
    public void SetPlayOnEnable(bool enable) => _isPlayOnEnable = enable;
    public void SetUpdateMode(UpdateMode updateMode) => _updateMode = updateMode;
    public void SetDeactivate_WhenFinish(bool deactivate) => _isDeactivate_WhenFinish = deactivate;

    private void OnEnable()
    {
        if (Application.isPlaying)
        {
            if (_isPlayOnEnable)
            {
                Play();
            }
        }
        else
        {
            UpdateRenderer(_animatedSprites.FirstOrDefault());
        }
    }


    IEnumerator PlayCoroutine(System.Func<float, object> getYield)
    {
        yield return getYield(_playBeforeDelay);

        IsPlaying = true;

        int spriteCount = _animatedSprites.Count;
        float waitSecondsPerFrame = CalculateWaitSecondsPerFrame();

        do
        {
            int spriteIndex = 0;
            OnStartAnimation.Invoke(this);
            while (spriteIndex < spriteCount)
            {
                OnChangeSpriteIndex.Invoke(spriteIndex);
                UpdateRenderer(_animatedSprites[spriteIndex]);
                yield return getYield(waitSecondsPerFrame);
                spriteIndex++;
            }
            OnFinishAnimation.Invoke(this);
        } while (IsLoop);

        IsPlaying = false;
        if (_isDeactivate_WhenFinish)
        {
            // gameObject.SetActive(false);
        }
    }

    private void UpdateRenderer(Sprite sprite)
    {
        _renderers.ForEach(renderer => renderer.UpdateSprite(sprite));
    }

    private float CalculateWaitSecondsPerFrame()
    {
        return 1f / _frameCount;
    }
}

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(SpriteAnimation))]
public class SpriteAnimationEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SpriteAnimation animation = target as SpriteAnimation;
        if (animation.IsPlaying)
        {
            if (GUILayout.Button("Stop"))
            {
                animation.Stop();
            }
        }
        else
        {
            if (GUILayout.Button("Play"))
            {
                animation.Play();
            }
        }
    }
}
#endif