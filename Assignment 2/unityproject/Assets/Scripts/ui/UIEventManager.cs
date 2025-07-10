using DG.Tweening;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* This is a script that handles the button animations using Tweening
    Drag this script to the canvas of your UI 
    and add your buttons to the Selectables in the Editor */
/* It does not work properly, so this can be ignored.*/
public class UIEventManager : MonoBehaviour
{
    [Header("References")]
    public List<Selectable> Selectables = new List<Selectable>();

    [Header("Animations")]
    [SerializeField] protected float _selectedAnimationScale = 1.1f;
    [SerializeField] protected float _scaleDuration = 0.25f;

    protected Dictionary<Selectable, Vector3> _scales = new Dictionary<Selectable, Vector3>();

    protected Tween _scaleUpTween;
    protected Tween _scaleDownTween;

    protected virtual void Awake()
    {
        foreach (Selectable p in Selectables)
        {
            AddSelectionListeners(p);
            _scales.Add(p, p.transform.localScale);
        }
    }

    public virtual void OnEnable()
    {
        for (int i = 0; i < Selectables.Count; i++)
        {
            Selectables[i].transform.localScale = _scales[Selectables[i]];
        }
    }


    public void StopAllTweensAndReset()
    {
        for (int i = 0; i < Selectables.Count; i++)
        {
            DOTween.Kill(Selectables[i]);
            Selectables[i].transform.localScale = _scales[Selectables[i]];
        }

        _scaleUpTween?.Kill();
        _scaleDownTween?.Kill();
    }
    
    protected virtual void AddSelectionListeners(Selectable selection)
    {   
        // Adding a listener for events
        EventTrigger trigger = selection.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = selection.gameObject.AddComponent<EventTrigger>();
        }

        // Here we are adding our events
        EventTrigger.Entry SelectEntry = new EventTrigger.Entry { eventID = EventTriggerType.Select };
        SelectEntry.callback.AddListener(OnSelect);
        trigger.triggers.Add(SelectEntry);

        EventTrigger.Entry DeselectEntry = new EventTrigger.Entry { eventID= EventTriggerType.Deselect };
        DeselectEntry.callback.AddListener(OnDeselect);
        trigger.triggers.Add(DeselectEntry);

        EventTrigger.Entry OnPointerEnterEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        OnPointerEnterEntry.callback.AddListener(OnPointerEnter);
        trigger.triggers.Add(OnPointerEnterEntry);

        EventTrigger.Entry OnPointerExitEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        OnPointerExitEntry.callback.AddListener(OnPointerExit);
        trigger.triggers.Add(OnPointerExitEntry);

    }

    public void OnSelect(BaseEventData eventData)
    {
        if (_scaleUpTween != null && _scaleUpTween.IsActive())
        {
            _scaleUpTween.Kill();
        }

        Vector3 newScale = eventData.selectedObject.transform.localScale * _selectedAnimationScale;
        _scaleUpTween = eventData.selectedObject.transform.DOScale(newScale, _scaleDuration);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Selectable sel = eventData.selectedObject.GetComponent<Selectable>();

        if (_scaleDownTween != null && _scaleDownTween.IsActive())
        {
            _scaleDownTween.Kill();
        }

        _scaleDownTween = eventData.selectedObject.transform.DOScale(_scales[sel], _scaleDuration);
    }

    public void OnPointerEnter(BaseEventData eventData) {
        PointerEventData pointerEventData = eventData as PointerEventData;
        if (pointerEventData != null)
        {
            Selectable sel = pointerEventData.pointerEnter.GetComponentInParent<Selectable>();

            if (sel == null)
            {
                sel = pointerEventData.pointerEnter.GetComponentInChildren<Selectable>();
            }

            pointerEventData.selectedObject = sel.gameObject;
        }

    }

    public void OnPointerExit(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        if (pointerEventData != null)
        {
            pointerEventData.selectedObject = null;
        }
    }
}
