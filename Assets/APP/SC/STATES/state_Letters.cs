using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class state_Letters : State
{
    

    public async void btn_ExitToLogin()
    {
        await StateMachine.Instance.Exe_State("state_Enter");
    }
}
