using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAnimations : MonoBehaviour
{

    public Animator animator;
    public void AnimationStart()
    {
        animator.SetTrigger("Start");
    }

    public void AnimationReset()
    {
        animator.ResetTrigger("Start");
    }

    public void AnimationEnd()
    {
        animator.SetTrigger("End");
    }
}
