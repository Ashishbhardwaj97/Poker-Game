using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ChairAnimation : MonoBehaviour
{
    public float time;
    public AnimationCurve animationCurve;
    public static ChairAnimation instance;
    public bool isChairAnimComplete;

    public GameObject[] chairs;
    public Vector3[] chairPosition; 

    private void Awake()
    {
        instance = this;
        isChairAnimComplete = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        SaveInitialChairPosition();
       // PositionAnimation(animateObject, destinationPosition);
    }

    public void SaveInitialChairPosition()
    {
        for (int i = 0; i < chairs.Length; i++)
        {
            //chairPosition[i] = new Vector3();
            chairPosition[i] = chairs[i].transform.localPosition;
        }
    }

    public void ResetChairPosition(bool activateChairs) 
    {
        for (int i = 0; i < chairs.Length; i++)
        {
            chairs[i].transform.localPosition = chairPosition[i];
        }
        if(activateChairs)
        {
            for (int i = 0; i < chairs.Length; i++)
            {
                chairs[i].SetActive(true);
            }
        }
    }

    public void PositionAnimation(Transform animateObject, Vector3 destinationPosition)
    {
        if (destinationPosition.y > 0)
        {
            destinationPosition = new Vector3(0, -10, 0);
        }
        else
        {
            destinationPosition = new Vector3(0, 10, 0);
        }
        StartCoroutine(PositionAnimationCoroutine(animateObject, destinationPosition));
    }

    public IEnumerator PositionAnimationCoroutine(Transform animateObject, Vector3 destinationPosition)
    {
        // yield return new WaitForSeconds(1f);
        float evaluateValue = 0;
        float animationTime;
        while (evaluateValue < 1)
        {
            evaluateValue += 1 / time * Time.deltaTime;
            if (evaluateValue > 1)
            {
                evaluateValue = 1;
            }

            animationTime = animationCurve.Evaluate(evaluateValue);
            animateObject.localPosition = Vector3.Lerp(animateObject.localPosition, destinationPosition, animationTime);
            yield return new WaitForSeconds(0.001f);
            //yield return null;
        }
        animateObject.gameObject.SetActive(false);
    }

    //public void ScalingAnimation(Transform animateObject, Vector3 TargetScale)
    //{
    //    StartCoroutine(ScalingAnimationCoroutine(animateObject, TargetScale));
    //}
    //public IEnumerator ScalingAnimationCoroutine(Transform animateObject, Vector3 TargetScale) 
    //{

    //    yield return new WaitForSeconds(0.5f);
    //    float evaluateValue = 0;
    //    float animationTime;
    //    while (evaluateValue < 1)
    //    {
    //        evaluateValue += 1 / time * Time.deltaTime;
    //        if (evaluateValue > 1)
    //        {
    //            evaluateValue = 1;
    //        }

    //        animationTime = animationCurve.Evaluate(evaluateValue);
    //        animateObject.localScale = Vector3.Lerp(animateObject.localScale, TargetScale, animationTime);
    //        yield return new WaitForSeconds(0.001f);
    //        //animateObject121.gameObject.SetActive(false);
    //        //yield return null;
    //    }
    //}

    //[Space]
    //[Space]
    //public float lerptime;
    //public float timer;
    //public IEnumerator PositionAnimationCoroutine2(Transform animateObject, Vector3 destinationPosition) 
    //{
    //    yield return new WaitForSeconds(1f);
    //    while (true)
    //    {
    //        timer += Time.deltaTime;
    //        if (timer > lerptime)
    //        {
    //            timer = lerptime;
    //        }
    //        float lerpratio = timer / lerptime;
    //        animateObject.localPosition = Vector3.Lerp(animateObject.localPosition, destinationPosition, lerpratio);

    //        yield return new WaitForSeconds(0.001f);
    //        if (animateObject.transform.localPosition == destinationPosition)
    //        {
    //            print("breaksssssssssssssssssssssssssssssss");
    //            break;
    //        }
    //    }
    //    timer = 0;
    //}


    public void StartAnimatePlayerChairCorutine(Transform chairTrans, Transform playerPrefab, string playerNumber)
    {
        isChairAnimComplete = false;
        print("int.Parse(playerNumber) ......." + int.Parse(playerNumber));
        //StartCoroutine(AnimatePlayerChair(chairTrans, playerPrefab));
        if (int.Parse(playerNumber) == 1)
        {
            StartCoroutine(AnimatePlayerChair(chairs[0].transform, playerPrefab, chairs[0].transform.localPosition, new Vector3(29, -36f, 0)));
        }
        if (int.Parse(playerNumber) == 2)
        {
            StartCoroutine(AnimatePlayerChair(chairs[1].transform, playerPrefab, chairs[1].transform.localPosition, new Vector3(-29, -36f, 0)));
        }
        if (int.Parse(playerNumber) == 3)
        {
            StartCoroutine(AnimatePlayerChair(chairs[2].transform, playerPrefab, chairs[2].transform.localPosition, new Vector3(-25f, 11f, 0)));
        }
        if (int.Parse(playerNumber) == 4)
        {
            StartCoroutine(AnimatePlayerChair(chairs[3].transform, playerPrefab, chairs[3].transform.localPosition, new Vector3(25f, 35f, 0)));
        }
        if (int.Parse(playerNumber) == 5)
        {
            StartCoroutine(AnimatePlayerChair(chairs[4].transform, playerPrefab, chairs[4].transform.localPosition, new Vector3(-25f, 35f, 0)));
        }
        if (int.Parse(playerNumber) == 6)
        {
            StartCoroutine(AnimatePlayerChair(chairs[5].transform, playerPrefab, chairs[5].transform.localPosition, new Vector3(30f, 11f, 0)));
        }
    }

    IEnumerator AnimatePlayerChair(Transform chairTrans, Transform playerPrefab, Vector3 start , Vector3 destination)
    {
        yield return new WaitForSeconds(0.1f);
        AnimateTransformFunctions.ins.AnimateTransform(chairTrans, start, destination, 0.5f, AnimateTransformFunctions.TransformTypes.Position, AnimateTransformFunctions.AnimAxis.Local, "EaseOut");
        //if (int.Parse(chairTrans.parent.parent.gameObject.name) > 4)
        //{

        //    AnimateTransformFunctions.ins.AnimateTransform(chairTrans, new Vector3(0, -100f, 0), new Vector3(0, 10f, 0), 1f, AnimateTransformFunctions.TransformTypes.Position, AnimateTransformFunctions.AnimAxis.Local, "EaseOut");
        //}
        //else
        //{

        //    AnimateTransformFunctions.ins.AnimateTransform(chairTrans, new Vector3(0, 100f, 0), new Vector3(0, -10f, 0), 1f, AnimateTransformFunctions.TransformTypes.Position, AnimateTransformFunctions.AnimAxis.Local, "EaseOut");
        //}

        yield return new WaitForSeconds(1f);
        AnimateTransformFunctions.ins.AnimateTransform(playerPrefab, new Vector3(0, 0, 0), new Vector3(1, 1, 1), 1f, AnimateTransformFunctions.TransformTypes.Scale, AnimateTransformFunctions.AnimAxis.Local, "EaseOut");
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(ScalingAnimationCoroutine2(chairTrans, new Vector3(0,0,0)));
        //chairTrans.transform.gameObject.SetActive(false);
    }

    public IEnumerator ScalingAnimationCoroutine2(Transform animateObject, Vector3 TargetScale)
    {

        //yield return new WaitForSeconds(0.5f);
        float evaluateValue = 0;
        float animationTime;
        while (evaluateValue < 1)
        {
            evaluateValue += 1 / time * Time.deltaTime;
            if (evaluateValue > 1)
            {
                evaluateValue = 1;
            }

            animationTime = animationCurve.Evaluate(evaluateValue);
            animateObject.localScale = Vector3.Lerp(animateObject.localScale, TargetScale, animationTime);
        
            //yield return new WaitForSeconds(0.001f);
            yield return null;
        }
        animateObject.gameObject.SetActive(false);
        animateObject.localScale = Vector3.one;
        ResetChairPosition(true);
        yield return new WaitForSeconds(0.1f);
        isChairAnimComplete = true;
    }
}
