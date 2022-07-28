using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneController : MonoBehaviour
{
    public Camera mainCam;
    private float camSpeed = .01f;
    public GameObject camStart;
    public GameObject camStop;

    public GameObject ship;
    private float shipSpeed = .1f;
    private float error = .01f;
    public GameObject shipStart;
    public GameObject shipStop;

    // Start is called before the first frame update
    void Start()
    {
        ship.transform.position = shipStart.transform.position;
        mainCam.transform.position = camStart.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ship.transform.position = Vector3.MoveTowards(ship.transform.position, shipStop.transform.position, shipSpeed);
        mainCam.transform.position = Vector3.MoveTowards(mainCam.transform.position, camStop.transform.position, camSpeed);

        loadStartScene();
    }

    private void loadStartScene()
    {
        if (Mathf.Abs(ship.transform.position.x - shipStop.transform.position.x) < error && Mathf.Abs(ship.transform.position.y - shipStop.transform.position.y) < error) SceneManager.LoadScene(0);
    }
}
