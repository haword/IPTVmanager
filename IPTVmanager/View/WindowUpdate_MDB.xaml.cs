﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace IPTVman.ViewModel
{
   
    /// <summary>
    /// Логика взаимодействия для Window
    /// </summary>
    public partial class WindowMDB : Window 
    {
        System.Timers.Timer Timer1;
        
        public WindowMDB()
        {
            InitializeComponent();
            CreateTimer1(5000);
            textBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            textBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            textBox.Text = "";
            Access.Event_Print += Access_Event_Print;

            TEXTmask.Text = IPTVman.Model.bd_data.s1;
            TEXT1.Text = IPTVman.Model.bd_data.s2;
            TEXT2.Text = IPTVman.Model.bd_data.s3;
        }

        private void Access_Event_Print(string obj)
        {
            update_block(obj);
        }

        static UInt16 ct=0;

        private void clear()
        {
            textBox.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                textBox.Text = "";
            }));
        }
        private void update_block(string text)
        {

            if (text == "") clear();

            textBox.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                ct++; if (ct > 500) clear();
                textBox.Text += text;
            }));

        }

        public void CreateTimer1(int ms)
        {
            if (Timer1 == null)
            {
                Timer1 = new System.Timers.Timer();
                //Timer1.AutoReset = false; //
                Timer1.Interval = ms;
                Timer1.Elapsed += Timer1Tick;
                Timer1.Enabled = true;
                Timer1.Start();
            }
        }

        private void Timer1Tick(object source, System.Timers.ElapsedEventArgs e)
        {

        }

        //closing
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ViewModelWindowMDB._bd = null;
            IPTVman.Model.bd_data.s1 = TEXTmask.Text;
            IPTVman.Model.bd_data.s2 = TEXT1.Text;
            IPTVman.Model.bd_data.s3 = TEXT2.Text;
        }

        //key ЗАКРЫТЬ
        private void exit_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (this != null) this.Close();
        }
    }
}