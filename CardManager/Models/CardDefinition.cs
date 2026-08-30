namespace CardManager.Models;

public sealed record CardDefinition(
    string Id,
    string Name,
    Rarity Rarity,
    Element Element,
    Slot Slot,
    string LevelCurveId,
    string CostCurveId,
    string StatCurveId
);