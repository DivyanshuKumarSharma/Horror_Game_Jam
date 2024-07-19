using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimControllerSwitcher : MonoBehaviour
{
    public Animator animator;
    public RuntimeAnimatorController Idle;
    public RuntimeAnimatorController Moving;
    public RuntimeAnimatorController Running;
    public RuntimeAnimatorController Jumping;
    public RuntimeAnimatorController Strafing;
    public RuntimeAnimatorController NormalAttack;
    public RuntimeAnimatorController HeavyAttack;

    private PlayerMovement playerMovement;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        playerMovement = GetComponent<PlayerMovement>();
        animator.runtimeAnimatorController = Idle;
    }

    void Update()
    {
        Animate();
    }

    void Animate()
    {
        if (playerMovement.isAttacking)
        {
            SwitchController(NormalAttack);
        }
        else if (playerMovement.isHeavyAttacking)
        {
            SwitchController(HeavyAttack);
        }
        else if (playerMovement.isMoving)
        {
            SwitchController(Moving);
        }
        else if (playerMovement.isRunning)
        {
            SwitchController(Running);
        }
        else if (playerMovement.isStrafing)
        {
            SwitchController(Strafing);
        }
        else if (playerMovement.isJumping)
        {
            SwitchController(Jumping);
        }
        else
        {
            SwitchController(Idle);
        }
    }

    void SwitchController(RuntimeAnimatorController newController)
    {
        if (animator.runtimeAnimatorController != newController)
        {
            animator.runtimeAnimatorController = newController;
        }
    }
}
