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

        //����
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("attack");
            //animator.ResetTrigger("attack");
            //animator.SetBool("is_attack", false);
        
        }


        //�뽬
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
        
        //����
        if (Input.GetButtonDown("Jump") && is_ground == true)
        {
            is_jump = true;
            rig.AddForce(Vector2.up * jump_power, ForceMode2D.Impulse);
            animator.SetBool("is_jump", true);
            //���� 
            if (rig.velocity.y > 0)
            {
                
                animator.SetBool("is_onair", true);
               
            }
        }
       
        
        //����
        if (rig.velocity.y < 0)
        {
            //animator.SetBool("is_onair", false);  ���� ������ �ʹ� ª�� ����� �Ǵ����� �𸣰� ���߿� �ִ� �ð��� ������ ��ü 
            animator.SetBool("is_droping", true);
         //����
            if (is_ground == true)
            {
                is_jump = false;
                animator.SetTrigger("ground"); // ���� ����� ���� �Ǿ� �⺻ ���·� �ٷ� �̾���.
                /*
                 ���� ������ �ִϸ��̼� ������ ���� �ϴ� ���߿��� �����ϸ� �ִϸ��̼��� ���ܼ� �׻��·� �����ϸ� ���� ���¿� ������ ���¸�������
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
    


