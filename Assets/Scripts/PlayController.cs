using System.Collections;
using UnityEngine;

public class PlayController : MonoBehaviour
{
    public GameObject LaserPrefab;
    public float Health = 350f;
    public AudioClip FireSound;
    public AudioClip DeathSound;

    private MovementController _movementController;
    private HorizontalMovementInfo _horizontalMovementInfo;
    private const float SpaceShipSpeedFactor = 10f;
    private const float LaserSpeedFactor = 5f;
    private const float ProjectileRepeatRate = 0.7f;
    private float _xMin;
    private float _xMax;

    void Start ()
    {
        SetMovementBorders();
        InitializeData();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        EnemyLaser projectile = collider.gameObject.GetComponent<EnemyLaser>();
        if (projectile)
        {
            if ((Health -= projectile.Damage) <= 0)
            {
                if (DeathSound != null) AudioSource.PlayClipAtPoint(DeathSound, Camera.main.transform.position, 1f);
                StartCoroutine(LoadNewLevel());
            }
            projectile.Destroy();
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _horizontalMovementInfo.MoveRight = false;
            _movementController.MoveHorizontally(_horizontalMovementInfo);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _horizontalMovementInfo.MoveRight = true;
            _movementController.MoveHorizontally(_horizontalMovementInfo);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InvokeRepeating(nameof(FireProjectile), 0f, ProjectileRepeatRate);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke();
        }
    }

    IEnumerator LoadNewLevel()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(DeathSound.length);
        Destroy(gameObject);
        FindObjectOfType<LevelManager>().LoadLevel("Win screen");
    }

    private void SetMovementBorders()
    {
        float cameraToGameObjectDistance = Mathf.Abs(transform.position.z - Camera.main.transform.position.z);
        float shipWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        _xMin = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, cameraToGameObjectDistance)).x + shipWidth / 1.5f;
        _xMax = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, cameraToGameObjectDistance)).x - shipWidth / 1.5f;
    }

    private void InitializeData()
    {
        _movementController = FindObjectOfType<MovementController>();
        _horizontalMovementInfo = new HorizontalMovementInfo
        {
            GameObject = gameObject,
            LeftBoundary = _xMin,
            RightBoundary = _xMax,
            SpeedFactor = SpaceShipSpeedFactor
        };
    }

    private void FireProjectile()
    {
        float yPosition = transform.position.y + transform.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2;
        GameObject laser = Instantiate(LaserPrefab, new Vector3(transform.position.x, yPosition, transform.position.z), Quaternion.identity);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, LaserSpeedFactor);
        if (FireSound != null) AudioSource.PlayClipAtPoint(FireSound, Camera.main.transform.position, 1f);
    }
}
