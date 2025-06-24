using System.Collections;
using UnityEngine;

public class LineRendererEffect : SpecialEffect
{
    private void OnEnable()
    {
        //StartCoroutine(DisableAfterSeconds());
    }

    private IEnumerator DisableAfterSeconds()
    {
        yield return new WaitForSeconds(1f);
        
        gameObject.
        gameObject.SetActive(false);
    }
}
