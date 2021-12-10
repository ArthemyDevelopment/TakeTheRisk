using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenAnimationsUI : MonoBehaviour
{
    [Header("Tween Settings")]
    [Tooltip("Setear si la animación empezara desde el comienzo de la escena o no, en caso de no requiere un boton o similar")]
    public bool B_playOnEnable;

    public bool B_PlayStart;

    [Tooltip("Setear que tipo de animación se quiere, para más de una animación se requieren varios scripts con sus propias configuraciones")]
    public WhichAnimation whichAnimation;

    public bool B_DiferentOutTime;

    [Tooltip("Tiempo en Float que dura la animación")]
    public float F_Duration;

    public float F_DifOutDuration;

    [Tooltip("Tipo de movimiento que tendra la animación")]
    public LeanTweenType easeType;

    [Tooltip("Setear si el moviemiento sera loop Ping Pong o no")]
    public bool B_isPingPong;

    [Tooltip("En caso de Loop Ping Pong setear cuantas veces se loopeara, -1 es infinito")]
    public int I_PingPongLoops;

    [Space(5)]
    [Header("Objects")]

    [Tooltip("El objeto que se animara")]
    public GameObject G_ObjectToMove;

    [Tooltip("Transform de referencia de como empezara el objeto")]
    public Transform Tr_From;

    [Tooltip("Transform de referencia de como terminara el objeto")]
    public Transform Tr_To;

    CanvasGroup Cv_ObjectCanvasGroup;
    LTDescr Tween;

    public bool B_pressed;

    public enum WhichAnimation
	{
        Move,
        Scale,
        FadeIn,
        FadeOut,
        Rotation

	}

	private void Start()
	{
        if (B_PlayStart)
        {
            PlayAnimation();

        }
    }
	void OnEnable()
    {
        if(B_playOnEnable)
		{
            PlayAnimation();

		}

    }

    public void ChangePresedStatus(bool b)
	{
        B_pressed = b;
	}

    public void PlayAnimation()
	{
        if(!B_pressed)
		{
            switch(whichAnimation)
		    {
                case WhichAnimation.Move:
                    Tween = LeanTween.move(G_ObjectToMove, Tr_To.position, F_Duration);
                    break;

                case WhichAnimation.Scale:
                    Tween = LeanTween.scale(G_ObjectToMove, Tr_To.localScale, F_Duration);
                    break;

                case WhichAnimation.Rotation:
                    Tween = LeanTween.rotate(G_ObjectToMove, Tr_To.localRotation.eulerAngles, F_Duration);
                    break;

                case WhichAnimation.FadeIn:
                    Tween = LeanTween.alphaCanvas(Cv_ObjectCanvasGroup, 1, F_Duration);
                    break;

                case WhichAnimation.FadeOut:
                    Tween = LeanTween.alphaCanvas(Cv_ObjectCanvasGroup, 0, F_Duration);
                    break;
            }
            Debug.Log("GoAnim" + gameObject.name);
            B_pressed = true;

		}
		else
		{
            switch (whichAnimation)
            {
                case WhichAnimation.Move:
                    if(B_DiferentOutTime)
					{
                        Tween = LeanTween.move(G_ObjectToMove, Tr_From.position, F_DifOutDuration);
                    }
                    else
					{
                        Tween = LeanTween.move(G_ObjectToMove, Tr_From.position, F_Duration);
					}
                    break;

                case WhichAnimation.Scale:
                    if (B_DiferentOutTime)
                    {
                        Tween = LeanTween.scale(G_ObjectToMove, Tr_From.localScale, F_DifOutDuration);
                    }
                    else
                    {
                        Tween = LeanTween.scale(G_ObjectToMove, Tr_From.localScale, F_Duration);    
                    }
                    break;

                case WhichAnimation.Rotation:
                    if (B_DiferentOutTime)
                    {
                        Tween = LeanTween.rotate(G_ObjectToMove, Tr_From.localRotation.eulerAngles, F_DifOutDuration);
                    }
                    else
                    {
                        Tween = LeanTween.rotate(G_ObjectToMove, Tr_From.localRotation.eulerAngles, F_Duration);
                    }
                    break;

                case WhichAnimation.FadeIn:
                    if (B_DiferentOutTime)
                    {
                        Tween = LeanTween.alphaCanvas(Cv_ObjectCanvasGroup, 0, F_DifOutDuration);
                    }
                    else
                    {
                        Tween = LeanTween.alphaCanvas(Cv_ObjectCanvasGroup, 0, F_Duration);
                    }
                    break;

                case WhichAnimation.FadeOut:
                    if (B_DiferentOutTime)
                    {
                        Tween = LeanTween.alphaCanvas(Cv_ObjectCanvasGroup, 1, F_DifOutDuration);
                    }
                    else
                    {
                        Tween = LeanTween.alphaCanvas(Cv_ObjectCanvasGroup, 1, F_Duration);
                    }
                    break;
            }
            B_pressed = false;
            Debug.Log("BackANim" + gameObject.name);
        }
        Tween.setEase(easeType);
        if(B_isPingPong)
		{
            Tween.setLoopPingPong(I_PingPongLoops);
		}
	}
}
