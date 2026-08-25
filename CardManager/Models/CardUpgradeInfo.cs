using LevelManager.Core.Models;

namespace CardManager.Models;

public record CardUpgradeInfo(
    int CurrentLevel,
    int MaxLevel,
    int CurrentExperience,
    int MaxExperienceForLevel,
    IReadOnlyList<ResourceCost> MaterialCost
);