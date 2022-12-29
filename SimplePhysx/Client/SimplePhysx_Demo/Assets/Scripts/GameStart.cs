using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEMath;
using SimplePhysx;

public class GameStart : MonoBehaviour
{
    public float Speed = 10;
    public Transform playerTrans;

    private LineRenderer line;
    private FixedPointCollider2DBase playerCol;
    private PEVector3 inputDir;

    private List<FixedPointCollider2DBase> envColliders = new List<FixedPointCollider2DBase>();

    private PEVector3 LogicDir;//Âß¼­³¯Ïò
    private PEVector3 LogicPos;//Âß¼­Î»ÖÃ
    private void Awake()
    {
        line = playerTrans.GetComponent<LineRenderer>();
        InitPlayer();
        InitEnvCollider();

    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        inputDir = new PEVector3(new Vector3(h, 0, v));
        inputDir = inputDir.normalized;
    }

    private void FixedUpdate()
    {
        playerCol.Pos += inputDir * (PEInt)Speed * (PEInt)Time.fixedDeltaTime;
        var moveDir = inputDir;
        var adjust = PEVector3.zero;
        playerCol.ClacCollison(envColliders, ref moveDir, ref adjust);
        if (LogicDir != moveDir)
        {
            LogicDir = moveDir;
        }
        if (LogicDir != PEVector3.zero)
        {
            LogicPos = playerCol.Pos + adjust;
        }

        playerCol.Pos = LogicPos;
        playerTrans.position = LogicPos.ConvertViewVector3();

        DrawPlayerLine(adjust * 5);
        //DrawVelocityLine();
    }
    private void InitPlayer()
    {
        if (playerTrans == null)
        {
            return;
        }
        var collider = playerTrans.GetComponent<SphereCollider>();
        if (collider != null)
        {
            var col = new FixedPointSphereCollider2D();
            col.Init(new PEVector3(playerTrans.position), collider.radius);
            playerCol = col;
        }
        else
        {
            var boxCol = playerTrans.GetComponent<BoxCollider>();
            if (boxCol != null)
            {
                var col = new FixedPointBoxCollider2D();
                col.Init(new PEVector3(playerTrans.position), new PEVector3(playerTrans.transform.localScale), new PEVector3(playerTrans.right), new PEVector3(playerTrans.up), new PEVector3(playerTrans.forward));
                playerCol = col;
            }
        }
        LogicPos = new PEVector3(playerTrans.position);
    }
    private void InitEnvCollider()
    {
        var envRoot = GameObject.Find("Env");
        var boxColliders = envRoot.GetComponentsInChildren<BoxCollider>();
        var sphereColliders = envRoot.GetComponentsInChildren<SphereCollider>();

        Transform box;
        Transform sphere;
        for (int i = 0; i < boxColliders.Length; i++)
        {
            box = boxColliders[i].transform;
            var boxCol = new FixedPointBoxCollider2D();
            boxCol.Init(new PEVector3(box.position), new PEVector3(box.transform.localScale), new PEVector3(box.right), new PEVector3(box.up), new PEVector3(box.forward));
            envColliders.Add(boxCol);
        }
        for (int i = 0; i < sphereColliders.Length; i++)
        {
            sphere = sphereColliders[i].transform;
            var sphereCol = new FixedPointSphereCollider2D();
            sphereCol.Init(new PEVector3(sphere.position), sphere.localScale.x / 2);
            envColliders.Add(sphereCol);
        }
    }
    private void DrawVelocityLine()
    {
        line.startWidth = 0.3f;
        line.endWidth = 0f;
        line.positionCount = 2;
        PEVector3 startPos = LogicPos;
        PEVector3 endPos = startPos + LogicDir;
        line.SetPositions(new Vector3[2] { startPos.ConvertViewVector3() + new Vector3(0, 1, 0), endPos.ConvertViewVector3() + new Vector3(0, 1, 0) });
    }
    private void DrawPlayerLine(PEVector3 dir)
    {
        line.startWidth = 0.3f;
        line.endWidth = 0f;
        line.positionCount = 2;
        PEVector3 startPos = LogicPos;
        PEVector3 endPos = startPos + dir;
        line.SetPositions(new Vector3[2] { startPos.ConvertViewVector3() + new Vector3(0, 1, 0), endPos.ConvertViewVector3() + new Vector3(0, 1, 0) });
    }
}
