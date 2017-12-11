using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public float Width;
    public float Height;

    private const float SpeedFactor = 2f;
    private const float SpawnTime = 0.5f;
    private MovementController _movementController;
    private HorizontalMovementInfo _horizontalMovementInfo;
    private float _xMin;
    private float _xMax;

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height));
    }

    void Start()
    {
        SetMovementBorders();
        InitializeData();
        SpawnEnemies();
    }

    void Update()
    {
        if (IsFormationEmpty()) SpawnEnemies();
        UpdateFormationMovement();
    }

    private void SetMovementBorders()
    {
        float cameraToGameObjectDistance = Mathf.Abs(transform.position.z - Camera.main.transform.position.z);
        _xMin = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, cameraToGameObjectDistance)).x;
        _xMax = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, cameraToGameObjectDistance)).x;
    }

    private void UpdateFormationMovement()
    {
        var leftBoundary = transform.position.x - Width / 2;
        var righBoundary = transform.position.x + Width / 2;
        if (leftBoundary < _xMin) _horizontalMovementInfo.MoveRight = true;
        else if (righBoundary >= _xMax) _horizontalMovementInfo.MoveRight = false;

        _movementController.MoveHorizontally(_horizontalMovementInfo);
    }

    private void SpawnEnemies()
    {
        Transform transform = GetNextEmptyFormation();
        SpawnEnemiesUntillFullBoard(transform);
    }

    private Transform GetNextEmptyFormation()
    {
        foreach (Transform childPosition in transform)
        {
            if (childPosition.childCount == 0) return childPosition;
        }
        return null;
    }

    private void SpawnEnemiesUntillFullBoard(Transform transform)
    {
        if (transform != null)
        {
            SpawnEnemyAtPosition(transform.position, transform);
            StartCoroutine(SpawnEnemies(GetNextEmptyFormation()));
        }
    }

    private void SpawnEnemyAtPosition(Vector3 position, Transform parent)
    {
        GameObject newEnemy = Instantiate(EnemyPrefab, position, Quaternion.identity);
        newEnemy.transform.parent = parent;
    }

    private IEnumerator SpawnEnemies(Transform transform)
    {
        yield return new WaitForSeconds(SpawnTime);
        SpawnEnemiesUntillFullBoard(transform);
    }

    private bool IsFormationEmpty()
    {
        foreach (Transform childPosition in transform)
        {
            if (childPosition.childCount > 0) return false;
        }
        return true;
    }

    private void InitializeData()
    {
        _movementController = FindObjectOfType<MovementController>();
        _horizontalMovementInfo = new HorizontalMovementInfo
        {
            GameObject = gameObject,
            SpeedFactor = SpeedFactor,
            LeftBoundary = _xMin,
            RightBoundary = _xMax
        };
    }   
}
