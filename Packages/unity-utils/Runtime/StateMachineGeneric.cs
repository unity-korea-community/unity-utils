using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UNKO.Utils;

namespace UNKO.Utils
{
    public interface IState
    {
        void OnAwake();
        IEnumerator OnStartCoroutine();
        void OnChangeState(IState newState);
        void OnFinishState();
    }

    /// <summary>
    /// State를 관리하는 머신
    /// </summary>
    public class StateMachineGeneric<STATE_ID, TSTATE>
        where TSTATE : class, IState
    {
        public enum CommandType
        {
            Change,
            Finish,
        }

        [System.Serializable]
        public struct Command
        {
            public CommandType commandType { get; private set; }
            public STATE_ID stateID { get; private set; }

            public Command(CommandType commandType)
            {
                this.commandType = commandType;
                stateID = default;
            }

            public Command(CommandType commandType, STATE_ID stateID)
            {
                this.commandType = commandType;
                this.stateID = stateID;
            }
        }

        public event System.Action<STATE_ID, TSTATE> OnChangeState;

        [SerializeField]
        private bool _debug;

        public STATE_ID currentStateID => _currentStateID;
        [SerializeField]
        private STATE_ID _currentStateID;

        public TSTATE currentState { get; private set; }
        protected MonoBehaviour _owner;
        protected Dictionary<STATE_ID, TSTATE> _stateInstance;

        // inspector에서 보기 위해 queue 대신 list 사용
        [SerializeField]
        List<Command> _commandQueue = new List<Command>();
        [SerializeField]
        List<STATE_ID> _waitQueue = new List<STATE_ID>();

        private Coroutine _currentCoroutine;

        public StateMachineGeneric(Dictionary<STATE_ID, TSTATE> stateInstances)
        {
            _stateInstance = stateInstances;
        }

        /// <summary>
        /// FSM을 시작합니다.
        /// </summary>
        /// <param name="owner">Mono</param>
        /// <param name="startState">시작할 스테이트</param>
        /// <param name="nextStates">시작 스테이트 다음 시작할 스테이트들</param>
        public void Start(MonoBehaviour owner, STATE_ID startState, params STATE_ID[] nextStates)
        {
            _owner = owner;

            Clear();
            ChangeState(startState);
            EnqueueToWaitQueue(nextStates);
            _owner.StartCoroutine(UpdateCoroutine());
        }


        /// <summary>
        /// 스테이트를 다음 스테이트로 변경합니다.
        /// </summary>
        /// <param name="state">변경할 스테이트</param>
        public void ChangeState(STATE_ID state)
        {
            if (_debug)
            {
                Debug.Log($"{_owner.name}.FSM.ChangeState changeState:{state}, wait:{_waitQueue.ToStringCollection()}", _owner);
            }

            _commandQueue.Add(new Command(CommandType.Change, state));
        }

        /// <summary>
        /// 현재 스테이트를 종료합니다.
        /// </summary>
        public void FinishState()
        {
            _commandQueue.Add(new Command(CommandType.Finish));
        }

        /// <summary>
        /// WaitQueue에 스테이트를 삽입합니다.
        /// </summary>
        /// <param name="nextStates"></param>
        public void EnqueueToWaitQueue(params STATE_ID[] nextStates)
        {
            if (_waitQueue.Count > 10)
            {
                Debug.LogWarning($"{_owner.name} _waitQueue.Count > 10, wait:{_waitQueue.ToStringCollection()}", _owner);
            }
            nextStates.Foreach(state => _waitQueue.Add(state));
        }

        public StateMachineGeneric<STATE_ID, TSTATE> ForEachState(System.Action<TSTATE> OnEach)
        {
            _stateInstance.Values.Foreach(OnEach);

            return this;
        }

        public StateMachineGeneric<STATE_ID, TSTATE> Clear()
        {
            ClearQueue();
            currentState = null;
            _currentStateID = default;

            return this;
        }

        public StateMachineGeneric<STATE_ID, TSTATE> ClearQueue()
        {
            _waitQueue.Clear();
            _commandQueue.Clear();

            return this;
        }

        IEnumerator UpdateCoroutine()
        {
            while (true)
            {
                while (_commandQueue.Count > 0)
                {
                    ProcessCommand(_commandQueue.Dequeue());
                }

                if (currentState == null && _waitQueue.Count > 0)
                {
                    OnStartState(_waitQueue.Dequeue());
                }

                yield return null;
            }
        }

        private void ProcessCommand(Command command)
        {
            switch (command.commandType)
            {
                case CommandType.Change:
                    OnStartState(command.stateID);
                    break;

                case CommandType.Finish:
                    OnFinishState();
                    break;

                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }

        private void OnStartState(STATE_ID stateID)
        {
            if (_debug)
            {
                Debug.Log($"{_owner.name}.FSM.OnStartState.Entry current:{currentStateID}, new:{stateID}, wait:{_waitQueue.ToStringCollection()}", _owner);
            }

            if (_stateInstance.TryGetValue(stateID, out TSTATE state) == false)
            {
                Debug.LogError($"{_owner.name}.FSM.OnStartState(StateWithParam:'{stateID}') state is not found", _owner);
                return;
            }

            if (state.Equals(currentState))
            {
                return;
            }

            if (_debug)
            {
                Debug.Log($"{_owner.name}.FSM.OnStartState.Execute current:{currentStateID}, new:{stateID}, wait:{_waitQueue.ToStringCollection()}", _owner);
            }

            currentState?.OnChangeState(state);
            state.OnAwake();
            currentState = state;
            _currentStateID = stateID;
            _currentCoroutine = _owner.StartCoroutine(StateCoroutine());
            OnChangeState?.Invoke(stateID, currentState);
        }

        private void OnFinishState()
        {
            if (_debug)
            {
                Debug.Log($"{_owner.name}.FSM.OnFinishState current:{currentStateID}, wait:{_waitQueue.ToStringCollection()}");
            }

            if (_currentCoroutine != null)
            {
                _owner.StopCoroutine(_currentCoroutine);
            }

            currentState?.OnFinishState();
            currentState = null;
            _currentStateID = default;
        }

        private IEnumerator StateCoroutine()
        {
            yield return currentState.OnStartCoroutine();
            _commandQueue.Add(new Command(CommandType.Finish));
        }
    }
}
