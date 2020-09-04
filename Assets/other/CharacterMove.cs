using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    private float speedMove = 1.5f;
    private float jumpPower = 6.5f;
    public float gravity;

    private float gravityForce;
    private Vector3 moveVector;

    private CharacterController characterController;
    private Animator characterAnimator;

    // Start is called before the first frame update
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
        CharacterGravity();
        //Shot();
    }


    //public void Shot()
    //{
    //    if(Input.GetButtonDown("Fire1"))
    //    {
    //        bullet = Instantiate(bulletPref, muzzle.position, muzzle.rotation);
    //        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * power, ForceMode.Impulse);
    //        Destroy(bullet, 2.0f);
    //    }
    //}


    //метод перемещения
    private void Move()
    {
        //перемещение по поверхности
        if (characterController.isGrounded)
        {
            //отключаем тригер прыжка при приземлении
            characterAnimator.ResetTrigger("Jump");

            moveVector = Vector3.zero;

            //управление 8 направлений
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            {
                var mv2 = Vector3.RotateTowards(Camera.main.transform.forward, Camera.main.transform.forward + Camera.main.transform.right, speedMove, 0.0f);
                Transform(Camera.main.transform.forward, true, mv2);                
            }
            else
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                var mv2 = Vector3.RotateTowards(Camera.main.transform.forward, Camera.main.transform.forward - Camera.main.transform.right, speedMove, 0.0f);
                Transform(Camera.main.transform.forward, true, mv2);
            }
            else
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            {
                var mv2 = Vector3.RotateTowards(-Camera.main.transform.forward, -Camera.main.transform.forward + Camera.main.transform.right, speedMove, 0.0f);
                Transform(Camera.main.transform.forward, true, mv2);
            }
            else
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
            { 
                var mv2 = moveVector = Vector3.RotateTowards(-Camera.main.transform.forward, -Camera.main.transform.forward - Camera.main.transform.right, speedMove, 0.0f);
                Transform(Camera.main.transform.forward, true, mv2);
            }
            else
            if (Input.GetKey(KeyCode.W))
            {
                Transform(Camera.main.transform.forward, false, new Vector3());
            }
            else
            if (Input.GetKey(KeyCode.A))
            {
                Transform(-Camera.main.transform.right, false, new Vector3());
            }
            else
            if (Input.GetKey(KeyCode.S))
            {
                Transform(-Camera.main.transform.forward, false, new Vector3());
            }
            else
            if (Input.GetKey(KeyCode.D))
            {               
                Transform(Camera.main.transform.right, false, new Vector3());
            }
            else
            {
                characterAnimator.SetBool("Move", false);
            }
        }

        moveVector.y = gravityForce;
        characterController.Move(moveVector * speedMove * Time.deltaTime);//Метод передвижение по направлению       
    }

    private void Transform(Vector3 mv, bool isTwoButton, Vector3 mv2)
    {
        moveVector = mv;
        characterAnimator.SetBool("Move", true);
        if (isTwoButton)
            moveVector = mv2;
        moveVector.y = 0;
        transform.rotation = Quaternion.LookRotation(moveVector);
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
            characterAnimator.SetTrigger("Jump");
        }
    }
}
