using UnityEngine;

public class LookingState : IBuyerState
{
    public BuyerStateType StateType => BuyerStateType.Looking;

    private float _timer;
    private BuyersMovement _buyer;

    public LookingState(BuyersMovement buyer)
    {
        _buyer = buyer;
    }


    public void EnterState()
    {
        _buyer.SetLookingTrigger();
    }

    public void UpdateState()
    {
        _timer += Time.deltaTime;

        if (_timer > _buyer._resumeMovingDelay)
        {
            _buyer.SetState(new NormalState(_buyer));
        }
    }

    public void ExitState() { }
}
