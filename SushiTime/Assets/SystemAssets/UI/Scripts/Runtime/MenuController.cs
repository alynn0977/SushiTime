using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Provides controls for a menu and it's animator.
/// </summary>
[RequireComponent(typeof(Animator))]
public class MenuController : MonoBehaviour
{
    [SerializeField]
    private string parameter;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private bool isPlayOnAwake = true;

    /// <summary>
    /// Define a series of events to play On Animation Finish.
    /// </summary>
    public UnityEvent OnAnimationFinished = new UnityEvent();
    /// <summary>
    /// Define a series of events to play On Animation Reverse.
    /// </summary>
    public UnityEvent OnReverseFinished = new UnityEvent();

    /// <summary>
    /// Play the animation serialized by name..
    /// </summary>
    public void PlayAnimation()
    {
        animator.SetBool(parameter, true);
    }

    /// <summary>
    /// Reset the above serialized animation.
    /// </summary>
    public void ReverseAnimation()
    {
        animator.SetBool(parameter, false);
    }

    /// <summary>
    /// Executes the events if any defined in <see cref="OnAnimationFinished"/>.
    /// </summary>
    public void PlayOnAnimatorFinish()
    {
        OnAnimationFinished?.Invoke();
    }

    /// <summary>
    /// Executes the events if any defined in <see cref="OnReverseFinished"/>.
    /// </summary>
    public void PlayOnAnimatorReverse()
    {
        OnReverseFinished?.Invoke();
    }


    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (isPlayOnAwake)
        {
            PlayAnimation();
        }

    }
}
