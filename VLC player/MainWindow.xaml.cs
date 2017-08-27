﻿using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using Declarations;
using Declarations.Events;
using Declarations.Media;
using Declarations.Players;
using Implementation;
using System.Threading.Tasks;

namespace IPTVman.ViewModel
{
    static class data
    {
       public static string url="";
       public static string name="";
    }
    /// <summary>
    /// nVLC lib Player
    /// </summary>
    public partial class Player : Window
    {
        IMediaPlayerFactory m_factory;
        IVideoPlayer m_player;
        IMedia m_media;
        System.Windows.Forms.Panel p;

        public Player()
        {
            InitializeComponent();
            init();
            slider2.Value = 100;

            reset();

            //use a timer to periodically update the memory usage
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += timer_Tick;
            timer.Start();

            slider2.Focus();
            Thread.Sleep(100);
            Play(data.url);
        }
        void init()
        {
            try
            {
                p = new System.Windows.Forms.Panel();
                p.BackColor = System.Drawing.Color.Black;
                windowsFormsHost1.Child = p;

                m_factory = new MediaPlayerFactory(true);
                m_player = m_factory.CreatePlayer<IVideoPlayer>();

                this.DataContext = m_player;

                //m_player.Events.PlayerPositionChanged += new EventHandler<MediaPlayerPositionChanged>(Events_PlayerPositionChanged);
                //m_player.Events.TimeChanged += new EventHandler<MediaPlayerTimeChanged>(Events_TimeChanged);
                //m_player.Events.MediaEnded += new EventHandler(Events_MediaEnded);
                //m_player.Events.PlayerStopped += new EventHandler(Events_PlayerStopped);

                m_player.WindowHandle = p.Handle;
            }
            catch (Exception ex) { System.Windows.Forms.MessageBox.Show("Ошибка библиотеки vlc "+ex.Message); }
        }

        string play_link;
        Task taskPLAY;
        public void Play(string link)
        {
            taskPLAY = Task.Factory.StartNew(() =>
            {
                try
                {
                    if (m_player == null)
                    {
                        init();
                    }
                    if (m_player.IsPlaying) m_player.Stop();
                    if (play_link != link) m_player.Stop();
                    while (m_player.IsPlaying) Thread.Sleep(100);

                    play_link = link;
                    m_media = m_factory.CreateMedia<IMedia>(link);

                    // m_media.Events.DurationChanged += new EventHandler<MediaDurationChange>(Events_DurationChanged);
                    // m_media.Events.StateChanged += new EventHandler<MediaStateChange>(Events_StateChanged);

                    m_player.Open(m_media);
                    m_media.Parse(true);
                    reset();
                    m_player.Play();
                }
                catch (Exception ex) { System.Windows.Forms.MessageBox.Show("error vlc " + ex.Message); }
            });
        }

        void reset()
        {
            tick = 0; i = 0;
        }
        uint tick = 0;
        static byte i = 0;
        bool updatename=false;
        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (m_player == null) return;

                if (!updatename)
                {
                    if (m_player.IsPlaying)
                    {
                        this.Title = data.name +" "+ data.url;
                        reset();
                        updatename = true;
                    }
                }
                if (!m_player.IsPlaying)
                {
                    updatename = false;
                    if (taskPLAY.IsFaulted || taskPLAY.IsCanceled)
                    { this.Title = "STOPPED!!!"; return; }
                    string s = "Opening";
                    if (i == 0)
                        this.Title = s + " ... ";
                    if (i < 5) this.Title = s + " .   " + tick.ToString();
                    if (i > 5 && i < 10) this.Title = s + " . .   " + tick.ToString();
                    if (i > 10 && i < 15) this.Title = s + " . . .   " + tick.ToString();
                    if (i > 15 && i < 20) this.Title = s + " . . . . . . .   " + tick.ToString();
                    i++;
                    if (i > 20) i = 1;
                    tick++;
                }
            }
            catch { }
        }

        void Events_PlayerStopped(object sender, EventArgs e)
        {
            //this.Dispatcher.BeginInvoke(new Action(delegate
            //{
            //    InitControls();
            //}));
        }

        void Events_MediaEnded(object sender, EventArgs e)
        {
            //this.Dispatcher.BeginInvoke(new Action(delegate
            //{
            //    InitControls();
            //}));
        }

        private void InitControls()
        {

        }

        void Events_TimeChanged(object sender, MediaPlayerTimeChanged e)
        {
            //this.Dispatcher.BeginInvoke(new Action(delegate
            //{
            //    //label1.Content = TimeSpan.FromMilliseconds(e.NewTime).ToString().Substring(0, 8);
            //}));
        }

        void Events_PlayerPositionChanged(object sender, MediaPlayerPositionChanged e)
        {

        }

        void Events_StateChanged(object sender, MediaStateChange e)
        {

        }

        void Events_DurationChanged(object sender, MediaDurationChange e)
        {

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //m_player.Pause();
        }
        private void button3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            // m_player.Stop();
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            m_player.ToggleMute();
        }

        private void slider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (m_player != null)
            {
                m_player.Volume = (int)e.NewValue;
            }
        }

        private void slider1_DragCompleted(object sender, DragCompletedEventArgs e)
        {
        }

        private void slider1_DragStarted(object sender, DragStartedEventArgs e)
        {
        }


        private void Window_Closing(object sender, CancelEventArgs e)
        {
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (m_player != null)
            {
                m_player.Stop();
                //while (m_player.IsPlaying) Thread.Sleep(100);
            }
        }

        private void Window_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {

        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void Window_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void windowsFormsHost1_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void windowsFormsHost1_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void windowsFormsHost1_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }

        private void windowsFormsHost1_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {


        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }
    }
}