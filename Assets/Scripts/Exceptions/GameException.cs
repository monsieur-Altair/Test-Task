using System;
using UnityEngine;

namespace Exceptions
{
    public class GameException : Exception
    {
        public GameException(string message)
        {
            Debug.LogError(message);
        }
    }
}