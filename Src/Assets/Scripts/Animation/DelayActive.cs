using System.Collections;
using UnityEngine;

public class DelayActive : MonoBehaviour
{
    public float delay = 1;
    void Start()
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        this.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        StartCoroutine(Delay());
    }

}
