using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private float baseLeft;

    private float baseRight;

    private RectTransform rectCompo;

    private void Awake()
    {
        rectCompo = GetComponent<RectTransform>();
        if (!rectCompo) {
            Destroy(this);
        }
    }

    public void UpdateSize(float percent)
    {
        rectCompo.localScale = new Vector2(percent, 1f);
    }
}
