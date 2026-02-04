using UnityEngine;
using System.Collections;

public class FlagTriggerSciprt : MonoBehaviour
{
    public string requiredPlayerTag;   // "Player1" or "Player2"
    public float holdTime = 1.5f;       // seconds to stay

    private Animator animator;
    private bool activated = false;
    private Coroutine raiseCoroutine;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;

        if (other.CompareTag(requiredPlayerTag))
        {
            raiseCoroutine = StartCoroutine(RaiseAfterDelay());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (activated) return;

        if (other.CompareTag(requiredPlayerTag))
        {
            if (raiseCoroutine != null)
            {
                StopCoroutine(raiseCoroutine);
                raiseCoroutine = null;
            }
        }
    }

    IEnumerator RaiseAfterDelay()
    {
        yield return new WaitForSeconds(holdTime);

        activated = true;
        animator.SetTrigger("Raise");

        LevelFinishManager.Instance.CheckCompletion();
    }

    public bool IsActivated()
    {
        return activated;
    }
}