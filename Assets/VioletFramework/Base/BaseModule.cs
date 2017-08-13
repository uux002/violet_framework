using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public enum ENModuleState {
    Normal,
    Error,
}

public class BaseModule
{
    public ENModuleState moduleState = ENModuleState.Normal;
    public string ERROR_MSG = string.Empty;

    public virtual void Initialize(){
        
    }

    public virtual void OnUpdate(){
        
    }

    public virtual void OnFixedUpdate(){
        
    }
}
