using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.5f;

    [Header("Physics child")]
    [SerializeField] private Transform physicsChild;

    [Header("Line spacing")]
    [SerializeField] private Vector2 moveDirection = Vector2.right;
    [SerializeField] private float stopDistance = 0.15f;
    [SerializeField] private float minimumAheadDistance = 0.01f;
    [SerializeField] private LayerMask unitLayerMask = ~0;

    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _boxCollider2D;
    private ContactFilter2D _unitContactFilter;
    private readonly RaycastHit2D[] _unitHits = new RaycastHit2D[8];

    private void Awake()
    {
        if (!TryFindPhysicsChild())
        {
            enabled = false;
            return;
        }

        _rigidbody2D = physicsChild.GetComponent<Rigidbody2D>();
        _boxCollider2D = physicsChild.GetComponent<BoxCollider2D>();

        if (_rigidbody2D == null)
        {
            _rigidbody2D = physicsChild.gameObject.AddComponent<Rigidbody2D>();
        }

        if (_boxCollider2D == null)
        {
            _boxCollider2D = physicsChild.gameObject.AddComponent<BoxCollider2D>();
        }

        ConfigurePhysicsComponents();
        ConfigureUnitContactFilter();
    }

    private void FixedUpdate()
    {
        if (IsUnitAhead())
        {
            return;
        }

        Vector2 direction = moveDirection.normalized;
        transform.position += (Vector3)(direction * (moveSpeed * Time.fixedDeltaTime));
    }

    private bool TryFindPhysicsChild()
    {
        if (physicsChild != null)
        {
            return true;
        }

        physicsChild = transform.Find("Sprite");

        if (physicsChild != null)
        {
            return true;
        }

        if (transform.childCount > 0)
        {
            physicsChild = transform.GetChild(0);
            Debug.LogWarning($"{name} nema child objekat nazvan 'Sprite'. Koristim prvi child '{physicsChild.name}' za physics komponente.", this);
            return true;
        }

        Debug.LogWarning($"{name} nema child objekat za physics komponente. Dodaj child 'Sprite' sa Rigidbody2D i BoxCollider2D.", this);
        return false;
    }

    private void ConfigurePhysicsComponents()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        _rigidbody2D.gravityScale = 0f;
        _rigidbody2D.freezeRotation = true;

        _boxCollider2D.isTrigger = false;
    }

    private void ConfigureUnitContactFilter()
    {
        _unitContactFilter = new ContactFilter2D();
        _unitContactFilter.SetLayerMask(unitLayerMask);
        _unitContactFilter.useTriggers = false;
    }

    private bool IsUnitAhead()
    {
        Vector2 direction = moveDirection.normalized;

        if (direction == Vector2.zero)
        {
            return true;
        }

        Bounds bounds = _boxCollider2D.bounds;
        Vector2 boxSize = bounds.size;
        float castDistance = stopDistance;

        int hitCount = Physics2D.BoxCast(bounds.center, boxSize, 0f, direction, _unitContactFilter, _unitHits, castDistance);

        for (int i = 0; i < hitCount; i++)
        {
            RaycastHit2D hit = _unitHits[i];

            if (hit.collider == null || hit.collider == _boxCollider2D)
            {
                continue;
            }

            Vector2 offsetToOtherUnit = hit.collider.bounds.center - bounds.center;
            float distanceInMoveDirection = Vector2.Dot(offsetToOtherUnit, direction);

            if (distanceInMoveDirection <= minimumAheadDistance)
            {
                continue;
            }

            if (hit.collider.GetComponentInParent<UnitMovement>() != null)
            {
                return true;
            }
        }

        return false;
    }
}