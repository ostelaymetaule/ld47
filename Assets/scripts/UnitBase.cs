using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class UnitBase : MonoBehaviour
{
    // Move player in 2D space
    public float maxSpeed = 3.4f;
    public float jumpHeight = 6.5f;
    public float gravityScale = 1.5f;
    public Camera mainCamera;
    public float magnitude = 0f;
    public float angle = 0f;
    bool facingRight = true;
    public float torque = 0f;
    Vector2 moveDirection = new Vector2(0, 0);
    //bool isGrounded = true;
    Vector3 cameraPos;
    Rigidbody2D r2d;
    Collider2D mainCollider;
    // Check every collider except Player and Ignore Raycast
    LayerMask layerMask = ~(1 << 2 | 1 << 8);
    Transform t;
    Animator animator;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        t = transform;
        r2d = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<Collider2D>();
        r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        r2d.gravityScale = gravityScale;
        
        facingRight = t.localScale.x > 0;
        gameObject.layer = 8;

        if (mainCamera)
            cameraPos = mainCamera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector2(0, 0);
        // Movement controls
        if ((Input.GetKey(KeyCode.A)))
        {
            moveDirection.x = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDirection.x = 1;
        }


        if ((Input.GetKey(KeyCode.W)))
        {
            moveDirection.y = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveDirection.y = -1;
        }

        if ((Input.GetKey(KeyCode.Space)))
        {
            animator.SetBool("attack", true);
            Debug.Log("attack");

        }
        else
        {
            animator.SetBool("attack", false);

            Debug.Log("attack -> false");
        }



        magnitude = moveDirection.sqrMagnitude;
      
        // Camera follow
        if (mainCamera)
            mainCamera.transform.position = new Vector3(t.position.x, cameraPos.y, cameraPos.z);
    }

    void FixedUpdate()
    {
        Bounds colliderBounds = mainCollider.bounds;
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, 0.1f, 0);
        // Apply movement velocity
        r2d.velocity = (moveDirection) * maxSpeed;
        if (moveDirection != Vector2.zero)
        {
            angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90;
            r2d.MoveRotation(angle);
            animator.SetBool("walking", true);
            Debug.Log("walking");
        }
        else
        {
            Debug.Log("walking -> false");

            animator.SetBool("walking", false);
        }
        // Simple debug
        //Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(0, 0.23f, 0), isGrounded ? Color.green : Color.red);
    }
}
