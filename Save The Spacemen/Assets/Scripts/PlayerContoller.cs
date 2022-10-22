using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerContoller : MonoBehaviour
{
    #region Variables

    #region Misc
    public GameObject mainContoller;
    private Rigidbody2D rb2d;
    private float moveForceX;
    private float moveForceY;
    private Vector2 moveDirection;
    private float moveHorizontal;
    private float moveVertical;
    public bool gameIsStarted = false;
    #endregion

    #region Ship Variables
    public Sprite[] shipSprites;
    private SpriteRenderer spriteRend;
    public float shipHealth;
    public float shipFuel;
    private float minVelocityToTakeDamage;
    private float damageValue;
    private float shipWidth;
    public int bombsLeft;
    public GameObject bombPrefab;
    #endregion

    #region UI
    public Slider fuelSlider;
    public Slider healthSlider;
    #endregion

    #region Animations
    public GameObject exp01;
    public GameObject flame;
    public GameObject flameR;
    public GameObject flameL;
    public GameObject exhaust;
    #endregion

    #region Sound
    public AudioSource burnerAudioSource;
    [SerializeField] private AudioClip shipTakesDamage;
    [SerializeField] private AudioClip ShipExplodes;
    [SerializeField] private AudioClip doorCloses;
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
        shipFuel = 100f;
        shipHealth = 100f;
        minVelocityToTakeDamage = 3f;
        damageValue = 20f;
        UpdateUISliders();
        flame.SetActive(false);
        flameR.SetActive(false);
        flameL.SetActive(false);
        shipWidth = .4f;
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerInputs();
    }

    void FixedUpdate()
    {
        #region Add force to ship and toggle fuel consumption
        if (moveVertical > 0.1f || moveVertical < -.1f || moveHorizontal > 0.1f || moveHorizontal < -.1f)
        {
            rb2d.AddForce(new Vector2(moveDirection.x * moveForceX, moveDirection.y *moveForceY), ForceMode2D.Force);

            ConsumeFuel();
            ToggleBurnerSound(true); 
        }

        else
        {
            ToggleBurnerSound(false);
        }
        #endregion

        #region Down Flame
        if (moveVertical > 0.1f)
        {
            // Set the  Down flame to active.
            if (!flame.gameObject.activeSelf)
            {
                flame.SetActive(true);
            }
        }

        else
        {
            if (flame.gameObject.activeSelf)
            {
                flame.SetActive(false);
            }
        }
        #endregion

        #region Right Flame
        if (moveHorizontal < -0.1f)
        {
            // Set the  Right flame to active.
            if (!flameR.gameObject.activeSelf)
            {
                flameR.SetActive(true);
            }
        }

        else
        {
            if (flameR.gameObject.activeSelf)
            {
                flameR.SetActive(false);
            }
        }
        #endregion

        #region Left Flame
        if (moveHorizontal > 0.1f)
        {
            // Set the  Left flame to active.
            if (!flameL.gameObject.activeSelf)
            {
                flameL.SetActive(true);
            }
        }

        else
        {
            if (flameL.gameObject.activeSelf)
            {
                flameL.SetActive(false);
            }
        }
        #endregion

        #region Exhaust
        // The exhaust is controlled by the Collision COntroller script, but it needs to be turned off when the ship is at rest.
        //if (Mathf.Abs(rb2d.velocity.y) < 0.01f && exhaust.activeInHierarchy) exhaust.SetActive(false);
        #endregion
        
    }

    #region Movement Functions
    void GetPlayerInputs()
    {
        if (gameIsStarted)
        {
            moveHorizontal = Input.GetAxisRaw("Horizontal");
            moveVertical = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector2(moveHorizontal, moveVertical); // This is a vector where the x, and y components can range from -1 to 1; 

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ReleaseBomb();
            }
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
            // If the ship lands on the astronaut, then the astronaut dies.
            if (Mathf.Abs(this.transform.position.x - col.transform.position.x) < shipWidth)
            {
                col.gameObject.GetComponentInChildren<AstronautController>().CrushAstro();
            }

            else
            {
                Destroy(col.transform.parent.gameObject);
                mainContoller.GetComponent<MainContoller>().AstroSavedOrDead(true);
                SoundManager.Instance.PlaySound(doorCloses);
            }
            
        }

        else if(col.transform.tag == "TopWall")
        {
            mainContoller.GetComponent<MainContoller>().CheckToSeeIfLevelIsComplete();
        }

        else
        {
            DamgeShipOnFall(col.relativeVelocity.y);  
        }
        
    }

    public void DamgeShipOnFall(float vel)
    {
        if (vel > minVelocityToTakeDamage)
        {
            shipHealth -= damageValue;
            UpdateUISliders();

            if (shipHealth < 0)
            {
                StartCoroutine("ShipExplosion");

                PlayShipExplodesAudio();
            }

            else PlayShipTakesDamgeAudio();
        }
    }

    public void DamgeShip(float dam)
    {
        shipHealth -= dam;
        UpdateUISliders();

        if (shipHealth < 0)
        {
            StartCoroutine("ShipExplosion");

            PlayShipExplodesAudio();
        }

        else PlayShipTakesDamgeAudio();

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
            if (this.transform.position.x  - col.transform.GetChild(0).transform.position.x > 0)
            {
                col.transform.GetComponentInChildren<AstronautController>()._renderer.flipX = true;
            }

            else
            {
                col.transform.GetComponentInChildren<AstronautController>()._renderer.flipX = false;
            }
        }

        else
        {
            col.transform.GetComponentInChildren<AstronautController>().runTowardsShip = false;
            col.transform.GetComponentInChildren<AstronautController>().target = null;
            col.transform.GetComponentInChildren<AstronautController>().SetVelocity(Vector2.zero);

            // Pick a random idle animation.
            int animIndex = Random.Range(0, 3);
            col.transform.GetComponentInChildren<AstronautController>().anim.Play(col.transform.GetComponentInChildren<AstronautController>().idleAnimationClipNames[animIndex]);
        } 
            
            

    }

    IEnumerator ShipExplosion()
    {
        spriteRend.sprite = shipSprites[2];
        Instantiate(exp01, this.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3);
        mainContoller.GetComponent<MainContoller>().EndGame();
        
    }

    #endregion

    #region UI Functions

    public void UpdateUISliders()
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

    public void ToggleBurnerSound(bool on)
    {
        if (on) burnerAudioSource.volume = 1;
        else burnerAudioSource.volume = 0;

    }

    #endregion

    #region Bombs

    public void ReleaseBomb()
    {
        if (bombsLeft > 0)
        {
            Vector3 releasePosition = new Vector3(this.transform.position.x + .1f, this.transform.position.y - .4f, 0f);
            Instantiate(bombPrefab, releasePosition, Quaternion.identity);
            bombsLeft -= 1;
        }
    }

    #endregion
}


