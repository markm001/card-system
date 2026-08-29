namespace CardManager.Models.DTO;

public sealed record StatCurveData(
    string Id,
    IReadOnlyList<StatBlockData> Stats
);

public sealed record StatBlockData(
    int Level,
    long ATK,
    long DEF,
    long HP,
    long DEX
);