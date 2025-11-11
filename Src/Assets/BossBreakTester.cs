using UnityEngine;
public class BossBreakTester : MonoBehaviour
{
    public BossShieldBreakFX fx;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Instantiate(fx, transform.position, Quaternion.identity);
    }
}
