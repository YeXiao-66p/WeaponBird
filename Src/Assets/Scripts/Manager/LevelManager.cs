using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager>
{
    public List<Level> levels;
    public Level level;
    public GameObject levelList;
    public List<Level> levelList2;
    public void LoadLevel(int levelID)
    {
        if(levelID <= levels.Count)
        {
            this.level = Instantiate<Level>(levels[levelID - 1], this.levelList.transform);
            this.levelList2.Add(this.level);
        }
            
    }
    public void ClearLevel()
    {
        foreach(var level in levelList2)
        {
            if(level != null)
                Destroy(level.gameObject);
        }
    }
}
