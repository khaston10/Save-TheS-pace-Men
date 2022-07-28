using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine("WaitForExplosion");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator WaitForExplosion()
    {
        float lengthOfClip = anim.GetCurrentAnimatorClipInfo(0).GetLength(0);
        yield return new WaitForSeconds(lengthOfClip);
        Destroy(gameObject);
        
    }
}
