using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
	//控制的坦克
	public Tank tank;
	//状态枚举
	public enum Status
	{
		Patrol,
		Attack,
	}
	private Status status = Status.Patrol;//默认状态是巡逻
	private GameObject target=null;//搜寻的坦克
        //视野范围
	private float sightDistance = 30;
	//上一次搜寻的时间
	private float lastSearchTargetTime = 0;
	//搜寻间隔
	private float searchTargetInterval = 3;

	//更改状态
	public void ChangeStatus(Status status)
	{
		if (status == Status.Patrol)
			PatrolStart();
		else if (status == Status.Attack)
			AttackStart();
	}

	// Update is called once per frame
	void Update()
	{
		TargetUpdate();
		if (tank.ctrlType != Tank.CtrlType.computer) return;
		if (status == Status.Patrol) PatrolUpdate();
		if (status == Status.Attack) AttackUpdate(); 

	}
	//人工智能的各个状态以及其处理
	void PatrolStart() { }
	void AttackStart() { }
	void PatrolUpdate() { }
	void AttackUpdate() { }
	//搜寻目标
	void TargetUpdate()
	{
		Debug.Log("进入搜索阶段");
		//cd时间
		float interval = Time.time- lastSearchTargetTime;
		if (interval < searchTargetInterval) return;
		lastSearchTargetTime = Time.time;
		//已有目标的情况，判断目标是否丢失
		if (target != null)
			HasTarget();
		else
			NoTarget();
	}
	void HasTarget()
	{
		Debug.Log("已有坦克，进入判断玩家阶段");
		//搜索逻辑，已有目标的情况
		Tank targetTank = target.GetComponent<Tank>();
		Vector3 pos = transform.position;
		Vector3 targetPos = target.transform.position;
		if (targetTank.ctrlType == Tank.CtrlType.none)
		{
			Debug.Log("目标死亡，丢失目标");
			target = null;
		}
		else if (Vector3.Distance(pos,targetPos) > sightDistance)
		{
			Debug.Log("距离太远，丢失目标");
			target = null;
		}
	}
	void NoTarget()
	{
		//没有目标的情况，搜索视野内的坦克
		//最小生命值
		float minHp = float.MaxValue;
		GameObject[] targets = GameObject.FindGameObjectsWithTag("Tank");
		for (int i=0;i<targets.Length;i++)
		{
			Tank tank = targets[i].GetComponent<Tank>();
			if (tank ==null) continue;
			if (targets[i] = gameObject) continue;
			//死亡
			if (tank.ctrlType == Tank.CtrlType.none) continue;
			//判断距离
			Vector3 pos = transform.position;
			Vector3 targetPos = targets[i].transform.position;
			if (Vector3.Distance(pos, targetPos) > sightDistance)
				continue;
			Debug.Log("坦克之间的距离为："+Vector3.Distance(pos, targetPos));
			//判断生命值？？
			if (minHp>tank.hp)
			{
				target = tank.gameObject;
				minHp = tank.hp;
			}
		}
		if (target != null)
			Debug.Log("获取目标" + target.name);
	}
	public void OnAttacted(GameObject attactTank)
	{
		target = attactTank;
	}
}
