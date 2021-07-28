using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo
{
    public partial class CTextBox : Component
    {
        public CTextBox()
        {
            InitializeComponent();
        }

        public CTextBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
