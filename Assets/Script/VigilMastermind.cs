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
    [SerializeField]
    Transform _robotPlace;
    [SerializeField]
    Transform _boundaryPlace;

    //전투의 태세를 의미.
    EBaseCondition _currentSituation = EBaseCondition.Peaceful;
    //현재 경계하는 테두리를 나타낸다. ->> 벡터 리스트로 변경하고 이를 자동으로 탐지및 측정하는 시스쳄을 만들어 볼것,
    [Header("무엇보다도 일단은 시계방향으로 폴리곤 만들듯이 배치해야함")]
    [SerializeField]
    List<GameObject> _borderObjectList = new List<GameObject>();
    [SerializeField]
    List<Vector3> _borderPointList = new List<Vector3>();
    
    [SerializeField]
    RobotPathListDict _robotpathdic = new RobotPathListDict();

    public List<Vector3> BaseBorderPointList { get => _borderPointList; set => _borderPointList = value; }

    public void Start()
    {
        RefreshRobots();
        RefreshBoundary();
        CheckBoundary();
    }
    public void Update()
    {

    }

    void RefreshRobots()
    {
        _robotpathdic.Clear();
        for (int i = 0; i < _robotPlace.childCount; i++)
        {
            _robotpathdic.Add(_robotPlace.GetChild(i).GetComponent<VigilRobot>(), new List<Vector3>());
        }
    }

    void RefreshBoundary() 
    {
        _borderObjectList.Clear();
        for (int i = 0; i < _boundaryPlace.childCount; i++) 
        {
            _borderObjectList.Add(_boundaryPlace.GetChild(i).gameObject);
        }
    }


    //일차적으로 단순한 행동 반경의 루트를 설정하기위해서 이렇게 정한다.
    void CheckBoundary()
    {
        //또한 지금은모든 병력이 전부 순찰을 돌지만 추후에는 이 병력을 기반으로

        #region// 사전 계산 부분
        //테두리를 계산한다.
        //테두리의 사이즈를 측정한 다음에 테두리의 면적에 따라 그 경계면을 순회하는 시스템을 개발할 것.
        BaseBorderPointList.Add(_borderObjectList[0].transform.position);
        float totalDistance = 0;
        for (int i = 1; i < _borderObjectList.Count; i++)
        {
            BaseBorderPointList.Add(_borderObjectList[i].transform.position);
            totalDistance += Vector3.Distance(BaseBorderPointList[i], BaseBorderPointList[i - 1]);
        }
        BaseBorderPointList.Add(_borderObjectList[0].transform.position);

        totalDistance += Vector3.Distance(BaseBorderPointList[_borderObjectList.Count - 1], BaseBorderPointList[0]);

        //총 스피드를 구한다.
        float totalSpeed = 0;
        foreach (KeyValuePair<IRobotFunction, List<Vector3>> _pair in _robotpathdic)
            totalSpeed += _pair.Key.GetSpeed();
        #endregion


        #region

        //영역에 따른 배분을 할 변수.
        float areaPercent = 0;
        //지금은 그냥 첫 점으로 삼지만 나중에는 중앙점을 기준으로 가장 먼 로봇을 중심으로 배분 을 할것이다.
        //

        //
        //Debug.Log(totalDistance);
        int currentPoint = 1;
        Vector3 StartPoint = BaseBorderPointList[0];
        // 리스트를 짠다음에 그 리스트에서
        foreach (KeyValuePair<IRobotFunction, List<Vector3>> _pair in _robotpathdic)
        {
            areaPercent = _pair.Key.GetSpeed() / totalSpeed;
            areaPercent = totalDistance * areaPercent;
            _pair.Value.Clear();
            //시작점에서 부터 차근 차근 변경을 시작한다.

            int safe = 10;
            _pair.Value.Add(StartPoint);
            while (safe>0) 
            {
                safe--;
                //다음점과 현재 점 사이의 거리를 잰다
                Vector3 targetPoint = BaseBorderPointList[currentPoint];
                float distanceToNextDot = Vector3.Distance(StartPoint, targetPoint);
                //Debug.Log(currentPoint + " / " + BaseBorderPointList.Count + ":" + StartPoint +"\n "+ 
                //areaPercent + " / " + totalDistance + ":" + distanceToNextDot);

                //거리가 가쟈하는 영역의 크기보다 작을 경우
                if (areaPercent <= distanceToNextDot)
                {
                    //향하는 길이보다 짧은 상황 이제 방향을 잡고 해당하는 만큼 이동시켜주면 된다.
                    StartPoint -= Vector3.Normalize(StartPoint - targetPoint) * areaPercent;
                    _pair.Value.Add(StartPoint);
                    Debug.Log("Got it"+ StartPoint);
                    break;
                }
                else 
                {
                    Debug.Log("pass");
                    StartPoint = BaseBorderPointList[currentPoint];
                    currentPoint++;
                    _pair.Value.Add(StartPoint);
                }
                areaPercent -= distanceToNextDot;
                //동동 남은 길이가 남아있을 경우 다음 으로 넘어간다.
            }

            _pair.Key.SetDPathOnRobot(_pair.Value);
            _pair.Key.BeginWork();
            // 총거리와 로봇의 거리에 따라서 이동할 거시를 정한다.
        }



        //DebugRayManager.ShowRoute(BaseBorderPointList, Color.red, 6);

        #endregion
        //그리고 변경된 사항을 한번에 호출해서 변경함으로 중간에 발생할수있는 오류 줄임.
        //RefresrobotMove();
    }

    void RefresrobotMove()
    {
        foreach (KeyValuePair<IRobotFunction, List<Vector3>> _pair in _robotpathdic)
            _pair.Key.SetDPathOnRobot(_pair.Value);
    }


}
