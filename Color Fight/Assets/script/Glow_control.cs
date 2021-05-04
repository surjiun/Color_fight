using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Glow_control : MonoBehaviour
{
    public Material d_material;
    public Material c_material;
    public GameObject sword;


    private void Start()
    {
        d_material = GetComponent<SpriteRenderer>().material;
        
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            D();
            
           
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            Change();
            
        }
    }
    void Change()
    {
        GetComponent<SpriteRenderer>().material = c_material;
        sword.GetComponent<SpriteRenderer>().material = c_material;
    }

    void D()
    {
        GetComponent<SpriteRenderer>().material = d_material;
        sword.GetComponent<SpriteRenderer>().material = d_material;
    }

   

}



