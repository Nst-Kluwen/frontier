using Robust.Shared.GameStates;
<<<<<<< HEAD
=======
using Robust.Shared.Audio;
>>>>>>> upstream/master

namespace Content.Shared._NF.GridAccess;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class GridAccessComponent : Component
{
    /// <summary>
    /// Frontier - Grid access
    /// The uid to which this device is limited to be used on.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? LinkedShuttleUid = null;
<<<<<<< HEAD
=======

    [DataField]
    public SoundSpecifier ErrorSound =
        new SoundPathSpecifier("/Audio/Effects/Cargo/buzz_sigh.ogg");

    [DataField]
    public SoundSpecifier SwipeSound =
        new SoundPathSpecifier("/Audio/Machines/id_swipe.ogg");

    [DataField]
    public SoundSpecifier InsertSound =
        new SoundPathSpecifier("/Audio/Machines/id_insert.ogg");
>>>>>>> upstream/master
}
