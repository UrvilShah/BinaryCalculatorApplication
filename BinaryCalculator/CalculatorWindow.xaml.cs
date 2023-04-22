using BinaryCalculator.Constant;
using BinaryCalculator.Extension;
using BinaryCalculator.Helper;
using BinaryCalculator.VM;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BinaryCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public MainWindow()
        {
            InitializeComponent();
            XmlConfigurator.Configure();
            DataContext = new BinaryCalculatorViewModel();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                txtDisplay.Focus();
                Keyboard.Focus(txtDisplay);
            }
            catch (Exception ex)
            {
                String errorMessage = $"Exception while handling the widow load event. Exception: {ex.GetExceptionDetail()}";
                _logger.Error(errorMessage);
                new UIHelper().ShowErrorMessage(errorMessage);
            }
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int plusCount = lblInput.Text.Count(c => c.Equals(CharConstant.Plus));
            int minusCount = lblInput.Text.Count(c => c.Equals(CharConstant.Minus));

            bool isValidOperation = (
                                         StringConstant.Plus.Equals(e.Text) ||
                                         StringConstant.Minus.Equals(e.Text) ||
                                         StringConstant.One.Equals(e.Text) ||
                                         StringConstant.Zero.Equals(e.Text) ||
                                         StringConstant.C.Equals(e.Text) ||
                                         StringConstant.CE.Equals(e.Text) 
                                    ) && (plusCount + minusCount) <= 1;

            e.Handled = !isValidOperation;
        }
    }
}
