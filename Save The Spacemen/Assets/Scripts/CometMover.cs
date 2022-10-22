using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometMover : MonoBehaviour
{
    public  float cometSpeed; // Values of 0.5f - 5.0f
    public Vector2 cometDirection;
    private Rigidbody2D rb;
    public float cometEndPointX; // Comet will destroy itself when the x value of its position reaches this.

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + (cometDirection * cometSpeed) * Time.fixedDeltaTime);
        CheckCometPositionAndDestroyAtEndPoint();
    }    

    public void SetRandomSpeed()
    {
        cometSpeed = Random.Range(0.5f, 5.0f);
    }

    private void CheckCometPositionAndDestroyAtEndPoint()
    {
        // If the comet is moving right.
        if (cometDirection.x > 0)
        {
            if (this.transform.position.x > cometEndPointX)
            {
                Destroy(this.gameObject);
            }
        }
        

        // If the comet is moving left.
        else
        {
            if (this.transform.position.x < cometEndPointX)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Player")
        {
            col.transform.GetComponent<PlayerContoller>().DamgeShip(col.transform.GetComponent<PlayerContoller>().shipHealth + 1);
            Destroy(this.gameObject);
        }

        else Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), col.transform.GetComponent<Collider2D>());

    }
}
