using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class StateMachine : MonoBehaviour
{
    public static StateMachine Instance { get; private set; }

    [SerializeField] private State state_start;
    Dictionary<string,State> all_states=new Dictionary<string, State>();
    private State state_current;

    public static bool hasStop = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(this.gameObject); // Опционально, если нужно сохранять между сценами
    }

    private async void Start()
    {
        Debug.Log("StateMachine initialized");
        await Start_Machine();
    }

    public async Task Start_Machine()
    {
        Debug.Log("Starting state machine");
        await Exe_State(state_start);
        SearchAllStates();
    }
    void SearchAllStates()
    {
        foreach (State s in Resources.FindObjectsOfTypeAll<State>())
        {
            all_states.Add(s.GetComponent<State>().GetType().ToString(), s);
        }
        Debug.Log(all_states.First().Key);
    }

    public async Task Exe_State(string _state)
    {
        await Exe_State(all_states[_state]);
    }
    public async Task Exe_State(State _state)
    {
        if (_state == null)
        {
            Debug.LogError("Next state is null!");
            return;
        }

        Debug.Log($"Executing state: {_state.name}");

        if (state_current != null)
        {
            await state_current.Exit();
        }

        if (hasStop)
        {
            hasStop = false;
            return;
        }

        if (!_state.gameObject.activeSelf)
        {
            _state.gameObject.SetActive(true);
        }

        await _state.Enter();
        state_current = _state;
    }
}