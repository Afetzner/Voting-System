using System;

namespace VotingSystem.Utils
{
    //Grabbed from microsoft documentation https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/exceptions/creating-and-throwing-exceptions 
    [Serializable]
    public class InvalidBuilderParameterException : Exception
    {
        public InvalidBuilderParameterException() : base() { }
        public InvalidBuilderParameterException(string message) : base(message) { }
        public InvalidBuilderParameterException(string message, Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected InvalidBuilderParameterException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
