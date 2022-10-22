using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AstronautController : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer _renderer;

    public string[] idleAnimationClipNames = new string[] { "idle_up", "idle_side", "idle_down", "run_side"};
    private int animIndex;

    public float airAmount; // This is a timer, amount of air left in seconds.
    public Slider airLeftSlider;
    private bool panicked = false;

    public bool gameIsStarted = false;
    

    #region Movement Variables
    private Rigidbody2D rgb2;
    public bool runTowardsShip = false;
    public GameObject target;
    private Vector2 velocity;


    #endregion

    #region Crushed Anims and Sounds
    public GameObject crushedAstroAnim;
    [SerializeField] private AudioClip crushAstroClip;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        rgb2 = gameObject.GetComponent<Rigidbody2D>();
        _renderer = gameObject.GetComponent<SpriteRenderer>();
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
    }

    void UpdateAstronaut()
    {
        airAmount -= Time.deltaTime;
        airLeftSlider.value = airAmount;

        if (airAmount < 10 && !panicked && !runTowardsShip)
        {
            panicked = true;
            anim.Play("hit1_down");
        }
        if (airAmount < 0) KillAstronaut();
        
    }

    public void SetVelocity(Vector2 vel)
    {
        rgb2.velocity = vel.normalized;
    }

    void RunTowardsShip()
    {
        velocity = new Vector2(target.transform.position.x - transform.position.x, 0f);
        SetVelocity(velocity);

        // Stop movement if the astronaut is outside of the bounds of the partents colider.
    }

    public void CrushAstro()
    {
        Instantiate(crushedAstroAnim, this.transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySound(crushAstroClip);
        Destroy(gameObject.transform.parent.gameObject);
        GameObject.Find("Main Controller").GetComponent<MainContoller>().AstroSavedOrDead(false);
        
    }

    public void KillAstronaut()
    {
        StartCoroutine("Kill");
    }
    IEnumerator Kill()
    {
        // Hide Slider.
        airLeftSlider.gameObject.SetActive(false);

        anim.Play("vanish");

        float lengthOfClip = anim.GetCurrentAnimatorClipInfo(0).GetLength(0);
        yield return new WaitForSeconds(lengthOfClip);

        Destroy(gameObject.transform.parent.gameObject);

        GameObject.Find("Main Controller").GetComponent<MainContoller>().AstroSavedOrDead(false);
    }

}
