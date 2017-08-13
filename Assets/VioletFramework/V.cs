using UnityEngine;
using System.Collections;

public class V : VMonoSingleton<V>
{
    public Audio vAudio = null;
    public Resource vResource = null;
    public UnityTicker vTicker = null;
    public GridSystem vGrid = null;
    public HexagonSystem vHexagon = null;
    public MsgSystem vMsg = null;
    public ThreadBridge vThread = null;
    public Net vNet = null;

	public override void Initialize()
    {
        base.Initialize();

        vAudio = new Audio();
        vAudio.Initialize();

        vResource = new Resource();
        vResource.Initialize();

        vTicker = new UnityTicker();
        vTicker.Initialize();

        vGrid = new GridSystem();
        vGrid.Initialize();

        vHexagon = new HexagonSystem();
        vHexagon.Initialize();

        vMsg = new MsgSystem();
        vMsg.Initialize();

        vThread = new ThreadBridge();
        vThread.Initialize();

        vNet = new Net();
        vNet.Initialize();
    }


    private void Update()
    {
        vAudio.OnUpdate();
        vResource.OnUpdate();
        vTicker.OnUpdate();
        vGrid.OnUpdate();
        vHexagon.OnUpdate();
        vMsg.OnUpdate();
        vThread.OnUpdate();
        vNet.OnUpdate();
    }

    private void FixedUpdate()
    {
        vAudio.OnFixedUpdate();
        vResource.OnFixedUpdate();
        vTicker.OnFixedUpdate();
        vGrid.OnFixedUpdate();
        vHexagon.OnFixedUpdate();
        vMsg.OnFixedUpdate();
        vThread.OnFixedUpdate();
        vNet.OnFixedUpdate();
    }



}
