using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gTargetManager : MonoBehaviour
{
    public GameObject targetPrefab = null;
    Transform transformP = null;

    public int targetCount = 1;
    public int goalCount = 1;

    List<gTarget> targetList = new List<gTarget>();

    public void OnEpisodeBegin()
    {
        transformP = this.transform;
        if (targetCount != targetList.Count)
        {
            for (int i = 0; i < targetCount; i++)
            {
                targetList.Add(GameObject.Instantiate(targetPrefab, transformP).GetComponent<gTarget>());
            }
        }

        foreach (var target in targetList)
        {
            float rx = 0;
            float rz = 0;

            rx = Random.value * 16 - 8;
            rz = Random.value * 16 - 8;

            target.transform.localPosition = new Vector3(rx, 0.5f, rz);
            target.gameObject.SetActive(true);
        }
    }
}
