using UnityEngine;

public class ExampleComponentBehaviour : ComponentBehaviour
{
  

    protected override void Awake()
    {
        base.Awake();
        Logger.Log($"Awake");
    }

    private void Start()
    {
        //Must use start for enabling and disabling from editor
    }


    public override void OnUpdate()
    {
        base.OnUpdate();
        Debug.Log($"OnUpdate");
    }

    public override void Tick()
    {
        base.Tick();
        Debug.Log($"Tick");
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        Debug.Log($"OnFixedUpdate");
    }

    public override void FixedTick()
    {
        base.FixedTick();
        Debug.Log($"FixedTick");
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        Debug.Log($"OnLateUpdate");
    }

    public override void LateTick()
    {
        base.LateTick();
        Debug.Log($"LateTick");
    }


    protected override void OnPause()
    {
        base.OnPause();
        Debug.Log($"OnPause");
    }

    protected override void OnResume()
    {
        base.OnResume();
        Debug.Log($"OnResume");
    }
}
