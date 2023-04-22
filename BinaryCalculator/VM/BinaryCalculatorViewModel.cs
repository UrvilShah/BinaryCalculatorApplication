using BinaryCalculator.Command.Handler;
using BinaryCalculator.Constant;
using BinaryCalculator.Extension;
using BinaryCalculator.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BinaryCalculator.VM
{
    public class BinaryCalculatorViewModel : INotifyPropertyChanged
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private ICommand _calculatorCommand;

        public ICommand CalculatorCommand
        {
            get
            {
                if (_calculatorCommand == null)
                    _calculatorCommand = new CommandHandler(cp => Execute(cp), b => CanExecute(b));
                return _calculatorCommand;
            }
            set
            {
                _calculatorCommand = value;
            }
        }

        private void Execute(Object o)
        {
            try
            {
                String s = o.ToString();

                if (StringConstant.Zero.Equals(s) || StringConstant.One.Equals(s))
                {
                    HandleDigitPress(s);
                }
                else if (StringConstant.Plus.Equals(s) || StringConstant.Minus.Equals(s))
                {
                    HandleOperatorPress(s);
                }
                else if (StringConstant.CE.Equals(s))
                {
                    HandleBackspacePress(s);
                }
                else if (StringConstant.C.Equals(s))
                {
                    HandleClearPress(s);
                }
                else if (StringConstant.EqualOperator.Equals(s))
                {
                    DoBinaryOperation(s);
                }
            }
            catch(Exception ex)
            {
                String errorMessage = $"Exception while handling the Execute method. Exception: {ex.GetExceptionDetail()}";
                _logger.Error(errorMessage);
                new UIHelper().ShowErrorMessage(errorMessage);
            }
            finally
            {
                DisplayTextFocus = false;
                DisplayTextFocus = true;
            }
        }

        private bool CanExecute(Object o)
        {
            try
            {
                if(!String.IsNullOrWhiteSpace(InputText))
                {
                    int plusCount = InputText.Count(c => c.Equals(CharConstant.Plus));
                    int minusCount = InputText.Count(c => c.Equals(CharConstant.Minus));

                    if(plusCount + minusCount > 1)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                String errorMessage = $"Exception while handling the Execute method. Exception: {ex.GetExceptionDetail()}";
                _logger.Error(errorMessage);
                new UIHelper().ShowErrorMessage(errorMessage);
                return false;
            }
        }

        #region Private Methods

        private void DoBinaryOperation(String s)
        {
            try
            {                
                int num1 = 0;
                int num2 = 0;
                String op = String.Empty;

                if(!String.IsNullOrWhiteSpace(InputText))
                {
                    if (InputText.IndexOf(StringConstant.Plus) > 0)
                    {
                        op = StringConstant.Plus;
                    }
                    else if (InputText.IndexOf(StringConstant.Minus) > 0)
                    {
                        op = StringConstant.Minus;
                    }
                    else
                    {
                        return;
                    }

                    if (InputText.IndexOf(StringConstant.EqualOperator) > 0)
                    {
                        num1 = Convert.ToInt32(DisplayText, 2);
                        InputText = InputText.Substring(InputText.IndexOf(op) + 1);
                        InputText = InputText.Substring(0, InputText.Length - 1);
                        num2 = Convert.ToInt32(InputText, 2);
                        InputText = Convert.ToString(num1, 2) + op + Convert.ToString(num2, 2) + StringConstant.EqualOperator;
                    }
                    else
                    {
                        num1 = Convert.ToInt32(InputText.Substring(0, InputText.IndexOf(op)), 2);
                        num2 = Convert.ToInt32(InputText.Substring(InputText.IndexOf(op) + 1), 2);
                        InputText += StringConstant.EqualOperator;
                    }
                }

                DisplayText = StringConstant.Plus.Equals(op) ? Convert.ToString(num1 + num2, 2) : Convert.ToString(num1 - num2, 2);
            }
            catch (Exception ex)
            {
                String errorMessage = $"Exception while doing binary operation press. Exception: {ex.GetExceptionDetail()}";
                _logger.Error(errorMessage);
                new UIHelper().ShowErrorMessage(errorMessage);
            }
        }

        private void HandleDigitPress(String s)
        {
            try
            {
                if(!String.IsNullOrWhiteSpace(InputText))
                {
                    char lastChar = InputText[InputText.Length - 1];

                    if(CharConstant.EqualOperator.Equals(lastChar))
                    {
                        DisplayText = String.Empty;
                        InputText = String.Empty;
                    }
                }

                DisplayText += s;
                InputText += s;                
            }
            catch (Exception ex)
            {
                String errorMessage = $"Exception while handling digit press. Exception: {ex.GetExceptionDetail()}";
                _logger.Error(errorMessage);
                new UIHelper().ShowErrorMessage(errorMessage);
            }
        }

        private void HandleOperatorPress(String s)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(InputText))
                {
                    char lastChar = InputText[InputText.Length - 1];

                    if (CharConstant.EqualOperator.Equals(lastChar))
                    {
                        return;
                    }
                }

                DisplayText = String.Empty;
                InputText += s;
            }
            catch (Exception ex)
            {
                String errorMessage = $"Exception while handling operator press. Exception: {ex.GetExceptionDetail()}";
                _logger.Error(errorMessage);
                new UIHelper().ShowErrorMessage(errorMessage);
            }
        }

        private void HandleBackspacePress(String s)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(InputText) && !String.IsNullOrWhiteSpace(DisplayText))
                {
                    DisplayText = DisplayText.Substring(0, DisplayText.Length - 1);
                    InputText = InputText.Substring(0, InputText.Length - 1);
                }
                else if (!String.IsNullOrWhiteSpace(InputText) && String.IsNullOrWhiteSpace(DisplayText))
                {
                    char lastChar = InputText[InputText.Length - 1];

                    String prevNum = String.Empty;

                    if (CharConstant.Plus.Equals(lastChar) || CharConstant.Minus.Equals(lastChar))
                    {
                        prevNum = InputText.Substring(0, InputText.Length - 1);
                    }

                    if(prevNum.IndexOf(StringConstant.Plus) > 0)
                    {
                        prevNum = prevNum.Substring(prevNum.IndexOf(StringConstant.Plus) + 1);
                    }
                    else if (prevNum.IndexOf(StringConstant.Minus) > 0)
                    {
                        prevNum = prevNum.Substring(prevNum.IndexOf(StringConstant.Minus) + 1);
                    }

                    DisplayText = prevNum;
                    InputText = InputText.Substring(0, InputText.Length - 1);
                }    
            }
            catch (Exception ex)
            {
                String errorMessage = $"Exception while handling backspace press. Exception: {ex.GetExceptionDetail()}";
                _logger.Error(errorMessage);
                new UIHelper().ShowErrorMessage(errorMessage);
            }
        }

        private void HandleClearPress(String s)
        {
            try
            {
                DisplayText = String.Empty;
                InputText = String.Empty;
            }
            catch (Exception ex)
            {
                String errorMessage = $"Exception while handling backspace press. Exception: {ex.GetExceptionDetail()}";
                _logger.Error(errorMessage);
                new UIHelper().ShowErrorMessage(errorMessage);
            }
        }

        #endregion

        #region DisplayText

        private string _displayText;

        public string DisplayText
        {
            get
            {
                return _displayText;
            }
            set
            {
                _displayText = value;
                OnPropertyChanges(nameof(DisplayText));
            }
        }        

        #endregion

        #region InputText

        private string _inputText;

        public string InputText
        {
            get
            {
                return _inputText;
            }
            set
            {
                _inputText = value;
                OnPropertyChanges(nameof(InputText));
            }
        }

        #endregion

        #region DisplayTextFocus

        private bool _displayTextFocus;

        public bool DisplayTextFocus
        {
            get
            {
                return _displayTextFocus;
            }
            set
            {
                _displayTextFocus = value;
                OnPropertyChanges(nameof(DisplayTextFocus));
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanges([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
