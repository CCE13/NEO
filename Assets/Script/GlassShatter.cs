using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassShatter : MonoBehaviour
{
    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;

    public ParticleSystem glassParticle;

    // Start is called before the first frame update
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        glassParticle = GetComponentInChildren<ParticleSystem>();
        PauseMenuController.Restarting += ResetGlass;
        PlayerManager.PlayerDied += ResetGlass;
    }
 
    public void OnDestroy()
    {
        PauseMenuController.Restarting -= ResetGlass;
        PlayerManager.PlayerDied -= ResetGlass;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BreakGlass();
            CameraShake.Instance.CameraShakeControl(7f, 0.2f);
            
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Weight"))
        {
            BreakGlass();
            CameraShake.Instance.CameraShakeControl(4f, 0.2f);
        }
    }

    public void BreakGlass()
    {
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
        glassParticle.Play();
        AudioManager.Instance.Play("GlassShatter", transform, true);
    }

    private void ResetGlass()
    {
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
    }
}
