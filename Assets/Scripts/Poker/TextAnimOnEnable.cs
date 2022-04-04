using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAnimOnEnable : MonoBehaviour
{
    public GameObject TextToAnim;
    public Transform start;
    public Transform end;
    public float time;
    public int delayToActivate;

    //Transform initialPos;
    void OnEnable()
    {
        RunOnEnable();
    }

    void RunOnEnable()
    {
        StartCoroutine(Delay());

    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delayToActivate);
        TextToAnim.SetActive(true);
        AnimateTransformFunctions.ins.AnimateTransform(TextToAnim.transform, start.position, end.position, time, AnimateTransformFunctions.TransformTypes.Position, AnimateTransformFunctions.AnimAxis.World, "EaseOut");
        yield return new WaitForSeconds(time);
        print("FAlse.......text...");
        TextToAnim.SetActive(false);
        TextToAnim.transform.position = start.position;
    }
}
