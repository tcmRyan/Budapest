using UnityEngine;
using Fungus;

namespace FungusExtensions
{
    [EventHandlerInfo("Sprite",
        "Dynamic Drag Completed",
        "This block will execute when the player drags an object and succesfully drops it on a target object")]
    [AddComponentMenu("")]
    class DynamicDragCompleted : EventHandler
    {
        bool overTarget = false;
        
        [Tooltip("The current target. Populated by Scripting")]
        public Collider2D targetObject;
        [Tooltip("Draggable object. Overriden by the script")]
        public DynamicDraggable2D draggableObject;

        public virtual bool IsOverTarget()
        {
            return overTarget;
        }

        public virtual void DynamicOnDragEntered(DynamicDraggable2D draggable, Collider2D target)
        {
            draggableObject = draggable;
            targetObject = target;
            overTarget = true;
        }

        public virtual void DynamicOnDragCompleted()
        {

            if (overTarget)
            {
                overTarget = false;
                ExecuteBlock();
                Destroy(draggableObject.gameObject);
                Destroy(targetObject.gameObject);

            }
        }

        public virtual void DynamicOnDragExited()
        {
            overTarget = false;
            targetObject = null;
        }

        public override string GetSummary()
        {
            string summary = "";
            if (draggableObject != null)
            {
                summary += "\nDraggable: " + draggableObject.name;
            }
            if (targetObject != null)
            {
                summary += "\nTarget: " + targetObject.name;
            }
            if (summary.Length == 0)
            {
                return "None";
            }

            return summary;
        }
    }
}
