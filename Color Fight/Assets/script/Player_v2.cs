using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player_v2: MonoBehaviour
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
    public int dash_count;

    public GameObject sword;

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
        is_ground = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 1.5f), new Vector2(0.3f, 0.1f), 0f, ground_mask);

        move.x = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("speed", Mathf.Abs(move.x));
        if (Mathf.Abs(move.x) > 0.1 && is_jump == false)
        {
            animator.SetBool("is_walk", true);
        }
        else if (Mathf.Abs(move.x) < 0.1)
        {
            animator.SetBool("is_walk", false);
        }

        //����
        if (Input.GetButtonDown("Fire1"))
        {
            sword.SetActive(true);
            var scale = sword.transform.localScale;
            var pos = sword.transform.localPosition;
            var scaleX = math.abs(scale.x);
            var posX = math.abs(pos.x);
            if (sr.flipX)
            {
                transform.Translate(Vector3.left);
                scale.x = -scaleX;
                pos.x = -posX;

                sword.transform.localPosition = pos;
                sword.transform.localScale = scale;
            }
            else
            {
                transform.Translate(Vector3.right);
                scale.x = scaleX;
                pos.x = posX;

                sword.transform.localPosition = pos;
                sword.transform.localScale = scale;
            }

            var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (target != null)
            {
                Vector2 dir = target - sword.transform.position;
                var usingFlip = transform.position - target;

                var local = sword.transform.localScale;
                if (usingFlip.x > 0)
                {
                    local.x *= -1f;
                    local.y *= -1f;
                }
                sword.transform.localScale = local;


                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                sword.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            animator.SetTrigger("attack");
            //animator.ResetTrigger("attack");
            //animator.SetBool("is_attack", false);
            sword.SetActive(true);
        }

        //�뽬
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
        //.MovePosition(rig.position + move * speed * Time.fixedDeltaTime);
        rig.velocity = new Vector2(move.x * speed, rig.velocity.y);

        Flip();

    }
    //filp
    void Flip()
    {
        if (move.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);

        }
        else if (move.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);

        }
    }
}



