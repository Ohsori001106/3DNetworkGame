using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterShakeAbility : MonoBehaviour
{
    public Transform TargetTransform;
    public float Duration = 0.5f;
    public float Strength = 0.2f;

    public void Shake()
    {
        StopAllCoroutines();
        StartCoroutine(Show_Coroutine());
    }
    private IEnumerator Show_Coroutine()
    {
        float elapsedTime = 0;

        Vector3 startPosition = TargetTransform.localPosition;

        while (elapsedTime <= Duration)
        {
            elapsedTime += Time.deltaTime;
            Vector3 randomPosition = Random.insideUnitSphere.normalized * Strength;
            randomPosition.y = startPosition.y;
            TargetTransform.localPosition += randomPosition;
            yield return null;
        }

        TargetTransform.localPosition = startPosition;
    }

}
