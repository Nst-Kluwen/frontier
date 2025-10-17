using Content.Shared.EntityTable.EntitySelectors;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;

namespace Content.Shared.EntityTable.Conditions;

/// <summary>
/// Used for implementing conditional logic for <see cref="EntityTableSelector"/>.
/// </summary>
[ImplicitDataDefinitionForInheritors, UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract partial class EntityTableCondition
{
    /// <summary>
    /// If true, inverts the result of the condition.
    /// </summary>
    [DataField]
    public bool Invert;

<<<<<<< HEAD
    public bool Evaluate(IEntityManager entMan, IPrototypeManager proto)
    {
        var res = EvaluateImplementation(entMan, proto);
=======
    public bool Evaluate(EntityTableSelector root, IEntityManager entMan, IPrototypeManager proto, EntityTableContext ctx)
    {
        var res = EvaluateImplementation(root, entMan, proto, ctx);
>>>>>>> upstream/master

        // XOR eval to invert the result.
        return res ^ Invert;
    }

<<<<<<< HEAD
    public abstract bool EvaluateImplementation(IEntityManager entMan, IPrototypeManager proto);
=======
    protected abstract bool EvaluateImplementation(EntityTableSelector root, IEntityManager entMan, IPrototypeManager proto, EntityTableContext ctx);
>>>>>>> upstream/master
}
