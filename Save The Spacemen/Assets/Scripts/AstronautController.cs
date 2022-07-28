using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AstronautController : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer renderer;

    public string[] idleAnimationClipNames = new string[] { "idle_up", "idle_side", "idle_down", "run_side"};
    private int animIndex;

    private float airAmount; // This is a timer, amount of air left in seconds.
    private float airAmountMax = 60;
    private float airAmountMin = 30;
    public Slider airLeftSlider;
    private bool panicked = false;

    public bool gameIsStarted = false;
    

    #region Movement Variables
    private Rigidbody2D rgb2;
    public bool runTowardsShip = false;
    private float movementSpeed = .1f;
    public GameObject target;
    private Vector2 velocity;
    bool flipped = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        rgb2 = gameObject.GetComponent<Rigidbody2D>();
        renderer = gameObject.GetComponent<SpriteRenderer>();
        SetAstoOnStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsStarted)
        {
            UpdateAstronaut();
        }
        
    }

    private void FixedUpdate()
    {
        if (runTowardsShip) RunTowardsShip();
    }

    void SetAstoOnStart()
    {
        // Pick a random idle animation.
        int animIndex = Random.Range(0, 3);
        anim.Play(idleAnimationClipNames[animIndex]);

        // Pick a random amount of tile.
        airAmount = Random.Range(airAmountMin, airAmountMax);
    }

    void UpdateAstronaut()
    {
        airAmount -= Time.deltaTime;
        airLeftSlider.value = airAmount;

        if (airAmount < 10 && !panicked)
        {
            panicked = true;
            anim.Play("hit1_down");
        }
        if (airAmount < 0) StartCoroutine("KillAstronaut");
        
    }

    void RunTowardsShip()
    {
        this.transform.Translate(new Vector3(target.transform.position.x * movementSpeed * Time.deltaTime, 0f, 0f), Space.World);
    }

    IEnumerator KillAstronaut()
    {
        // Hide Slider.
        airLeftSlider.gameObject.SetActive(false);

        anim.Play("vanish");

        float lengthOfClip = anim.GetCurrentAnimatorClipInfo(0).GetLength(0);
        yield return new WaitForSeconds(lengthOfClip);

        Destroy(gameObject.transform.parent); 
    }
}
