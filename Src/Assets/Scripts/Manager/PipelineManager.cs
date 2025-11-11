using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipelineManager : MonoSingleton<PipelineManager>
{
    public GameObject template;

    public GameObject pipelineList;
    Queue<GameObject> pipelines = new Queue<GameObject>();
    void Start()
    {

    }
    public void Init()
    {

    }
    Coroutine coroutine = null; //StopCoroutine��
                                // Update is called once per frame
    public void StartRun()
    {
        if (pipelines.Count > 0)
        {
            foreach (var item in pipelines)
            {
                Pipeline p = item.GetComponent<Pipeline>();
                p.enabled = true;
            }
        }

        coroutine = StartCoroutine(GeneratePipelines());

    }

    public void StopRun()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        foreach (var item in pipelines)
        {
            Pipeline p = item.GetComponent<Pipeline>();
            p.enabled = false;
        }
    }
    private IEnumerator GeneratePipelines()
    {
        while (true)
        {
            if (pipelines.Count <= 5)
            {
                GeneratePipeline();
                yield return new WaitForSeconds(1.8f);
            }
            else
            {
                GeneratePipeline();
                yield return new WaitForSeconds(0.8f);
            }

        }
    }

    private void GeneratePipeline()
    {

        if (pipelines.Count <= 5)
        {
            Instantiate(template, this.pipelineList.transform);
            Pipeline p = template.GetComponent<Pipeline>();
            p.enabled = true;
            pipelines.Enqueue(template);

        }
        else
        {
            pipelines.Dequeue();
        }


    }
}
