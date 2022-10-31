using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundController : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region Public Method
    public void OnPointerEnter(PointerEventData eventData)
    {
        CommonSoundManager.Instance.PlayButtonOverSound();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CommonSoundManager.Instance.PlayButtonPushedSound();
    }
    #endregion Public Method
}
