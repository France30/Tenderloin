using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    //private EnemyAudioController enemySoundEffect;

    private float maxSpeed;

    public void Play(string state)
    {
        if (IsPlaying(state)) return;

        StopAllAnimations();

        animator.SetTrigger(state);
    }

    public void Stop(string state)
    {
        animator.ResetTrigger(state);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        maxSpeed = animator.speed;

        //enemySoundEffect = GetComponent<EnemyAudioController>();
    }

    private void OnEnable()
    {
        animator.speed = maxSpeed;

        Stop("Run");
        Stop("Jump");
    }

    private bool IsPlaying(string state)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(state);
    }

    private void StopAllAnimations()
    {
        foreach (var param in animator.parameters)
        {
            if (param.type != AnimatorControllerParameterType.Trigger) continue;

            animator.ResetTrigger(param.name);
        }
    }
}
