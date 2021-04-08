using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontrol : MonoBehaviour
{
    public float speed;
    public float d_speed;
    public float jump_power;
    public Rigidbody2D rig;
    public Animator animator;
    public bool is_ground;
    public LayerMask ground_mask;
    public bool is_jump;
    public bool is_dash;
    public float dashspeed;
    public float dash_time;
    public float d_time;
    
    Vector2 move;
    SpriteRenderer sr;

    void Start()
    {
        rig.GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

    }


    void Update()
    {
        is_ground = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 1.4f), new Vector2(0.3f, 0.1f), 0f, ground_mask);

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

        //공격
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("attack");
            //animator.ResetTrigger("attack");
            //animator.SetBool("is_attack", false);
        
        }


        //대쉬
        if (Input.GetKey(KeyCode.LeftShift))
        {
            is_dash = true;
        }
        if(dash_time <=0)
        {
            d_speed = speed;
            if (is_dash)
            {
                dash_time = d_time;
            }
            is_dash = false;
        }
        else
        {
            dash_time -= Time.deltaTime;
            d_speed = dash_time;

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
            //animator.SetBool("is_onair", false);  점프 시작이 너무 짧아 재생이 되는지도 모르게 공중에 있는 시간을 점프로 대체 
            animator.SetBool("is_droping", true);
         //착지
            if (is_ground == true)
            {
                is_jump = false;
                animator.SetTrigger("ground"); // 착지 모션이 제거 되어 기본 상태로 바로 이어짐.
                /*
                 땅에 닿으면 애니메이션 설정을 변경 하니 공중에서 공격하면 애니메이션이 끊겨서 그상태로 착지하면 점프 상태와 떨어짐 상태를변경함
                animator.SetBool("is_droping", false);
                animator.SetBool("is_jump", false);
                */ 
            }
        }
        if(is_jump == false)
        {
            
            animator.SetBool("is_droping", false);
            animator.SetBool("is_jump", false);
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
    


