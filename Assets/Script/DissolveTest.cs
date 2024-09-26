using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveTest : MonoBehaviour
{
    public Material Material;
    public float amount = -1;
    public bool bDissolve;


    private void Start()
    {
        amount = -1;
        Material.SetFloat("_Amount",amount);
    }
    // Update is called once per frame
    void Update()
    {
        if (bDissolve)
        {
            amount += Time.deltaTime;
            Material.SetFloat("_Amount", amount);
        }
    }
}
