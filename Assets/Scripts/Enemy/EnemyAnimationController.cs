using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;
    private EnemyAudioController enemySoundEffect;
    private EnemyController enemy;

    private float maxSpeed;

    public void Play(string state)
    {
        if (IsPlaying(state)) return;
        
        animator.SetTrigger(state);

        if (state == "Attack") //disable weapon collider at the start of the animation
            enemy.Weapon.Collider.enabled = false;

        if (state == "Hit" || state == "Death")
            StartCoroutine(WaitForAnimation(state));
    }

    public void Stop(string state)
    {
        animator.ResetTrigger(state);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        maxSpeed = animator.speed;

        enemySoundEffect = GetComponent<EnemyAudioController>();
        enemy = GetComponent<EnemyController>();
    }

    private void OnEnable()
    {
        animator.speed = maxSpeed;
        Play("Chase");

        Stop("Attack");
        Stop("Hit");
        Stop("Death");
    }

    private bool IsPlaying(string state)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(state);
    }

    private IEnumerator WaitForAnimation(string state)
    {
        
        enemy.PauseMove();

        enemySoundEffect.Play(state);

        yield return new WaitForSeconds(GetCurrentAnimationLength());

        enemy.PauseMove();

        if (state == "Death")
            enemy.OnDeath();
        else
            enemySoundEffect.Play("Idle");
    }

    private float GetCurrentAnimationLength()
    {
        AnimatorStateInfo animatorState = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = animatorState.normalizedTime % 1;
        return animationLength;
    }
}
