<<<<<<< HEAD
// SPDX-FileCopyrightText: 2025 Ark
// SPDX-FileCopyrightText: 2025 ark1368
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Numerics;
using Content.Shared._Goobstation.Vehicles;
using Content.Shared._NF.Radar;
using Content.Shared.Projectiles;
using Content.Shared.Shuttles.Components;

namespace Content.Server._NF.Radar;

public sealed partial class RadarBlipSystem : EntitySystem
{
    [Dependency] private readonly SharedTransformSystem _xform = default!;
=======
using System.Numerics;
using Content.Shared._Goobstation.Vehicles;
using Content.Shared._NF.Radar;
using Content.Shared.GameTicking;
using Content.Shared.Movement.Components;
using Content.Shared.Shuttles.Components;
using Robust.Shared.Network;
using Robust.Shared.Timing;

namespace Content.Server._NF.Radar;

/// <summary>
/// A system that handles and rate-limits client-made requests for radar blips.
/// </summary>
/// <remarks>
/// Ported from Monolith's RadarBlipsSystem.
/// </remarks>
public sealed partial class RadarBlipSystem : SharedRadarBlipSystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly SharedTransformSystem _xform = default!;

    private Dictionary<NetUserId, TimeSpan> _nextBlipRequestPerUser = new();

    // The minimum amount of time between handled blip requests.
    private static readonly TimeSpan MinRequestPeriod = TimeSpan.FromSeconds(1);
    // Maximum distance for blips to be considered visible
    private const float MaxBlipRenderDistance = 300f;

>>>>>>> upstream/master
    public override void Initialize()
    {
        base.Initialize();
        SubscribeNetworkEvent<RequestBlipsEvent>(OnBlipsRequested);
<<<<<<< HEAD
    }

=======

        SubscribeLocalEvent<RoundRestartCleanupEvent>(OnRoundRestart);
    }

    /// <summary>
    /// Handles a network request for radar blips and sends the blip data to the requesting client.
    /// </summary>
>>>>>>> upstream/master
    private void OnBlipsRequested(RequestBlipsEvent ev, EntitySessionEventArgs args)
    {
        if (!TryGetEntity(ev.Radar, out var radarUid))
            return;

        if (!TryComp<RadarConsoleComponent>(radarUid, out var radar))
            return;

<<<<<<< HEAD
        var blips = AssembleBlipsReport((EntityUid)radarUid, radar);
        var hitscans = AssembleHitscanReport((EntityUid)radarUid, radar);

        // Combine the blips and hitscan lines
        var giveEv = new GiveBlipsEvent(blips, hitscans);
        RaiseNetworkEvent(giveEv, args.SenderSession);
    }

    private List<(NetEntity? Grid, Vector2 Position, float Scale, Color Color, RadarBlipShape Shape)> AssembleBlipsReport(EntityUid uid, RadarConsoleComponent? component = null)
    {
        var blips = new List<(NetEntity? Grid, Vector2 Position, float Scale, Color Color, RadarBlipShape Shape)>();

        if (Resolve(uid, ref component))
        {
            var radarXform = Transform(uid);
            var radarPosition = _xform.GetWorldPosition(uid);
            var radarGrid = _xform.GetGrid(uid);
            var radarMapId = radarXform.MapID;

            // Check if the radar is on an FTL map
            var isFtlMap = HasComp<FTLComponent>(radarXform.GridUid);

            var blipQuery = EntityQueryEnumerator<RadarBlipComponent, TransformComponent>();

            while (blipQuery.MoveNext(out var blipUid, out var blip, out var blipXform))
            {
                if (!blip.Enabled)
                    continue;

                // Don't show radar blips for projectiles on different maps than the one they were fired from
                if (TryComp<ProjectileComponent>(blipUid, out var projectile))
                {
                    // If the projectile is on a different map than the radar, don't show it
                    if (blipXform.MapID != radarMapId)
                        continue;

                    // If we can determine the shooter and they're on a different map, don't show the blip
                    if (projectile.Shooter != null &&
                        TryComp<TransformComponent>(projectile.Shooter, out var shooterXform) &&
                        shooterXform.MapID != blipXform.MapID)
                        continue;
                }

                // This prevents blips from showing on radars that are on different maps
                if (blipXform.MapID != radarMapId)
                    continue;

                var blipGrid = _xform.GetGrid(blipUid);

                // if (HasComp<CircularShieldRadarComponent>(blipUid))
                // {
                //     // Skip if in FTL
                //     if (isFtlMap)
                //         continue;
                //
                //     // Skip if no grid
                //     if (blipGrid == null)
                //         continue;
                //
                //     // Ensure the grid is a valid MapGrid
                //     if (!HasComp<MapGridComponent>(blipGrid.Value))
                //         continue;
                //
                //     // Ensure the shield is a direct child of the grid
                //     if (blipXform.ParentUid != blipGrid)
                //         continue;
                // }

                var blipPosition = _xform.GetWorldPosition(blipUid);
                var distance = (blipPosition - radarPosition).Length();
                if (distance > component.MaxRange)
                    continue;

                if (blip.RequireNoGrid)
                {
                    if (blipGrid != null)
                        continue;

                    // For free-floating blips without a grid, use world position with null grid
                    blips.Add((null, blipPosition, blip.Scale, blip.RadarColor, blip.Shape));
                }
                else if (blip.VisibleFromOtherGrids)
                {
                    // For blips that should be visible from other grids, add them regardless of grid
                    // If on a grid, use grid-relative coordinates
                    if (blipGrid != null)
                    {
                        // Local position relative to grid
                        var gridMatrix = _xform.GetWorldMatrix(blipGrid.Value);
                        Matrix3x2.Invert(gridMatrix, out var invGridMatrix);
                        var localPos = Vector2.Transform(blipPosition, invGridMatrix);

                        // Add grid-relative blip with grid entity ID
                        blips.Add((GetNetEntity(blipGrid.Value), localPos, blip.Scale, blip.RadarColor, blip.Shape));
                    }
                    else
                    {
                        // Fallback to world position with null grid
                        blips.Add((null, blipPosition, blip.Scale, blip.RadarColor, blip.Shape));
                    }
                }
                else
                {
                    // If we're requiring grid, make sure they're on the same grid
                    if (blipGrid != radarGrid)
                        continue;

                    // For grid-aligned blips, store grid NetEntity and grid-local position
                    if (blipGrid != null)
                    {
                        // Local position relative to grid
                        var gridMatrix = _xform.GetWorldMatrix(blipGrid.Value);
                        Matrix3x2.Invert(gridMatrix, out var invGridMatrix);
                        var localPos = Vector2.Transform(blipPosition, invGridMatrix);

                        // Add grid-relative blip with grid entity ID
                        blips.Add((GetNetEntity(blipGrid.Value), localPos, blip.Scale, blip.RadarColor, blip.Shape));
                    }
                    else
                    {
                        // Fallback to world position with null grid
                        blips.Add((null, blipPosition, blip.Scale, blip.RadarColor, blip.Shape));
                    }
                }
            }
        }

        return blips;
    }

    /// <summary>
    /// Assembles trajectory information for hitscan projectiles to be displayed on radar
    /// </summary>
    private List<(NetEntity? Grid, Vector2 Start, Vector2 End, float Thickness, Color Color)> AssembleHitscanReport(EntityUid uid, RadarConsoleComponent? component = null)
    {
        var hitscans = new List<(NetEntity? Grid, Vector2 Start, Vector2 End, float Thickness, Color Color)>();

        if (!Resolve(uid, ref component))
            return hitscans;

        var radarXform = Transform(uid);
        var radarPosition = _xform.GetWorldPosition(uid);
        var radarGrid = _xform.GetGrid(uid);
        var radarMapId = radarXform.MapID;

        var hitscanQuery = EntityQueryEnumerator<HitscanRadarComponent>();

        while (hitscanQuery.MoveNext(out var hitscanUid, out var hitscan))
        {
            if (!hitscan.Enabled)
                continue;

            // Check if either the start or end point is within radar range
            var startDistance = (hitscan.StartPosition - radarPosition).Length();
            var endDistance = (hitscan.EndPosition - radarPosition).Length();

            if (startDistance > component.MaxRange && endDistance > component.MaxRange)
                continue;

            // If there's an origin grid, use that for coordinate system
            if (hitscan.OriginGrid != null && hitscan.OriginGrid.Value.IsValid())
            {
                var gridUid = hitscan.OriginGrid.Value;

                // Convert world positions to grid-local coordinates
                var gridMatrix = _xform.GetWorldMatrix(gridUid);
                Matrix3x2.Invert(gridMatrix, out var invGridMatrix);

                var localStart = Vector2.Transform(hitscan.StartPosition, invGridMatrix);
                var localEnd = Vector2.Transform(hitscan.EndPosition, invGridMatrix);

                hitscans.Add((GetNetEntity(gridUid), localStart, localEnd, hitscan.LineThickness, hitscan.RadarColor));
            }
            else
            {
                // Use world coordinates with null grid
                hitscans.Add((null, hitscan.StartPosition, hitscan.EndPosition, hitscan.LineThickness, hitscan.RadarColor));
            }
        }

        return hitscans;
    }
=======
        if (_nextBlipRequestPerUser.TryGetValue(args.SenderSession.UserId, out var requestTime) && _timing.RealTime < requestTime)
            return;

        _nextBlipRequestPerUser[args.SenderSession.UserId] = _timing.RealTime + MinRequestPeriod;

        var blips = AssembleBlipsReport((radarUid.Value, radar));

        var giveEv = new GiveBlipsEvent(blips);
        RaiseNetworkEvent(giveEv, args.SenderSession);
    }

    /// <summary>
    /// Clears blip request data between rounds.
    /// </summary>
    public void OnRoundRestart(RoundRestartCleanupEvent ev)
    {
        _nextBlipRequestPerUser.Clear();
    }

    /// <summary>
    /// Assembles a list of radar blips visible to the given radar console.
    /// </summary>
    private List<(NetEntity? Grid, Vector2 Position, float Scale, Color Color, RadarBlipShape Shape)> AssembleBlipsReport(Entity<RadarConsoleComponent> ent)
    {
        var blips = new List<(NetEntity? Grid, Vector2 Position, float Scale, Color Color, RadarBlipShape Shape)>();

        if (!TryComp(ent, out TransformComponent? radarXform))
            return blips;
        var radarPosition = _xform.GetWorldPosition(ent);
        var radarGrid = radarXform.GridUid;
        var radarMapId = radarXform.MapID;
        var radarRange = MathF.Min(ent.Comp.MaxRange, MaxBlipRenderDistance);

        // Non-positive range, nothing to return.
        if (radarRange <= 0)
            return blips;

        var blipQuery = EntityQueryEnumerator<RadarBlipComponent, TransformComponent>();

        while (blipQuery.MoveNext(out var blipUid, out var blip, out var blipXform))
        {
            if (!blip.Enabled)
            {
                Log.Debug($"Blip {blipUid} skipped: not enabled.");
                continue;
            }

            if (blipXform.MapID != radarMapId)
            {
                Log.Debug($"Blip {blipUid} skipped: different map.");
                continue;
            }

            // Run cheaper grid checks before distance checks
            var blipGrid = blipXform.GridUid;
            if (blip.RequireNoGrid && blipGrid != null)
            {
                Log.Debug($"Blip {blipUid} skipped: has grid but requires none.");
                continue;
            }

            if (!blip.VisibleFromOtherGrids && blipGrid != radarGrid)
            {
                Log.Debug($"Blip {blipUid} skipped: not on same grid as radar.");
                continue;
            }

            var blipPosition = _xform.GetWorldPosition(blipUid);
            var distance = (blipPosition - radarPosition).Length();
            if (distance > radarRange)
            {
                Log.Debug($"Blip {blipUid} skipped: out of range.");
                continue;
            }

            // Convert blip position to grid coords if needed.
            NetEntity? blipNetGrid = null;
            if (blipGrid != null)
            {
                blipNetGrid = GetNetEntity(blipGrid.Value);
                blipPosition = Vector2.Transform(blipPosition, _xform.GetInvWorldMatrix(blipGrid.Value));
            }
            blips.Add((blipNetGrid, blipPosition, blip.Scale, blip.RadarColor, blip.Shape));
        }
        return blips;
    }

>>>>>>> upstream/master
    /// <summary>
    /// Configures the radar blip for a jetpack or vehicle entity.
    /// </summary>
    private void SetupRadarBlip(EntityUid uid, Color color, float scale, bool visibleFromOtherGrids = true, bool requireNoGrid = false)
    {
        var blip = EnsureComp<RadarBlipComponent>(uid);
        blip.RadarColor = color;
        blip.Scale = scale;
        blip.VisibleFromOtherGrids = visibleFromOtherGrids;
        blip.RequireNoGrid = requireNoGrid;
    }

    /// <summary>
    /// Configures the radar blip for a vehicle entity.
    /// </summary>
    public void SetupVehicleRadarBlip(Entity<VehicleComponent> uid)
    {
        SetupRadarBlip(uid, Color.Cyan, 1f, true, true);
    }
}
