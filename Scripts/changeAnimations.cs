using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAnim
{
    public AnimationClip idle;
    public AnimationClip left;
    public AnimationClip stand1;
    public AnimationClip right;
    public AnimationClip stand2;
}

public class changeAnimations : MonoBehaviour
{
    public PlayerAnim playerAnim;
    public Animation anim;
    float startTime;
    int mState;

    // Start is called before the first frame update
    void Start()
    {

        anim = GetComponent<Animation>();

        anim.clip = playerAnim.idle;
        anim.Play();
    }

    // Update is called once per frame
    void Update()
    {
        int temp = GameObject.Find("user").GetComponent<userXML>().state;
        Debug.Log(anim.clip.name);
        if (mState != temp)
        {
         
            mState = temp;
            Debug.Log(mState);
            switch (mState)
            {
                case 1:
                    anim.clip = playerAnim.right;
                    anim.Play();
                    break;
                case 2:
                    anim.clip = playerAnim.stand1;
                    anim.Play();
                    break;
                case 3:
                    anim.clip = playerAnim.left;
                    anim.Play();
                    break;
                case 4:
                    anim.clip = playerAnim.stand2;
                    anim.Play();
                    break;
                default:
                    anim.clip = playerAnim.idle;
                    anim.Play();
                    break;
            }
        }
    }
    
}
