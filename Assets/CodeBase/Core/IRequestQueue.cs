using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace CodeBase.Core
{
    public interface IRequestQueue
    {
        UniTask<T> EnqueueRequest<T>(string url, Func<string, T> parser);
        UniTask<Texture2D> EnqueueTextureRequest(string url);
        void CancelAllRequests();
    }
}