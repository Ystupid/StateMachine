using System;
using System.Collections.Generic;
using System.Text;

public abstract class StateBase<StateID, Value> where StateID : System.Enum
{
    private int m_Flag;
    protected StateMachine<StateID, Value> m_StateMachine;

    public int Flag { get => m_Flag; set => m_Flag = value; }

    protected StateBase(StateMachine<StateID, Value> stateMachine, int flag = StateMachineDefine.DEFAULT_VALUE)
    {
        m_Flag = flag;
        m_StateMachine = stateMachine;
    }

    /// <summary>
    /// 进入状态
    /// </summary>
    public abstract void OnEnter(StateID lastState = default(StateID));
    /// <summary>
    /// 退出状态
    /// </summary>
    public abstract void OnExit(StateID nextState = default(StateID));

    /// <summary>
    /// 更新
    /// </summary>
    public virtual void Update() { }
    /// <summary>
    /// 延迟帧更新
    /// </summary>
    public virtual void LateUpdate() { }
    /// <summary>
    /// 物理帧更新
    /// </summary>
    public virtual void FixedUpdate() { }

    /// <summary>
    /// 是否可以切换至
    /// </summary>
    /// <param name="stateID"></param>
    /// <returns></returns>
    public bool CanTransitionTo(int stateID) => (m_Flag & stateID) == stateID;
    /// <summary>
    /// 激活可切换状态
    /// </summary>
    /// <param name="stateID"></param>
    public StateBase<StateID,Value> EnableTransition(int stateID)
    {
        m_Flag |= stateID;
        return this;
    }
    /// <summary>
    /// 禁用可切换状态
    /// </summary>
    /// <param name="stateID"></param>
    public StateBase<StateID, Value> DisableTransition(int stateID)
    {
        m_Flag &= ~stateID;
        return this;
    }
}
