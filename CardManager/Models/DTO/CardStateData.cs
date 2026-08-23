namespace CardManager.Models.DTO;

public sealed record CardStateData(
    string InstanceId,
    int Level,
    int Experience,
    int Rank,
    bool IsFavourite,
    IReadOnlyList<string> Slots
);