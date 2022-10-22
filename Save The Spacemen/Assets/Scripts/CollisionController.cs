using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    public GameObject exhaustAnim;
    public GameObject fireAnim;
    

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.tag == "Exhaust")
        {
            exhaustAnim.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.transform.tag == "Exhaust")
        {

            if(!exhaustAnim.activeInHierarchy && fireAnim.activeInHierarchy) exhaustAnim.SetActive(true);
            else if(exhaustAnim.activeInHierarchy && !fireAnim.activeInHierarchy) exhaustAnim.SetActive(false);
        }       
    }
}
