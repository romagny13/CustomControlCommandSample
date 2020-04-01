using System.Windows.Input;

namespace CustomControlCommandSample
{
    public static class SplitButtonCommands
    {
        private static RoutedUICommand firstCommand;

        public static RoutedUICommand FirstCommand
        {
            get
            {
                if (SplitButtonCommands.firstCommand == null)
                {
                    SplitButtonCommands.firstCommand = new RoutedUICommand("FirstCommand", "FirstCommand", typeof(SplitButtonCommands));
                }
                return SplitButtonCommands.firstCommand;
            }
        }
    }
}
