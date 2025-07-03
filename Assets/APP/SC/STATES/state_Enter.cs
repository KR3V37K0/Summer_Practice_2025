using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class state_Enter : State
{
    [SerializeField] TMP_InputField input_name, input_password;
    [SerializeField] TMP_Dropdown drop_tag;
    [SerializeField] GameObject obj_error;

    public override Task Enter()
    {
        obj_error.SetActive(false);
        input_name.text = "";


        return base.Enter();
    }
    async Task<bool> check_in_database()
    {
        return await DatabaseCommands.Entering(input_name.text, input_password.text);
            //DatabaseManager.db.Entering(input_name.text, drop_tag.itemText.text));
    }


    public async void btn_Enter()
    {
        if (await check_in_database())
        {
            Debug.Log("вы вошли как " + Info.user.name);
        }
        else
        {
            //StateMachine.fsm.hasStop = true;
            StateMachine.hasStop = true;
            obj_error.SetActive(true);
        }
    }
}
