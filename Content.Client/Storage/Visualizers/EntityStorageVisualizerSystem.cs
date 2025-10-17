<<<<<<< HEAD
using Content.Shared.SprayPainter.Prototypes; // Upstream#37341
=======
using Content.Shared.SprayPainter.Prototypes;
>>>>>>> upstream/master
using Content.Shared.Storage;
using Robust.Client.GameObjects;
using Robust.Shared.Prototypes;

namespace Content.Client.Storage.Visualizers;

public sealed class EntityStorageVisualizerSystem : VisualizerSystem<EntityStorageVisualsComponent>
{
<<<<<<< HEAD
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!; // Upstream#37341
    [Dependency] private readonly IComponentFactory _componentFactory = default!; // Upstream#37341
=======
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IComponentFactory _componentFactory = default!;
>>>>>>> upstream/master

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<EntityStorageVisualsComponent, ComponentInit>(OnComponentInit);
    }

    /// <summary>
    /// Sets the base sprite to this layer. Exists to make the inheritance tree less boilerplate-y.
    /// </summary>
    private void OnComponentInit(EntityUid uid, EntityStorageVisualsComponent comp, ComponentInit args)
    {
        if (comp.StateBaseClosed == null)
            return;

        comp.StateBaseOpen ??= comp.StateBaseClosed;
        if (!TryComp<SpriteComponent>(uid, out var sprite))
            return;

        SpriteSystem.LayerSetRsiState((uid, sprite), StorageVisualLayers.Base, comp.StateBaseClosed);
    }

<<<<<<< HEAD
    // Upstream#37341
=======
>>>>>>> upstream/master
    protected override void OnAppearanceChange(EntityUid uid,
        EntityStorageVisualsComponent comp,
        ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null
            || !AppearanceSystem.TryGetData<bool>(uid, StorageVisuals.Open, out var open, args.Component))
            return;

        var forceRedrawBase = false;
        if (AppearanceSystem.TryGetData<string>(uid, PaintableVisuals.Prototype, out var prototype, args.Component))
        {
            if (_prototypeManager.TryIndex(prototype, out var proto))
            {
                if (proto.TryGetComponent(out SpriteComponent? sprite, _componentFactory))
                {
<<<<<<< HEAD
                    args.Sprite.BaseRSI = sprite.BaseRSI;
=======
                    SpriteSystem.SetBaseRsi((uid, args.Sprite), sprite.BaseRSI);
>>>>>>> upstream/master
                }
                if (proto.TryGetComponent(out EntityStorageVisualsComponent? visuals, _componentFactory))
                {
                    comp.StateBaseOpen = visuals.StateBaseOpen;
                    comp.StateBaseClosed = visuals.StateBaseClosed;
                    comp.StateDoorOpen = visuals.StateDoorOpen;
                    comp.StateDoorClosed = visuals.StateDoorClosed;
                    forceRedrawBase = true;
                }
            }
        }
<<<<<<< HEAD
        // End Upstream#37341

        // Open/Closed state for the storage entity.
        if (args.Sprite.LayerMapTryGet(StorageVisualLayers.Door, out _, false)) // Upstream#37341
=======

        // Open/Closed state for the storage entity.
        if (args.Sprite.LayerMapTryGet(StorageVisualLayers.Door, out _, false))
>>>>>>> upstream/master
        {
            if (open)
            {
                if (comp.OpenDrawDepth != null)
                    args.Sprite.DrawDepth = comp.OpenDrawDepth.Value;

                if (comp.StateDoorOpen != null)
                {
                    args.Sprite.LayerSetState(StorageVisualLayers.Door, comp.StateDoorOpen);
                    args.Sprite.LayerSetVisible(StorageVisualLayers.Door, true);
                }
                else
                {
                    args.Sprite.LayerSetVisible(StorageVisualLayers.Door, false);
                }

                if (comp.StateBaseOpen != null)
<<<<<<< HEAD
                    args.Sprite.LayerSetState(StorageVisualLayers.Base, comp.StateBaseOpen);
                else if (forceRedrawBase && comp.StateBaseClosed != null) // Upstream#37341
                    args.Sprite.LayerSetState(StorageVisualLayers.Base, comp.StateBaseClosed); // Upstream#37341
=======
                    SpriteSystem.LayerSetRsiState((uid, args.Sprite), StorageVisualLayers.Base, comp.StateBaseOpen);
                else if (forceRedrawBase && comp.StateBaseClosed != null)
                    SpriteSystem.LayerSetRsiState((uid, args.Sprite), StorageVisualLayers.Base, comp.StateBaseClosed);
>>>>>>> upstream/master
            }
            else
            {
                if (comp.ClosedDrawDepth != null)
                    args.Sprite.DrawDepth = comp.ClosedDrawDepth.Value;

                if (comp.StateDoorClosed != null)
                {
                    args.Sprite.LayerSetState(StorageVisualLayers.Door, comp.StateDoorClosed);
                    args.Sprite.LayerSetVisible(StorageVisualLayers.Door, true);
                }
                else
                    args.Sprite.LayerSetVisible(StorageVisualLayers.Door, false);

                if (comp.StateBaseClosed != null)
<<<<<<< HEAD
                    args.Sprite.LayerSetState(StorageVisualLayers.Base, comp.StateBaseClosed);
                else if (forceRedrawBase && comp.StateBaseOpen != null) // Upstream#37341
                    args.Sprite.LayerSetState(StorageVisualLayers.Base, comp.StateBaseOpen); // Upstream#37341
=======
                    SpriteSystem.LayerSetRsiState((uid, args.Sprite), StorageVisualLayers.Base, comp.StateBaseClosed);
                else if (forceRedrawBase && comp.StateBaseOpen != null)
                    SpriteSystem.LayerSetRsiState((uid, args.Sprite), StorageVisualLayers.Base, comp.StateBaseOpen);
>>>>>>> upstream/master
            }
        }
    }
}

public enum StorageVisualLayers : byte
{
    Base,
    Door
}
