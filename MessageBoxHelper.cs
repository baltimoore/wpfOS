using System.Windows;

namespace wpfOs
{
    public static class MessageBoxHelper
    {
        /// <summary>
        /// Displays error messages in a bullet-point format.
        /// Returns true if errors were displayed, false if the error list was empty.
        /// </summary>
        /// <param name="errors">List of error messages to display</param>
        /// <returns>True if errors were displayed, false if the list was empty</returns>
        public static bool ShowError(List<string> errors)
        {
            if (errors.Count == 0)
                return false;

            // first newline needed, because join only adds *between* elements
            string msg = ("• " + string.Join("\n• ", errors));
            MessageBox.Show(
                messageBoxText: msg,
                caption: "Input error",
                icon: MessageBoxImage.Exclamation,
                button: MessageBoxButton.OK
            );
            return true;
        }

        /// <summary>
        /// Displays error messages in a bullet-point format.
        /// Returns true if errors were displayed, false if the error list was empty.
        /// </summary>
        /// <param name="errors">An error message to display</param>
        /// <returns>True if errors were displayed, false if nothing was passed</returns>
        public static bool ShowError(string error)
        {
            if (string.IsNullOrWhiteSpace(error))
                return false;

            List<string> errorList = [error];
            return ShowError(errorList);
        }

        /// <summary>
        /// Displays a success message.
        /// </summary>
        /// <param name="message">Success message to display</param>
        public static void ShowSuccess(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            MessageBox.Show(
                messageBoxText: message,
                caption: "Success!",
                icon: MessageBoxImage.Information,
                button: MessageBoxButton.OK
            );
        }

        /// <summary>
        /// Displays a system error message.
        /// </summary>
        /// <param name="message">Message displayed to user (defaults to "System error!")</param>
        public static void ShowSystemError(string message = "System error!")
        {
            MessageBox.Show(
                messageBoxText: message,
                caption: "System Error",
                icon: MessageBoxImage.Error,
                button: MessageBoxButton.OK
            );
        }
    }
}

