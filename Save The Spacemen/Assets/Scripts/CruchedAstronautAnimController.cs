using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CruchedAstronautAnimController : MonoBehaviour
{
    float lengthOfClip = .7f;
    float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > lengthOfClip) Destroy(this.gameObject);
    }
}
