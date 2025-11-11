using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int n = 10000000;
        using (ProTimer p = new ProTimer("Function"))
        {
            Function(n);
        }
        using (ProTimer p = new ProTimer("Function", n))
        {
            Function(n);
        }
    }

    void Function(int count)
    {
        int t = 0;
        for(int i= 0; i< count ; i++)
        {
            t++;
        }

    }
    // Update is called once per frame
    void Update()
    {
        CustomProfile();
    }

    private void CustomProfile()
    {
        UnityEngine.Profiling.Profiler.BeginSample("CustomProfile");

        for(int i= 0;i< 100; i++)
        {
            CustomFunction();
        }
        UnityEngine.Profiling.Profiler.EndSample();
    }

    private void CustomFunction()
    {
        UnityEngine.Profiling.Profiler.BeginSample("CustomFunction");

        for (int i = 0; i < 100; i++)
        {
            CustomCalc();
        }
        UnityEngine.Profiling.Profiler.EndSample();
    }

    private void CustomCalc()
    {
        UnityEngine.Profiling.Profiler.BeginSample("CustomCalc");

        float t = 100, f = 0f;
        for (int i = 0; i < 100; i++)
        {
            f += Mathf.Pow(Mathf.Sin(i), t);
        }
        UnityEngine.Profiling.Profiler.EndSample();
    }
}
