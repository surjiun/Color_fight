using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontrol : MonoBehaviour
{
    public float speed = 5f;
    public float jump_power;
    public Rigidbody2D rig;
    public Animator animator;
    public bool is_ground;
    public LayerMask ground_mask;
    public bool is_jump;

    Vector2 move;
    SpriteRenderer sr;

    void Start()
    {
        rig.GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

    }


    void Update()
    {
        is_ground = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 1.5f), new Vector2(0.3f, 0.1f), 0f, ground_mask);

        move.x = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("speed", Mathf.Abs(move.x));
        if(Mathf.Abs(move.x) > 0.1 && is_jump == false)
        {
            animator.SetBool("is_walk", true);
        }
        else if(Mathf.Abs(move.x) <0.1)
        {
            animator.SetBool("is_walk", false);
        }


        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("attack");
            //animator.SetBool("is_attack", false);
   
        }

        //점프
        if (Input.GetButtonDown("Jump") && is_ground == true)
        {
            is_jump = true;
            rig.AddForce(Vector2.up * jump_power, ForceMode2D.Impulse);
            animator.SetBool("is_jump", true);
            //공중 
            if (rig.velocity.y > 0)
            {
                
                animator.SetBool("is_onair", true);
               
            }
        }
       
        
        //낙하
        if (rig.velocity.y < 0)
        {
            animator.SetBool("is_onair", false);
            animator.SetBool("is_droping", true);
            if (is_ground == true)
            {
                
                animator.SetTrigger("ground");
                animator.SetBool("is_droping", false);
                animator.SetBool("is_jump", false);
                is_jump = false;
            }
        }
    }



    void FixedUpdate()
    {
        //.MovePosition(rig.position + move * speed * Time.fixedDeltaTime);
        rig.velocity = new Vector2(move.x * speed, rig.velocity.y);
        if (move.x < 0f)
        {
            sr.flipX = true;
        }
        else if (move.x > 0f)
        {
            sr.flipX = false;
        }
    }
}
    


