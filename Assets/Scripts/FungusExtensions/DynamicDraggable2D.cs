using UnityEngine;
using System.Collections;
using Fungus;
using UnityEngine.Serialization;

namespace FungusExtensions
{
    public class DynamicDraggable2D : MonoBehaviour
    {

        [Tooltip("Is object dragging enabled")]
        public bool dragEnabled = true;

        [Tooltip("Move object back to its starting position when drag is cancelled")]
        [FormerlySerializedAs("returnToStartPos")]
        public bool returnOnCancelled = false;

        [Tooltip("Move object back to its starting position when drag is completed")]
        public bool returnOnCompleted = false;

        [Tooltip("Time object takes to return to its starting position")]
        public float returnDuration = 1f;

        [Tooltip("Mouse texture to use when hovering mouse over object")]
        public Texture2D hoverCursor;

        protected Vector3 startingPosition;
        protected bool updatePosition = false;
        protected Vector3 newPosition;
        //Have The draggable Script handle the collider to pass that to the drag scripts
        protected Collider2D targetObject;

        protected void Start()
        {
            gameObject.tag = "target";
        }

        protected virtual void OnMouseDown()
        {
            startingPosition = transform.position;

            foreach (DynamicDragStarted handler in GetHandlers<DynamicDragStarted>())
            {
                handler.DynamicOnDragStarted();
            }
        }

        protected virtual void OnMouseDrag()
        {
            if (!dragEnabled)
            {
                return;
            }

            float x = Input.mousePosition.x;
            float y = Input.mousePosition.y;
            float z = transform.position.z;

            newPosition = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 10f));
            newPosition.z = z;
            updatePosition = true;
        }

        protected virtual void LateUpdate()
        {
            // iTween will sometimes override the object position even if it should only be affecting the scale, rotation, etc.
            // To make sure this doesn't happen, we force the position change to happen in LateUpdate.
            if (updatePosition)
            {
                transform.position = newPosition;
                updatePosition = false;
            }
        }

        protected virtual void OnMouseUp()
        {
            if (!dragEnabled)
            {
                return;
            }

            bool dragCompleted = false;

            DynamicDragCompleted[] handlers = GetHandlers<DynamicDragCompleted>();
            foreach (DynamicDragCompleted handler in handlers)
            {
                if (handler.IsOverTarget())
                {
                    handler.DynamicOnDragCompleted();
                    dragCompleted = true;

                    if (returnOnCompleted)
                    {
                        LeanTween.move(gameObject, startingPosition, returnDuration).setEase(LeanTweenType.easeOutExpo);
                    }
                }
            }

            if (!dragCompleted)
            {
                foreach (DynamicDragCancelled handler in GetHandlers<DynamicDragCancelled>())
                {
                    handler.DynamicOnDragCancelled(this);
                }

                if (returnOnCancelled)
                {
                    LeanTween.move(gameObject, startingPosition, returnDuration).setEase(LeanTweenType.easeOutExpo);
                }
            }

        }
        protected virtual T[] GetHandlers<T>() where T : EventHandler
        {
            return GameObject.FindObjectsOfType<T>();
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (!dragEnabled)
            {
                return;
            }

            Collider2D other = col.GetComponent<Collider2D>();
            if (other.tag == "target")
            {
                foreach (DynamicDragEntered handler in GetHandlers<DynamicDragEntered>())
                {
                    handler.DynamicOnDragEntered(this, other);
                }

                foreach (DynamicDragCompleted handler in GetHandlers<DynamicDragCompleted>())
                {
                    handler.DynamicOnDragEntered(this, other);
                }
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if (!dragEnabled)
            {
                return;
            }
            Collider2D other = col.GetComponent<Collider2D>();
            if (other.tag == "target")
            {
                foreach (DynamicDragExited handler in GetHandlers<DynamicDragExited>())
                {
                    handler.DynamicOnDragExited(this, other);
                }

                foreach (DynamicDragCompleted handler in GetHandlers<DynamicDragCompleted>())
                {
                    handler.DynamicOnDragExited();
                }
            }
        }
    }
}

