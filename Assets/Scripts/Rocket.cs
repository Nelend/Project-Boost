using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;

    [SerializeField] private float forceThrust = 5f;
    [SerializeField] private float forceRotate = 5f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;


    enum State
    {
        Alive,
        Dying,
        Transcending
    }
    State state = State.Alive;

    bool collisionsDisabled = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        if (Debug.isDebugBuild)
        {
            DebugKeys();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || collisionsDisabled) return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequene();
                break;
            default:
                StartDeathSequene();
                break;
        }
    }

    private void StartSuccessSequene()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        //mainEngineParticles.Stop();
        successParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void StartDeathSequene()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        //mainEngineParticles.Stop();
        deathParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void LoadNextLevel()
    {
        if (state == State.Transcending)
        {
            SceneManager.LoadScene(1);
        }
        else if (state == State.Dying)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            StopApplyingThrust();
        }
    }

    private void StopApplyingThrust()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * forceThrust * Time.deltaTime);
        if (audioSource.isPlaying == false)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (!mainEngineParticles.isPlaying)
        {
            mainEngineParticles.Play();
        }
    }

    private void RespondToRotateInput()
    {
        rigidBody.angularVelocity = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * forceRotate * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * forceRotate * Time.deltaTime);
        }
    }

    private void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            int currentLevel = SceneManager.GetActiveScene().buildIndex;

            if (SceneManager.sceneCountInBuildSettings == currentLevel + 1)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(currentLevel + 1);
            }
            
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
        }
    }

}
