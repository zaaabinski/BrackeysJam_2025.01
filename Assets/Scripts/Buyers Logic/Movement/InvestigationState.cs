using UnityEngine;

public class InvestigationState : IBuyerState
{
    private BuyersMovement _buyer;
    public BuyerStateType StateType => BuyerStateType.Investigating;

    public InvestigationState(BuyersMovement buyer)
    {
        _buyer = buyer;
    }

    public void EnterState() {
        _buyer.LerpScaredMarkColor(0); // completely white mark

        _buyer.MoveToAnomaly();
        _buyer.ToggleScaredMark(true);
        _buyer.SetWalkingTrigger();
     }

    public void UpdateState()
    {
        if (_buyer.HasReachedDestination()){
            _buyer.SetState(new ScaredState(_buyer));
        }
    }

    public void ExitState() { }
}