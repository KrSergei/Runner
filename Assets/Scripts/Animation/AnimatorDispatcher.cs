using UnityEngine;

public class AnimatorDispatcher : MonoBehaviour
{
    public Animator animator;

    public void SetAnimationStart()
    {
        animator.SetTrigger("Start");
    }
}
