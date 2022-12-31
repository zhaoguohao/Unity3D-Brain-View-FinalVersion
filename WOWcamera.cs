using UnityEngine;
using UnityEngine.Serialization;

/// 
/// 将此脚本附加到任意镜头上，可以使其拥有WOW镜头的控制方式
/// 
public class WoWCamera : MonoBehaviour
{
    /// 
    /// 镜头目标 The target of the camera,any object can be added
    /// 
    [FormerlySerializedAs("Target")] public Transform target;

    /// 
    /// 镜头离目标的距离 the distance between the camera and the object
    /// 
    [FormerlySerializedAs("Distance")] public float distance = 200.0f;

    /// 
    /// 最大镜头距离 the max distance of the distance between the camera and the object
    ///  
    [FormerlySerializedAs("MaxDistance")] public float maxDistance = 200.0f;

    /// 
    /// 鼠标滚轮拉近拉远速度系数 mouse middle rolling
    /// 
    [FormerlySerializedAs("ScrollFactor")] public float scrollFactor;

    /// 
    /// 镜头旋转速度比率 speed of turning the camera
    /// 
    [FormerlySerializedAs("RotateFactor")] public float rotateFactor = 10.0f;

    /// 
    /// 镜头水平环绕角度 the start angle of the camera
    /// 
    [FormerlySerializedAs("HorizontalAngle")]
    public float horizontalAngle = 45;

    ///  
    /// 镜头竖直环绕角度 the start angle of the camera.
    /// 
    [FormerlySerializedAs("VerticalAngle")]
    public float verticalAngle = 0;

    private Transform _mCameraTransform;

    public WoWCamera()
    {
        scrollFactor = 10.0f;
    }

    void Start()
    {
        _mCameraTransform = transform;
    }

    void Update()
    {
        //滚轮向前：拉近距离；滚轮向后：拉远距离 scrolling of the middle mouse
        var scrollAmount = Input.GetAxis("Mouse ScrollWheel");
        distance -= scrollAmount * scrollFactor;
        //保证镜头距离合法 legal distance 
        if (distance < 0)
            distance = 0;
        else if (distance > maxDistance)
            distance = maxDistance;
        //按住鼠标左右键移动，镜头随之旋转 if we click left or right, the camera will be turning
        var isMouseLeftButtonDown = Input.GetMouseButton(0);
        var isMouseRightButtonDown = Input.GetMouseButton(1);
        if (isMouseRightButtonDown)
        {
#pragma warning disable 618
            Screen.lockCursor = true;
#pragma warning restore 618
            var axisX = Input.GetAxis("Mouse X");
            var axisY = Input.GetAxis("Mouse Y");
            //HorizontalAngle += axisX * RotateFactor;
            horizontalAngle += 0.1f * rotateFactor;
            verticalAngle += axisY * rotateFactor;
            //如果是鼠标右键移动，则旋转人物在水平面上与镜头方向一致
            target.rotation = Quaternion.Euler(0, horizontalAngle, 0);
        }

        if (isMouseLeftButtonDown)
        {
#pragma warning disable 618
            Screen.lockCursor = true;
#pragma warning restore 618
            var axisX = Input.GetAxis("Mouse X");
            var axisY = Input.GetAxis("Mouse Y");
            //HorizontalAngle += axisX * RotateFactor;
            horizontalAngle += -0.1f * rotateFactor;
            verticalAngle += axisY * rotateFactor;
            if (isMouseRightButtonDown)
            {
                //如果是鼠标右键移动，则旋转人物在水平面上与镜头方向一致
                target.rotation = Quaternion.Euler(0, horizontalAngle, 0);
            }
        }
        else
        {
#pragma warning disable 618
            Screen.lockCursor = false;
#pragma warning restore 618
        }

        //按镜头距离调整位置和方向
        var rotation = Quaternion.Euler(-verticalAngle, horizontalAngle, 0);
        var offset = rotation * Vector3.back * distance;
        _mCameraTransform.position = target.position + offset;
        _mCameraTransform.rotation = rotation;
    }
}