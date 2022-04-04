using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    //public static DragAndDrop instance;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public GameObject animObj;
    public int throwableAmount;

    Vector3 initialPos;
    Animator anim;

    [SerializeField] private Canvas canvas;

    void Awake()
    {
        //instance = this;

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        anim = animObj.transform.GetComponentInChildren<Animator>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;

        initialPos = transform.localPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    float animLength;
    public void OnEndDrag(PointerEventData eventData)
    {
        if (GameManagerScript.instance.isVideoTable)
        {
            Debug.Log("OnEndDrag_Video");
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            print("PointerName: " + eventData.pointerCurrentRaycast.gameObject.name);
            if (eventData.pointerCurrentRaycast.gameObject.CompareTag("PlayerVideoPanel") || eventData.pointerCurrentRaycast.gameObject.CompareTag("RawImagePanel"))
            {
                if (!eventData.pointerCurrentRaycast.gameObject.transform.GetComponentInParent<PokerPlayerController>().isLocalPlayer)
                {
                    SocialTournamentScript.instance.animationName = anim.gameObject.name;
                    animObj.transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
                    animObj.SetActive(true);

                    AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;

                    animLength = clips[0].length;

                    StartCoroutine(ResetThrowableBtn(animLength));
                    //animObj.transform.GetComponent<Animator>().enabled = true;

                    print("play animation....");

                    transform.localScale = Vector3.zero;

                    SocialTournamentScript.instance.FindLocalPlayer();
                    if (eventData.pointerCurrentRaycast.gameObject.CompareTag("PlayerVideoPanel"))
                    {
                        SocialTournamentScript.instance.throwableDestination = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.gameObject.name;
                    }
                    else if (eventData.pointerCurrentRaycast.gameObject.CompareTag("RawImagePanel"))
                    {
                        SocialTournamentScript.instance.throwableDestination = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject.name;
                    }

                    SocialTournamentScript.instance.throwableCharge = throwableAmount;

                    if (GameManagerScript.instance.isTournament)
                    {
                        TournamentManagerScript.instance.ThrowableEmitter();
                    }
                    else
                    {
                        PokerNetworkManager.instance.ThrowableEmitter();
                    }
                    SocialPokerGameManager.instance.endPosition = eventData.pointerCurrentRaycast.gameObject.transform.position;
                    SocialPokerGameManager.instance.selectedDrag = transform.gameObject;
                }

                else
                {
                    transform.localPosition = initialPos;
                }
            }

            else
            {
                transform.localPosition = initialPos;
            }
        }
        else
        {
            //............For non video.......//

            Debug.Log("OnEndDrag_NonVideo");
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            print(" non video PointerName: " + eventData.pointerCurrentRaycast.gameObject.name);

            if (eventData.pointerCurrentRaycast.gameObject.CompareTag("PlayerNonVideoPanel"))
            {
                if (!eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetChild(0).GetComponentInParent<PokerPlayerController>().isLocalPlayer)
                {
                    print("play animation....");

                    animObj.transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
                    animObj.SetActive(true);

                    AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;

                    animLength = clips[0].length;

                    transform.localScale = Vector3.zero;
                    StartCoroutine(ResetThrowableBtn(animLength));

                    SocialTournamentScript.instance.animationName = anim.gameObject.name;
                    SocialTournamentScript.instance.FindLocalPlayer();
                    SocialTournamentScript.instance.throwableDestination = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetChild(0).gameObject.name;
                    SocialTournamentScript.instance.throwableCharge = throwableAmount;

                    if (GameManagerScript.instance.isTournament)
                    {
                        TournamentManagerScript.instance.ThrowableEmitter();
                    }
                    else
                    {
                        PokerNetworkManager.instance.ThrowableEmitter();
                    }
                    SocialPokerGameManager.instance.endPosition = eventData.pointerCurrentRaycast.gameObject.transform.position;
                    SocialPokerGameManager.instance.selectedDrag = transform.gameObject;
                }

                else
                {
                    transform.localPosition = initialPos;
                }
            }

            else
            {
                transform.localPosition = initialPos;
            }
        }
    }

    IEnumerator ResetThrowableBtn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        print("ResetThrowableBtn.....");
        transform.localPosition = initialPos;
        transform.localScale = Vector3.one;
        animObj.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }
}