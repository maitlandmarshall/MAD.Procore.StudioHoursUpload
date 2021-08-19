using System;
using System.Runtime.Serialization;

namespace MAD.Procore.StudioHoursUpload.Jobs
{
    [Serializable]
    internal class StudioProjectMappingNotFoundException : Exception
    {
        public StudioProjectMappingNotFoundException()
        {
        }

        public StudioProjectMappingNotFoundException(string message) : base(message)
        {
        }

        public StudioProjectMappingNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StudioProjectMappingNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}