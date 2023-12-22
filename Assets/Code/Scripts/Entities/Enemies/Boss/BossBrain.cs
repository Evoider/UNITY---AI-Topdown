// --------------------------------------- //
// --------------------------------------- //
//  Creation Date: 20/12/23
//  Description: AI - Topdown
// --------------------------------------- //
// --------------------------------------- //

using System;
using System.Collections.Generic;
using UnityEngine;

public class BossBrain : EnemyBrain
{
    //#region Events
    //public event Action OnPhaseStart;
    //public void InvokeOnPhaseStart()
    //{
    //    OnPhaseStart?.Invoke();
    //}

    //public event Action OnPhaseEnd;
    //public void InvokeOnPhaseEnd()
    //{
    //    OnPhaseEnd?.Invoke();
    //}

    //public event Action<int> OnPhaseChange;
    //public void InvokeOnPhaseChange(int phase)
    //{
    //    OnPhaseChange?.Invoke(phase);
    //}
    //#endregion Events

    public Boss Boss => _boss;
    private Boss _boss => _entity as Boss;

    [SerializeField] private GameObject _legs;

    private BossStateManager _stateManager;

    public bool IsUnlocked
    {
        get => _isUnlocked;
        set => _isUnlocked = value;
    }
    private bool _isUnlocked;

    public bool IsEnded
    {
        get => _isEnded;
        set => _isEnded = value;
    }
    private bool _isEnded;

    #region Phase
    public List<Phase> Phases => _phases;
    private List<Phase> _phases = new();

    public int Phase
    {
        get => _phase;
        set
        {
            _phase = value;
            _boss.BaseData = _boss.PhaseBaseData[_phase];
            _aiPath.maxSpeed = _entity.MovementSpeed / 50f;
        }
    }
    [SerializeField] protected int _phase = 0;

    public Phase CurrentPhase => _phases[_phase];
    #endregion Phase

    protected override void Awake()
    {
        base.Awake();
        _stateManager = new BossStateManager(this);

        foreach (var phaseData in _boss.PhaseBaseData)
        {
            var newPhase = new GameObject(phaseData.name).AddComponent<Phase>();
            newPhase.transform.parent = transform;
            newPhase.BaseData = phaseData;
            _phases.Add(newPhase);
        }

        _boss.BaseData = _boss.PhaseBaseData[_phase];

        if (_legs != null)
            return;

        _legs = transform.root.Find("Render").Find("Skin").Find("Legs").gameObject;
    }

    protected override void Update()
    {
        base.Update();
        _stateManager.Update();
    }

    public void ShowLegs(bool show)
    {
        _legs.SetActive(show);
    }
}