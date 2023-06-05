using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boatscript : MonoBehaviour
{
    [HideInInspector]
    public bool goDriveIntoTheSun;
    public Transform Sun;

    private void Update()
    {
        if (goDriveIntoTheSun)
        {
            Quaternion targetrotation = Quaternion.LookRotation(Sun.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetrotation, 1 * Time.deltaTime);
            transform.position += transform.forward * 1f * Time.deltaTime;
        }
    }
    public void StartEndScene()
    {
        SceneManager.LoadScene("FinalScene");
    }
}
