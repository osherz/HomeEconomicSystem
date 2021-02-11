using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.BL
{
    [Serializable]
    public class FailedBecauseNotEmptyException : Exception
    {
        public FailedBecauseNotEmptyException() { }
        public FailedBecauseNotEmptyException(string message) : base(message) { }
        public FailedBecauseNotEmptyException(string message, Exception inner) : base(message, inner) { }
        protected FailedBecauseNotEmptyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
