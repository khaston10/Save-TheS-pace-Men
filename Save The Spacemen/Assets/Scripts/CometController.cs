using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometController : MonoBehaviour
{
    public GameObject AreaOfBand;
    public float density; // Spans between 0.0 - 1.0
    private float timer;
    public float timeBetweenComets; // In seconds, Spans between 0.5 - 5
    public GameObject cometPrefab;
    public bool cometsMoveRight;
    private float cometStartPositionX;
    private float cometStartPositionY;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        density = 0.5f;
        timeBetweenComets = 1f;
        timer = 0f;
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > timeBetweenComets)
        {
            // Create new comet.
            GameObject cometTemp = Instantiate(cometPrefab);
            cometTemp.transform.parent = this.gameObject.transform;

            // Please the comet at the starting location.
            cometStartPositionY = this.transform.position.y + Random.Range(-(sr.bounds.size.y / 2), sr.bounds.size.y / 2);
            cometStartPositionX = this.transform.position.x + sr.bounds.size.x / 2;


            if (cometsMoveRight)
            {
                cometStartPositionX = this.transform.position.x - sr.bounds.size.x / 2;
                cometTemp.transform.position = new Vector2(cometStartPositionX, cometStartPositionY);
            }

            else cometTemp.transform.position = new Vector2(cometStartPositionX, cometStartPositionY);


            // Set comet attributes.
            cometTemp.GetComponent<CometMover>().SetRandomSpeed(); // Speed

            if (cometsMoveRight)
            {
                cometTemp.GetComponent<CometMover>().cometDirection = Vector2.right; // Direction
                cometTemp.GetComponent<CometMover>().cometEndPointX = this.transform.position.x + (sr.bounds.size.x / 2);

            }
            else
            {
                cometTemp.GetComponent<CometMover>().cometDirection = Vector2.left; // Direction
            } 
                
                
  

            timer = 0f;
        }

        else timer += Time.deltaTime;
    }
}
