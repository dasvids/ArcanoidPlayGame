using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    #region Singleton

    private static Paddle _instance;

    public static Paddle Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else _instance = this;
    }
    #endregion

    private Camera mainCamera;
    private float paddleY;

    [HideInInspector] public float minX;
    [HideInInspector] public float maxX;
    private SpriteRenderer sr;

    public void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        paddleY = this.transform.position.y;
        this.sr = GetComponent<SpriteRenderer>();
        //(minX, maxX) = (-5.68f, 5.68f); // hardcode moment (
        CalculateBoundary();
    }

    private void Update()
    {
        PaddleMovement();
    }

    private void CalculateBoundary()
    {
        float rs = 0.24f; //хотел без хардкода, но не получилось ((
        float paddleHalfWidth = sr.bounds.extents.x; // половина ширины ракетки
        minX = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + paddleHalfWidth + rs;
        maxX = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - paddleHalfWidth - rs;
    }

    private void PaddleMovement()
    {
        float mousePositionWorldX = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, 0)).x;
        float clampedX = Mathf.Clamp(mousePositionWorldX, minX, maxX);
        this.transform.position = new Vector3(clampedX, paddleY, 0);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ball")
        {
            Rigidbody2D ballRb = coll.gameObject.GetComponent<Rigidbody2D>();
            Vector3 hitPoint = coll.contacts[0].point;
            Vector3 paddleCenter = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y);

            ballRb.velocity = Vector2.zero;

            float diff = hitPoint.x - paddleCenter.x;
            float forceX = Mathf.Sign(diff) * Mathf.Abs(diff * 200);

            ballRb.AddForce(new Vector2(forceX, BallsManager.Instance.initialBallSpeed));
        }
    }
}
