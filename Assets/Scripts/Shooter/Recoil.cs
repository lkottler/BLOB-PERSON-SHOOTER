using System.Collections;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    //Rotations
    private Vector3 currentRotation;
    private Vector3 targetRotation;

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

    private WaitForSeconds patternResetDuration = new WaitForSeconds(0.01f);
    private int shotCount = 0;
    private float shotTime = 0;

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
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilFire()
    {
        shotCount++;
        targetRotation += new Vector3(recoilX * Random.Range(0.8f, 1.2f), recoilY * Mathf.Sin(shotCount * spreadRate), recoilZ * Mathf.Sin((shotCount * spreadRate)));
        Debug.Log(Mathf.Sin(shotCount * spreadRate));
        shotTime = Time.time;
    }

    public void resetPattern()
    {
        shotCount = 0;
        targetRotation = new Vector3(0, 0);
        StartCoroutine(buffReturn());
    }

    private IEnumerator buffReturn()
    {
        float originalReturnSpeed = returnspeed;
        returnspeed *= 4;
        yield return patternResetDuration;
        returnspeed = originalReturnSpeed;
    }
}
