using System;
using System.Collections.Generic;
using System.Text;

namespace Recognizer.Adapter.Infrastructure.RequestResponse
{
    public class RecognizerResponse
    {
        public string ExecutionId { get; set; }

        public string FinishTime { get; set; }
        public string StartTime { get; set; }
        public Result Result { get; set; }

    }
}
