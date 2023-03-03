using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class PostProcessing : MonoBehaviour
{
    [SerializeField] private float holdingIntensity, dashingIntensity;
    private VolumeProfile _volumeProfile;
    private Vignette _vignette;
    private ChromaticAberration _chroma;
    private ColorAdjustments _color;
    private DepthOfField _depth;
    Dashing _dashing;

    public Color FallingColor;
    private Color originalColor;
    // Start is called before the first frame update

    private void Awake()
    {
        _dashing = GameObject.FindGameObjectWithTag("Player").GetComponent<Dashing>();
        _volumeProfile = GetComponent<Volume>()?.profile;
        if (!_volumeProfile) throw new System.NullReferenceException(nameof(VolumeProfile));

        // You can leave this variable out of your function, so you can reuse it throughout your class.
        if (!_volumeProfile.TryGet(out _vignette)) throw new System.NullReferenceException(nameof(_vignette));
        if (!_volumeProfile.TryGet(out _chroma)) throw new System.NullReferenceException(nameof(_chroma));
        if (!_volumeProfile.TryGet(out _color)) throw new System.NullReferenceException(nameof(_color));
        originalColor = _vignette.color.value;
        


    }
    void Start()
    {
        _dashing.OnPlayerStateChanged += SetVignette;
        PauseMenuController.gamePaused += PausedColor;
        _dashing.MustDie += MustDie;
    }

    private void OnDestroy()
    {
        _dashing.OnPlayerStateChanged -= SetVignette;
        PauseMenuController.gamePaused -= PausedColor;
        _dashing.MustDie -= MustDie;
    }
    public void SetVignette(Dashing.PlayerState player)
    {
        if(player == Dashing.PlayerState.Holding)
        {
            _vignette.color.Override(originalColor);
            _vignette.intensity.Override(holdingIntensity);
            _chroma.intensity.Override(0.25f);
        }
        else if(player == Dashing.PlayerState.Dashing)
        {
            _vignette.intensity.Override(dashingIntensity);
            _chroma.intensity.Override(0f);
        }
    }

    public void MustDie()
    {
        _vignette.color.Override(FallingColor);
        _vignette.intensity.Override(holdingIntensity);
    }

    public void PausedColor()
    {
        if (!_color.active)
        {
            _color.active = true;
            
        }
        else
        {
            _color.active = false;
            
        }
        
    }
    

  
}
