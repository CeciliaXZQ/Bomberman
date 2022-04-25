using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D myBody;
    [SerializeField] private Transform playerSprite;


    public float m_MoveSpeed = 2.0f;

    public float m_ContactOffset = 0.05f;

    public int m_MaxIterations = 2;

    public int health = 3;

    public bool inDamage = false;

    public ContactFilter2D m_MovementFilter;

    private Rigidbody2D m_Rigidbody;
    private Vector2 m_Movement;
    private List<RaycastHit2D> m_MovementHits = new List<RaycastHit2D>(1);
    private PlayerLibrary playerLibrary;
    private float horizontalVal, verticalVal;
    private int hitCount = 0;
    public CameraController cameraController;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        FindObjectOfType<CameraController>().AssignTarget(this);
        health = 3;
    }

    // Update is called once per frame
    private void Update()
    {
       MoveDirection();
       if (Input.GetKeyDown(KeyCode.Space)) SpawnBomb();
        if (Input.GetKeyDown(KeyCode.E)) SpawnSuperBomb();
    }
    void SpawnBomb() => playerLibrary.SpawnBomb();
    void SpawnSuperBomb() => playerLibrary.SpawnSuperBomb();
    public void Damage() => playerLibrary.PlayerDamaged();
    private void FixedUpdate()
    {
        MoveUpdate();
    }

    private void MoveDirection()
    {
        horizontalVal = Input.GetAxisRaw("Horizontal");
        verticalVal = Input.GetAxisRaw("Vertical");

        m_Movement = new Vector2(horizontalVal, verticalVal);

        if (horizontalVal > 0 && playerSprite.rotation.y != 0)
        {
            playerSprite.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }else if (horizontalVal < 0 && playerSprite.rotation.y != 180)
        {
            playerSprite.rotation = Quaternion.Euler(new Vector3(0, 180, 0));

        }
    }
    void MoveUpdate()
    {
        const float Epsilon = 0.005f;

        if (m_Movement.sqrMagnitude <= Epsilon)
            return;

        var movementDirection = m_Movement.normalized;
        var distanceRemaining = m_MoveSpeed * Time.fixedDeltaTime;
        var maxIterations = m_MaxIterations;
        var startPosition = m_Rigidbody.position;

        while (
            maxIterations-- > 0 &&
            distanceRemaining > Epsilon &&
            movementDirection.sqrMagnitude > Epsilon
            )
        {
            var distance = distanceRemaining;
            var hitCount = m_Rigidbody.Cast(movementDirection, m_MovementFilter, m_MovementHits, distance);

            if (hitCount > 0)
            {
                var hit = m_MovementHits[0];
                if (hit.distance > m_ContactOffset)
                {
                    distance = hit.distance - m_ContactOffset;
                    m_Rigidbody.position += movementDirection * distance;
                }
                else
                {
                    distance = 0f;
                }

                movementDirection -= hit.normal * Vector2.Dot(movementDirection, hit.normal);
            }
            else
            {
                m_Rigidbody.position += movementDirection * distance;
            }

            distanceRemaining -= distance;
        };


        m_Movement = Vector2.zero;

        var targetPosition = m_Rigidbody.position;
        m_Rigidbody.position = startPosition;
        m_Rigidbody.MovePosition(targetPosition);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<IKillable>() != null)
        {
            hitCount++;
            if (hitCount <= 1)
            {
                Damage();
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<IKillable>() != null)
        {
            hitCount = 0;
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<IKillable>() != null)
        {
            Damage();
        }
        if(collision.gameObject.GetComponent<Door>() != null)
        {
            playerLibrary.PlayerSurvived();
        }
        RemoveOverlap(collision);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        RemoveOverlap(collision);
    }

    void RemoveOverlap(Collision2D collision)
    {
        if (m_MovementFilter.IsFilteringLayerMask(collision.collider.gameObject))
            return;

        var colliderDistance = Physics2D.Distance(collision.otherCollider, collision.collider);
        if (colliderDistance.isOverlapped)
            collision.otherRigidbody.position += colliderDistance.normal * colliderDistance.distance;
    }

    public void GetLibrary(PlayerLibrary playerLibrary) =>
this.playerLibrary = playerLibrary;

}
