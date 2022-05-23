using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationUpdater : MonoBehaviour
{
    [SerializeField] Animator anim;
    public void UpdateAnimations(bool value)
    {
        anim.SetBool("Walking", value);
    }
}
