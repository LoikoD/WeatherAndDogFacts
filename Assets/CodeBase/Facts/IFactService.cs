using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace CodeBase.Facts
{
    public interface IFactService
    {
        UniTask<List<BreedData>> GetBreeds();
        UniTask<BreedFact> GetBreedFact(string id);
    }
}