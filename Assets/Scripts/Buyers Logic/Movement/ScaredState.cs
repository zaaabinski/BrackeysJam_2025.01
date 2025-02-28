using UnityEngine;

public class ScaredState : IBuyerState
{
    private BuyersMovement _buyer;
    public BuyerStateType StateType => BuyerStateType.Scared;

    private GameObject _anomalyFollowing;
    private AnomalyScript _anomalyScript;

    private float _scaredTimer = 0f;

    public ScaredState(BuyersMovement buyer)
    {
        _buyer = buyer;
        _anomalyFollowing = _buyer.FindClosestAnomaly();
    }

    public void EnterState()
    { 
        if (_anomalyFollowing == null){
            _buyer.SetState(new NormalState(_buyer));
            return;
        }

        _anomalyScript = _anomalyFollowing.GetComponent<AnomalyScript>();

        if (_anomalyScript.anomalyActive == false)
        {
            _buyer.SetState(new NormalState(_buyer));
        }

        _buyer.SetLookingTrigger();
    }

    public void UpdateState()
    {
        _scaredTimer += Time.deltaTime;

        if (_anomalyScript.anomalyActive == false)
        {
            _buyer.SetState(new NormalState(_buyer));
        }

        GameObject closestAnomaly = _buyer.FindClosestAnomaly();

        if ((closestAnomaly != _anomalyFollowing)){
            _buyer.SetState(new NormalState(_buyer));
        }

        if (_scaredTimer > _buyer._scareTimeBeforeRunning){
            _buyer.SetState(new EscapingState(_buyer));
        }

        _buyer.LerpScaredMarkColor(_scaredTimer);
    }

    public void ExitState() { }
}