using UnityEngine;

public class ScaredState : IBuyerState
{
    private BuyersMovement _buyer;
    public BuyerStateType StateType => BuyerStateType.Scared;

    public ScaredState(BuyersMovement buyer)
    {
        _buyer = buyer;
    }

    public void EnterState() { }

    public void UpdateState()
    {
        if (_buyer.ShouldEscape()){
            _buyer.SetState(new EscapingState(_buyer));
        }

        _buyer.LerpScaredMarkColor();
    }

    public void ExitState() { }
}