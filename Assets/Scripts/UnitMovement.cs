using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.5f;

    private void Update()
    {
        transform.Translate(Vector2.right * (moveSpeed * Time.deltaTime));
    }
}