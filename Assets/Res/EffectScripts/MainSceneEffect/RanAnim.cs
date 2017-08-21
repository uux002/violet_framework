using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimType {
    Normal,
    Multi,
    Sun
}

[RequireComponent(typeof(Animator))]
public class RanAnim : MonoBehaviour {

    public AnimType animType = AnimType.Normal;

    [Tooltip("含有Animator的对象")]
    private Animator anim;

    [Tooltip("每次随机时间间隔最小值:单位秒")]
    public float ranTimeMin;
    [Tooltip("每次随机时间间隔最大值:单位秒")]
    public float ranTimeMax;
    
    [Tooltip("被触发概率最小值：范围1-100")]
    public int ranRateMin;
    [Tooltip("被触发概率最大值：范围1-100")]
    public int ranRateMax;
    [Tooltip("动画的参数个数")]
    public int paramCount = 1;

    public int sunAnimTime = 120;

    private const string IDLE = "idle";
    private const string ANIM_PRE_FIX = "anim";
    private const string SUN_PARAM = "hei";

    private void Start() {
        anim = gameObject.GetComponent<Animator>();
        paramCount += 1;
        switch(animType) {
            case AnimType.Normal:
                NormalHandler();
                break;
            case AnimType.Multi:
                MultiHandler();
                break;
            case AnimType.Sun:
                SunHandler();
                break;
        }
    }

    private void NormalHandler() {
        float ranTime = Random.Range(ranTimeMin, ranTimeMax);
        InvokeRepeating("NormalRanHandler", 0, ranTime);
    }
    private void NormalRanHandler() {
        int ranRate = Random.Range(ranRateMin, ranRateMax);
        int ran = Random.Range(0, 100);
        if(ran < ranRate) {
            anim.SetTrigger(IDLE);
        }
    }
    
    private void MultiHandler() {
        float ranTime = Random.Range(ranTimeMin, ranTimeMax);
        InvokeRepeating("MultiRanHandler", 0, ranTime);
    }
    private void MultiRanHandler() {
        int ranRate = Random.Range(ranRateMin, ranRateMax);
        int ranCount = Random.Range(1, paramCount);
        List<int> ranIndexList = new List<int>();
        for(int i = 0; i < ranCount; i++) {
            int ran = Random.Range(0, 100);
            if(ran < ranRate) {
                int ranIndex = Random.Range(1, paramCount);
                if(!ranIndexList.Contains(ranIndex)) {
                    ranIndexList.Add(ranIndex);
                }
            }
        }
        foreach(int item in ranIndexList) {
            string animParam;
            if(item < 10) {
                animParam = ANIM_PRE_FIX + "0" + item;
            } else {
                animParam = ANIM_PRE_FIX + item;
            }

            anim.SetTrigger(animParam);
        }
    }
    private void SunHandler() {
        InvokeRepeating("SunRepeating", sunAnimTime, sunAnimTime);
    }
    private void SunRepeating() {
        bool sunParam = anim.GetBool(SUN_PARAM);
        anim.SetBool(SUN_PARAM, !sunParam);
    }

    private void OnDestroy() {
        
    }

}
