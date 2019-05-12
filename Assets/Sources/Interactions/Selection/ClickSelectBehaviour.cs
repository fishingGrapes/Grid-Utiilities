using UnityEngine;
using UnityEngine.EventSystems;
using CielaSpike;
using System.Collections;

namespace VH
{
    [RequireComponent(typeof(DragSelectBehaviour))]
    public class ClickSelectBehaviour : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField, Range(0, 0.2f)]
        private float behaviourUpdateInterval = 0.1f;

        [Space]
        [SerializeField]
        private Selectables m_Selectables = null;

        #region Click Behaviour variables

        private Camera m_Camera;
        private DragSelectBehaviour m_DragBehaviour;

        private bool b_Clicked;
        private bool b_GroupSelectMode;

        private Vector2 m_ClickPosition_WorldSpace;

        #endregion

        #region Performance and GC

        private WaitForSeconds m_WaitForSeconds;

        #endregion


        #region Monobehaviour Callbacks

        protected void Awake()
        {
            m_Camera = Camera.main;
            m_DragBehaviour = this.GetComponent<DragSelectBehaviour>();
            m_WaitForSeconds = new WaitForSeconds(behaviourUpdateInterval);

            this.StartCoroutineAsync(this.PollSelectables());
        }

        private void OnDestroy()
        {
            this.StopCoroutine(PollSelectables());
            m_WaitForSeconds = null;
        }

        #endregion

        #region Event System Callbacks

        public void OnPointerClick(PointerEventData eventData)
        {
            m_ClickPosition_WorldSpace = m_Camera.ScreenToWorldPoint(eventData.position);

            b_Clicked = !m_DragBehaviour.Dragging && true;
            b_GroupSelectMode = Input.GetKey(KeyCode.LeftControl);

        }


        #endregion


        /// <summary>
        /// Runs in the background Thread and Polls through every Selectable
        /// in the Scene and does appropriate Click Select Action.
        /// </summary>
        /// <returns></returns>
        private IEnumerator PollSelectables()
        {
            yield return Ninja.JumpBack;

            while (true)
            {
                if (b_Clicked)
                {
                    b_Clicked = false;

                    int length = m_Selectables.Count;
                    for (int i = 0; i < length; i++)
                    {
                        Selectable selectable = m_Selectables.Get(i);
                        if (selectable.Rect.Contains(m_ClickPosition_WorldSpace))
                        {
                            if (b_GroupSelectMode)
                                selectable.Selected = !selectable.Selected;
                            else
                                selectable.Selected = (length > 1) ? true : !selectable.Selected;
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
