using UnityEngine;

public class InfiniteParallax : MonoBehaviour
{
    private float spriteLength, startpos;
    public GameObject camera;
    public float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.x;
        spriteLength = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distanceFromCamera = (camera.transform.position.x * (1 - parallaxEffect));
        float distanceFromStartPos = (camera.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startpos + distanceFromStartPos, transform.position.y, transform.position.z);

        if (distanceFromCamera > startpos + spriteLength)
        {
            startpos += spriteLength;
        }
        else if (distanceFromCamera < startpos - spriteLength)
        {
            startpos -= spriteLength;
        }
    }
}
