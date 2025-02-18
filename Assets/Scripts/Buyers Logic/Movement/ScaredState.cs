public class ScaredState : IBuyerState
{
    private BuyersMovement _buyer;
    public BuyerStateType StateType => BuyerStateType.Normal;

    public ScaredState(BuyersMovement buyer)
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