using UnityEngine;
using System.Threading.Tasks;

public class StateMachine : MonoBehaviour
{
    public static StateMachine Instance { get; private set; }

    [SerializeField] private State state_start;
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
        DontDestroyOnLoad(this.gameObject); // Опционально, если нужно сохранять между сценами
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
    }

    public async Task Exe_State(State _state)
    {
        if (_state == null)
        {
            Debug.LogError("Next state is null!");
            return;
        }

        Debug.Log($"Executing state: {_state.gameObject.name}");

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