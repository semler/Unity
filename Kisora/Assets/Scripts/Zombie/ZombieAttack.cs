using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NendUnityPlugin.AD.Video;

public class ZombieAttack : MonoBehaviour
{
    static int unityChanDamageState = Animator.StringToHash("Base Layer.damage");
    static public int hp = 10;

    private Animator unityChanAnimator;
    private UnityChanAction unityChanAction;

    // nend
    #if UNITY_IOS
    private string spotId = "802555", apiKey = "ca80ed7018734d16787dbda24c9edd26c84c15b8";
    #else
    private string spotId = "802559", apiKey = "e9527a2ac8d1f39a667dfe0f7c169513b090ad44";
    #endif
    private NendAdRewardedVideo m_RewardedVideoAd;

    void Start()
    {
        m_RewardedVideoAd = NendAdRewardedVideo.NewVideoAd(spotId, apiKey);

        m_RewardedVideoAd.AdLoaded += (instance) => {
            // 広告ロード成功のコールバック
            m_RewardedVideoAd.Show();
        };
        m_RewardedVideoAd.AdFailedToLoad += (instance, errorCode) => {
            // 広告ロード失敗のコールバック
        };
        m_RewardedVideoAd.AdFailedToPlay += (instance) => {
            // 再生失敗のコールバック
        };
        m_RewardedVideoAd.AdShown += (instance) => {
            // 広告表示のコールバック
        };
        m_RewardedVideoAd.AdStarted += (instance) => {
            // 再生開始のコールバック
        };
        m_RewardedVideoAd.AdStopped += (instance) => {
            // 再生中断のコールバック
        };
        m_RewardedVideoAd.AdCompleted += (instance) => {
            // 再生完了のコールバック
        };
        m_RewardedVideoAd.AdClicked += (instance) => {
            // 広告クリックのコールバック
        };
        m_RewardedVideoAd.InformationClicked += (instance) => {
            // オプトアウトクリックのコールバック
        };
        m_RewardedVideoAd.AdClosed += (instance) => {
            // 広告クローズのコールバック
            unityChanAnimator.SetBool("isDead", false);

            StartCoroutine(Delay(3.0f));
        };
        m_RewardedVideoAd.Rewarded += (instance, rewardedItem) => {
            // リワード報酬のコールバック
        };
    }

    //オブジェクトと接触した瞬間に呼び出される
    void OnTriggerEnter(Collider other)
    {
        if (other.tag.CompareTo("UnityChan") == 0)
        {
            GameObject unityChan = other.gameObject;
            if (unityChanAnimator == null)
            {
                unityChanAnimator = unityChan.GetComponent<Animator>();
                unityChanAction = unityChan.GetComponent<UnityChanAction>();
            }

            AnimatorStateInfo unityChanStateInfo = unityChanAnimator.GetCurrentAnimatorStateInfo(0);
            if (unityChanStateInfo.fullPathHash != unityChanDamageState && unityChanAction.isDamage == false) {
                hp--;
                HealthBar.health = hp;
                if (hp == 0)
                {
                    unityChanAnimator.SetBool("isDead", true);
                    unityChanAction.isDead = true;
                    m_RewardedVideoAd.Load();
                }
                else
                {
                    unityChanAnimator.SetBool("isDamage", true);
                    unityChanAction.isDamage = true;
                }

            }
        }
    }

    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        unityChanAnimator.SetBool("isDead", false);
        unityChanAction.isDead = false;
        hp = 10;
        HealthBar.health = hp;
    }




}