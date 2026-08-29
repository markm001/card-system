using CardManager.Models;

namespace CardManager.Repositories;

public interface ICardStatRepository
{
    IReadOnlyDictionary<string, IReadOnlyList<StatBlock>> Stats { get; }
    
    StatBlock GetStats(string statCurveId, int level);
}