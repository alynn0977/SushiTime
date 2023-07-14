using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    [SerializeField]
    private float speed = 400f;

    [SerializeField]
    private float maxBounceAngle = 75f;
    public Rigidbody2D ThisRigidBody { get; private set; }

    private void Awake()
    {
        ThisRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Invoke(nameof(ReturnRandomTraj), .4f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.TryGetComponent(out BallBehaviour ball))
        {
            float calculateNewAngle = CalculateRotation(ball, collision);
            Quaternion rotation = Quaternion.AngleAxis(calculateNewAngle, Vector3.forward);
            ball.ThisRigidBody.velocity = rotation * Vector2.up * ball.ThisRigidBody.velocity.magnitude;
        }
    }

    private float CalculateRotation(BallBehaviour ball, Collision2D collision)
    {
        Vector3 paddlePosition = transform.position;
        Vector2 contactPoint = collision.GetContact(0).point;

        float offset = paddlePosition.x - contactPoint.x;
        float width = collision.otherCollider.bounds.size.x * 5f;

        float currentAngle = Vector2.SignedAngle(Vector2.up, ball.ThisRigidBody.velocity);
        float bounceAngle = (offset / width) * maxBounceAngle;

        float newAngle = Mathf.Clamp(currentAngle + bounceAngle, -maxBounceAngle, maxBounceAngle);

        return newAngle;
    }
    private void ReturnRandomTraj()
    {
        Vector2 force = Vector2.zero;
        force.x = Random.Range(-1f, 1f);
        force.y = -1f;

        ThisRigidBody.AddForce(force.normalized * speed);
    }
}
