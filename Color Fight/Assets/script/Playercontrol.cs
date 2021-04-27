using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Playercontrol : MonoBehaviour
{
    float inputX;
    public float speed;
    public float d_speed;
    public float jump_power;
    public Rigidbody2D rig;
    public Animator animator;
    public bool is_ground;
    public Transform chkPos;
    public float check_Radius;
    public LayerMask ground_mask;
    public bool is_jump;
    public bool is_dash;
    public float dashspeed;
    public float dash_time;
    public float d_time;
    public int dash_count;
    public bool left_check;
    public GameObject sword;
    public bool canMove = true;
    private const float MoveSize = 5f;
    
    Vector2 move;
    SpriteRenderer sr;

    void Start()
    {
        rig.GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

    }



    void Update()
    {
        if(move.x==0)
        {
            if(rig.velocity.y<=0.01f)
                rig.velocity = new Vector2(0, rig.velocity.y);
        }
           
        //is_ground = Physics2D.OverlapCircle(chkPos.position, check_Radius, ground_mask);
        move.x = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(move.x) > 0.1 && is_jump == false)
        {
            animator.SetBool("is_walk", true);
        }
        else if (Mathf.Abs(move.x) < 0.1)
        {
            animator.SetBool("is_walk", false);
        }

        
        if (Input.GetButtonDown("Fire1"))
        {
            sword.SetActive(true);
            animator.SetTrigger("attack");
            AttackDash();
        }
        void AttackDash()
        {
            Vector2 input = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = input - (Vector2)transform.position;
            dir.Normalize();
            if (dir.y <= 0.3f)
                dir.y = 0.3f;
            rig.velocity = dir * 10f;
            canMove = false;
            StartCoroutine(cor());
        }

        IEnumerator cor()
        {
            yield return new WaitForSeconds(0.3f);
            canMove = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && is_dash == false && dash_count < 2)
        {
            is_dash = true;
            if (is_jump == true)
            {
                dash_count++;
            }
        }
        if (dash_time <= 0)
        {
            speed = d_speed;
            animator.SetBool("dash", is_dash);
            if (is_dash)
            {
                dash_time = d_time;

            }

        }
        else
        {
            dash_time -= Time.deltaTime;
            animator.SetBool("dash", true);
            speed = dashspeed;

        }
        is_dash = false;



       
        if (Input.GetButtonDown("Jump") && is_ground == true)
        {
            is_ground = false;
            is_jump = true;
            rig.AddForce(Vector2.up * jump_power, ForceMode2D.Impulse);
            animator.SetBool("is_jump", true);
            
            if (rig.velocity.y > 0)
            {

                animator.SetBool("is_onair", true);

            }

        }


       
        if (rig.velocity.y < 0)
        {
            //animator.SetBool("is_onair", false);  
            animator.SetBool("is_droping", true);

            if (is_ground == true)
            {
                animator.SetTrigger("ground");
                is_jump = false;
                /*
                animator.SetBool("is_droping", false);
                animator.SetBool("is_jump", false);
                */
            }
        }
        if (is_jump == false)
        {

            animator.SetBool("is_droping", false);
            animator.SetBool("is_jump", false);
            dash_count = 0;
        }
    }



    void FixedUpdate()
    {
        if (canMove)
        {
            if(!is_jump)
                rig.velocity=new Vector2(0,rig.velocity.y);
            transform.position += new Vector3(move.x * speed * Time.deltaTime, 0);   
        }
        Flip();

    }

    //filp
    void Flip()
    {
        if (rig.velocity.x > 0|| move.x>0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            left_check = false;
        }
        else if (rig.velocity.x < 0 || move.x<0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            left_check = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {

                animator.SetTrigger("ground");
                is_jump = false;
                is_ground = true;

        }
            
    }
}