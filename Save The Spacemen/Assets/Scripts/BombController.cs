using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rdgb;
    private float bombTimer;
    private float bombRadius;
    private float bombDam;
    private bool hasExploded = false;
    public AudioSource audioSource;
    public AudioClip bombExplodeClip;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rdgb = GetComponent<Rigidbody2D>();
        bombTimer = 5f;
        bombRadius = 1f;
        bombDam = 20f;
    }

    private void Update()
    {
        bombTimer -= Time.deltaTime;

        if (bombTimer < 0 && !hasExploded)
        {
            StartCoroutine("Explode");
            hasExploded = true;
        }
    }
 
    IEnumerator Explode()
    {
        audioSource.PlayOneShot(bombExplodeClip);
        anim.Play("BombExplode");
        rdgb.gravityScale = 0;
        ExplosionDamage(this.transform.position, bombRadius);
        float lengthOfClip = anim.GetCurrentAnimatorClipInfo(0).GetLength(0);

        yield return new WaitForSeconds(lengthOfClip);

        Destroy(gameObject);
    }


    void ExplosionDamage(Vector2 center, float radius)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.tag == "Destructable")
            {
                Destroy(hitCollider.gameObject);
            }

            else if (hitCollider.gameObject.tag == "astronauts")
            {
                hitCollider.gameObject.GetComponent<AstronautController>().KillAstronaut();
            }

            else if (hitCollider.gameObject.tag == "Player")
            {
                hitCollider.gameObject.GetComponent<PlayerContoller>().DamgeShip(bombDam);
            }


        }
    }
}
