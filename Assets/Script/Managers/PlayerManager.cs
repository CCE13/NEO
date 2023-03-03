using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour, ISaveable
{
    [SerializeField] private CollectableCollecter collectableCollecter;
    [SerializeField] private CollectableType orb;
    [SerializeField] private CollectableType newCollectable;
    [SerializeField] private GameObject deathParticle;
    private static string Name => "Player";
    public float respawnDuration;
    public List<GameObject> enemiesKilledSinceCheckPoint;
    public GameObject ragdoll;
    public GameObject ghost;
    public GameObject scarf;
    private GameObject queuedRagdoll;

    [Space(10)]
    private PlayerAnimation _playerAnimation;
    private Dashing _dashing;
    private DashHoldingStamina _stamina;
    public bool _recordReplay;
    private bool _playOnce;
    private float _interval;
    private float _time;
    [HideInInspector]public Vector3 _positionToSpawn;
    private GroundCheck groundCheck;

    public static event Action PlayerDied;

    public CollectableCollecter CollectableCollecter => collectableCollecter;

    private void Awake()
    {
        _dashing = GetComponent<Dashing>();
        _stamina = GetComponent<DashHoldingStamina>();
        _playerAnimation = GetComponentInChildren<PlayerAnimation>();
        groundCheck = GetComponent<GroundCheck>();
        _positionToSpawn = transform.position;

    }
    // Start is called before the first frame update
    private void Start()
    {
        queuedRagdoll = Instantiate(ragdoll, transform.position, Quaternion.identity);
        collectableCollecter.ValueChanged += OnHitOrb;
        collectableCollecter.ValueChanged += OnHitCollectable;
        _dashing.OnPlayerStateChanged += SetTime;
        if (!ScreenSwipe.returningToMainmenu)
        {
            CheckPoint.CheckPointHit += EnemyCheckpointCheck;
        }
        


        _interval = TimeController.sharedInstance.slowMoTimeScale; 
        _time = 0;
    }

    private void OnDestroy()
    {
        collectableCollecter.ValueChanged -= OnHitOrb;
        collectableCollecter.ValueChanged -= OnHitCollectable;
        _dashing.OnPlayerStateChanged -= SetTime;
        if (!ScreenSwipe.returningToMainmenu)
        {
            CheckPoint.CheckPointHit -= EnemyCheckpointCheck;
        }
        
    }

    private void SetTime(Dashing.PlayerState playerState)
    {
        if (playerState == Dashing.PlayerState.Dashing || playerState == Dashing.PlayerState.Falling || playerState == Dashing.PlayerState.Death)
        {
            TimeController.sharedInstance.TransitionToRealTime();
        }
        else if (playerState == Dashing.PlayerState.Holding)
        {
            TimeController.sharedInstance.BackToSlowMotion();
            _playOnce = false;
        }

        if (playerState == Dashing.PlayerState.Death && !_playOnce)
        {

            Death();
            StartCoroutine(ResetPlayerToCheckpoint());
            _playOnce = true;
        }
    }

    private void Death()
    {
        ragdoll.transform.position = transform.position;
        if (transform.localScale.x < 0)
        {
            ragdoll.transform.localScale = new Vector3(-ragdoll.transform.localScale.x, ragdoll.transform.localScale.y, ragdoll.transform.localScale.z);
        }
        else
        {
            ragdoll.transform.localScale = new Vector3(ragdoll.transform.localScale.x, ragdoll.transform.localScale.y, ragdoll.transform.localScale.z);
        }

        ragdoll.SetActive(true);
        ghost.SetActive(false);
        scarf.SetActive(false);
        AudioManager.Instance.Play("PlayerDeath", transform, true);
        GameObject blood = Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(blood, 2f);
    }
    private IEnumerator ResetPlayerToCheckpoint()
    {
        if (ScreenSwipe.instance)
        {
            ScreenSwipe.instance.Respawn();
        }
        yield return new WaitForSecondsRealtime(respawnDuration);
        Destroy(ragdoll);
        ragdoll = queuedRagdoll;
        queuedRagdoll = Instantiate(ragdoll, transform.position,Quaternion.identity);
        ghost.SetActive(true);
        scarf.SetActive(true);
        PlayerDied?.Invoke();


    }

    private void EnemyCheckpointCheck()
    {
        
        if (PauseMenuController.restarting) 
        {
            enemiesKilledSinceCheckPoint.Clear();
            PauseMenuController.restarting = false;
            return;
        }
        if (_dashing.playerState != Dashing.PlayerState.Dashing)
        { 
            enemiesKilledSinceCheckPoint.Clear();
            return; 
       }
        if(enemiesKilledSinceCheckPoint.Count == 0)
        {
            return;
        }
        foreach (GameObject enemies in enemiesKilledSinceCheckPoint)
        {
            Destroy(enemies);
        }
        enemiesKilledSinceCheckPoint.Clear();

    }



    private void OnHitOrb()
    {
        if (collectableCollecter.CountOf(orb) > 0)
        {
            _dashing.ResetDashSlowMo(true);
            TimeController.sharedInstance.BackToSlowMotion();
            _dashing.setDashCount(1);
            collectableCollecter.Remove(orb);
        }
    }
    private void OnHitCollectable()
    {
        if(collectableCollecter.CountOf(newCollectable) > 0)
        {
            Debug.Log("collectable collected");
        }
    }

    #region Saving

    public void Save()
    {
        PlayerData data = new PlayerData();

        data.positionX = _positionToSpawn.x;
        data.positionY = _positionToSpawn.y;
        data.positionZ = _positionToSpawn.z;

        SavingManager.Save(data, Name);
    }

    public void Load()
    {
        //_dashing.playerState = Dashing.PlayerState.Holding;

        if (SavingManager.SaveExists(Name))
        {
            PlayerData data = SavingManager.Load<PlayerData>(Name);
            _dashing.ResetDash();
            _dashing.playerState = Dashing.PlayerState.Holding;
            TimeController.sharedInstance.BackToSlowMotion();
            transform.position = new Vector3(data.positionX, data.positionY, data.positionZ);
            _stamina.ResetTime();
            foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
            {
                sprite.enabled = true;
            }
            _playerAnimation.playerAnimation.Play("Player Lighted Idle");
            transform.tag = "Player";

        }

    }

    [Serializable]
    public class PlayerData
    {
        public float positionX, positionY, positionZ;
    }


    #endregion Saving
}