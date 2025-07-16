using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CookingState
{
    Idle,
    Cooking,
    Done,
    Succes
} 

public class PotAnimationController : MonoBehaviour
{
    #region animationName
    [SpineAnimation]
    public string idleAnimationName;
    [SpineAnimation]
    public string cookingAnimationName;
    [SpineAnimation]
    public string doneAnimationName;
    [SpineAnimation]
    public string successAnimationName;
    #endregion
    [SerializeField]
    private SkeletonGraphic skeletonGraphic;
    private Spine.AnimationState animationState;
    // Start is called before the first frame update
    void Awake()
    {
        skeletonGraphic = this.gameObject.GetComponent<SkeletonGraphic>();
        animationState = skeletonGraphic.AnimationState;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAnimation(CookingState state)
    {

        switch (state)
        {
            case CookingState.Idle:
                animationState.SetAnimation(0, idleAnimationName, true);
                break;
            case CookingState.Cooking:
                animationState.SetAnimation(0, cookingAnimationName, true);
                break;
            case CookingState.Done:
                animationState.SetAnimation(0, doneAnimationName, true);
                break;
            case CookingState.Succes:
                animationState.SetAnimation(0, successAnimationName, false);
                animationState.AddAnimation(0, idleAnimationName, true, 0);
                break;
        }

    }

}

