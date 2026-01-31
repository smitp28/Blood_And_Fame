using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using System.Runtime.CompilerServices;




public class CameraShakeManager : MonoBehaviour
{
    public float globalShakeForce = 1f;
   public static CameraShakeManager instance;   

   public void Awake()
   {
       if (instance == null)
       {
           instance = this;
       }
   }
   public void CameraShake(CinemachineImpulseSource impulseSource)
   {
       impulseSource.GenerateImpulseWithForce(globalShakeForce);
   }
}
