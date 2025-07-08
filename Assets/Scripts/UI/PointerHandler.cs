using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerHandler : MonoBehaviour
    , IPointerEnterHandler  // 모바일에서는 OnPointerEnter가 호출되지 않음
    , IPointerExitHandler   // 모바일에서는 OnPointerExit가 호출되지 않음
    , IPointerUpHandler
    , IPointerDownHandler
    , IPointerClickHandler
    , IPointerMoveHandler
    , IDragHandler
    , IBeginDragHandler
    , IEndDragHandler
{
    public event Action<PointerEventData> Enter;
    public event Action<PointerEventData> Exit;
    public event Action<PointerEventData> Up;
    public event Action<PointerEventData> Down;
    public event Action<PointerEventData> Click;
    public event Action<PointerEventData> Move;
    public event Action<PointerEventData> Drag;
    public event Action<PointerEventData> BeginDrag;
    public event Action<PointerEventData> EndDrag;

    public void OnPointerEnter(PointerEventData eventData)  => Enter?.Invoke(eventData);
    public void OnPointerExit(PointerEventData eventData)   => Exit?.Invoke(eventData);
    public void OnPointerUp(PointerEventData eventData)     => Up?.Invoke(eventData);
    public void OnPointerDown(PointerEventData eventData)   => Down?.Invoke(eventData);
    public void OnPointerClick(PointerEventData eventData)  => Click?.Invoke(eventData);
    public void OnPointerMove(PointerEventData eventData)   => Move?.Invoke(eventData);
    public void OnDrag(PointerEventData eventData)          => Drag?.Invoke(eventData);
    public void OnBeginDrag(PointerEventData eventData)     => BeginDrag?.Invoke(eventData);
    public void OnEndDrag(PointerEventData eventData)       => EndDrag?.Invoke(eventData);
}
