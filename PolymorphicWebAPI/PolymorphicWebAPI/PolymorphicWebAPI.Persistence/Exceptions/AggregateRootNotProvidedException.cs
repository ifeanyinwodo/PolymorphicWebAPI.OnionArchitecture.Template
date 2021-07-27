using System;
using System.Security.Permissions;

namespace PolymorphicWebAPI.Persistence.Exceptions
{
    //Exception Implementation
    [Serializable]public class AggregateRootNotProvidedException: Exception
    {   
        public AggregateRootNotProvidedException(string message): base(message)
        {

        }
    }


    [Serializable] public class IdException : Exception
    {
        public IdException(string message) : base(message)
        {

        }
    }
}
