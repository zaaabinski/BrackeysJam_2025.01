public class HasEscapedState : IBuyerState
{
    private BuyersMovement _buyer;
    public BuyerStateType StateType => BuyerStateType.Normal;

    public HasEscapedState(BuyersMovement buyer)
    {
        _buyer = buyer;
    }

    public void EnterState() { }

    public void UpdateState()
    {
        if (_buyer.CheckForAnomalies()) {
            _buyer.SetState(new InvestigationState(_buyer));
        } else if (_buyer.HasReachedDestination()) {
            _buyer.PickRandomDestination();
        }
    }

    public void ExitState() { }
}