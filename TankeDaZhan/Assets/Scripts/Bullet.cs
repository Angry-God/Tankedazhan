using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    //爆炸音效
    public AudioClip explodeClip;
    //炮弹飞行速度
    public float speed = 100f;
    //爆炸效果
    public GameObject explode;
    //攻击方
    public GameObject attactTank;
    //最大生存时间
    public float MaxLiftTime = 2f;
    //炮弹发射时间
    public float instantiateTime = 0f;
	// Use this for initialization
	void Start () {
        instantiateTime = Time.time;
	
	}
	
	// Update is called once per frame
	void Update () {
        //前进
        transform.position += transform.forward * speed * Time.deltaTime;
        //摧毁
        if (Time.time-instantiateTime>MaxLiftTime)
        {
            Destroy(gameObject);
        }
	}
    //碰撞
    private void OnCollisionEnter(Collision collision)
    {
        //打倒自身
        if (collision.gameObject == attactTank)
            return;
        //爆炸效果
        GameObject explodeObj = (GameObject)Instantiate(explode, transform.position, transform.rotation);
        //爆炸音效
        AudioSource audioSource = explodeObj.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1;
        audioSource.PlayOneShot(explodeClip);
        //摧毁自身
        Destroy(gameObject);
        //击中坦克
        Tank tanck = collision.gameObject.GetComponent<Tank>();
        if (null!=tanck)
        {
            float att = GetAtt();
            tanck.BeAttacked(att,attactTank);
        }
    }
    private float GetAtt()
    {
        //计算攻击力
        float att = 100 - (Time.time - instantiateTime) * 40;
        if (att < 1)
            att = 1;
        return att;
    }
}
