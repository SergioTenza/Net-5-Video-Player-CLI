using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PVS.MediaPlayer;

namespace VideoPlayer
{
    public partial class Form1 : Form
    {
        private int y;
        private int x;
        private Size size;
        private int width;
        private int height;
        private string filepath;
        private Player myPlayer;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Player.MFPresent) 
            {
                MessageBox.Show(Player.MFPresent_ResultString);
                Environment.Exit(0);
            }
            if (Program.argumentos != null)
            {
                if (Program.argumentos.Count() > 0)
                {
                    List<string> comandos = new List<string>();
                    foreach (var item in Program.argumentos)
                    {
                        comandos.Add(item);
                    }
                    if (comandos.Count > 0)
                    {
                        x = Convert.ToInt32(comandos[0]);
                        y = Convert.ToInt32(comandos[1]);
                        width = Convert.ToInt32(comandos[2]);
                        height = Convert.ToInt32(comandos[3]);
                        size = new Size(width, height);
                        filepath = comandos[4];
                        this.Location = new Point(x, y);
                        this.ClientSize = size;
                        if (File.Exists(filepath))
                        {
                            myPlayer = new Player();
                            myPlayer.Events.MediaEnded += MyPlayer_MediaEnded;
                            myPlayer.Play(filepath, panel1);
                            return;
                        }
                    }
                    MessageBox.Show("Ha ocurrido un problema con los argumentos.");
                    Environment.Exit(0);
                }
                MessageBox.Show("No se han proporcionado argumentos de ejecucion.\n [ X ] [ Y ] [ WIDTH ] [ HEIGTH ] [ RUTA_VIDEO ]");
                Environment.Exit(0);
            }
            
        }

        private void MyPlayer_MediaEnded(object sender, EndedEventArgs e)
        {
            switch (e.StopReason)
            {
                case StopReason.Finished:
                    myPlayer.Play(filepath, panel1);
                    // media has finished playing
                    break;

                case StopReason.AutoStop:
                    myPlayer.Play(filepath, panel1);
                    // media is stopped by the player to play next media
                    break;

                case StopReason.UserStop:
                    // media is stopped with the Player.Stop method
                    break;

                case StopReason.Error:
                    if (myPlayer != null)
                    {
                        myPlayer.Stop();
                        myPlayer.Dispose();
                        myPlayer = null;
                    }
                    MessageBox.Show("No se puede reproducir el video.");
                    // media has stopped because an error has occurred
                    break;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (myPlayer != null)
            {
                myPlayer.Stop();
                myPlayer.Dispose();
                myPlayer = null;
            }
        }
    }
}
