using UnityEngine.UI;
using UnityEngine.EventSystems;

public interface IScrollable
{
    bool Dragging { get; }
}

public class MyScrollView : ScrollRect, IScrollable
{
    private bool _Dragging;
    public bool Dragging
    {
        get { return _Dragging; }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        _Dragging = true;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        _Dragging = false;
    }
}