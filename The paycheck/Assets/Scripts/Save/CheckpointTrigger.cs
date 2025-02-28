using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

sealed class CheckpointTrigger : MonoBehaviour
{
    [Header("Event")]
    [SerializeField]
    private UnityEvent _onTrigger;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _onTrigger.Invoke();
        }
    }
}
