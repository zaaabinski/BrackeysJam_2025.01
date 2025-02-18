public class EscapingState : IBuyerState
{
    private BuyersMovement _buyer;
    public BuyerStateType StateType => BuyerStateType.Normal;

    public EscapingState(BuyersMovement buyer)
    {
        _buyer = buyer;
    }

    public void EnterState() { }

    public void UpdateState()
    {
        
    }

    public void ExitState() { }
}