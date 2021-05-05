using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isPunch;
    public GameObject attack;
    private Animator anim;
    private bool canMove = true;
    private Rigidbody2D rigid;
    private Transform target;
    private bool isChase = false;
    public float range=5f;
    public float attackRange = 2f;
    public float speed = 3;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isChase)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right*(transform.localScale.x==1 ? 1f:-1f), range,LayerMask.GetMask("Player"));
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    isChase = true;
                    target = hit.collider.transform;
                }
            }
        }
        else
        {
            if(canMove)
            {
                transform.localScale=new Vector3(Mathf.Abs(transform.localScale.x)*(transform.position.x>target.transform.position.x ? -1 : 1),transform.localScale.y);
                
                anim.Play("Walk");
                Vector2 dir = target.position - transform.position;
                dir.Normalize();
                dir.y = 0;
                rigid.velocity = dir * 3;
                
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right*(transform.localScale.x==1 ? 1f:-1f), attackRange,LayerMask.GetMask("Player"));
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        canMove = false;
                        anim.Play("Attack");
                        if(isPunch) 
                            StartCoroutine(Attack1());
                        else
                            StartCoroutine(Attack2());
                    }
                }
            }
            else
            {
                rigid.velocity=Vector2.zero;
            }
        }
    }

    IEnumerator Attack1()
    {
        attack.SetActive(true);
        //공격딜레이 원하는대로 넣고싶으면 저거대신에 WaitForsecond

        yield return new WaitUntil(()=>anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f);
        attack.SetActive(false);
        canMove = true;
    }
    IEnumerator Attack2()
    {
        GameObject b=Instantiate(attack, transform.GetChild(0).transform.position, quaternion.identity);
        b.transform.localScale=new Vector3(b.transform.localScale.x*(transform.localScale.x>0 ? 1 : -1),b.transform.localScale.y);
        //공격딜레이 원하는대로 넣고싶으면 저거대신에 WaitForsecond
        yield return new WaitUntil(()=>anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f);
        canMove = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sword"))
        {
            if (other.transform.parent.GetComponent<SpriteRenderer>().material.color == GetComponent<SpriteRenderer>().material.color)
            {
                canMove = false;
                StopAllCoroutines();
                StartCoroutine(Die());
            }
        }
        if (other.CompareTag("Counter"))
        {
            if (other.GetComponent<SpriteRenderer>().material.name == GetComponent<SpriteRenderer>().material.name)
            {
                Destroy(other.gameObject);
                canMove = false;
                StopAllCoroutines();
                StartCoroutine(Die());
            }
        }
    }
    IEnumerator Die()
    {
        if(isPunch) 
            attack.SetActive(false);
        anim.Play("Die");
        yield return new WaitUntil(()=>anim.GetCurrentAnimatorStateInfo(0).IsName("Die") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f);
        Destroy(gameObject);
    }
}
