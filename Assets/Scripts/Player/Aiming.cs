using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    public PlayerStatus playerStatus;
    public Animator animator;
    public PlayerMovement playerMovement; 

    public bool isAiming;
    public bool debugAiming;

    public Weapon weapon;

    void Update()
    {
        if(!debugAiming)
        {
            playerStatus.isAiming = Input.GetMouseButton(1);
        }
        else
        {
            playerStatus.isAiming = isAiming;
        }

        //если целимся и жмев выстрелить (если зажмем всеровно выстрелит 1 раз)
        if(playerStatus.isAiming && Input.GetButtonDown("Fire1"))
        {
            weapon.Shoot();
        }

        if (playerStatus.isAiming)
        {
            playerStatus.isAiming = true;
        }
        else
        {
            playerStatus.isAiming = false;
        }

        if (playerStatus.isAiming)
        {
            AnimationAiming();
        }
        else
        {
            AnimationNormal();
        }

    }

    private void AnimationNormal()
    {
        animator.SetBool("Aiming", false);
    }

    private void AnimationAiming()
    {
        //float v = playerMovement.vertical;
        //float h = playerMovement.horizontal;

        animator.SetBool("Aiming", true);
    }
}
