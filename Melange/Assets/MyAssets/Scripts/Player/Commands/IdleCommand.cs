using UnityEngine;
using System.Collections;

public class IdleCommand : BaseCommand
{
    private const PlayerState _state = PlayerState.IDLE;

    // Use this for initialization
    void Start()
    {
        print("DOG");
        _isInteruptable = true;
    }
    public override void Init()
    {
        print("RUNNING");
    }
    public PlayerState GetState()
    {
        return _state;
    }

    public override void Execute()
    {
    //    print("IDLING");
    }

    public override void Reset()
    {

    }
}
