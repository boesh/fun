using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    RuntimeAnimatorController rn;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = rn;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
