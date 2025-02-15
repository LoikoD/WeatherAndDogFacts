using CodeBase.Core;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Facts
{
    public class FactService : IFactService
    {
        private const string BreedsUrl = "https://dogapi.dog/api/v2/breeds";
        private const string BreedFactUrl = "https://dogapi.dog/api/v2/breeds/{0}";

        private readonly IRequestQueue _requestQueue;

        public FactService(IRequestQueue requestQueue)
        {
            _requestQueue = requestQueue;
        }

        public async UniTask<List<BreedData>> GetBreeds()
        {
            return await _requestQueue.EnqueueRequest(BreedsUrl, ParseBreeds);
        }

        public async UniTask<BreedFact> GetBreedFact(string id)
        {
            string url = string.Format(BreedFactUrl, id);
            return await _requestQueue.EnqueueRequest(url, ParseBreedFact);
        }

        private List<BreedData> ParseBreeds(string json)
        {
            DogBreed[] breeds = JsonUtility.FromJson<BreedsApiResponse>(json).data;

            List<BreedData> breedsData = new();
            foreach (DogBreed breed in breeds)
            {
                breedsData.Add(new BreedData(breed.id, breed.attributes.name));
            }
            return breedsData;
        }

        private BreedFact ParseBreedFact(string json)
        {
            BreedDetailAttributes breedDetailAttributes = JsonUtility.FromJson<BreedDetailResponse>(json).data.attributes;

            return new BreedFact(breedDetailAttributes.name, breedDetailAttributes.description);
        }
    }
}