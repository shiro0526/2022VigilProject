using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gAgentManager : MonoBehaviour
{
    public List<gRollerAgent> agentList = new List<gRollerAgent>();

    public void EndEpisode()
    {

        foreach (var agent in agentList)
        {
            agent.EndEpisode();
        }    
    }
}
