using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBall : Collectable
{
    [Range(-1, 1)]
    public int HealthModifier;

    protected override void ApplyEffect()
    {
        GameManager.Instance.Lives += HealthModifier;
        GameManager.Instance.UpdateLivesText();
    }
}
