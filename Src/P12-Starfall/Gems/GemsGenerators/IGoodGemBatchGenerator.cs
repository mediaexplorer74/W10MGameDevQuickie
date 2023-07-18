using GameManager.Gems;
using System.Collections.Generic;

namespace GameManager.Gems.GemsGenerators
{
    public interface IGoodGemBatchGenerator
    {
        IEnumerable<GoodGem> GenerateGems();
    }
}
