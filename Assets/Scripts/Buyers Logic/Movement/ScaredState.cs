using UnityEngine;

public class ScaredState : IBuyerState
{
    private BuyersMovement _buyer;
    public BuyerStateType StateType => BuyerStateType.Scared;

    private GameObject _anomalyFollowing;
    private AnomalyScript _anomalyScript;

    public ScaredState(BuyersMovement buyer)
    {
        _buyer = buyer;
        _anomalyFollowing = _buyer.FindClosestAnomaly();
    }

    public void EnterState() {
        if (_anomalyFollowing == null){
            _buyer.SetState(new NormalState(_buyer));
            return;
        }

        _anomalyScript = _anomalyFollowing.GetComponent<AnomalyScript>();

        if (_anomalyScript.anomalyActive == false)
        {
            _buyer.SetState(new NormalState(_buyer));
        }
    }

    public void UpdateState()
    {
        if (_anomalyScript.anomalyActive == false)
        {
            _buyer.SetState(new NormalState(_buyer));
        }

        if ((_buyer.FindClosestAnomaly() != _anomalyFollowing)){
            _buyer.SetState(new NormalState(_buyer));
        }

        if (_buyer.ShouldEscape()){
            _buyer.SetState(new EscapingState(_buyer));
        }

        _buyer.LerpScaredMarkColor();
    }

    public void ExitState() { }
}