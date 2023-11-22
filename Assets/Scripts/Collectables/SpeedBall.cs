using System.Linq;
using UnityEngine;

public class SpeedBall : Collectable
{
    [Range(-90, 100)]
    public int Speedmultiplier;
    protected override void ApplyEffect()
    {
        /*foreach (Ball ball in BallsManager.Instance.Balls.ToList())
        {
            //Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();
            //ballRb.velocity += (ballRb.velocity * Speedmultiplier/100);

        }*/
        BallsManager.Instance.initialBallSpeed += BallsManager.Instance.initialBallSpeed * Speedmultiplier / 100;
    }
}