using UnityEngine;

/// <summary>
/// A Compoenent that makes an Obejct "Selectable" using Mouse Actions
/// </summary>
public class Selectable : ComponentBehaviour
{

    /// <summary>
    /// The Entire Collection of Selectables in the Scene.
    /// </summary>
    [SerializeField]
    private Selectables m_Selectables = null;

    /// <summary>
    /// The Collection of Selected Selectables.
    /// </summary>
    [SerializeField]
    private Selectables m_Selected = null;

    /// <summary>
    /// Used for Checking whether the Selctable lies within a rect in the Background thread.
    /// </summary>
    [HideInInspector]
    public Vector2 Position { get; private set; }

    /// <summary>
    /// Used for Checking whether the Selctable encompassed the Mouse Click Position Background thread.
    /// </summary>
    [HideInInspector]
    public Rect Rect
    {
        get
        {
            rect.Set(Position.x - (width * 0.5f), Position.y - (height * 0.5f), width, height);
            return rect;
        }
    }

    /// <summary>
    /// Is the Selectable Currently Selected.
    /// </summary>
    [HideInInspector]
    public bool Selected
    {
        get { return isSelected; }
        set
        {

            if (value)
            {
                if (!m_Selected.Contains(this))
                    m_Selected.Add(this);

                color = Color.blue;

            }
            else
            {
                if (m_Selected.Contains(this))
                    m_Selected.Remove(this);

                color = Color.red;
            }

            isSelected = value;
        }
    }

    /// <summary>
    /// The name Of the Selectable.
    /// </summary>
    [HideInInspector]
    public string Name;

    #region Local variables 

    private Color color;

    private bool isSelected;
    private Rect rect;

    //TODO: Change to Agent Width AND Height
    private float width = 1f, height = 1f;

    #endregion


    #region ComponentBehaviour Callbacks

    protected override void Awake()
    {
        base.Awake();

        Name = this.transform.name;
        rect = new Rect();

        this.Selected = false;
    }

    public override void FixedTick()
    {
        DebugExtension.DebugPoint(this.transform.position, color, 1);
        Position = this.transform.position;
    }

    #endregion

    #region MonoBehaviour Callbacks

    private void OnEnable()
    {
        m_Selectables.Add(this);

    }

    private void OnDisable()
    {
        m_Selectables.Remove(this);
    }

    #endregion
}
