using UnityEngine;
using Fungus;

namespace FungusExtensions
{
    [EventHandlerInfo("Sprite",
        "Dynamic Drag Canceled",
        "Dynamic Cancel Drag and Drop")]
    [AddComponentMenu("")]
    class DynamicDragCancelled : EventHandler
    {
        [Tooltip("Draggable object to listen for drag events on")]
        public DynamicDraggable2D draggableObject;

        public virtual void DynamicOnDragCancelled(DynamicDraggable2D draggableObject)
        {
            this.draggableObject = draggableObject;
            ExecuteBlock();      
        }

        public override string GetSummary()
        {
            if (draggableObject != null)
            {
                return draggableObject.name;
            }

            return "None";
        }
    }
}
