using UnityEngine;

/// <summary>
/// Made by Feiko Joosten 
/// Edited by Viggy#4023 - 31/01/2019
/// Components must Derive from this for
/// more Control, better Performance
/// </summary>

public class ComponentBehaviour : MonoBehaviour
{

    #region Unity Callbacks

    /// <summary>
    /// If your class uses the Awake function, please use  protected override void Awake() instead.
    /// Also don't forget to call ComponentBehaviour.Awake(); first.
    /// If your class does not use the Awake function, this object will be added to the UpdateManager automatically.
    /// Do not forget to replace your Update function with public override void UpdateMe()
    /// </summary>
    public virtual void OnUpdate() { }

    /// <summary>
    /// If your class uses the Awake function, please use  protected override void Awake() instead.
    /// Also don't forget to call ComponentBehaviour.Awake(); first.
    /// If your class does not use the Awake function, this object will be added to the UpdateManager automatically.
    /// Do not forget to replace your Fixed Update function with public override void FixedUpdateMe()
    /// </summary>
    public virtual void OnFixedUpdate() { }

    /// <summary>
    /// If your class uses the Awake function, please use  protected override void Awake() instead.
    /// Also don't forget to call ComponentBehaviour.Awake(); first.
    /// If your class does not use the Awake function, this object will be added to the UpdateManager automatically.
    /// Do not forget to replace your Late Update function with public override void LateUpdateMe()
    /// </summary>
    public virtual void OnLateUpdate() { }

    #endregion


    #region Tickables

    /// <summary>
    /// Similar To OnUpdate But can be Paused
    /// </summary>
    public virtual void Tick() { }

    /// <summary>
    /// Similar to OnFixedUpdate But can be Paused
    /// </summary>
    public virtual void FixedTick() { }

    /// <summary>
    /// Similar to OnLateUpdate But can be Paused
    /// </summary>
    public virtual void LateTick() { }

    #endregion


    #region Pause and Resume 

    public ObservingVariable<bool> Paused;

    /// <summary>
    /// Called Automagically When the Game is Paused
    /// and Scripts Implemting Interface "ITickable"
    /// won't recieve any Calls from the GameCore
    /// </summary>
    protected virtual void OnPause() { }

    /// <summary>
    /// Called Automagically When the Game is Resumed
    /// and Scripts Implemting Interface "ITickable"
    /// will start recieving Calls from the GameCore
    /// </summary>
    protected virtual void OnResume() { }

    #endregion


    protected virtual void Awake()
    {
        GameCore.AddItem(this);

        //Pause for Tickables
        Paused = new ObservingVariable<bool>();
        Paused.AddCallback((paused) =>
        {
            if (paused)
            {
                this.OnPause();
            }
            else
            {
                this.OnResume();
            }
        });
    }
    [Zenject.Inject]
    protected VH.Services.ILogService Logger;
}
