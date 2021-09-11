using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 有限状态机
/// </summary>
/// <typeparam name="StateID"></typeparam>
/// <typeparam name="Value"></typeparam>
public class StateMachine<StateID, Value> where StateID : System.Enum
{
    public Value Context { get; private set; }
    public StateID CurrentStateID { get; private set; }
    public StateBase<StateID, Value> CurrentState { get; private set; }
    protected Dictionary<StateID, StateBase<StateID, Value>> m_StateMap;

    public StateMachine(Value context)
    {
        Context = context;
        if (m_StateMap == null) m_StateMap = new Dictionary<StateID, StateBase<StateID, Value>>();
    }

    /// <summary>
    /// 设置默认状态
    /// </summary>
    /// <param name="stateID"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    public StateMachine<StateID, Value> SetDefaultState(StateID stateID, StateBase<StateID, Value> state)
    {
        if (CurrentState == null)
        {
            CurrentStateID = stateID;
            CurrentState = state;
        }

        return this;
    }

    /// <summary>
    /// 添加状态
    /// </summary>
    /// <param name="stateID">添加的状态ID</param>
    /// <param name="state">对应的状态对象</param>
    public StateMachine<StateID, Value> AddState(StateID stateID, StateBase<StateID, Value> state)
    {
        if (m_StateMap.Count >= StateMachineDefine.MAX_CAPACITY)
        {
            DeLog($"已经达到状态机最大容量");
            return this;
        }

        if (m_StateMap.ContainsKey(stateID))
        {
            DeLog($"状态ID:{stateID}已经存在，不能重复添加");
            return this;
        }

        m_StateMap.Add(stateID, state);

        return this;
    }

    /// <summary>
    /// 移除状态
    /// </summary>
    /// <param name="stateID">要移除的状态ID</param>
    public StateMachine<StateID, Value> RemoveState(StateID stateID)
    {
        if (!m_StateMap.ContainsKey(stateID))
        {
            DeLog($"状态ID:{stateID}不存在，不需要移除");
            return this;
        }
        m_StateMap.Remove(stateID);

        return this;
    }

    /// <summary>
    /// 改变状态
    /// </summary>
    /// <param name="stateID">需要转换到的目标状态ID</param>
    public void ChangeState(StateID stateID)
    {
        if (!m_StateMap.ContainsKey(stateID))
        {
            DeLog($"状态ID:{stateID}不存在！");
            return;
        }

        if (CurrentState != null && !CurrentState.CanTransitionTo(1 << stateID.GetHashCode()))
        {
            DeLog($"无法切换至{stateID}");
            return;
        }

        var lastStateID = CurrentStateID;
        CurrentStateID = stateID;
        CurrentState?.OnExit(stateID);
        CurrentState = m_StateMap[stateID];
        CurrentState.OnEnter(lastStateID);
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update() => CurrentState?.Update();
    /// <summary>
    /// 延迟更新
    /// </summary>
    public void LateUpdate() => CurrentState?.LateUpdate();
    /// <summary>
    /// 物理更新
    /// </summary>
    public void FixedUpdate() => CurrentState?.FixedUpdate();

    /// <summary>
    /// 打印
    /// </summary>
    /// <param name="content"></param>
    public void DeLog(string content)
    {
        Console.WriteLine(content);
    }
}
