﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using IPTVman.Model;
using IPTVman.Helpers;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Net.Sockets;


namespace IPTVman.ViewModel
{
    partial class ViewModelMain : ViewModelBase
    {
        public RelayCommand key_UpdateMDBCommand { get; set; }
        public RelayCommand key_DelDUBLICATCommand { get; set; }
        public RelayCommand key_ReplaceCommand { get; set; }
        public RelayCommand key_OPENclipboarCommand { get; set; }
        public RelayCommand key_SetAllBestCommand { get; set; }
        public RelayCommand key_FILTERmoveDragCommand { get; set; }
        public RelayCommand key_FILTERmoveCommand { get; set; }
        public RelayCommand key_AUTOPINGCommand { get; set; }
        public RelayCommand key_ADDCommand { get; set; }
        public RelayCommand key_OPENCommand { get; set; }
        public RelayCommand key_SAVECommand { get; set; }
        public RelayCommand key_delCommand { get; set; }

        public RelayCommand key_DelFILTERCommand { get; set; }

        public RelayCommand key_DelALLkromeBESTCommand{ get; set; }
        public RelayCommand key_FILTERCommand { get; set; }

        public RelayCommand key_FilterOnlyBESTCommand { get; set; }

        void ini_command()
        {
            key_UpdateMDBCommand = new RelayCommand(Update_MDB);
            key_DelDUBLICATCommand = new RelayCommand(DelDUBLICAT);
            key_ReplaceCommand =  new RelayCommand(key_Replace);
            key_OPENclipboarCommand = new RelayCommand(key_OPEN_clipboard);
            key_SetAllBestCommand = new RelayCommand(key_set_all_best);
            key_FILTERmoveDragCommand = new RelayCommand(key_dragdrop);
            key_FILTERmoveCommand = new RelayCommand(key_move);
            key_AUTOPINGCommand = new RelayCommand(key_AUTOPING);
            key_ADDCommand = new RelayCommand(key_ADD);
            key_OPENCommand = new RelayCommand(key_OPEN);
            key_SAVECommand = new RelayCommand(key_SAVE);
            key_delCommand = new RelayCommand(key_del);
            key_DelFILTERCommand = new RelayCommand(key_delFILTER);
            key_FILTERCommand = new RelayCommand(key_FILTER);
            key_FilterOnlyBESTCommand = new RelayCommand(key_FILTERbest);
            key_DelALLkromeBESTCommand = new RelayCommand(key_delALLkromeBEST);
        }

        void key_dragdrop(object parameter)
        {


        }

        void key_move(object parameter)
        {


        }


        Window winrep;
        /// <summary>
        /// replace
        /// </summary>
        /// <param name="parameter"></param>
        void key_Replace(object parameter)
        {
            if (myLISTbase == null) return;
            if (myLISTbase.Count == 0) return;
            if (winrep != null) return;
            winrep = new WindowReplace
            {
                Title = "ЗАМЕНА",
                Topmost = true,
                WindowStyle = WindowStyle.ToolWindow,
                Name = "winReplace"
            };

            winrep.Closing += Winrep_Closing;
            winrep.Show();
        }

        private void Winrep_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            winrep = null;
        }

        /// <summary>
        /// set all best
        /// </summary>
        /// <param name="parameter"></param>
        void key_set_all_best(object parameter)
        {
            if (myLISTfull == null) return;
            if (data.canal.name == "") return;

            MessageBoxResult result = MessageBox.Show("  Переместить пустые группы в избранное" , " ПЕРЕМЕЩЕНИЕ",
                                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) return;

            int ct=0;
            data.set_best();


            foreach (var s in ViewModelMain.myLISTbase)
            {

                ct = 0;
                foreach (var j in ViewModelMain.myLISTfull)
                {

                    if (j.Compare() == s.Compare() && s.group_title=="")
                    {
                        ViewModelMain.myLISTfull[ct].ExtFilter = data.best1;
                        ViewModelMain.myLISTfull[ct].group_title = data.best2;
                        break;
                    }
                    ct++;
                }  
               
            }

            Update_collection();

        }


        Window ap;
        /// <summary>
        /// AUTO PING
        /// </summary>
        /// <param name="parameter"></param>
        void key_AUTOPING(object parameter)
        {
            if (myLISTbase==null) return;
            if (myLISTbase.Count == 0) return;
            if (ap!=null) return;

            ap = new WindowPING
            {
                Title = "АВТО ПИНГ",
                Topmost = true,
                WindowStyle = WindowStyle.ToolWindow,
                Name = "winPING"
            };

            ap.Closing += Ap_Closing;
            ap.Show();
        }

        private void Ap_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ap = null;
        }



        Window mdb;
        /// <summary>
        /// UPDATE MDB
        /// </summary>
        /// <param name="parameter"></param>
        void Update_MDB(object parameter)
        {
            if (myLISTbase == null) return;
            if (myLISTbase.Count == 0) return;
            if (mdb != null) return;

            mdb = new WindowMDB
            {
                Title = "Обновлние базы Access",
                Topmost = true,
                WindowStyle = WindowStyle.ToolWindow,
                Name = "update_mdb"
            };

            mdb.Closing += MDB_Closing;
            mdb.Show();
        }

        private void MDB_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mdb = null;
        }

        /// <summary>
        ///   ADD
        /// </summary>
        /// <param name="parameter"></param>
        void key_ADD(object parameter)
        {
            CollectionisCreate();
            if (parameter == null) return;
            myLISTfull.Add(new ParamCanal
            { name = parameter.ToString(), ExtFilter = parameter.ToString(), group_title = "" });

            Update_collection();

        }
        
        /// <summary>
        /// DEL
        /// </summary>
        /// <param name="parameter"></param>
        void key_del(object parameter)
        {
            //if (parameter == null || !data.delete) return;
            if (myLISTfull == null) return;
            if (data.canal.name=="") return;

            MessageBoxResult result = MessageBox.Show("  УДАЛЕНИЕ " + data.canal.name + "\n" + data.canal.http, "  УДАЛЕНИЕ",
                                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) return;


            var item = ViewModelMain.myLISTfull.Find(x =>
                  (x.http == data.canal.http && x.ExtFilter == data.canal.ExtFilter 
                      && x.group_title == data.canal.group_title));

            myLISTfull.Remove(item);

            Update_collection();
        }

        /// <summary>
        /// del filter
        /// </summary>
        /// <param name="parameter"></param>
        void key_delFILTER(object parameter)
        {
            if (myLISTfull == null) return;

            MessageBoxResult result = MessageBox.Show("  УДАЛЕНИЕ ВСЕХ ПО ФИЛЬТРУ !!!", "  УДАЛЕНИЕ",
                                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) return;

            uint ct = 0;

            foreach (var obj in myLISTbase)
            {
                var item = ViewModelMain.myLISTfull.Find(x =>
                 (x.http == obj.http && x.ExtFilter == obj.ExtFilter && x.group_title == obj.group_title));

                if (item != null) { myLISTfull.Remove(item);  ct++; }

            }


            Update_collection();
            //MessageBox.Show("  УДАЛЕНО "+ct.ToString()+ " Каналов", " ",
            //                   MessageBoxButton.OK, MessageBoxImage.Information);

        }

        void DelDUBLICAT(object parameter)
        {
            if (myLISTfull == null) return;
 
            MessageBoxResult result = MessageBox.Show("  УДАЛЕНИЕ ДУБЛИКАТОВ name,url,Exfilter !!!", "  УДАЛЕНИЕ",
                                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) return;

            bool first;
            first = false;
            bool del_ok;

            uint index = 0;

            while (1 == 1)
            {
                
                del_ok = false;
                first = false;
                Update_collection();
                foreach (var k in myLISTbase)
                {
                    //MessageBox.Show("СТАРТ "+k.http);
                    first = false;
                    foreach (var j in myLISTbase)
                    {

                        if (k.name == j.name && k.http == j.http && k.ExtFilter == j.ExtFilter)
                        {
                            //MessageBox.Show("нашли " + j.http+"  "+first.ToString());
                            var item = ViewModelMain.myLISTfull.Find(x =>
                             (x.http == k.http && x.ExtFilter == k.ExtFilter && x.name == k.name));

                            if (item != null && first)
                            {
                                first = false;
                                del_ok = true;
                                result = MessageBox.Show(" Найдено дублировние\n Удалить " + item.name + "\n" + item.http +
                                    "\n" + item.ExtFilter, "  УДАЛЕНИЕ",
                                MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                                if (result == MessageBoxResult.Yes) myLISTfull.Remove(item);
                                if (result == MessageBoxResult.Cancel) { Update_collection(); return; }
                                break;
                            }
                            if (item != null && !first) { first = true; }//нахождение самого себя
                        }

                    }
                    index++;
                    if (del_ok) break;
                   

                }

                if (index > myLISTbase.Count) break;
            }

            Update_collection();
            //MessageBox.Show("  УДАЛЕНО "+ct.ToString()+ " Каналов", " ",
            //                   MessageBoxButton.OK, MessageBoxImage.Information);

        }

        /// <summary>
        /// del krome best
        /// </summary>
        /// <param name="parameter"></param>
        void key_delALLkromeBEST(object parameter)
        {
            if (myLISTfull == null) return;

            MessageBoxResult result = MessageBox.Show("  УДАЛЕНИЕ ВСЕХ КРОМЕ ИЗБРАННЫХ(ExtFilter)!!!", "  УДАЛЕНИЕ",
                                MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
            if (result != MessageBoxResult.Yes) return;

            uint ct = 0;

            try
            {
                int i;
                for (i = 0; i < myLISTfull.Count; i++)
                {
                    if (myLISTfull[i].ExtFilter != data.favorite1_1 &&
                        myLISTfull[i].ExtFilter != data.favorite2_1 &&
                        myLISTfull[i].ExtFilter != data.favorite3_1
                        /*|| myLISTfull[i].group_title != data.best2*/)
                    {  myLISTfull.RemoveAt(i); ct++; i--; }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Ошибка удаления = "+ex.Message.ToString(),"",
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }

            Update_collection();
            MessageBox.Show("  УДАЛЕНО " + ct.ToString() + " Каналов", " ",
                               MessageBoxButton.OK, MessageBoxImage.Information);

        }

        
        /// <summary>
        ///  save
        /// </summary>
        /// <param name="parameter"></param>
        void key_SAVE(object parameter)
        {
        
            if (myLISTfull == null) return;
            if (myLISTfull.Count == 0) return;

            SaveFileDialog openFileDialog = new SaveFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                using (StreamWriter sr = new StreamWriter(openFileDialog.FileName))
                {
                    sr.Write(text_title +'\n');

                    string n = "";
                    foreach (var obj in myLISTfull)
                    {
                        n = "";
                       if (text_title == @"#EXTM3U $BorpasFileFormat="+'"'+'1'+'"') n += "#EXTINF:-1";
                       else n += "#EXTINF:0";

                        if (obj.ExtFilter != "") n += " $ExtFilter=" + '"' + obj.ExtFilter + '"';
                        if (obj.group_title != "") n += " group-title=" + '"' + obj.group_title + '"';
                        if (obj.logo != "") n+= " tvg-logo=" + '"' + obj.logo + '"';
                        if (obj.tvg_name != "") n += " tvg-name=" + '"' + obj.tvg_name + '"';

                        n += "," +obj.name + '\n';
                        sr.Write(n+ obj.http + '\n');
                    } 
                }

            }
        }

        /// <summary>
        ///  filter
        /// </summary>
        /// <param name="parameter"></param>
        void key_FILTER(object parameter)
        {
            data.filtr_best = false;
            Update_collection();
        }


      
        void key_FILTERbest(object parameter)
        {
            data.filtr_best = true;
            Update_collection();
        }


        /// <summary>
        /// open from clipboard
        /// </summary>
        /// <param name="parameter"></param>
        void key_OPEN_clipboard(object parameter)
        {
            if (chek1) { MessageBox.Show("ОБНОВЛЕНИЕ ТОЛЬКО ЧЕРЕЗ ФАЙЛ");return; }

            CollectionisCreate();
            string[] str=null;
            string get=Clipboard.GetText();
            try
            {
                 str = get.Split(new Char[] { '\n' });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            Regex regex1 = new Regex("#EXTINF");
            Regex regex2 = new Regex("#EXTM3U");
            Match match = null;

            if (str == null || str.Length<2) { MessageBox.Show("Каналы не найдены"); return; }

            
            //ПОИСК заголовка
            foreach (var s in str)
            {       
                 match = regex2.Match(s);
                if (match.Success) { text_title = s; break; }
            }

            // MessageBox.Show(str.Length.ToString());
            int ct_dublicat = 0;
            int index = 0;
            //ПОИСК каналов
            foreach (var line in str)
            {
                index++;

                string newname = "";
                string str_ex = "", str_name = "", str_http = "", str_gt = "", str_logo = "", str_tvg = "";
                
                    if (line == null || line == "") continue;

                    match = regex1.Match(line);
                    ///========== разбор EXINF
                    if (match.Success)
                    {
                        Regex regex3 = new Regex("ExtFilter=", RegexOptions.IgnoreCase);
                        Regex regex4 = new Regex("group-title=", RegexOptions.IgnoreCase);
                        Regex regex5 = new Regex("logo=", RegexOptions.IgnoreCase);
                        Regex regex6 = new Regex("tvg-name=", RegexOptions.IgnoreCase);

                        string[] split = line.Split(new Char[] { '"' });

                        str_name = "";
                        str_ex = ""; str_name = ""; str_http = ""; str_gt = ""; str_logo = ""; str_tvg = "";

                        if (split.Length == 0)
                        {
                            newname = line;
                        }
                        else
                        {

                        split = line.Split(new Char[] { ',' });

                        if (split.Length < 1) { MessageBox.Show("Каналы не найдены"); return; }

                        if (split.Length <=2) str_name = split[split.Length - 1];

                            //int i = 0;
                            //for (i = 0; i < split.Length; i++)
                            //{
                            //    string s = split[i];
                            //    //------------- разбор строки EXINF
                            //    if (str_ex == "!") str_ex = s;
                            //    if (str_gt == "!") str_gt = s;
                            //    if (str_logo == "!") str_logo = s;
                            //    if (str_tvg == "!") str_tvg = s;


                            //    if (i >= split.Length - 1) { str_name = s; }

                            //    match = regex3.Match(s);
                            //    if (match.Success)
                            //    {
                            //        str_ex = "!";
                            //    }

                            //    match = regex4.Match(s);
                            //    if (match.Success)
                            //    {
                            //        str_gt = "!";
                            //    }

                            //    match = regex5.Match(s);
                            //    if (match.Success)
                            //    {
                            //        str_logo = "!";
                            //    }

                            //    match = regex6.Match(s);
                            //    if (match.Success)
                            //    {
                            //        str_tvg = "!";
                            //    }
                       

                            //}//for

                        if (str_name != "")
                            if (str_name.IndexOf('\r') != -1)
                                newname = str_name.Substring(0, str_name.Length - 1);//remove перевода строки
                            else newname = str_name;
                    }
                    try
                    {
                            str_http = str[index].Substring(0, str[index].Length - 1);//remove перевода строки
                    }
                    catch { }
                       

                    }

                ParamCanal item=null;
                if (newname == "") continue;
              
                   item = ViewModelMain.myLISTfull.Find(x =>
                    (x.http == str_http && x.ExtFilter == str_ex && x.group_title == str_gt));

              // MessageBox.Show(str_http);

                if (newname.Trim() != "" && str_http.Trim() != "")
                {
                        if (item == null)
                        {
                            ViewModelMain.myLISTfull.Add(new ParamCanal
                            {
                                name = newname.Trim(),
                                ExtFilter = str_ex.Trim(),
                                http = str_http.Trim(),
                                group_title = str_gt.Trim(),
                                logo = str_logo.Trim(),
                                tvg_name = str_tvg.Trim()

                            });
                        }
                        else ct_dublicat++;
                }
            }

            RaisePropertyChanged("mycol");///updte LIST!!
            RaisePropertyChanged("numberCANALS");

            if (ct_dublicat != 0) MessageBox.Show("ПРОПУЩЕНО ДУБЛИРОВАННЫХ ССЫЛОК " + ct_dublicat.ToString(), " ",
                                MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
        }

        void key_OPEN(object parameter)
        {
            if (loc.openfile) return;
            loc.openfile = true;
            CollectionisCreate();
            Open();
        }

        public Task<string> AsyncTaskGet()
        {

            return Task.Run(() =>
            {
                //----------------

                PARSING_FILE();
                return "";

                //----------------
            });
        }


        async void Open()
        {
            string rez = await AsyncTaskGet();
        }

        void PARSING_FILE()
        {
            uint ct_dublicat = 0;
            uint ct_update = 0;
            string line = null;
            string newname = "";

            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                if (openFileDialog.ShowDialog() == true)
                {
                    loc.open = true;
                    Regex regex1 = new Regex("#EXTINF");
                    Regex regex2 = new Regex("#EXTM3U");
                    Match match = null;

                    win_loading = true;
                    //ПОИСК заголовка
                    using (StreamReader sr = new StreamReader(openFileDialog.FileName))
                    {
                        string header = "";
                        while (true)
                        {
                            try
                            {
                                header = sr.ReadLine();
                            }
                            catch
                            {
                                MessageBox.Show("Ошибка m3u", "нет заголовка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            }

                            if (header == null) header = "";
                            if (sr.EndOfStream) break;

                            match = regex2.Match(header);
                            if (match.Success) break;
                        }
                        text_title = header;
                    }



                    //ПОИСК каналов
                    using (StreamReader sr = new StreamReader(openFileDialog.FileName))
                    {

                        string str_ex = "", str_name = "", str_http = "", str_gt = "", str_logo = "", str_tvg = "";
                        bool yes = false;

                        while (!sr.EndOfStream)
                        {
                            try
                            {
                                line = sr.ReadLine();
                            }
                            catch { }

                            if (line == null || line == "") continue;


                            match = regex1.Match(line);
                            if (match.Success) yes = true; else yes = false;


                            ///========== разбор EXINF
                            if (yes)
                            {

                                Regex regex3 = new Regex("ExtFilter=", RegexOptions.IgnoreCase);
                                Regex regex4 = new Regex("group-title=", RegexOptions.IgnoreCase);
                                Regex regex5 = new Regex("logo=", RegexOptions.IgnoreCase);
                                Regex regex6 = new Regex("tvg-name=", RegexOptions.IgnoreCase);

                                string[] split = line.Split(new Char[] { '"' });


                                str_ex = ""; str_name = ""; str_http = ""; str_gt = ""; str_logo = ""; str_tvg = "";

                                if (split.Length <= 1)
                                {
                                    split = line.Split(new Char[] { ',' });
                                    str_name = split[split.Length - 1];
                                    newname = str_name;
                                }
                                else
                                {


                                    int i = 0;
                                    for (i = 0; i < split.Length; i++)
                                    {
                                        string s = split[i];
                                        //------------- разбор строки EXINF
                                        if (str_ex == "!") str_ex = s;
                                        if (str_gt == "!") str_gt = s;
                                        if (str_logo == "!") str_logo = s;
                                        if (str_tvg == "!") str_tvg = s;


                                        if (i >= split.Length - 1) { str_name = s; }

                                        match = regex3.Match(s);
                                        if (match.Success)
                                        {
                                            str_ex = "!";
                                        }

                                        match = regex4.Match(s);
                                        if (match.Success)
                                        {
                                            str_gt = "!";
                                        }

                                        match = regex5.Match(s);
                                        if (match.Success)
                                        {
                                            str_logo = "!";
                                        }

                                        match = regex6.Match(s);
                                        if (match.Success)
                                        {
                                            str_tvg = "!";
                                        }
                                    }//foreach

                                    newname = "";
                                    if (str_name != "") newname = str_name.Remove(0, 1);

                                }
                                str_http = sr.ReadLine();

                            }

                            
                            if (!chek1)
                            {

                                var item = ViewModelMain.myLISTfull.Find(x =>
                                (x.http == str_http && x.ExtFilter == str_ex && x.group_title == str_gt));

                                if (newname.Trim() != "" && str_http.Trim() != "")
                                {

                                    if (item == null)
                                    {
                                        ViewModelMain.myLISTfull.Add(new ParamCanal
                                        {
                                            name = newname.Trim(),
                                            ExtFilter = str_ex.Trim(),
                                            http = str_http.Trim(),
                                            group_title = str_gt.Trim(),
                                            logo = str_logo.Trim(),
                                            tvg_name = str_tvg.Trim()

                                        });
                                    }
                                    else ct_dublicat++;
                                }

                            }
                            else//ОБНОВЛЕНИЕ
                            {
                                newname = newname.Trim();
                                newname = newname.Trim();

                                string[] words = newname.Split(new char[] { '(' }, StringSplitOptions.RemoveEmptyEntries);

                                if (words != null)
                                {
                                    if (words.Length != 0)
                                    {
                                        if (words[0] != null && words[0] != "") newname = words[0];
                                    }
                                }

                                newname = newname.Trim();
                                newname = newname.Trim();

                                if (newname != "")
                                {

                                    int index = 0;
                                    bool replace_ok;
                                    replace_ok = false;
                                    foreach (var k in ViewModelMain.myLISTbase)
                                    {
                                        if (newname == k.name)
                                        {
  
                                            int ind = 0;
                                            foreach (var j in ViewModelMain.myLISTfull)
                                            {

                                                if (j.Compare() == k.Compare())
                                                {
                                                    ViewModelMain.myLISTfull[ind].http = str_http;
                                                    
                                                    ct_update++;
                                                    replace_ok = true;
                                                    break;
                                                }
                                                ind++;

                                            }

                                            if (replace_ok) break;
                                        }
                                        index++;

                                    }
                                }

                            }

                        }///чтение фала


                    }

                }

                RaisePropertyChanged("mycol");///updte LIST!!
                RaisePropertyChanged("numberCANALS");
            }
            catch { }

            loc.open = false;
            if (ct_dublicat != 0) MessageBox.Show("ПРОПУЩЕНО ДУБЛИРОВАННЫХ ССЫЛОК " + ct_dublicat.ToString(), " ",
                                MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);

            if (ct_update != 0) MessageBox.Show("ОБНОВЛЕННО " + ct_update.ToString(), " ",
                               MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
            // if (Event_WIN_WAIT != null) Event_WIN_WAIT(2);
            loc.openfile = false;
        }


    }//class

}//namespace
