using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public struct CustomButtonStruct
    {
        public HorizontalAlignment buttonHorizontalAlignment;//Center
        public VerticalAlignment buttonVerticalAlignment;//Top
        public Thickness buttonMargin;//20,0,0,0
        public double buttonWidth;//75
        public string buttonStringContent;//Add Custom

        public StackPanel buttonParent;
    }
}
