using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;


namespace EoF_Configuration_Manager.Helpers.CommonPrompts
{
    public static class YesNoDialog
    {

        public static MessageBoxResult PromptForDeletion()
        {
            return MessageBox.Show("Are you sure you wish to delete this entry?", "Remove Entry", MessageBoxButton.YesNo);
        }

    }
}
