using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIK : MonoBehaviour
{
    public Animator animator;
    public PlayerMovement playerMovement;
    public PlayerInventory playerInventory;
    public PlayerStatus playerStatus;
    public Transform targetLook;

    public Transform l_Hand;
    public Transform l_Hand_Target;
    public Transform r_Hand;

    Quaternion lh_Rotation;

    public float rh_Weight;//вес правой руки

    public Transform shoulder;
    public Transform aimPivot;   

    // Start is called before the first frame update
    void Start()
    {
        //находим правое плечо
        shoulder = animator.GetBoneTransform(HumanBodyBones.RightShoulder).transform;

        //пустышка
        aimPivot = new GameObject().transform;
        aimPivot.name = "aim pivot";
        aimPivot.transform.parent = transform;

        //пустышка
        r_Hand = new GameObject().transform;
        r_Hand.name = "right hand";
        r_Hand.transform.parent = aimPivot;

        //пустышка
        l_Hand = new GameObject().transform;
        l_Hand.name = "left hand";
        l_Hand.transform.parent = aimPivot;

        //устанавливаем позицию и ротацию из свойства m4
        r_Hand.position = playerInventory.firstWeapon.rHandPos;
        Quaternion rotRight = Quaternion.Euler(playerInventory.firstWeapon.rHandRot.x, playerInventory.firstWeapon.rHandRot.y, playerInventory.firstWeapon.rHandRot.z);
        r_Hand.rotation = rotRight;
    }

    // Update is called once per frame
    void Update()
    {
        lh_Rotation = l_Hand_Target.rotation;
        l_Hand.position = l_Hand_Target.position;

        //изменяем режим
        if (playerStatus.isAiming)
        {
            rh_Weight += Time.deltaTime * 2;
        }
        else
        {
            rh_Weight -= Time.deltaTime * 2;
        }
        rh_Weight = Mathf.Clamp(rh_Weight, 0, 1);
    }

    private void OnAnimatorIK()
    {
        aimPivot.position = shoulder.position;

        if (playerStatus.isAiming)
        {
            aimPivot.LookAt(targetLook);

            //следим за таргетом на  30%
            animator.SetLookAtWeight(1f, 0.3f, 1f);
            animator.SetLookAtPosition(targetLook.position);

            ////IK для рук
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rh_Weight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rh_Weight);
            animator.SetIKPosition(AvatarIKGoal.RightHand, r_Hand.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, r_Hand.rotation);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, l_Hand.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, lh_Rotation);
        }
        else
        {
            //следим за таргетом на  30%
            animator.SetLookAtWeight(.3f, .3f, .3f);
            animator.SetLookAtPosition(targetLook.position);

            ////IK для рук
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rh_Weight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rh_Weight);
            animator.SetIKPosition(AvatarIKGoal.RightHand, r_Hand.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, r_Hand.rotation);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, l_Hand.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, lh_Rotation);


        }
    }
}
