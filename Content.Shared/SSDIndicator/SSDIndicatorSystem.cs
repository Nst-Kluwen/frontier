<<<<<<< HEAD
using Content.Shared.CCVar;
using Content.Shared.StatusEffectNew;
using Robust.Shared.Configuration;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
=======
﻿using Content.Shared.Bed.Sleep;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;
using Robust.Shared.Player;
>>>>>>> upstream/master
using Robust.Shared.Timing;

namespace Content.Shared.SSDIndicator;

/// <summary>
///     Handle changing player SSD indicator status
/// </summary>
public sealed class SSDIndicatorSystem : EntitySystem
{
<<<<<<< HEAD
    public static readonly EntProtoId StatusEffectSSDSleeping = "StatusEffectSSDSleeping";

    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly StatusEffectsSystem _statusEffects = default!;
=======
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
>>>>>>> upstream/master

    private bool _icSsdSleep;
    private float _icSsdSleepTime;

    public override void Initialize()
    {
        SubscribeLocalEvent<SSDIndicatorComponent, PlayerAttachedEvent>(OnPlayerAttached);
        SubscribeLocalEvent<SSDIndicatorComponent, PlayerDetachedEvent>(OnPlayerDetached);
        SubscribeLocalEvent<SSDIndicatorComponent, MapInitEvent>(OnMapInit);

        _cfg.OnValueChanged(CCVars.ICSSDSleep, obj => _icSsdSleep = obj, true);
        _cfg.OnValueChanged(CCVars.ICSSDSleepTime, obj => _icSsdSleepTime = obj, true);
    }

    private void OnPlayerAttached(EntityUid uid, SSDIndicatorComponent component, PlayerAttachedEvent args)
    {
        component.IsSSD = false;

        // Removes force sleep and resets the time to zero
        if (_icSsdSleep)
        {
            component.FallAsleepTime = TimeSpan.Zero;
<<<<<<< HEAD
            _statusEffects.TryRemoveStatusEffect(uid, StatusEffectSSDSleeping);
        }

=======
            if (component.ForcedSleepAdded) // Remove component only if it has been added by this system
            {
                EntityManager.RemoveComponent<ForcedSleepingComponent>(uid);
                component.ForcedSleepAdded = false;
            }
        }
>>>>>>> upstream/master
        Dirty(uid, component);
    }

    private void OnPlayerDetached(EntityUid uid, SSDIndicatorComponent component, PlayerDetachedEvent args)
    {
        component.IsSSD = true;

        // Sets the time when the entity should fall asleep
        if (_icSsdSleep)
        {
            component.FallAsleepTime = _timing.CurTime + TimeSpan.FromSeconds(_icSsdSleepTime);
        }
<<<<<<< HEAD

=======
>>>>>>> upstream/master
        Dirty(uid, component);
    }

    // Prevents mapped mobs to go to sleep immediately
    private void OnMapInit(EntityUid uid, SSDIndicatorComponent component, MapInitEvent args)
    {
        if (_icSsdSleep &&
            component.IsSSD &&
            component.FallAsleepTime == TimeSpan.Zero)
        {
            component.FallAsleepTime = _timing.CurTime + TimeSpan.FromSeconds(_icSsdSleepTime);
        }
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (!_icSsdSleep)
            return;

        var query = EntityQueryEnumerator<SSDIndicatorComponent>();

        while (query.MoveNext(out var uid, out var ssd))
        {
            // Forces the entity to sleep when the time has come
<<<<<<< HEAD
            if (ssd.IsSSD &&
                ssd.FallAsleepTime <= _timing.CurTime &&
                !TerminatingOrDeleted(uid))
            {
                _statusEffects.TrySetStatusEffectDuration(uid, StatusEffectSSDSleeping, null);
=======
            if(ssd.IsSSD &&
                !ssd.PreventSleep && // Frontier
                ssd.FallAsleepTime <= _timing.CurTime &&
                !TerminatingOrDeleted(uid) &&
                !HasComp<ForcedSleepingComponent>(uid)) // Don't add the component if the entity has it from another sources
            {
                EnsureComp<ForcedSleepingComponent>(uid);
                ssd.ForcedSleepAdded = true;
>>>>>>> upstream/master
            }
        }
    }
}
