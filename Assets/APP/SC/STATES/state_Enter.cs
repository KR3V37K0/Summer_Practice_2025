using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class state_Enter : State
{
    [Header("ENTERING")]
    [SerializeField] TMP_InputField input_name, input_password;
    [SerializeField] GameObject obj_error;
    [SerializeField] State state_next;

    [Header("BOTTOM PANEL")]
    [SerializeField] Transform trans_bottom;

    public override Task Enter()
    {
        obj_error.SetActive(false);
        input_name.text = "";
        input_password.text = "";


        return base.Enter();
    }
    async Task<bool> EnteringDatbase()
    {
        return await DatabaseCommands.Entering(input_name.text, input_password.text);
        //DatabaseManager.db.Entering(input_name.text, drop_tag.itemText.text));
    }


    public async void btn_Enter()
    {

        if (await EnteringDatbase())
        {
            Debug.Log("вы вошли как " + Info.user.name);
            AccessBottom();
            StateMachine.Instance.Exe_State(state_next);
        }
        else
        {
            //StateMachine.fsm.hasStop = true;
            //StateMachine.hasStop = true;
            obj_error.SetActive(true);
        }
    }
    void AccessBottom()
    {
        List<string> roles = new List<string> { "Пользователь", "Пользователь", "Автор", "Администратор", "Администратор" };
        for (int i = 0; i < 5; i++)
        {
            trans_bottom.GetChild(i).gameObject.SetActive(Info.user.tags.Contains(roles[i]));
        }
        trans_bottom.parent.gameObject.SetActive(true);       
    }
}
