
/// <summary>
/// Made by Viggy#4023 - 31/01/2019 
/// A Utility Variable for Storing Previous State 
/// and Invoking State Change Callbacks
/// </summary>
/// <typeparam name="T">IComparable Types</typeparam>

public struct ObservingVariable<T> where T : System.IComparable
{
    public T previous { get; private set; }
    private T current;
    private System.Action<T> action;

    public ObservingVariable(T value, System.Action<T> onValueChanged = null)
    {
        this.previous = value;
        this.current = value;
        this.action = onValueChanged;
    }

    public void AddCallback(System.Action<T> onValueChanged)
    {
        this.action = onValueChanged;
    }

    public T Value
    {
        get { return current; }

        set
        {
            if (!IsEqual(current, value))
            {
                action?.Invoke(value);
            }

            previous = current;
            current = value;
        }
    }

    private bool IsEqual(T t1, T t2)
    {
        return (current.CompareTo(previous) == 0);
    }

}
