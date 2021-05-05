using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    
    private void Start()
    {
        Destroy(gameObject,2f);
    }

    void Update()
    {
     transform.Translate(Vector3.right*speed*Time.deltaTime*(transform.localScale.x>0 ? 1f : -1f));   
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy") && !other.CompareTag("Bullet")&&!other.CompareTag("Sword"))
        {
            Destroy(gameObject);
        }
      

        if (other.CompareTag("Sword"))
        {
            transform.localScale=new Vector3(transform.localScale.x*-1,transform.localScale.y);
            gameObject.tag = "Counter";
        }
    }
}
