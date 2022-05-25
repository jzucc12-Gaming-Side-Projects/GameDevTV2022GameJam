using System;
using UnityEngine;

public class CharacterCollider : MonoBehaviour
{
    #region //Variables
    [SerializeField] private LayerMask hazardMask = -1;
    [SerializeField] private AudioSource deathSfx = null;
    [SerializeField] private AudioSource flySfx = null;
    private Animator animator = null;
    private ParticleSystem deathParticles = null;
    public static event Action OnDeath;
    #endregion


    #region //Monobehaviour
    private void Awake()
    {
        deathParticles = GetComponentInParent<ParticleSystem>();
        animator = GetComponentInParent<Animator>();
    }

    private void OnEnable()
    {
        Timer.OnVictory += flySfx.Stop;
    }

    private void OnDisable()
    {
        Timer.OnVictory -= flySfx.Stop;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(1 << other.gameObject.layer != hazardMask) return;
        animator.SetTrigger(other.tag);
    }
    #endregion

    #region //Animation events
    public void PlayDeathFX()
    {
        flySfx.Stop();
        deathSfx.Play();
        deathParticles.Play();
    }

    public void Death()
    {
        OnDeath?.Invoke();
    }
    #endregion
}