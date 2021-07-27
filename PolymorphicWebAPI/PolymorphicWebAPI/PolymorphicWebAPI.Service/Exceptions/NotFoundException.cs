using System;

namespace PolymorphicWebAPI.Service.Exceptions
{
    //Exception Implementation
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}