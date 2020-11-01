using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JAMDL
{
    public partial class ThemeCreator : Form
    {
        public ThemeCreator()
        {
            InitializeComponent();
        }

        private void MainHelpPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    pictureBox2.BackgroundImage = JAMDL.Properties.Resources.nutral;
                    break;
                case 1:
                    pictureBox2.BackgroundImage = JAMDL.Properties.Resources.excited;
                    break;
                case 2:
                    pictureBox2.BackgroundImage = JAMDL.Properties.Resources.sad;
                    break;
                case 3:
                    pictureBox2.BackgroundImage = JAMDL.Properties.Resources.eyy;
                    break;
                case 4:
                    pictureBox2.BackgroundImage = JAMDL.Properties.Resources.bored;
                    break;
                case 5:
                    pictureBox2.BackgroundImage = JAMDL.Properties.Resources.jesus;
                    break;
                case 6:
                    pictureBox2.BackgroundImage = JAMDL.Properties.Resources.yt;
                    break;
                case 7:
                    pictureBox2.BackgroundImage = JAMDL.Properties.Resources.soundc;
                    break;
                case 8:
                    pictureBox2.BackgroundImage = JAMDL.Properties.Resources.reddit;
                    break;
                case 9:
                    pictureBox2.BackgroundImage = JAMDL.Properties.Resources.dailym;
                    break;
                case 10:
                    pictureBox2.BackgroundImage = JAMDL.Properties.Resources.bandcamp;
                    break;
                case 11:
                    pictureBox2.BackgroundImage = JAMDL.Properties.Resources.vimeo;
                    break;
                case 12:
                    pictureBox2.BackgroundImage = JAMDL.Properties.Resources.twitter;
                    break;
                case 13:
                    pictureBox2.BackgroundImage = JAMDL.Properties.Resources.error;
                    break;
                case 14:
                    pictureBox2.BackgroundImage = JAMDL.Properties.Resources.nutral;
                    break;
                case 15:
                    pictureBox2.BackgroundImage = JAMDL.Properties.Resources.happy;
                    break;
                case 16:
                    pictureBox2.BackgroundImage = JAMDL.Properties.Resources.cry;
                    break;
                case 17:
                    pictureBox2.BackgroundImage = JAMDL.Properties.Resources.downLeft;
                    break;
            }
        }
    }
}
