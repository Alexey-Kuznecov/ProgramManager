using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProgramManager.Resources
{
    class ButtonExtension : Button
    {
        public string Category { get; set; }
        public string IconName { get; set; }

        public event DilCommander Commander;
       
        public delegate void DilCommander();

        public ButtonExtension()
        {

        }

        protected virtual void OnCommander()
        {
            Commander?.Invoke();
            MessageBox.Show("it works!");
        }
    }
}
