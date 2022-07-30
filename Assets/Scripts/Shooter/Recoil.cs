using System.Collections;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    //Rotations
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    private Vector3 cameraCurrent;
    private Vector3 cameraTarget;

    //Hipfire Recoil
    [SerializeField]
    private float recoilX,
                  recoilY,
                  recoilZ,
                  spreadRate;

    //Settings
    [SerializeField]
    private float snappiness,
                  returnspeed,
                  patternResetSeconds;

    private WaitForSeconds patternResetDuration = new WaitForSeconds(0.1f);
    private int shotCount = 0;
    private float shotTime = 0;

    [SerializeField]
    Transform CameraRecoil;

    private Vector3 cameraReduction = new Vector3(0.6f, 0.3f);
    private float cameraSpeed = 0.6f;
    private float cameraSnap = 2.0f;

    void Start()
    {
    }

    void Update()
    {
        if (shotCount > 0 && Time.time > shotTime + patternResetSeconds)
        {
            resetPattern();
        }
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnspeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);

        cameraTarget = Vector3.Lerp(Vector3.Scale(targetRotation, cameraReduction), Vector3.zero, cameraSpeed * Time.deltaTime);
        cameraCurrent = Vector3.Slerp(cameraCurrent, cameraTarget, snappiness * cameraSnap * Time.fixedDeltaTime);

        transform.localRotation = Quaternion.Euler(currentRotation);
        CameraRecoil.localRotation = Quaternion.Euler(cameraCurrent);
    }

    public void RecoilFire()
    {
        shotCount++;
        Vector3 bulletRecoil = new Vector3(recoilX * Random.Range(0.8f, 1.2f), recoilY * Mathf.Sin(Mathf.Pow(shotCount, 1.5f) *  spreadRate), recoilZ * Mathf.Sin((shotCount * spreadRate)));
        targetRotation += bulletRecoil;
        shotTime = Time.time;
    }

    public void resetPattern()
    {
        shotCount = 0;
        //targetRotation = new Vector3(0, 0);
        StartCoroutine(buffReturn());
    }

    private IEnumerator buffReturn()
    {
        float originalReturnSpeed = returnspeed;
        returnspeed *= 5f;
        yield return patternResetDuration;
        returnspeed = originalReturnSpeed;
    }
}
