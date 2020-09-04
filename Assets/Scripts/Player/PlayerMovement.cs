using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    public Transform cameraTransform;
    public PlayerStatus playerStatus;
    public Animator animator;   

    public float vertical;//движение по вертикали
    public float horizontal;//по горизонатли(лево, право)
    public float moveAmount;//по диоганали
    public float rotationSpeed = 0.4f;

    public Vector3 rotationDirection;//направление поворота
    public Vector3 moveDirection;//направление движения

    public float gravityForce;
    public float jumpPower = 6;
    private float playersSpeed = 2f;

    public void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    public void Update()
    {        
        MoveUpdate();
        CharacterGravity();       
    }

    public void MoveUpdate()
    {        
        if (characterController.isGrounded)
        {           
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");
            moveAmount = Mathf.Clamp01(Mathf.Abs(vertical) + Mathf.Abs(horizontal));
            
            //анимация
            if (moveDirection.x != 0 || moveDirection.z != 0)
                animator.SetBool("Move", true);
            else
                animator.SetBool("Move", false);
           
            Vector3 moveDir = cameraTransform.forward * vertical;
            moveDir += cameraTransform.right * horizontal;
            moveDir.Normalize();
            moveDirection = moveDir;
            rotationDirection = cameraTransform.forward;

            RotationNormal();            
        }
        //инвертируем упраление поскольку без будет двигаться в противоположные необходимым направления
        moveDirection = moveDirection * -1;

        moveDirection.y = gravityForce;
        characterController.Move(moveDirection * playersSpeed * Time.deltaTime);//перемещение по направлению        
    }

    public void RotationNormal()
    {       
        //если целимся то поварачиваемся в направлении прицела
        if (!playerStatus.isAiming)
        {
            rotationDirection = moveDirection;
        }       
        Vector3 targetDir = -rotationDirection;       
        targetDir.y = 0;//чтобы не повернуться в пол
        if (targetDir == Vector3.zero)
        {
            targetDir = transform.forward;
        }
        
        //поворачиваем
        Quaternion lookDir = Quaternion.LookRotation(targetDir);
        Quaternion targetRot = Quaternion.Slerp(transform.rotation, lookDir, rotationSpeed);
        transform.rotation = targetRot;
    }

    private void CharacterGravity()
    {
        if (!characterController.isGrounded)
            gravityForce -= 20f * Time.deltaTime;
        else
            gravityForce = -1f;

        if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
        {
            gravityForce = jumpPower;
            //включаем тригер прыжка
            animator.SetTrigger("Jump");
        }                       
    }
}
