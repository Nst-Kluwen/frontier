using Content.Shared.Xenoarchaeology.XenoArtifacts;
using Robust.Client.GameObjects;

namespace Content.Client.Xenoarchaeology.XenoArtifacts;

public sealed class RandomArtifactSpriteSystem : VisualizerSystem<RandomArtifactSpriteComponent>
{
    protected override void OnAppearanceChange(EntityUid uid, RandomArtifactSpriteComponent component, ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null)
            return;

        if (!AppearanceSystem.TryGetData<int>(uid, SharedArtifactsVisuals.SpriteIndex, out var spriteIndex, args.Component))
            return;

        if (!AppearanceSystem.TryGetData<bool>(uid, SharedArtifactsVisuals.IsUnlocking, out var isUnlocking, args.Component))
            isUnlocking = false;

        if (!AppearanceSystem.TryGetData<bool>(uid, SharedArtifactsVisuals.IsActivated, out var isActivated, args.Component))
            isActivated = false;

        var spriteIndexStr = spriteIndex.ToString("D2");
        var spritePrefix = isUnlocking ? "_on" : "";

        // layered artifact sprite
<<<<<<< HEAD
        if (args.Sprite.LayerMapTryGet(ArtifactsVisualLayers.UnlockingEffect, out var layer))
        {
            var spriteState = "ano" + spriteIndexStr;
            args.Sprite.LayerSetState(ArtifactsVisualLayers.Base, spriteState);
            args.Sprite.LayerSetState(layer, spriteState + "_on");
            args.Sprite.LayerSetVisible(layer, isUnlocking);

            if (args.Sprite.LayerMapTryGet(ArtifactsVisualLayers.ActivationEffect, out var activationEffectLayer))
            {
                args.Sprite.LayerSetState(activationEffectLayer, "artifact-activation");
                args.Sprite.LayerSetVisible(activationEffectLayer, isActivated);
=======
        if (SpriteSystem.LayerMapTryGet((uid, args.Sprite), ArtifactsVisualLayers.UnlockingEffect, out var layer, false))
        {
            var spriteState = "ano" + spriteIndexStr;
            SpriteSystem.LayerSetRsiState((uid, args.Sprite), ArtifactsVisualLayers.Base, spriteState);
            SpriteSystem.LayerSetRsiState((uid, args.Sprite), layer, spriteState + "_on");
            SpriteSystem.LayerSetVisible((uid, args.Sprite), layer, isUnlocking);

            if (SpriteSystem.LayerMapTryGet((uid, args.Sprite), ArtifactsVisualLayers.ActivationEffect, out var activationEffectLayer, false))
            {
                SpriteSystem.LayerSetRsiState((uid, args.Sprite), activationEffectLayer, "artifact-activation");
                SpriteSystem.LayerSetVisible((uid, args.Sprite), activationEffectLayer, isActivated);
>>>>>>> upstream/master
            }
        }
        // non-layered
        else
        {
            var spriteState = "ano" + spriteIndexStr + spritePrefix;
            SpriteSystem.LayerSetRsiState((uid, args.Sprite), ArtifactsVisualLayers.Base, spriteState);
        }
    }
}

public enum ArtifactsVisualLayers : byte
{
    Base,
    UnlockingEffect, // doesn't have to use this
    ActivationEffect
}
