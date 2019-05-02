using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARWT.Commands;

namespace ARWT.ViewModel
{
    public abstract class WindowBaseModel : ViewModelBase
    {
        private bool m_Close;
        private WindowExitResult m_ExitResult = WindowExitResult.Notset;

        private ActionCommandWithParameter _ClosingCommand;
        public ActionCommandWithParameter ClosingCommand
        {
            get
            {
                return _ClosingCommand ?? (_ClosingCommand = new ActionCommandWithParameter()
                {
                    ExecuteAction = WindowClosing
                });
            }
        }

        public bool Close
        {
            get
            {
                return m_Close;
            }
            set
            {
                if (Equals(m_Close, value))
                {
                    return;
                }

                m_Close = value;

                NotifyPropertyChanged();
            }
        }

        public WindowExitResult ExitResult
        {
            get
            {
                return m_ExitResult;
            }
            protected set
            {
                m_ExitResult = value;
            }
        }

        protected void CloseWindow()
        {
            Close = true;
        }

        private void WindowClosing(object param)
        {
            CancelEventArgs args = param as CancelEventArgs;

            if (args == null)
            {
                return;
            }

            WindowClosing(args);
        }

        protected virtual void WindowClosing(CancelEventArgs closingEventArgs)
        {

        }
    }

    public enum WindowExitResult
    {
        Notset,
        Ok,
        Cancel,
    }
}
