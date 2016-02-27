using UnityEngine;
using Fungus;

namespace FungusExtensions
{
    [EventHandlerInfo("Sprite",
        "Dynamic Drag Exited",
        "Dynamic Drag Exit")]
    [AddComponentMenu("")]
    class DynamicDragExited : EventHandler
    {
        [Tooltip("Draggable object to listen for drag events on")]
        public DynamicDraggable2D draggableObject;

        [Tooltip("Drag target object to listen for drag events on")]
        public Collider2D targetObject;

        public virtual void DynamicOnDragExited(DynamicDraggable2D draggableObject, Collider2D targetObject)
        {
            this.draggableObject = draggableObject;
            this.targetObject = targetObject;
            ExecuteBlock();
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
