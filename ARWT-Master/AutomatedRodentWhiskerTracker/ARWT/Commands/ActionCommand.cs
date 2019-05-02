﻿using System;
using System.Windows.Input;

namespace ARWT.Commands
{
    public class ActionCommand : ICommand
    {
        private Action m_ExecuteAction;
        private Func<bool> m_CanExecuteAction = () => true;

        public event EventHandler CanExecuteChanged;

        public Action ExecuteAction
        {
            get
            {
                return m_ExecuteAction;
            }
            set
            {
                m_ExecuteAction = value;
            }
        }

        public Func<bool> CanExecuteAction
        {
            get
            {
                return m_CanExecuteAction;
            }
            set
            {
                m_CanExecuteAction = value;

                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, EventArgs.Empty);
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteAction();
        }

        public void Execute(object parameter)
        {
            ExecuteAction();
        }

        public void RaiseCanExecuteChangedNotification()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
