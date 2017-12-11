using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float Health = 150f;
    public GameObject LaserPrefab;
    public int ScoreValue = 50;
    public AudioClip FireSound;
    public AudioClip DeathSound;

    private const float LaserSpeedFactor = 6f;
    private const float FireFrequency = 0.2f;
    private ScoreKeeper _scoreKeeper;

    void Start()
    {
        _scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerLaser projectile = collider.gameObject.GetComponent<PlayerLaser>();
        if (projectile)
        {
            if ((Health -= projectile.Damage) <= 0)
            {
                if (DeathSound != null) AudioSource.PlayClipAtPoint(DeathSound, Camera.main.transform.position, 1f);
                _scoreKeeper.AddScore(ScoreValue);
                Destroy(gameObject);
            }
            projectile.Destroy();
        }
    }

    void Update()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            var probabilityOfFire = Time.deltaTime * FireFrequency;
            if (Random.value < probabilityOfFire)
            {
                FireProjectile();
            }
        }
    }

    private void FireProjectile()
    {
        GetComponent<Animator>().Play("Shoot");
        float yPosition = transform.position.y - transform.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2;
        GameObject enemyLaser = Instantiate(LaserPrefab, new Vector3(transform.position.x, yPosition, transform.position.z), Quaternion.identity);
        enemyLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -LaserSpeedFactor);
        if (FireSound != null) AudioSource.PlayClipAtPoint(FireSound, Camera.main.transform.position, 1f);
    }
}
