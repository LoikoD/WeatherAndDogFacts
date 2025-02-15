using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace CodeBase.Core
{
    public class RequestQueue : IRequestQueue
    {
        private readonly Queue<Func<UniTask>> _queue = new();
        private bool _isProcessing;
        private CancellationTokenSource _cancellationTokenSource = new();

        public async UniTask<T> EnqueueRequest<T>(string url, Func<string, T> parser)
        {
            UniTaskCompletionSource<T> tcs = new();

            _queue.Enqueue(async () =>
            {
                try
                {
                    var response =  await SendRequest(url);
                    T result = parser(response);
                    tcs.TrySetResult(result);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Request error: {ex.Message}");
                    tcs.TrySetException(ex);
                }
                finally
                {
                    await ProcessNext();
                }
            });

            if (!_isProcessing)
                await ProcessNext();

            return await tcs.Task;
        }

        public async UniTask<Texture2D> EnqueueTextureRequest(string url)
        {

            UniTaskCompletionSource<Texture2D> tcs = new();

            _queue.Enqueue(async () =>
            {
                try
                {
                    Texture2D result = await SendTextureRequest(url);
                    tcs.TrySetResult(result);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Request error: {ex.Message}");
                    tcs.TrySetException(ex);
                }
                finally
                {
                    await ProcessNext();
                }
            });

            if (!_isProcessing)
                await ProcessNext();

            return await tcs.Task;
        }

        public void CancelAllRequests()
        {
            _queue.Clear();
            _isProcessing = false;

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private async UniTask<string> SendRequest(string url)
        {
            using var request = UnityWebRequest.Get(url);
            await request.SendWebRequest().WithCancellation(_cancellationTokenSource.Token);

            if (request.result != UnityWebRequest.Result.Success)
                throw new Exception(request.error);

            return request.downloadHandler.text;
        }
        private async UniTask<Texture2D> SendTextureRequest(string url)
        {
            using var request = UnityWebRequestTexture.GetTexture(url);
            await request.SendWebRequest().WithCancellation(_cancellationTokenSource.Token);

            if (request.result != UnityWebRequest.Result.Success)
                throw new Exception(request.error);

            return ((DownloadHandlerTexture)request.downloadHandler).texture;
        }

        private async UniTask ProcessNext()
        {
            if (_queue.Count == 0)
            {
                _isProcessing = false;
                return;
            }

            _isProcessing = true;
            await _queue.Dequeue().Invoke();
        }
    }
}