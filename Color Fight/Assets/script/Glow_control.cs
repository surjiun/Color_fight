using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Glow_control : MonoBehaviour
{
    public Material y_material;
    public Material b_material;
    public Material r_material;

    public GameObject sword;


    private void Start()
    {
        y_material = GetComponent<SpriteRenderer>().material;
        
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            GetComponent<SpriteRenderer>().material = y_material;
            sword.GetComponent<SpriteRenderer>().material = y_material;


        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            GetComponent<SpriteRenderer>().material = b_material;
            sword.GetComponent<SpriteRenderer>().material = b_material;
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            GetComponent<SpriteRenderer>().material = r_material;
            sword.GetComponent<SpriteRenderer>().material = r_material;
        }
    }
    
   

}



