using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    //相机距离
    public float distance = 8f;
    //距离范围
    public float maxDistance = 22f;
    public float minDistance = 5f;
    //距离变化速度
    public float zoomSpeed = 0.2f;
    public float rot = 0;
    //横向旋转速度
    public float rotSpeed = 0.2f;
    private float roll = 30f * Mathf.PI * 2 / 360;
    //纵向角度范围
    private float maxRoll = 70f * Mathf.PI * 2 / 360;
    private float minRoll = -10f * Mathf.PI * 2 / 360;
    //纵向旋转速度
    public float rollSpeed = 0.2f;
    private GameObject target;
	void Start () {
        //target = GameObject.Find("tank");
        SetTarget(GameObject.Find("tank"));
	}
    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }
        if (Camera.main == null)
        {
            return;
        }
        Vector3 targetPos = target.transform.position;
        //使用三角函数计算相机位置
        Vector3 cameraPos;
        float d = distance * Mathf.Cos(roll);
        float height = distance * Mathf.Sin(roll);
        cameraPos.x = targetPos.x + d * Mathf.Cos(rot);
        cameraPos.z = targetPos.z + d * Mathf.Sin(rot);
        cameraPos.y = targetPos.y + height;
        Camera.main.transform.position = cameraPos;
        Camera.main.transform.LookAt(target.transform);
        //纵向旋转
        Roll();
        //横向旋转
        Rotate();     
        //调整距离
        Zoom();
    }
    //设置目标
    private void SetTarget(GameObject target)
    {
        if (target.transform.Find("cameraPoint") != null)
        {
            this.target = target.transform.Find("cameraPoint").gameObject;
        }
        else
        {
            this.target = target;
        }
    }
    //横向旋转
    void Rotate()
    {
        float w = Input.GetAxis("Mouse X") * rotSpeed;
        rot -= w;
    }
    //纵向旋转
    void Roll()
    {
        float w = Input.GetAxis("Mouse Y") * rollSpeed*0.5f;
        roll -= w;
        if (roll < minRoll)
        {
            roll = minRoll;
        }
        if (roll >maxRoll)
        {
            roll = maxRoll;
        }
    }
    //调整距离
    void Zoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (distance > minDistance)
                distance -= zoomSpeed;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (distance < maxDistance)
                distance += zoomSpeed;
        }
    }
     
  
}
