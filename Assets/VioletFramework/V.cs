using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public class V : VMonoSingleton<V>
{
    public Audio vAudio = null;
    public ResourceSystem vResource = null;
    public BundleSystem vBundle = null;
    public UnityTicker vTicker = null;
    public GridSystem vGrid = null;
    public HexagonSystem vHexagon = null;
    public MsgSystem vMsg = null;
    public ThreadBridge vThread = null;
    public Net vNet = null;
    public ConfigTable vTable = null;

	public override void Initialize()
    {
        base.Initialize();

        vAudio = new Audio();
        vAudio.Initialize();

        vResource = new ResourceSystem();
        vResource.Initialize();

        vBundle = new BundleSystem();
        vBundle.Initialize();

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

        vTable = new ConfigTable();
        vTable.Initialize();

        vNet = new Net();
        vNet.Initialize();
    }

    private void Update()
    {
        vAudio.OnUpdate();
        vResource.OnUpdate();
        vBundle.OnUpdate();
        vTicker.OnUpdate();
        vGrid.OnUpdate();
        vHexagon.OnUpdate();
        vMsg.OnUpdate();
        vThread.OnUpdate();
        vNet.OnUpdate();
        vTable.OnUpdate();
    }

    private void FixedUpdate()
    {
        vAudio.OnFixedUpdate();
        vResource.OnFixedUpdate();
        vBundle.OnFixedUpdate();
        vTicker.OnFixedUpdate();
        vGrid.OnFixedUpdate();
        vHexagon.OnFixedUpdate();
        vMsg.OnFixedUpdate();
        vThread.OnFixedUpdate();
        vNet.OnFixedUpdate();
        vTable.OnFixedUpdate();
    }



}
