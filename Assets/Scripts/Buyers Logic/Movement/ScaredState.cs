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
    }

    public void EnterState() {
        if (_anomalyFollowing == null){
            // This means that the anomaly it was investigating was removed by the player
            _buyer.SetState(new NormalState(_buyer));
        }
     }

    public void UpdateState()
    {
        Debug.Log(_anomalyFollowing);

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