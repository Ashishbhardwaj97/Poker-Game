using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRefreshControl : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Serializable] public class RefreshControlEvent : UnityEvent { }

    [SerializeField] private ScrollRect m_ScrollRect;
    [SerializeField] private float m_PullDistanceRequiredRefresh = 120f;
    
    [SerializeField] RefreshControlEvent m_OnRefresh = new RefreshControlEvent();


    private float m_InitialPosition;
    private float m_Progress;
    private bool m_IsPulled;
    private bool m_IsRefreshing;
    private Vector2 m_PositionStop;
    private IScrollable m_ScrollView;

    /// <summary>
    /// Progress until refreshing begins. (0-1)
    /// </summary>
    public float Progress
    {
        get { return m_Progress; }
    }

    /// <summary>
    /// Refreshing?
    /// </summary>
    public bool IsRefreshing
    {
        get { return m_IsRefreshing; }
    }

    /// <summary>
    /// Callback executed when refresh started.
    /// </summary>
    public RefreshControlEvent OnRefresh
    {
        get { return m_OnRefresh; }
        set { m_OnRefresh = value; }
    }

  

    /// <summary>
    /// Call When Refresh is End.
    /// </summary>
    public void EndRefreshing()
    {
        m_IsPulled = false;
        m_IsRefreshing = false;
       
    }

    private void Start()
    {
        m_InitialPosition = GetContentAnchoredPosition();
        m_PositionStop = new Vector2(m_ScrollRect.content.anchoredPosition.x, m_InitialPosition - m_PullDistanceRequiredRefresh);
        m_ScrollView = gameObject.GetComponent<IScrollable>();
        Debug.Log(m_ScrollView == null);
        m_ScrollRect.onValueChanged.AddListener(OnScroll);
    }

    private void LateUpdate()
    {
        if (!m_IsPulled)
        {
            return;
        }

        if (!m_IsRefreshing)
        {
            return;
        }

        m_ScrollRect.content.anchoredPosition = m_PositionStop;
    }

    private void OnScroll(Vector2 normalizedPosition)
    {
        var distance = m_InitialPosition - GetContentAnchoredPosition();

        if (distance < 0f)
        {
            return;
        }

        OnPull(distance);
    }

    private void OnPull(float distance)
    {

        if (m_IsRefreshing && Math.Abs(distance) < 1f)
        {
            m_IsRefreshing = false;
        }

        if (m_IsPulled && Dragging)
        {
            return;
        }

        m_Progress = distance / m_PullDistanceRequiredRefresh;

        if (m_Progress < 1f)
        {
            return;
        }
     
        if (Dragging)
        {
            m_IsPulled = true;
           
        }

        if (m_IsPulled && !Dragging)
        {
            m_IsRefreshing = true;
            m_OnRefresh.Invoke();
        }

        m_Progress = 0f;
    }

    private float GetContentAnchoredPosition()
    {
        return m_ScrollRect.content.anchoredPosition.y;
    }

    public void OnDrag(PointerEventData eventData)
    {
        ((IDragHandler)m_ScrollRect).OnDrag(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ((IBeginDragHandler)m_ScrollRect).OnBeginDrag(eventData);
        _Dragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ((IEndDragHandler)m_ScrollRect).OnEndDrag(eventData);
        _Dragging = false;
    }




    private bool _Dragging;
    public bool Dragging
    {
        get { return _Dragging; }
    }


}
