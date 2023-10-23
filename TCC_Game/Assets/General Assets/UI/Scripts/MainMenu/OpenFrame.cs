using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenFrame : MonoBehaviour
{
   [SerializeField] AnimationCurve curve;
   [SerializeField]float duration;
   float startDuration = 0;
   float maxWidth;
   RectTransform imageRect;
   Image image;
   
   private void Start(){

      image = gameObject.GetComponent<Image>();
      imageRect = gameObject.GetComponent<RectTransform>();
      maxWidth = imageRect.sizeDelta.x;

      imageRect.sizeDelta = new Vector2(0, imageRect.sizeDelta.y);
   }

   public void ChangeWidth(bool open){
      //float w = open ? w = maxWidth : w = 0;
      StopAllCoroutines();
      StartCoroutine(OpenAndClose(open));
   }

   IEnumerator OpenAndClose(bool open){
      float w = maxWidth;
      float timer = 0;

      while(timer < duration){
         float t = timer / duration;
         
         /*AnimationCurve curve = w == 0 ? curve = invertedCurve : curve = this.curve;*/

         float curveTime = curve.Evaluate(t);

         float auxWidth = open ? w * curveTime : maxWidth - w * curveTime;
         imageRect.sizeDelta = new Vector2(auxWidth, imageRect.sizeDelta.y);
         
         timer += Time.deltaTime;

         yield return null;
      }
   }
}