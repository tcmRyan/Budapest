using UnityEngine;

namespace Assets.Scripts
{
    public class SimpleDragAndDrop : MonoBehaviour
    {
        public bool dragEnabled = true;
        private bool overTarget = false;
        protected Vector2 startingPosition;
        protected bool updatePosition = false;
        protected Vector2 newPosition;
        private Collider2D targetObject;

        public virtual void EnableObject()
        {
            dragEnabled = true;
            OnMouseDown();
        }

        protected virtual void OnMouseDown()
        {
            startingPosition = transform.position;
            OnDragStarted();
        }

        protected virtual void OnMouseDrag()
        {
            if (!dragEnabled)
            {
                return;
            }

            newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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

            if (overTarget)
            {
                OnDragCompleted();
                dragCompleted = true;
            }

            if (!dragCompleted)
            {
                OnDragCancelled();
            }
        }
        protected void OnDragStarted()
        {
            BaseWord baseWord = this.GetComponent<BaseWord>();
            if (baseWord)
            {
                baseWord.OnClick();
            }
        }

        protected void OnDragCancelled()
        {
            return;
        }

        protected virtual void OnDragCompleted()
        {
            Debug.Log("Drag Completed");
            if (overTarget)
            {
                overTarget = false;
                Debug.Log("Drag Completed");
                Word drag = this.GetComponent<Word>();
                Word targetWord = targetObject.GetComponent<Word>();
                drag.wordUpdate(targetWord);
            }
        }

        protected virtual void OnDragEntered(Collider2D targetObject)
        {
            this.targetObject = targetObject;
            overTarget = true;
            Debug.Log(overTarget);
        }

        protected virtual void OnDragExited(Collider2D targetObject)
        {
            overTarget = false;
            targetObject = null;
        }
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (!dragEnabled)
            {
                return;
            }

            if (other.tag == "target")
            {
                OnDragEntered(other);
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (!dragEnabled)
            {
                return;
            }
            if (other.tag == "target")
            {
                OnDragExited(other);
            }
        }
    }
}
