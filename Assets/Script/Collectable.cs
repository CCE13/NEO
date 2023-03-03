
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] CollectableType collectableType;
    public CollectableType CollectableType => collectableType;

    private SpriteRenderer sprite;
    private BoxCollider2D boxCollider;

    private void OnValidate()
    {
        gameObject.name = collectableType.name;
    }

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        PauseMenuController.Restarting += ResetCollectible;
        PlayerManager.PlayerDied += ResetCollectible;
    }

    public void OnDestroy()
    {
        PauseMenuController.Restarting -= ResetCollectible;
        PlayerManager.PlayerDied -= ResetCollectible;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.TryGetComponent<PlayerManager>(out var player))
        {
            //if(player.GetComponent<Dashing>().playerState != Dashing.PlayerState.Falling)
            //{
                player.CollectableCollecter.AddToList(this);
                sprite.enabled = false;
                boxCollider.enabled = false;
            //}
            
        }
    }

    public void ResetCollectible()
    {
        sprite.enabled = true;
        boxCollider.enabled = true;
    }
}
