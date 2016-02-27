using UnityEngine;
using Fungus;


namespace FungusExtensions
{
    [EventHandlerInfo("Sprite",
        "Dynamic Drag Started",
        "This block executes when the player starts dragging. Does not require target objects")]
    [AddComponentMenu("")]
    public class DynamicDragStarted : EventHandler
    {
        public virtual void DynamicOnDragStarted()
        {
            ExecuteBlock();
        }

        public override string GetSummary()
        {
            return "None";
        }
    }
}
