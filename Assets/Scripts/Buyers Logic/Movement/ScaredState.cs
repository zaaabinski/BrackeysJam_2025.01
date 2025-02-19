using UnityEngine;

public class ScaredState : IBuyerState
{
    private BuyersMovement _buyer;
    public BuyerStateType StateType => BuyerStateType.Scared;

    private GameObject _anomalyFollowing;

    public ScaredState(BuyersMovement buyer)
    {
        _buyer = buyer;
        _anomalyFollowing = _buyer.FindClosestAnomaly();

        if (_anomalyFollowing == null){
            Debug.LogError("The anomaly that the buyer follows cant be null");
        }
    }

    public void EnterState() { }

    public void UpdateState()
    {
        if (_buyer.FindClosestAnomaly() != _anomalyFollowing){
            _buyer.SetState(new NormalState(_buyer));
        }

        if (_buyer.ShouldEscape()){
            _buyer.SetState(new EscapingState(_buyer));
        }

        _buyer.LerpScaredMarkColor();
    }

    public void ExitState() { }
}