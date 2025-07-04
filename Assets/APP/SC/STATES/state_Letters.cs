using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class state_Letters : State
{
    [Header("CHANELS")]
    [SerializeField] GameObject pref_btn_Chanel;
    [SerializeField] Transform trans_chanels_container;
    List<chanel> chanels = new List<chanel>();
    public override async Task Enter()
    {
        await Tools.DeleteChildren(trans_chanels_container);
        chanels = await DatabaseCommands.GetChanels(Info.user.id, "Слушатель");
        Debug.Log(chanels.Count);
        foreach (chanel cha in chanels)
        {
            GameObject pan = Instantiate(pref_btn_Chanel, trans_chanels_container);
            pan.GetComponentInChildren<TMP_Text>().text = cha.name;
            pan.transform.GetChild(1).gameObject.SetActive(cha.noReaded);
        }

        await base.Enter();
    }
    public async void btn_ExitToLogin()
    {
        await StateMachine.Instance.Exe_State("state_Enter");
    }
}



