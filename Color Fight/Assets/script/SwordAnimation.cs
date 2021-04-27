using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAnimation : MonoBehaviour
{
    public void DisableSword()
    {
        gameObject.SetActive(false);
        FindObjectOfType<Playercontrol>().canMove = true;
    }
}
