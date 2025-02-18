using UnityEngine;

public class HasEscapedState : IBuyerState
{
    private BuyersMovement _buyer;
    public BuyerStateType StateType => BuyerStateType.HasEscaped;

    public HasEscapedState(BuyersMovement buyer)
    {
        _buyer = buyer;
    }

    public void EnterState() { 
        _buyer.gameObject.SetActive(false);
        GameManager.instance.RemoveBuyer(_buyer.gameObject);
    }

    public void UpdateState() { }

    public void ExitState() { }
}