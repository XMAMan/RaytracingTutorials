﻿using System;
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

            this.BackgroundImage = new SimpleRenderer(this.ClientSize.Width, this.ClientSize.Height).CreateBitmap();
        }
    }
}
