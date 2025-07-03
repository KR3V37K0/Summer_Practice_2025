using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class State : MonoBehaviour
{
    [SerializeField] State previous;
    [SerializeField] GameObject canvas;

    public virtual async Task Enter()
    {

    }
    public virtual async Task Back()
    {
        previous.gameObject.SetActive(true);
        previous.Enter();
        Exit();
    }
    public virtual async Task Exit()
    {
        gameObject.SetActive(false);
    }
}
