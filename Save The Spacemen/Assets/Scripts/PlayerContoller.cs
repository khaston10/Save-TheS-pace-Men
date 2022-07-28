using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerContoller : MonoBehaviour
{
    #region Variables
    // Fields that need to be populated for each scene.
    public GameObject mainContoller;
    private Rigidbody2D rb2d;
    private float moveForceX;
    private float moveForceY;
    private Vector2 moveDirection;
    private float moveHorizontal;
    private float moveVertical;

    public Sprite[] shipSprites;
    private SpriteRenderer spriteRend;
    public float shipHealth;
    public float shipFuel;
    public int shipType; // 0 - Red Ship, 1 - Grey Ship, 2 - Alien Ship
    private float[] ShipMaxHealth = new float[] {10, 20, 30};
    private float[] ShipMaxFuel = new float[] {100, 120, 140};
    private float[] ShipMinVelForDmg = new float[] { 3, 2, 2 };
    private float minVelocityToTakeDamage;

    public Slider fuelSlider;
    public Slider healthSlider;

    public bool gameIsStarted = false;

    #region Animations
    public GameObject exp01;
    public GameObject flame;
    #endregion

    #region Sound
    [SerializeField] private AudioClip shipTakesDamage;
    [SerializeField] private AudioClip ShipExplodes;
    #endregion

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        spriteRend = gameObject.GetComponent<SpriteRenderer>();
        moveForceY = 20f;
        moveForceX = 5f;

        // Set the ship attributes.
        shipType = 0;
        shipFuel = ShipMaxFuel[0];
        shipHealth = ShipMaxHealth[0];
        minVelocityToTakeDamage = ShipMinVelForDmg[0];
        UpdateUISliders();

        flame.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerInputs();
    }

    void FixedUpdate()
    {
        if (moveVertical > 0.1f || moveVertical < -.1f || moveHorizontal > 0.1f || moveHorizontal < -.1f)
        {
            rb2d.AddForce(new Vector2(moveDirection.x * moveForceX, moveDirection.y *moveForceY), ForceMode2D.Force);

            ConsumeFuel();

            // Set the flame to active.
            if (!flame.gameObject.activeSelf) flame.SetActive(true);
        }

        else
        {
            if(flame.gameObject.activeSelf) flame.SetActive(false);
        }

    }

    #region Load/ Save Functions



    #endregion

    #region Movement Functions

    void GetPlayerInputs()
    {
        
        if (gameIsStarted)
        {
            moveHorizontal = Input.GetAxisRaw("Horizontal");
            moveVertical = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector2(moveHorizontal, moveVertical); // This is a vector where the x, and y components can range from -1 to 1; 
        }
 
    }

    void ConsumeFuel()
    {
        shipFuel -= Time.deltaTime;
        UpdateUISliders();
    }

    #endregion

    #region Collisions

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "astronauts")
        {
            Destroy(col.transform.parent.gameObject);
            mainContoller.GetComponent<MainContoller>().AstroSavedOrDead(true);
        }

        else if(col.transform.tag == "TopWall")
        {
            mainContoller.GetComponent<MainContoller>().CheckToSeeIfLevelIsComplete();
        }



        else
        {
            if (Mathf.Abs(col.relativeVelocity.y) > minVelocityToTakeDamage)
            {
                shipHealth -= 10;
                UpdateUISliders();

                if (shipHealth < 0)
                {
                    StartCoroutine("ShipExplosion");

                    PlayShipExplodesAudio();
                }

                else PlayShipTakesDamgeAudio();
            }
        }
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "AstronautBase")
        {
            TriggerAstroMan(col.transform, true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.tag == "AstronautBase")
        {
            TriggerAstroMan(col.transform, false);
        }
    }

    void TriggerAstroMan(Transform col, bool runToShip)
    {
        if (runToShip)
        {
            col.transform.GetComponentInChildren<AstronautController>().runTowardsShip = true;
            col.transform.GetComponentInChildren<AstronautController>().target = gameObject;
            col.transform.GetComponentInChildren<AstronautController>().anim.Play(col.transform.GetComponentInChildren<AstronautController>().idleAnimationClipNames[3]);

            // Pick the correct run animation.
            if (this.transform.position.x  - col.transform.position.x> 0)
            {
                col.transform.GetComponentInChildren<AstronautController>().renderer.flipX = true;
            }

            else
            {
                col.transform.GetComponentInChildren<AstronautController>().renderer.flipX = false;
            }
        }

        else
        {
            col.transform.GetComponentInChildren<AstronautController>().runTowardsShip = false;
            col.transform.GetComponentInChildren<AstronautController>().target = null;

            // Pick a random idle animation.
            int animIndex = Random.Range(0, 3);
            col.transform.GetComponentInChildren<AstronautController>().anim.Play(col.transform.GetComponentInChildren<AstronautController>().idleAnimationClipNames[animIndex]);
        } 
            
            

    }

    IEnumerator ShipExplosion()
    {
        spriteRend.sprite = shipSprites[6];
        Instantiate(exp01, this.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3);
        mainContoller.GetComponent<MainContoller>().EndGame();
        
    }

    #endregion

    #region UI Functions

    void UpdateUISliders()
    {
        fuelSlider.value = shipFuel;
        healthSlider.value = shipHealth;

    }

    #endregion

    #region Sound Functions

    public void PlayShipTakesDamgeAudio()
    {
        SoundManager.Instance.PlaySound(shipTakesDamage);
    }

    public void PlayShipExplodesAudio()
    {
        SoundManager.Instance.PlaySound(ShipExplodes);
    }

    #endregion
}


