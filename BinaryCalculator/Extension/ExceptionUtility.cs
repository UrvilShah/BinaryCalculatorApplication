using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BinaryCalculator.Extension
{
    public static class ExceptionUtility
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static String GetExceptionDetail(this Exception e)
        {
            try
            {
                StringBuilder errorMessage = new StringBuilder(e.Message);

                errorMessage.AppendLine();

                if (e.InnerException != null)
                {
                    errorMessage.AppendLine("Inner Exception: " + e.InnerException.Message);
                }
                if (e.StackTrace != null)
                {
                    errorMessage.AppendLine("Stack Trace: " + e.StackTrace);
                }
                return errorMessage.ToString();
            }
            catch (Exception ex)
            {
                _logger.Error(String.Format("Original Exception before calling GetExceptionDetail(). Exception Message: {0}. StackTrace {1}", e.Message, e.StackTrace));
                _logger.Error(String.Format("Exception while calling GetExceptionDetail(). Exception Message: {0}. StackTrace {1}", ex.Message, ex.StackTrace));
                return "Exception while calling GetExceptionDetail()";
            }

        }
    }
}
