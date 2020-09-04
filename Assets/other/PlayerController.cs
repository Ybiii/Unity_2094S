using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rigidBody;
    private float speed = 0.2f;
    public AnimationClip[] animations;
    private int jumpForce = 50;
    public bool isGround = true;

    private Animator anim;
    private AnimatorStateInfo currentBaseState;

    public enum Direction { right, alt_right, forward, alt_forward, stay }
    Direction dir;

    // Start is called before the first frame update
    void Start()
    {
        dir = Direction.stay;
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Fly();

        if (Input.GetKey(KeyCode.D) && isGround)
        {
            transform.Translate(transform.right * speed);
            dir = Direction.right;
        }
        //else
        //{
        //    if (!isGround && dir == Direction.right)
        //        transform.Translate(transform.right * speed);
        //}

        if (Input.GetKey(KeyCode.A) && isGround)
        {
            transform.Translate(transform.right * -speed);
            
            dir = Direction.alt_right;
        }

        if (Input.GetKey(KeyCode.W) && isGround)
        {
            dir = Direction.forward;
            transform.Translate(transform.forward * speed);
        }

        if (Input.GetKey(KeyCode.S) && isGround)
        {
            dir = Direction.alt_forward;
            transform.Translate(transform.forward * -speed);
        }


        //Jump();
    }

    //движение по инерции в полете после прыжка
    private void Fly()
    {
        if (!isGround)
        {
            switch (dir)
            {
                case Direction.right:
                    transform.Translate(transform.right * speed);
                    break;

                case Direction.alt_right:
                    transform.Translate(transform.right * -speed);
                    break;

                case Direction.forward:
                    transform.Translate(transform.forward * speed);
                    break;

                case Direction.alt_forward:
                    transform.Translate(transform.forward * -speed);
                    break;

                case Direction.stay:
                    break;
            }
        }
        else
        {
            dir = Direction.stay;
        }
    }

    public void Jump()
    {
        //создаем луч из центра обьекта вниз
        Ray ray = new Ray(gameObject.transform.position, Vector3.down);

        //если луч сталкивается с поверхностью
        //3 параметр длина луча
        RaycastHit rh;
        if (Physics.Raycast(ray, out rh, 0.5f))
        {
            isGround = true;
        }
        else
        { isGround = false; }

        if (Input.GetKey(KeyCode.Space) && isGround)
        {
            print("Jump");
            rigidBody.AddForce(Vector3.up * jumpForce);
        }      
    }
}
