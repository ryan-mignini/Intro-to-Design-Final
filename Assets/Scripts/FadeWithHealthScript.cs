using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeWithHealthScript : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Color baseColor;
    private EntityHealth hp;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        baseColor = sprite.color;
        hp = GetComponent<EntityHealth>();
    }

    void FixedUpdate()
    {
        sprite.color = new Color(baseColor.r * hp.Health / hp.MaxHealth, baseColor.g * hp.Health / hp.MaxHealth, baseColor.b * hp.Health / hp.MaxHealth);
    }
}
