using UnityEngine;
using UnityEngine.EventSystems;

namespace Utility
{
    public class CustomButton : EventTrigger
    {
        public Vector2 fingerPos;

        public override void OnPointerDown(PointerEventData eventData)
        {
            fingerPos = eventData.position;
            base.OnPointerDown(eventData);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            fingerPos = eventData.position;
            base.OnDrag(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            fingerPos = new Vector2(-1, -1);
            base.OnPointerUp(eventData);
        }
    }
}