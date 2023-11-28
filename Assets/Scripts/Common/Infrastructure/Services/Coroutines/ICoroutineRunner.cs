using System.Collections;
using UnityEngine;

namespace Common.Infrastructure.Services.Coroutines
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutineSafe(Coroutine coroutine);
    }
}