using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using CielaSpike;

namespace VH
{
    /// <summary>
    /// Handles Mouse Drag Behaviour for Unit Selection
    /// </summary>
    public class DragSelectBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        [SerializeField, Range(0, 0.2f)]
        private float behaviourUpdateInterval = 0.1f;

        [SerializeField]
        private Selectables m_Selectables = null;

        [SerializeField]
        private GUIStyle m_MouseDragSkin = null;

        #region Drag Behaviour variables

        private Camera m_Camera;

        private bool b_Dragging;
        private bool b_GroupSelectMode;

        private Rect m_BoxRect_ScreenSpace;
        private Rect m_BoxRect_WorldSpace;

        private Vector2 m_DragStart_ScreenSpace;
        private Vector2 m_DragEnd_ScreenSpace;

        private Vector2 m_DragStart_WorldSpace;
        private Vector2 m_DragEnd_WorldSpace;

        #endregion

        #region Performance and GC

        private WaitForSeconds m_WaitForSeconds;
        private Texture2D m_DragTexture;

        #endregion

        public bool Dragging { get { return b_Dragging; } }

        #region MonoBehaviour Callbacks

        protected void Awake()
        {
            m_Camera = Camera.main;
            m_MouseDragSkin.normal.background = m_DragTexture = TextureUtils.LoadTexture_DDS("UI/DragSelect.DDS");
            m_WaitForSeconds = new WaitForSeconds(behaviourUpdateInterval);

            this.StartCoroutineAsync(PollSelectables());
        }


        private void OnGUI()
        {
            if (b_Dragging)
                GUI.Box(m_BoxRect_ScreenSpace, "", m_MouseDragSkin);
        }

        private void OnDestroy()
        {
            this.StopCoroutine(PollSelectables());

            m_WaitForSeconds = null;
            m_DragTexture = null;
        }

        #endregion

        #region Event System Callbacks

        public void OnBeginDrag(PointerEventData eventData)
        {
            b_Dragging = true;
            b_GroupSelectMode = Input.GetKey(KeyCode.LeftControl);

            m_DragStart_ScreenSpace = eventData.position;
            m_DragStart_ScreenSpace.y = Screen.height - eventData.position.y;

            m_DragStart_WorldSpace = m_Camera.ScreenToWorldPoint(eventData.position);

        }

        public void OnDrag(PointerEventData eventData)
        {
            b_Dragging = true;
            b_GroupSelectMode = Input.GetKey(KeyCode.LeftControl);

            m_DragEnd_ScreenSpace = eventData.position;
            m_DragEnd_ScreenSpace.y = Screen.height - eventData.position.y;
            m_BoxRect_ScreenSpace.CalculateFromPoints(m_DragStart_ScreenSpace, m_DragEnd_ScreenSpace);

            m_DragEnd_WorldSpace = m_Camera.ScreenToWorldPoint(eventData.position);
            m_BoxRect_WorldSpace.CalculateFromPoints(m_DragStart_WorldSpace, m_DragEnd_WorldSpace);

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            b_Dragging = false;

            m_DragEnd_ScreenSpace = eventData.position;
            m_DragEnd_ScreenSpace.y = Screen.height - eventData.position.y;
            m_BoxRect_ScreenSpace.CalculateFromPoints(m_DragStart_ScreenSpace, m_DragEnd_ScreenSpace);

            m_DragEnd_WorldSpace = m_Camera.ScreenToWorldPoint(eventData.position);
            m_BoxRect_WorldSpace.CalculateFromPoints(m_DragStart_WorldSpace, m_DragEnd_WorldSpace);

        }

        #endregion



        /// <summary>
        /// Runs in the background Thread and Polls through every Selectable
        /// in the Scene and does appropriate Drag Select Action.
        /// </summary>
        /// <returns></returns>
        private IEnumerator PollSelectables()
        {
            yield return Ninja.JumpBack;

            while (true)
            {
                if (b_Dragging)
                {
                    int count = m_Selectables.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Selectable selectable = m_Selectables.Get(i);
                        if (m_BoxRect_WorldSpace.Contains(selectable.Position))
                        {
                            selectable.Selected = true;
                            //If Selected add to a List
                        }
                        else
                        {
                            selectable.Selected = b_GroupSelectMode ? selectable.Selected : false;
                        }
                    }
                }
                yield return m_WaitForSeconds;
            }
        }


    }
}
