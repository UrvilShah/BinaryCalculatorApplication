using BinaryCalculator.Constant;
using BinaryCalculator.Extension;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BinaryCalculator.Helper
{
    public class UIHelper
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void ShowErrorMessage(String message)
        {
            try
            {
                MessageBox.Show(message, StringConstant.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception while showing error message dialog. Exception: {ex.GetExceptionDetail()}");
                throw;
            }
        }
    }
}
