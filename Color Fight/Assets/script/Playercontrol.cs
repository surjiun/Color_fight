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
    public Transform wallcheak;
    public LayerMask w_layer;
    public bool iswall_jump;
    public bool iswall;
    public float is_right;
    public float slide_speed;
    public float wallch_distanse;
    public float walljump_power;
    public bool is_attack;
    Vector2 move;
    SpriteRenderer sr;

    void Start()
    {
        rig.GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

    }



    void Update()
    {
        if (move.x == 0)
        {
            if (rig.velocity.y <= 0.01f)
                rig.velocity = new Vector2(0, rig.velocity.y);
        }


        //wall slide
        iswall = Physics2D.Raycast(wallcheak.position, is_right * Vector2.right, wallch_distanse, w_layer);
        animator.SetBool("is_slide", iswall);


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


        if (Input.GetButtonDown("Fire1")&& !is_attack)
        {
            is_attack = true;
            sword.SetActive(true);
            animator.SetTrigger("attack");
            AttackDash();
        }
        void AttackDash()
        {
            Vector2 input = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = input - (Vector2)transform.position;
            dir.Normalize();

            if (dir.x > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                left_check = false;
                is_right = 1;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                left_check = true;
                is_right = -1;
            }
            
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
            is_attack = false;
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




        if (Input.GetButtonDown("Jump") && is_ground == true && iswall == false)
        {
            Jump();
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
        if (canMove && !iswall_jump)
        {
            if (!is_jump)
                rig.velocity = new Vector2(0, rig.velocity.y);
            transform.position += new Vector3(move.x * speed * Time.deltaTime, 0);

        }
        Flip();
        //wallslide

        if (iswall&& is_jump==true)
       {
           //iswall_jump = false;
          
           animator.SetBool("is_jump", false);
           rig.velocity = new Vector2(rig.velocity.x, rig.velocity.y * slide_speed);
           //walljump

           if (Input.GetButtonDown("Jump") && !iswall_jump)
           {
               iswall_jump = true;
               Invoke("FreezeX",0.3f);
               rig.velocity=Vector2.zero;
               rig.velocity = new Vector2(is_right * walljump_power*-1,  walljump_power);
               //rig.velocity = new Vector2(is_right * walljump_power, 0.9f * walljump_power);
               Flip();

           }
       }
    }   


    //wall jump delay
    void FreezeX()
    {
        iswall_jump = false;
    }
    //filp
    void Flip()
    {
        if (is_attack)
            return;
        if (rig.velocity.x > 0 || move.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            left_check = false;
            is_right = 1;
        }
        else if (rig.velocity.x < 0 || move.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            left_check = true;
            is_right = -1;
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
    void Jump()
    {
        is_ground = false;
        is_jump = true;
        rig.AddForce(Vector2.up * jump_power, ForceMode2D.Impulse);
        animator.SetBool("is_jump", true);

        

    }
    //get attack
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            StartCoroutine(die());
        }
    }

    IEnumerator die()
    {
        animator.Play("Player_die");
        canMove = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}