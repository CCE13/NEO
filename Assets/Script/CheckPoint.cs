using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CheckPoint : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite checkpointHit;
    public ParticleSystem effect;
    public static event Action CheckPointHit;
    private Dashing _dashing;
    private SpriteRenderer sprite;

    private bool _played;

    private void Start()
    {
        _dashing = FindObjectOfType<PlayerManager>().GetComponent<Dashing>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _dashing.playerState != Dashing.PlayerState.Death)
        {
            collision.GetComponent<PlayerManager>()._positionToSpawn = transform.Find("SpawnPos").position;
            CheckPointHit?.Invoke();
            sprite.sprite = checkpointHit;

            if (!_played)
            {
                effect.Play();
                _played = true; 
            }
            
            
        }
    }
}
