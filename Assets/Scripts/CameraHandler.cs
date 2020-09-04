using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform pivot;
    public Transform Player;
    public Transform mTransform;

    public PlayerStatus playerStatus;
    public CameraConfig cameraConfig;
    public bool leftPivot;
    public float delta;

    public Transform targetLook;

    public float mouseX;
    public float mouseY;
    public float smoothX;
    public float smoothY;
    public float smoothXVelocity;
    public float smoothYVelocity;
    public float lookAngle;
    public float tiltAngle;

    private void Update()
    {
        Tick();
    }

    void Tick()
    {
        delta = Time.deltaTime;

        HandlePosition();
        HandleRotation();

        Vector3 targetPosition = Vector3.Lerp(mTransform.position, Player.position, 1);
        mTransform.position = targetPosition;

        TargetLook();
    }

    //направляем наш таргет лук
    void TargetLook()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward * 2000);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) 
        {
            //вызывает дергания при взгляде в небо
            targetLook.position = Vector3.Lerp(targetLook.position, hit.point, Time.deltaTime * 5);

        }
        else //если смотрим в небо
        {
            targetLook.position = Vector3.Lerp(targetLook.position, targetLook.transform.forward * 200, Time.deltaTime * 5);
        }
    }

    //позиция камеры
    void HandlePosition()
    {
        float targetX = cameraConfig.normalX;
        float targetY = cameraConfig.normalY;
        float targetZ = cameraConfig.normalZ;        

        //если целится
        //if (playerStatus.isAiming)
        {
            //targetX = cameraConfig.aimX;
            //targetZ = cameraConfig.aimZ;
        }
        
        //камера привязана к пивоту потому его перемещаем куда надо
        Vector3 newPivotPosition = pivot.localPosition;
        newPivotPosition.x = targetX;
        newPivotPosition.y = targetY;
        //отдаляем камеру от пивота на некоторое растояние
        Vector3 newCameraPosition = cameraTransform.localPosition;
        newCameraPosition.z = targetZ;

        //перемещаем камеру
        float t = delta * cameraConfig.pivotSpeed;
        //перемещаем пивот с начальной точки к конечной со скоростью t
        pivot.localPosition = Vector3.Lerp(pivot.localPosition, newPivotPosition, t);
        //перемещаем камеру
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newCameraPosition, t);
    }

    //поворот камеры
    void HandleRotation()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        if (cameraConfig.turnSmooth > 0)
        {
            smoothX = Mathf.SmoothDamp(smoothX, mouseX, ref smoothXVelocity, cameraConfig.turnSmooth);
            smoothY = Mathf.SmoothDamp(smoothY, mouseY, ref smoothYVelocity, cameraConfig.turnSmooth);
        }
        else
        {
            smoothX = mouseX;
            smoothY = mouseY;
        }

        //вращаем камеру по горизонтали
        lookAngle += smoothX * cameraConfig.X_rot_speed;
        Quaternion targetRot = Quaternion.Euler(0, lookAngle, 0);
        mTransform.rotation = targetRot;

        //вращаем камеру по вертикали
        tiltAngle += smoothY * cameraConfig.Y_rot_speed;
        tiltAngle = Mathf.Clamp(tiltAngle, cameraConfig.minAngle, cameraConfig.maxAngle);
        pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);
    }
}
