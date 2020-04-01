using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomControlCommandSample
{
    // very simple splitbutton with no visual states
    public class SplitButton : ContentControl
    {
        public readonly static RoutedEvent ClickEvent;

        public static readonly DependencyProperty DropDownContentProperty;
        public static readonly DependencyProperty CommandProperty;
        public static readonly DependencyProperty CommandParameterProperty;
        public static readonly DependencyProperty CommandTargetProperty;
        private Button button;
        private EventHandler canExecuteChangedHandler;
        private bool canExecute;

        static SplitButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitButton), new FrameworkPropertyMetadata(typeof(SplitButton)));

            CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(SplitButton), new PropertyMetadata(OnCommandChanged));
            CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(SplitButton), new PropertyMetadata(null));
            CommandTargetProperty = DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(SplitButton), new PropertyMetadata(null));
            DropDownContentProperty = DependencyProperty.Register("DropDownContent", typeof(object), typeof(SplitButton));

            CommandManager.RegisterClassCommandBinding(typeof(SplitButton), new CommandBinding(SplitButtonCommands.FirstCommand, new ExecutedRoutedEventHandler(OnExecuteFirstCommand), new CanExecuteRoutedEventHandler(OnCanExecuteFirstCommand)));

            ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SplitButton));
        }

        public object DropDownContent
        {
            get { return (object)GetValue(DropDownContentProperty); }
            set { SetValue(DropDownContentProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public IInputElement CommandTarget
        {
            get { return (IInputElement)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }

        protected override bool IsEnabledCore
        {
            get { return this.canExecute; }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.button = GetTemplateChild("PART_Button") as Button;
            this.button.Click += Button_Click;
        }

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SplitButton splitButton = d as SplitButton;
            splitButton.OnCommandChanged((ICommand)e.OldValue, (ICommand)e.NewValue);
        }

        private void OnCommandChanged(ICommand oldCommand, ICommand newCommand)
        {
            if (oldCommand != null)
            {
                oldCommand.CanExecuteChanged -= canExecuteChangedHandler;
            }

            if (newCommand != null)
            {
                this.canExecuteChangedHandler = new EventHandler(CanExecuteChanged);
                newCommand.CanExecuteChanged += canExecuteChangedHandler;
            }

            CanExecuteCommand();
        }

        private static void OnCanExecuteFirstCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private static void OnExecuteFirstCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SplitButton splitButton = sender as SplitButton;
            if (splitButton != null)
            {
                // do something
                MessageBox.Show((string)e.Parameter, "SplitButton RoutedCommand");

                splitButton.Focus();
            }
        }

        private void CanExecuteChanged(object sender, EventArgs e)
        {
            CanExecuteCommand();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnClick();
        }

        protected virtual bool CanExecuteCommand()
        {
            bool canExecute = false;
            if (Command != null)
            {
                RoutedCommand routedCommand = Command as RoutedCommand;
                if (routedCommand != null)
                {
                    IInputElement commandTarget = CommandTarget;
                    if (CommandTarget == null)
                        CommandTarget = this;

                    canExecute = routedCommand.CanExecute(CommandParameter, commandTarget);
                }
                else
                {
                    canExecute = Command.CanExecute(CommandParameter);
                }

                this.canExecute = canExecute;
                CoerceValue(UIElement.IsEnabledProperty);
            }
            return canExecute;
        }

        protected virtual void ExecuteCommand()
        {
            if (Command != null)
            {
                RoutedCommand routedCommand = Command as RoutedCommand;
                if (routedCommand != null)
                {
                    IInputElement commandTarget = CommandTarget;
                    if (CommandTarget == null)
                        CommandTarget = this;

                    routedCommand.Execute(CommandParameter, commandTarget);
                }
                else
                {
                    Command.Execute(CommandParameter);
                }
            }
        }

        protected virtual void OnClick()
        {
            RaiseClick();
            ExecuteCommand();
        }

        private void RaiseClick()
        {
            RaiseEvent(new RoutedEventArgs(SplitButton.ClickEvent));
        }

        public event EventHandler<RoutedEventArgs> Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }
    }
}
