using UnityEngine;

public class SpriteRotate : MonoBehaviour
{
    public Animator anim;
    public Vector3 velocity;
    public void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
        velocity = GetComponentInParent<Npc_Paparazzi>().vel;
        anim.SetFloat("DirX", velocity.x);
        anim.SetFloat("DirY", velocity.y);
        if(velocity!=Vector3.zero)
        {
            anim.SetBool("isIdle", false);
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isIdle", true);
            anim.SetBool("isWalking", false);
        }
    }
}


