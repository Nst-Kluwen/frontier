using Robust.Shared.Serialization;

namespace Content.Shared.Delivery;

[Serializable, NetSerializable]
public enum DeliveryVisuals : byte
{
    IsLocked,
    IsTrash,
    IsBroken,
    IsFragile,
<<<<<<< HEAD
=======
    IsBomb,
>>>>>>> upstream/master
    PriorityState,
    JobIcon,
}

[Serializable, NetSerializable]
public enum DeliveryPriorityState : byte
{
    Off,
    Active,
    Inactive,
}

[Serializable, NetSerializable]
<<<<<<< HEAD
=======
public enum DeliveryBombState : byte
{
    Off,
    Inactive,
    Primed,
}

[Serializable, NetSerializable]
>>>>>>> upstream/master
public enum DeliverySpawnerVisuals : byte
{
    Contents,
}
