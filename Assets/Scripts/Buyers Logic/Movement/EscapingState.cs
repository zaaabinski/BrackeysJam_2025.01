using UnityEngine;

public class EscapingState : IBuyerState
{
    private BuyersMovement _buyer;
    public BuyerStateType StateType => BuyerStateType.Escaping;

    public EscapingState(BuyersMovement buyer)
    {
        _buyer = buyer;
    }

    public void EnterState() {
        _buyer.RetargetToExit();
     }

    public void UpdateState()
    {
        if (_buyer.HasReachedDestination()){
            _buyer.SetState(new HasEscapedState(_buyer));
        }
    }

    public void ExitState() { }
}