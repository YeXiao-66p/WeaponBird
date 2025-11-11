using System.Collections.Generic;
using UnityEngine;

public class ItemDropRule : MonoBehaviour
{
    public List<Item> Items;
    public float dropRatio;
    public GameObject DropList;
    internal void Execute(Vector3 pos)
    {
       if(Random.Range(0f, 100f) < this.dropRatio)
       {
            Item item = Instantiate<Item>(GameUtil.GetRandom(this.Items), this.DropList.transform);
            item.transform.position = pos;
       }
    }
}
