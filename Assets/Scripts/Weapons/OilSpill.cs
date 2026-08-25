
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class OilSpill : Ammunition
{
    private float m_LifeTime = 0;

    private void Awake()
    {
        AmmunitionType = AmmunitionType.Oil;
    }

    private void OnEnable()
    {
        m_LifeTime = 0;
        StartCoroutine(ScaleTo(Vector3.one));
    }

    private void Update()
    {
        m_LifeTime += Time.deltaTime;

        if (m_LifeTime > 60)
        {
            ScaleTo(Vector3.zero);
            Deactivate();
        }
    }

    IEnumerator ScaleTo(Vector3 targetScale)
    {
        var startScale = transform.localScale;
        var timer = 0f;

        while (timer < 5)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, timer / 1);
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
