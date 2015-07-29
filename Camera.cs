@@ -0,0 +1,93 @@
﻿using UnityEngine;
using System.Collections;
/// 
/// 将此脚本附加到任意镜头上，可以使其拥有WOW镜头的控制方式
/// 
public class WOWcamera : MonoBehaviour
{
	/// 
	/// 镜头目标 The target of the camera,any object can be added
	/// 
	public Transform Target;
	/// 
	/// 镜头离目标的距离 the distance between the camera and the object
	/// 
	public float Distance = 200.0f;
	/// 
	/// 最大镜头距离 the max distance of the distance between the camera and the object
	///  
	public float MaxDistance = 200.0f;
	/// 
	/// 鼠标滚轮拉近拉远速度系数 mouse middle rolling
	/// 
	public float ScrollFactor = 10.0f;
	/// 
	/// 镜头旋转速度比率 speed of turning the camera
	/// 
	public float RotateFactor = 10.0f;
	/// 
	/// 镜头水平环绕角度 the start angle of the camera
	/// 
	public float HorizontalAngle = 45; 
	///  
	/// 镜头竖直环绕角度 the start angle of the camera.
	/// 
	public float VerticalAngle = 0;
	private Transform mCameraTransform;
	void Start()
	{
		mCameraTransform = transform;
	}
	void Update()
	{
		//滚轮向前：拉近距离；滚轮向后：拉远距离 scrolling of the middle mouse
		var scrollAmount = Input.GetAxis("Mouse ScrollWheel");
		Distance -= scrollAmount * ScrollFactor;
		//保证镜头距离合法 legal distance 
		if (Distance < 0)
			Distance = 0;
		else if (Distance > MaxDistance)
			Distance = MaxDistance;
		//按住鼠标左右键移动，镜头随之旋转 if we click left or right, the camera will be turning
		var isMouseLeftButtonDown = Input.GetMouseButton(0);
		var isMouseRightButtonDown = Input.GetMouseButton(1);
		if (isMouseRightButtonDown)
		{
			Screen.lockCursor = true;
			var axisX = Input.GetAxis("Mouse X");
			var axisY = Input.GetAxis("Mouse Y");
			//HorizontalAngle += axisX * RotateFactor;
			HorizontalAngle += 0.1f * RotateFactor;
			VerticalAngle += axisY * RotateFactor;
			if (isMouseRightButtonDown)
			{
				//如果是鼠标右键移动，则旋转人物在水平面上与镜头方向一致
				Target.rotation = Quaternion.Euler(0,HorizontalAngle , 0);
			}
		}

		if (isMouseLeftButtonDown)
		{
			Screen.lockCursor = true;
			var axisX = Input.GetAxis("Mouse X");
			var axisY = Input.GetAxis("Mouse Y");
			//HorizontalAngle += axisX * RotateFactor;
			HorizontalAngle += -0.1f * RotateFactor;
			VerticalAngle += axisY * RotateFactor;
			if (isMouseRightButtonDown)
			{
				//如果是鼠标右键移动，则旋转人物在水平面上与镜头方向一致
				Target.rotation = Quaternion.Euler(0,HorizontalAngle , 0);
			}
		}
		else
		{
			Screen.lockCursor = false;
		}
		//按镜头距离调整位置和方向
		var rotation = Quaternion.Euler(-VerticalAngle, HorizontalAngle, 0);
		var offset = rotation * Vector3.back * Distance;
		mCameraTransform.position = Target.position + offset;
		mCameraTransform.rotation = rotation;
	}
}
