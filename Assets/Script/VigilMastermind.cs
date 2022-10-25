using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EBaseCondition
{
    Peaceful = 0,
    Action_Manual = 1,
    Action_Improvised = 2
}


public class VigilMastermind : MonoBehaviour
{
    public static VigilMastermind _instance;
    //순찰을 돌고 경계 태세를 강화할 수 있는 로봇에 대한 인터페이스들
    List<IRobotFunction> _robotlist = new List<IRobotFunction>();
    //전투의 태세를 의미.
    EBaseCondition _currentSituation = EBaseCondition.Peaceful;
    
    //현재 경계하는 테두리를 나타낸다. ->> 벡터 리스트로 변경하고 이를 자동으로 탐지및 측정하는 시스쳄을 만들어 볼것,
    [Header("무엇보다도 일단은 시계방향으로 폴리곤 만들듯이 배치해야함")]
    [SerializeField]
    List<GameObject> _baseBorderObjectList = new List<GameObject>();
    List<Vector3> _baseBorderPointList = new List<Vector3>();
    [SerializeField]
    RobotPathListDict _robotpathdic = new RobotPathListDict();

    public void Start()
    {
        CheckBoundary();
    }
    public void Update()
    {
        
    }



    void CheckBoundary() 
    {
        //테두리를 계산한다.
        //테두리의 사이즈를 측정한 다음에 테두리의 면적에 따라 그 경계면을 순회하는 시스템을 개발할 것.
        _baseBorderPointList.Add(_baseBorderObjectList[0].transform.position);
        float totalDistance = 0;
        for (int i = 1; i < _baseBorderObjectList.Count;i++) 
        {
            _baseBorderPointList.Add(_baseBorderObjectList[i].transform.position);
            totalDistance += Vector3.Distance(_baseBorderPointList[i], _baseBorderPointList[i - 1]);
        }
        totalDistance += Vector3.Distance(_baseBorderPointList[_baseBorderObjectList.Count-1], _baseBorderPointList[0]);

        //총 스피드를 구한다.
        float totalSpeed = 0;
        foreach (KeyValuePair<IRobotFunction, List<Vector3>> _pair in _robotpathdic)
            totalSpeed += _pair.Key.GetSpeed();

        //영역에 따른 배분을 할 변수.
        float areaPercent = 0;
        Vector3 _latestPoint = new Vector3();
        //
        foreach (KeyValuePair<IRobotFunction, List<Vector3>> _pair in _robotpathdic) 
        {
            areaPercent = _pair.Key.GetSpeed()/ totalSpeed;
            areaPercent = totalDistance * areaPercent;
            // 총거리와 로봇의 거리에 따라서 이동할 거시를 정한다.




        }



        DebugRayManager.ShowRoute(_baseBorderPointList, Color.red, 6);
        //그리고 변경된 사항을 한번에 호출해서 변경함으로 중간에 발생할수있는 오류 줄임.
        //RefresrobotMove();
    }

    void RefresrobotMove() 
    {
        foreach (KeyValuePair<IRobotFunction, List<Vector3>> _pair in _robotpathdic) 
            _pair.Key.SetDPathOnRobot(_pair.Value);
    }


}
