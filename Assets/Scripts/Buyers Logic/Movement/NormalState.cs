using UnityEngine;

public class NormalState : IBuyerState
{
    private BuyersMovement _buyer;
    public BuyerStateType StateType => BuyerStateType.Normal;

    public NormalState(BuyersMovement buyer)
    {
        _buyer = buyer;
    }

    public void EnterState() { 
        _buyer.ToggleScaredMark(false);
        _buyer.SetWalkingTrigger();
        _buyer.PickRandomDestination();
    }

    public void UpdateState()
    {
        if (_buyer.CheckForAnomalies()) {
            _buyer.SetState(new InvestigationState(_buyer));
        } else if (_buyer.HasReachedDestination()) {
            _buyer.SetState(new LookingState(_buyer));
        }
    }

    public void ExitState() { }
}