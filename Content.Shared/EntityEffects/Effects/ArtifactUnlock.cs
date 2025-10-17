<<<<<<< HEAD:Content.Server/EntityEffects/Effects/ArtifactUnlock.cs
using Content.Server.Popups;
using Content.Server.Xenoarchaeology.Artifact;
using Content.Shared.EntityEffects;
using Content.Shared.Popups;
using Content.Shared.Xenoarchaeology.Artifact.Components;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;

namespace Content.Server.EntityEffects.Effects;
=======
using Content.Shared.Xenoarchaeology.Artifact;
using Content.Shared.EntityEffects;
using Content.Shared.Popups;
using Content.Shared.Xenoarchaeology.Artifact.Components;
using Robust.Shared.Prototypes;

namespace Content.Shared.EntityEffects.Effects;
>>>>>>> upstream/master:Content.Shared/EntityEffects/Effects/ArtifactUnlock.cs

/// <summary>
/// Sets an artifact into the unlocking state and marks the artifexium effect as true.
/// This is a very specific behavior intended for a specific chem.
/// </summary>
<<<<<<< HEAD:Content.Server/EntityEffects/Effects/ArtifactUnlock.cs
[UsedImplicitly]
=======
>>>>>>> upstream/master:Content.Shared/EntityEffects/Effects/ArtifactUnlock.cs
public sealed partial class ArtifactUnlock : EntityEffect
{
    public override void Effect(EntityEffectBaseArgs args)
    {
        var entMan = args.EntityManager;
<<<<<<< HEAD:Content.Server/EntityEffects/Effects/ArtifactUnlock.cs
        var xenoArtifactSys = entMan.System<XenoArtifactSystem>();
        var popupSys = entMan.System<PopupSystem>();
=======
        var xenoArtifactSys = entMan.System<SharedXenoArtifactSystem>();
        var popupSys = entMan.System<SharedPopupSystem>();
>>>>>>> upstream/master:Content.Shared/EntityEffects/Effects/ArtifactUnlock.cs

        if (!entMan.TryGetComponent<XenoArtifactComponent>(args.TargetEntity, out var xenoArtifact))
            return;

        if (!entMan.TryGetComponent<XenoArtifactUnlockingComponent>(args.TargetEntity, out var unlocking))
        {
            xenoArtifactSys.TriggerXenoArtifact((args.TargetEntity, xenoArtifact), null, force: true);
            unlocking = entMan.EnsureComponent<XenoArtifactUnlockingComponent>(args.TargetEntity);
        }
        else if (!unlocking.ArtifexiumApplied)
        {
            popupSys.PopupEntity(Loc.GetString("artifact-activation-artifexium"), args.TargetEntity, PopupType.Medium);
        }

        if (unlocking.ArtifexiumApplied)
            return;

        xenoArtifactSys.SetArtifexiumApplied((args.TargetEntity, unlocking), true);
    }

    protected override string ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        return Loc.GetString("reagent-effect-guidebook-artifact-unlock", ("chance", Probability));
    }
}
