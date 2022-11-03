using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;


public class gRollerAgent : Agent
{
    Rigidbody rBody;
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }
    public GameObject viewModel = null;

    public gTrainingAreaManager trainingAreaManager = null;
    int Pointlast = 0;
    int Point = 0;

    public override void OnEpisodeBegin()
    {
        //에피소드 시작시, 포지션 초기화
        if (this.transform.localPosition.y < 0)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
        }

        //타겟의 위치를 설정
        

        Pointlast = 0;
        Point = 0;


    }
    
    

    /// 강화학습 행동 결정
    ///
    
    public float forceMultiplier = 10;
    
    float m_ForwardSpeed = 1.0f;

    public void EnteredTarget()
    {
        Point++;
    }
   
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {

        
       

        

        //Rewards
        

        
        if (Pointlast < Point)
        {
            SetReward(1.0f);
            Pointlast = Point;
        }

        if (trainingAreaManager.targetManager.goalCount <= Point)
        {
            trainingAreaManager.needEpisodeEnd();
            
        }

        MoveAgent(actionBuffers.DiscreteActions);

     
    }
    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var forwardAxis = act[0];
        var rotateAxis = act[1];

        switch (forwardAxis)
        {
            case 1:
                dirToGo = transform.forward * m_ForwardSpeed;
                break;
        }

        switch (rotateAxis)
        {
            case 1:
                rotateDir = transform.up * -1f;
                break;
            case 2:
                rotateDir = transform.up * 1f;
                break;
        }

        transform.Rotate(rotateDir, Time.deltaTime * 100f);
        rBody.AddForce(dirToGo * forceMultiplier, ForceMode.VelocityChange);


    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut.Clear();
        //Forward
        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }

        //Rotate
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[1] = 2;
        }
    }
        
    
}
