using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Brick : MonoBehaviour
{
    [HideInInspector] public SpriteRenderer sr;

    public int Hitpoints = 1;
    public ParticleSystem DestroyEffect;
    public static event Action<Brick> OnBrickDestruction;

    private void Awake()
    {
        this.sr = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();

        if (ball != null)
        {
            ApplycollisionLogic(ball);
        }
    }

    private void ApplycollisionLogic(Ball ball)
    {
        this.Hitpoints--;

        if (this.Hitpoints <= 0)
        {
            BricksManager.Instance.RemainingBricks.Remove(this);
            OnBrickDestruction?.Invoke(this);

           SpawnDestroyEffect();
            Destroy(this.gameObject);
        }
        else if (this.Hitpoints <= 4)
        {
            this.sr.sprite = BricksManager.Instance.Sprites[this.Hitpoints - 1];
        }
        else this.sr.sprite = BricksManager.Instance.Sprites[4]; // max
    }

    private void SpawnDestroyEffect()
    {
        Vector3 brickPos = gameObject.transform.position;
        Vector3 spawnPos = new Vector3(brickPos.x, brickPos.y, brickPos.z - 0.2f);
        GameObject effect = Instantiate(DestroyEffect.gameObject, spawnPos, Quaternion.identity);

        MainModule mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = this.sr.color;
        Destroy(effect, DestroyEffect.main.startLifetime.constant);
    }

    internal void Init(Transform containerTransform, Sprite sprite, Color color, int hitpoints)
    {
        this.transform.SetParent(containerTransform);
        this.sr.sprite = sprite;
        this.sr.color = color;
        this.Hitpoints = hitpoints;
    }
}
