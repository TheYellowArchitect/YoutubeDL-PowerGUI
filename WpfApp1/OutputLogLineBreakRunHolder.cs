using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace WpfApp1
{
    /// <summary>
    /// Contains just a LineBreak, and a Run
    /// And isPercentETA, to detect if this, is % ETA [download] spam
    /// </summary>
    public struct OutputLogLineBreakRunHolder
    {
        public LineBreak lineBreakElement;
        public Run runElement;
        public bool isPercentETA;
    }
}
