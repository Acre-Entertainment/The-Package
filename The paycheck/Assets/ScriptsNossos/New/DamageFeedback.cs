using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DamageFeedback : MonoBehaviour
{
    [SerializeField]
    private float feedbackDuration;
    [SerializeField]
    private float timeBtwBlips;

    [SerializeField]
    private Color[] blipColors;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void PlayFeedback()
    {
        StopAllCoroutines();

        StartCoroutine(PlayFeedbackRoutine());
    }

    IEnumerator PlayFeedbackRoutine()
    {
        float timePassed = 0;
        float timeToBlip = 0;
        bool painted = false;

        while(timePassed < feedbackDuration)
        {
            if(timePassed >= timeToBlip)
            {
                timeToBlip += timeBtwBlips;

                if (painted)
                {
                    sr.color = Color.white;
                    painted = false;
                }
                else
                {
                    Paint();
                    painted = true;
                }
            }

            timePassed += Time.deltaTime;
            yield return null;
        }

        sr.color = Color.white;
    }

    void Paint()
    {
        sr.color = blipColors[Random.Range(0, blipColors.Length)];
    }
}
