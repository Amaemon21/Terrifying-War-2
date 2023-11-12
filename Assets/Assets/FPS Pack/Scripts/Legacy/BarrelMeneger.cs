using System.Collections;
using UnityEngine;

public class BarrelMeneger : MonoBehaviour
{
    [SerializeField] private GameObject[] explosiveBarrelScripts;

    public void Enable()
    {
        for (int i = 0; i < explosiveBarrelScripts.Length; i++)
        {
            explosiveBarrelScripts[i].SetActive(true);
        }
    }

}
