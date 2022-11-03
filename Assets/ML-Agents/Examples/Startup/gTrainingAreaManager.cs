using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gTrainingAreaManager : MonoBehaviour
{
    public gTargetManager targetManager = null;
    public gAgentManager agentManager = null;

    void Start()
    {
        EpisodeStart();
    }

    public void EpisodeStart()
    {
        targetManager.OnEpisodeBegin();
    }

    public void needEpisodeEnd()
    {
        agentManager.EndEpisode();
    }
}
