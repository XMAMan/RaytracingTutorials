using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RaytracingTutorials
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            MonteCarloIntegration.Example1();
            MonteCarloIntegration.Example2();
            MonteCarloIntegration.Example3();
            //MonteCarloIntegration.Example234();
        }
    }
}
