using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrail : MonoBehaviour
{
    public float activeTime = 2f;   //�ܻ�ȿ�� ����
    public MovementInput moveScript;    //ĳ������ ������ ����
    public float speedBoost = 6;    //�ܻ� ���� �ӵ� ������
    public Animator animator;   //ĳ������ �ִϸ��̼��� ����
    public float animSpeedBoost = 1.5f; //�ܻ�ȿ�� ���� �ִϸ��̼� �ӵ�������

    [Header("Mesh Releted")]    //�޽ø� ����
    public float meshRefreshRate = 0.1f;    //�ܻ��� �����Ǵ� �ð� ����
    public float meshDestoryDelay = 3.0f;   //������ �ܻ��� ������µ� �ɸ��� �ð�
    public Transform positionToSpawm;   //�ܻ��� ������ ��ġ

    [Header("Shader Releted")]
    public Material mat;    //�ܻ� ���뵵�� ��¡��
    public string shaderVarRef;//���̴����� ����� ���� �̸� (����)
    public float shaderVarRate = 0.1f;// ���̴� ȿ���� ��ȭ�ӵ�
    public float shaderVarRefreshRate = 0.05f;  //������Ʈ�Ǵ� �ð� ����

    private SkinnedMeshRenderer[] skinnedRenderers; //ĳ������ 3d ���� ������ �ϴ� ������Ʈ��
    private bool isTrailActive;//���� �ܻ�ȿ���� Ȱ��ȭ ���̾� �ִ��� Ȯ���ϴ� ����

    private float normalSpeed;  //���� �̵��ӵ��� �����ϴ� ����
    private float normalAnimSpeed;  //���� �ִϸ��̼� �ӵ��� �����ϴ� ����

    IEnumerator AnimateMaterialFloat (Material m, float valueGoal, float rate, float refreshRate)
    {
        float valueToAnimate = m.GetFloat(shaderVarRef);

        while (valueToAnimate > valueGoal)
        {
            valueToAnimate -= rate;
            m.SetFloat(shaderVarRef, valueToAnimate);
            yield return new WaitForSeconds(refreshRate);
        }
    }

    IEnumerator ActivateTrail(float timeActivated)
    {
        normalSpeed = moveScript.movementSpeed;
        moveScript.movementSpeed = speedBoost;

        normalAnimSpeed = animator.GetFloat("animSpeed");
        animator.SetFloat("animSpeed",animSpeedBoost);

        while (timeActivated > 0)
        {
            timeActivated -= meshRefreshRate;

            if (skinnedRenderers == null)
                skinnedRenderers = positionToSpawm.GetComponentsInChildren<SkinnedMeshRenderer>();

            for (int i = 0; i < skinnedRenderers.Length; i++)
            {
                GameObject gObj = new GameObject();
                gObj.transform.SetPositionAndRotation(positionToSpawm.position, positionToSpawm.rotation);

                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                MeshFilter mf = gObj.AddComponent<MeshFilter>();


                Mesh m = new Mesh();
                skinnedRenderers[i].BakeMesh(m);
                mf.mesh = m;
                mr.material = mat;

                StartCoroutine(AnimateMaterialFloat(mr.material, 0, shaderVarRate, shaderVarRefreshRate));

                Destroy(gObj, meshDestoryDelay);

            }
            yield return new WaitForSeconds(meshRefreshRate);
        }
        moveScript.movementSpeed = normalAnimSpeed;
        animator.SetFloat("animSpeed",normalAnimSpeed);
        isTrailActive = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) &&!isTrailActive)
        {
            isTrailActive=true;
            StartCoroutine(ActivateTrail(activeTime));
        }
    }

}
