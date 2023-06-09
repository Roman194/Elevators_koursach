﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elevators_koursach
{
    public partial class Form1 : Form
    {
        int floors = 9;

        int t = 2880;
        double P = 0.0;
        double P1 = 0.0;
        List<List<double>> Counters = new List<List<double>>();
        List<double> Counters1 = new List<double>();
        List<int> Intervals = new List<int>();
        List<int> Intervals1 = new List<int>();
        double Summ_counters;
        double Summ_counters1;
        Random rand = new Random();
        List<List<Passenger>> awaitings = new List<List<Passenger>>();
        bool[] elevators_direction = new bool[2];
        List<Button> floor_button = new List<Button>();
        List<int> elevator1_passengers = new List<int>();
        List<int> elevator2_passengers = new List<int>();
        int elevators_Capasity = 8;
        List<int> calls = new List<int>();
        List<int> calls_el1 = new List<int>();
        List<int> calls_el2 = new List<int>();
        List<int> special_calls = new List<int>();
        int special_call_for_el1 = 0;
        int special_call_for_el2 = 0;
        bool special_call_already_accept = false;
        uint summ_waiting_time = 0;
        uint[] average_waiting_time= new uint[2];

        uint max_waiting_time = 0;
        int system_time = 1;
        int system_on_stop_time;
        bool generation_stopped = false;

        bool checker_2 = true;
        bool moving = true;

        bool research_started = false;
        bool research_ended=false;

        public Form1()
        {
            InitializeComponent();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!research_started)
            {
                research_started = true;
                floors = Convert.ToInt32(numericUpDown4.Value);
                elevators_Capasity = Convert.ToInt32(numericUpDown3.Value);
                for (int i = 0; i < floors - 1; i++)
                {
                    Counters.Add(new List<double>());
                }
                trackBar1.Maximum = floors - 1;
                trackBar2.Maximum = floors - 1;


                double lamda = Convert.ToDouble(numericUpDown1.Value);
                double lamda1 = Convert.ToDouble(numericUpDown2.Value);
                double L = Math.Exp(-1 * lamda);
                double L1 = Math.Exp(-1 * lamda1);

                int factorial = 1;
                textBox1.Text = "Data\r\n";
                for (int i = 0; ; i++)
                {

                    P = (L * Math.Pow(lamda, i)) / factorial;

                    Counters[0].Add(Math.Round(t * P));
                    if (Counters[0][i] == 0)
                        break;

                    Summ_counters += Counters[0][i];

                    if (i > 0)
                        Intervals.Add(Intervals[i - 1] + Convert.ToInt32(Counters[0][i]));
               
                    else
                        Intervals.Add(Convert.ToInt32(Counters[0][i]));

                    factorial *= (i + 1);
                    textBox1.Text += "P: " + Convert.ToString(P) + " Cnt: " + Convert.ToString(Counters[0][i] + " Int: " + Convert.ToString(Intervals[i]) + "\r\n");
                }
                if (Summ_counters < t)
                {
                    Counters[0][1] += (t - Summ_counters);
                    textBox1.Text += "Поправка \r\n" + "Cnt1: " + Convert.ToString(Counters[0][1]) + " ";
                    for (int j = 1; j < Intervals.Count; j++)
                    {
                        Intervals[j] += Convert.ToInt32((t - Summ_counters));
                        textBox1.Text += "Int: " + Convert.ToString(Intervals[j]) + "\r\n";
                    }
                    Summ_counters += (t - Summ_counters);
                }
                else
                {
                    if (Summ_counters > t)
                    {
                        Counters[0][1] -= (Summ_counters - t);
                        textBox1.Text += "Поправка \r\n" + "Cnt1: " + Convert.ToString(Counters[0][1]) + " ";
                        for (int j = 1; j < Intervals.Count; j++)
                        {
                            Intervals[j] -= Convert.ToInt32((Summ_counters - t));
                            textBox1.Text += "Int: " + Convert.ToString(Intervals[j]) + "\r\n";
                        }
                        Summ_counters -= (Summ_counters - t);
                    }
                }
                textBox1.Text += "Summ: " + Convert.ToString(Summ_counters) + "\r\n";

                factorial = 1;
                for (int i = 0; ; i++)
                {

                    P1 = (L1 * Math.Pow(lamda1, i)) / factorial;
                    Counters1.Add(Math.Round(t * P1));
                    if (Counters1[i] == 0)
                        break;
                    Summ_counters1 += Counters1[i];
                    if (i > 0)
                        Intervals1.Add(Intervals1[i - 1] + Convert.ToInt32(Counters1[i]));
                    
                    else
                        Intervals1.Add(Convert.ToInt32(Counters1[i]));
                    
                    factorial *= (i + 1);
                    textBox1.Text += "P: " + Convert.ToString(P1) + " Cnt: " + Convert.ToString(Counters1[i] + " Int: " + Convert.ToString(Intervals1[i]) + "\r\n");

                }

                if (Summ_counters1 < t)
                {
                    Counters1[1] += (t - Summ_counters1);
                    textBox1.Text += "Поправка \r\n" + "Cnt1: " + Convert.ToString(Counters1[1]) + " ";
                    for (int j = 1; j < Intervals1.Count; j++)
                    {
                        Intervals1[j] += Convert.ToInt32((t - Summ_counters1));
                        textBox1.Text += "Int: " + Convert.ToString(Intervals1[j]) + "\r\n";
                    }
                    Summ_counters1 += (t - Summ_counters1);
                }
                else
                {
                    if (Summ_counters1 > t)
                    {
                        Counters1[1] -= (Summ_counters1 - t);
                        textBox1.Text += "Поправка \r\n" + "Cnt1: " + Convert.ToString(Counters1[1]) + " ";
                        for (int j = 1; j < Intervals1.Count; j++)
                        {
                            Intervals1[j] -= Convert.ToInt32((Summ_counters1 - t));
                            textBox1.Text += "Int: " + Convert.ToString(Intervals1[j]) + "\r\n";
                        }
                        Summ_counters1 -= (Summ_counters1 - t);
                    }
                }
                textBox1.Text += "Summ: " + Convert.ToString(Summ_counters1) + "\r\n";

                for (int z = 1; z < floors - 1; z++)
                {
                    for (int u = 0; u < Counters[0].Count - 1; u++)
                    {
                        Counters[z].Add(Counters[0][u]);
                    }

                }

                for (int i = 0; i < floors; i++)
                {
                    awaitings.Add(new List<Passenger>());
                    floor_button.Add(new Button(false, 0));
                }
                elevators_direction[0] = false;
                elevators_direction[1] = true;
                trackBar1.Maximum = trackBar2.Maximum = floors - 1;

                timer1.Start();
                timer2.Start();
                timer3.Start();
                label18.Text = "";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)

        {
            label1.Text = "Новые пассажиры:\r\n";
            int now = rand.Next(1, t);
            int destination = 0;
            label1.Text += "1: ";

            for (int i = 0; i < Counters1.Count-1; i++) //генерация пассажиров на первом 
            {
                if (now <= Intervals1[i] && Counters1[i] > 0)
                {
                    if ((floor_button[0].State == false) && (i != 0))
                    {
                        floor_button[0].State = true;
                        calls.Add(0);
                    }

                    label1.Text += Convert.ToString(i) + ": ";
                    Counters1[i]--;
                    for (int j = 0; j < i; j++)
                    {
                        destination = rand.Next(2, floors + 1);
                        awaitings[0].Add(new Passenger(destination,0));
                        label1.Text += Convert.ToString(destination) + ", ";
                    }
                    label1.Text += "Осталось: " + Convert.ToString(Counters1[i]) + "\r\n";
                    break;
                }
            }





            for (int z = 0; z < floors - 1; z++)//генерация пассажиров не на первом
            {

                now = rand.Next(1, t);
                destination = 1;
                label1.Text += Convert.ToString(z + 2) + ": ";
                for (int i = 0; i < Counters[0].Count-1; i++)
                {
                    if (now <= Intervals[i] && Counters[z][i] > 0)
                    {
                        if ((floor_button[z + 1].State == false) && (i != 0) )
                        {
                            floor_button[z + 1].State = true;
                            if((calls_el1.Contains(z + 1) == false) && (calls_el2.Contains(z + 1) == false))
                                calls.Add(z + 1);
                            
                        }

                        label1.Text += Convert.ToString(i) + ": ";
                        Counters[z][i]--;
                        for (int j = 0; j < i; j++)
                        {
                            awaitings[z + 1].Add(new Passenger(destination,0));
                            label1.Text += Convert.ToString(destination) + ", ";
                        }
                        label1.Text += "Осталось: " + Convert.ToString(Counters[z][i]) + "\r\n";
                        break;
                    }
                }

            }
            listFromMaxToMin(calls);

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
           
            label4.Text = "Ожидающие пассажиры\r\n";
            for (int j = 0; j < floors; j++)//вывод ожидающих пассажиров
            {
                label4.Text += Convert.ToString(j + 1) + ": ";
                if ((awaitings[j].Count != 0) && (floor_button[j].State == false))
                {
                    floor_button[j].State = true;
                    if((calls_el1.Contains(j) == false) && (calls_el2.Contains(j) == false))
                        calls.Add(j);
                }

                for (int i = 0; i < awaitings[j].Count; i++)
                {
                    awaitings[j][i].Waiting_time++;

                    label4.Text += Convert.ToString(awaitings[j][i].Destination) + " ";

                }
                label4.Text += "\r\n";

                if (floor_button[j].State == true)
                    floor_button[j].State_true_time++;
                if (floor_button[j].State_true_time > 25 && !special_calls.Contains(j))
                {
                    special_calls.Add(j);
                    listFromMaxToMin(special_calls);
                    
                }

            }
            listFromMaxToMin(calls);

        }

        private void timer2_Tick(object sender, EventArgs e)
        {

            system_time++;
            label7.Text = "";
            label8.Text = "";
            label9.Text = "Время работы:\r\n" + Convert.ToString(system_time) + " сек";
            bool boarding_state_1 = false, boarding_state_2 = false;
            bool landing_1 = false, landing_2 = false;
            bool change = false;
            bool button_clicked = false;
            bool special_moving_1 = false, special_moving_2 = false;
            for (int i = 0; i < floor_button.Count; i++)
            {
                if (floor_button[i].State == true)
                {
                    button_clicked = true;
                    break;
                }
            }


            if (button_clicked || (elevator1_passengers.Count>0) ||(elevator2_passengers.Count>0))
            {
                
                if (elevator1_passengers.Count == 0 )
                {
                    if (calls.Count > 0)
                    {
                        if (special_calls.Count > 0 && !special_call_already_accept)
                        {
                            special_moving_1 = true;
                            if (special_call_for_el1 == 0 && special_calls[0] != 0)
                            {
                                special_call_for_el1 = special_calls[0];
                            }
                            else
                            {
                                if (special_call_for_el1 < trackBar1.Value )
                                    elevators_direction[0] = true;
                                
                                else
                                {
                                    if (special_call_for_el1 > trackBar1.Value )
                                    {
                                        elevators_direction[0] = false;
                                    }
                                        
                                    else
                                    {
                                        boarding_1();
                                        boarding_state_1 = true;
                                        special_moving_1 = false;
                                        special_call_for_el1 = 0;
                                        label7.Text = "Остановка";
                                    }
                                }
                            }
                        }
                        else
                        {


                            if (calls_el1.Count == 0)
                            {//создание вызовов
                                calls_el1.Add(calls[0]);
                                calls.RemoveAt(0);
                                if (calls.Count > 1)
                                {
                                    calls_el1.Add(calls[0]);
                                    calls.RemoveAt(0);
                                    if (calls.Count > 2)
                                    {
                                        calls_el1.Add(calls[0]);
                                        calls.RemoveAt(0);
                                    } 
                                }
                                
                            }
                            else//обновление вызовов
                            {
                                if (calls_el2.Count > 0)
                                {
                                    if ((Math.Abs(calls_el2[0] - trackBar1.Value) < Math.Abs(calls_el2[0] - trackBar2.Value)) && (Math.Abs(calls_el2[0] - trackBar1.Value) < Math.Abs(calls_el1[0] - trackBar1.Value)))
                                    {
                                        List<int> cross = new List<int>(calls_el1);
                                        calls_el1 = new List<int>(calls_el2);
                                        calls_el2 = new List<int>(cross);
                                        change = true;
                                    }
                                
                                    if (elevators_direction[0] == false)
                                    {
                                        if (calls_el1[0] > calls_el2[0])
                                        {

                                            int cntr = 0;
                                            int marker_1 = calls_el1[0];
                                            while (marker_1 < calls[0])
                                            {

                                                calls_el1.Insert(0, calls[0]);
                                                calls.RemoveAt(0);
                                                if (calls_el1.Count == 4)
                                                {
                                                    calls.Add(calls_el1[calls_el1.Count - 1]);
                                                    calls_el1.RemoveAt(calls_el1.Count - 1);
                                                }
                                                cntr++;
                                                if (cntr == 3 || calls.Count == 0)
                                                    break;

                                            }
                                        }
                                        else
                                        {
                                            for (int i = 0; i < calls.Count; i++)
                                            {
                                                if (calls[i] < calls_el2[calls_el2.Count - 1])
                                                {
                                                    int cntr = 0;
                                                    int marker_1 = calls_el1[0];
                                                    while (marker_1 < calls[i])
                                                    {

                                                        calls_el1.Insert(0, calls[i]);
                                                        calls.RemoveAt(i);
                                                        if (calls_el1.Count == 4)
                                                        {
                                                            calls.Add(calls_el1[calls_el1.Count - 1]);
                                                            calls_el1.RemoveAt(calls_el1.Count - 1);
                                                        }
                                                        cntr++;
                                                        if (cntr == 3 || calls.Count == i)
                                                            break;

                                                    }
                                                    break;
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        for (int i = 0; i < calls.Count; i++)
                                        {
                                            if (calls[i] < trackBar1.Value + 1)
                                            {
                                                int cntr = 0;
                                                int marker_1 = calls_el1[0];
                                                while (marker_1 < calls[i])
                                                {

                                                    calls_el1.Insert(0, calls[i]);
                                                    calls.RemoveAt(i);
                                                    if (calls_el1.Count == 4)
                                                    {
                                                        calls.Add(calls_el1[calls_el1.Count - 1]);
                                                        calls_el1.RemoveAt(calls_el1.Count - 1);
                                                    }
                                                    cntr++;
                                                    if (cntr == 3 || calls.Count == i)
                                                        break;

                                                }
                                                break;
                                            }
                                        }
                                    }
                                    listFromMaxToMin(calls_el1);
                                    listFromMaxToMin(calls);

                                }
                                else
                                {
                                    if (elevators_direction[0] == false)
                                    {
                                        int cntr = 0;
                                        int marker_1 = calls_el1[0];
                                        while (marker_1 < calls[0])
                                        {

                                            calls_el1.Insert(0, calls[0]);
                                            calls.RemoveAt(0);
                                            if (calls_el1.Count == 4)
                                            {
                                                calls.Add(calls_el1[calls_el1.Count - 1]);
                                                calls_el1.RemoveAt(calls_el1.Count - 1);
                                            }
                                            cntr++;
                                            if (cntr == 3 || calls.Count == 0)
                                                break;

                                        }
                                    }
                                    else
                                    {
                                        for (int i = 0; i < calls.Count; i++)
                                        {
                                            if (calls[i] < trackBar1.Value + 1)
                                            {
                                                int cntr = 0;
                                                int marker_1 = calls_el1[0];
                                                while (marker_1 < calls[i])
                                                {

                                                    calls_el1.Insert(0, calls[i]);
                                                    calls.RemoveAt(i);
                                                    if (calls_el1.Count == 4)
                                                    {
                                                        calls.Add(calls_el1[calls_el1.Count - 1]);
                                                        calls_el1.RemoveAt(calls_el1.Count - 1);
                                                    }
                                                    cntr++;
                                                    if (cntr == 3 || calls.Count == i)
                                                        break;

                                                }
                                                break;
                                            }
                                        }
                                    }
                                    listFromMaxToMin(calls_el1);
                                    listFromMaxToMin(calls);

                                }

                            }
                        
                        }
                    }
                    if (calls_el1.Count > 0 && !special_moving_1)
                    {
                        if (calls_el1[0] < trackBar1.Value)
                            elevators_direction[0] = true;
                        else
                        {
                            if (calls_el1[0] > trackBar1.Value)
                                elevators_direction[0] = false;
                            else
                            {
                                boarding_1();
                                boarding_state_1 = true;
                                label7.Text = "Остановка";
                            }
                        }
                    }
                }
                else
                {
                    if(elevator1_passengers.Count==0)
                        moving = false;
                    if (elevator1_passengers.Contains(trackBar1.Value + 1))
                    {
                        landing_1 = true;
                        for (int i = 0; i < elevator1_passengers.Count; i++)//высадка пассажиров
                        {
                            if (elevator1_passengers[i] == trackBar1.Value + 1)
                            {
                                elevator1_passengers.RemoveAt(i);
                                i--;
                            }

                        }

                    }
                    
                    int debil = rand.Next(1, 6);
                    
                    if (floor_button[trackBar1.Value].State == true && (elevator1_passengers.Count < elevators_Capasity)
                        && (elevators_direction[0] == true && 
                        (calls_el2.Contains(trackBar1.Value) == false || trackBar1.Value == 0 || elevator2_passengers.Count==elevators_Capasity || 
                        elevators_direction[1]==false || (elevators_direction[1]==true && trackBar2.Value<trackBar1.Value)))
                        || (elevators_direction[0] == false && landing_1 == true && debil % 5 == 0))                 
                    {
                        boarding_1();
                        boarding_state_1 = true;
                        label7.Text = "Остановка";

                    }
                    else
                    {
                        if (landing_1 == true)
                        {
                            label7.Text = "Остановка";
                            passengers_Update_1(true);
                        }
                    }
                }

                if (boarding_state_1 == false && landing_1 == false && moving)//elevator1 moving
                {
                    if (trackBar1.Value == floors - 1) elevators_direction[0] = true;
                    else
                    {
                        if (trackBar1.Value == 0) elevators_direction[0] = false;

                    }

                    if (elevators_direction[0] == false)
                        trackBar1.Value++;
                    else
                        trackBar1.Value--;

                }
                moving = true;

                if (elevator2_passengers.Count == 0 )
                {
                    if (calls.Count > 0)
                    {

                        if (special_calls.Count > 1 && elevator1_passengers.Count == 0)
                        {
                            special_moving_2 = true;
                            if (special_call_for_el2 == 0 && special_calls[1] != 0)
                            {
                                special_call_for_el2 = special_calls[1];
                            }
                            else
                            {
                                if (special_call_for_el2 < trackBar2.Value )
                                    elevators_direction[1] = true;
                                else
                                {
                                    if (special_call_for_el2 > trackBar2.Value )
                                        elevators_direction[1] = false;
                                    else
                                    {
                                        boarding_2();
                                        boarding_state_2 = true;
                                        special_moving_2 = false;
                                        special_call_for_el2 = 0;
                                        label8.Text = "Остановка";
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (special_calls.Count > 0 && elevator1_passengers.Count != 0)
                            {
                                special_moving_2 = true;
                                special_call_already_accept = true;
                                if (special_call_for_el2 == 0 && special_calls[0] != 0)
                                {
                                    special_call_for_el2 = special_calls[0];
                                }
                                else
                                {
                                    
                                    if (special_call_for_el2 < trackBar2.Value )
                                        elevators_direction[1] = true;
                                    else
                                    {
                                        if (special_call_for_el2 > trackBar2.Value)
                                            elevators_direction[1] = false;
                                        else
                                        {
                                            boarding_2();
                                            boarding_state_2 = true;
                                            special_moving_2 = false;
                                            special_call_already_accept=false;
                                            special_call_for_el2 = 0;
                                            label8.Text = "Остановка";
                                        }
                                    }
                                }
                            }
                            else
                            {

                                if (calls_el2.Count == 0)
                                {
                                    calls_el2.Add(calls[0]);
                                    calls.RemoveAt(0);
                                    if (calls.Count > 1)
                                    {
                                        calls_el2.Add(calls[0]);
                                        calls.RemoveAt(0);
                                        if (calls.Count > 2)
                                        {
                                            calls_el2.Add(calls[0]);
                                            calls.RemoveAt(0);
                                        }
                                    }
                                }
                                else//обновление вызовов
                                {
                                    if (calls_el1.Count > 0)
                                    {

                                        if ((Math.Abs(calls_el1[0] - trackBar2.Value) < Math.Abs(calls_el1[0] - trackBar1.Value)) && (Math.Abs(calls_el1[0] - trackBar2.Value) < Math.Abs(calls_el2[0] - trackBar2.Value)) && !change)
                                        {
                                            List<int> cross = new List<int>(calls_el1);
                                            calls_el1 = new List<int>(calls_el2);
                                            calls_el2 = new List<int>(cross);
                                        }
                                    }

                                    if (elevators_direction[1] == false)
                                    {
                                        int cntr = 0;
                                        int marker_2 = calls_el2[0];
                                        while (marker_2 < calls[0])
                                        {

                                            calls_el2.Insert(0, calls[0]);
                                            calls.RemoveAt(0);
                                            if (calls_el2.Count == 4)
                                            {
                                                calls.Add(calls_el2[calls_el2.Count - 1]);
                                                calls_el2.RemoveAt(calls_el2.Count - 1);
                                            }
                                            cntr++;
                                            if (cntr == 3 || calls.Count == 0)
                                                break;

                                        }

                                    }
                                    else
                                    {
                                        for (int i = 0; i < calls.Count; i++)
                                        {
                                            if (calls[i] < trackBar2.Value + 1)
                                            {
                                                int cntr = 0;
                                                int marker_2 = calls_el2[0];
                                                while (marker_2 < calls[i])
                                                {

                                                    calls_el2.Insert(0, calls[i]);
                                                    calls.RemoveAt(i);
                                                    if (calls_el2.Count == 4)
                                                    {
                                                        calls.Add(calls_el2[calls_el2.Count - 1]);
                                                        calls_el2.RemoveAt(calls_el2.Count - 1);
                                                    }
                                                    cntr++;
                                                    if (cntr == 3 || calls.Count == i)
                                                        break;

                                                }
                                                break;
                                            }
                                        }
                                    }
                                    listFromMaxToMin(calls_el2);
                                    listFromMaxToMin(calls);

                                }

                            }
                        }
                    }
                    if (calls_el2.Count > 0 && !special_moving_2)
                    {
                        if (calls_el2[0] < trackBar2.Value)
                            elevators_direction[1] = true;
                        else
                        {
                            if (calls_el2[0] > trackBar2.Value)
                                elevators_direction[1] = false;
                            else
                            {
                                boarding_2();
                                boarding_state_2 = true;
                                label8.Text = "Остановка";
                            }
                        }
                    }
                }
                else
                {
                    if (elevator2_passengers.Count == 0)
                        moving =false;
                    if (elevator2_passengers.Contains(trackBar2.Value + 1))
                    {
                        landing_2 = true;
                        for (int i = 0; i < elevator2_passengers.Count; i++)//высадка пассажиров
                        {
                            label8.Text += Convert.ToString(i);
                            if (elevator2_passengers[i] == trackBar2.Value + 1)
                            {
                                elevator2_passengers.RemoveAt(i);
                                i--;
                            }

                        }

                    }

                    int debil = rand.Next(1, 6);
                    if (floor_button[trackBar2.Value].State == true && (elevator2_passengers.Count < elevators_Capasity)
                        && (elevators_direction[1] == true && (calls_el1.Contains(trackBar2.Value) == false || trackBar2.Value == 0 || elevator1_passengers.Count == elevators_Capasity ||
                        elevators_direction[0] == false || (elevators_direction[0] == true && trackBar1.Value < trackBar2.Value)))
                        || (elevators_direction[1] == false && landing_2 == true && debil % 5 == 0))   
                    {
                        boarding_2();
                        boarding_state_2 = true;
                        label8.Text = "Остановка";

                    }
                    else
                    {
                        if (landing_2 == true)
                        {
                            passengers_Update_2(true);
                            label8.Text = "Остановка";
                        }
                    }
                }

                
                if (system_time % 15 == 0 && checker_2)
                {
                    average_waiting_time[0] += summ_waiting_time / 15;
                    average_waiting_time[1]++;

                    if (generation_stopped)
                    {
                        system_on_stop_time = system_time;
                        checker_2 = false;
                    }

                    summ_waiting_time = 0;
                }

                if (boarding_state_2 == false && landing_2 == false && moving)//elevator2 moving
                {
                    if (trackBar2.Value == floors - 1) elevators_direction[1] = true;
                    else
                    {
                        if (trackBar2.Value == 0) elevators_direction[1] = false;
                    }


                    if (elevators_direction[1] == false)
                        trackBar2.Value++;
                    else
                        trackBar2.Value--;
                }
                moving = true;


                label2.Text = Convert.ToString(trackBar1.Value + 1);
                label3.Text = Convert.ToString(trackBar2.Value + 1);
            }
        }

        async void boarding_1()
        {
            await Task.Delay(1000);
            bool checker = true;
            
            while (awaitings[trackBar1.Value].Count != 0)//посадка на свободные места
            {
                if (elevator1_passengers.Count >= elevators_Capasity) break;
                elevator1_passengers.Add(awaitings[trackBar1.Value][0].Destination);
                
                summ_waiting_time += awaitings[trackBar1.Value][0].Waiting_time;
                
                
                if (awaitings[trackBar1.Value][0].Waiting_time > max_waiting_time)
                    max_waiting_time = awaitings[trackBar1.Value][0].Waiting_time;

                awaitings[trackBar1.Value].RemoveAt(0);
            }
            floor_button[trackBar1.Value].State = false;
            floor_button[trackBar1.Value].State_true_time = 0;
            if (calls.Contains(trackBar1.Value)) 
                calls.Remove(trackBar1.Value); 
            if (calls_el1.Contains(trackBar1.Value))
                calls_el1.Remove(trackBar1.Value);
            if (calls_el2.Contains(trackBar1.Value))
                calls_el2.Remove(trackBar1.Value);
            if (special_calls.Contains(trackBar1.Value))
                special_calls.Remove(trackBar1.Value);

            passengers_Update_1(checker);


        }

        async void boarding_2()
        {
            await Task.Delay(1000);
            bool checker = true;

            while (awaitings[trackBar2.Value].Count != 0)//посадка на свободные места
            {
                if (elevator2_passengers.Count >= elevators_Capasity) break;
                elevator2_passengers.Add(awaitings[trackBar2.Value][0].Destination);
               
                summ_waiting_time += awaitings[trackBar2.Value][0].Waiting_time;

                if (awaitings[trackBar2.Value][0].Waiting_time > max_waiting_time)
                    max_waiting_time = awaitings[trackBar2.Value][0].Waiting_time;

                awaitings[trackBar2.Value].RemoveAt(0);
            }
            floor_button[trackBar2.Value].State = false;
            floor_button[trackBar2.Value].State_true_time = 0;

            if (calls.Contains(trackBar2.Value)) 
                calls.Remove(trackBar2.Value);
            if (calls_el1.Contains(trackBar2.Value))
                calls_el1.Remove(trackBar2.Value);
            if (calls_el2.Contains(trackBar2.Value))
                calls_el2.Remove(trackBar2.Value);
            if(special_calls.Contains(trackBar2.Value))
                special_calls.Remove(trackBar2.Value);
           
            passengers_Update_2(checker);

        }



        void passengers_Update_1(bool checker)
        {
            label5.Text = "В лифте 1:\r\n";

            for (int i = 0; i < elevator1_passengers.Count; i++)//вывод пассажиров после посадки
            {
                label5.Text += Convert.ToString(elevator1_passengers[i]) + " ";
                if (elevator1_passengers[i] != 1 && checker)
                    checker = false;
                if (i % 4 == 3)
                    label5.Text += "\r\n";
            }

            if (checker && elevators_direction[0] == false)//если всем на 1 то едем вниз
                elevators_direction[0] = true;
        }

        void passengers_Update_2(bool checker)
        {
            label6.Text = "В лифте 2:\r\n";

            for (int i = 0; i < elevator2_passengers.Count; i++)//вывод пассажиров после посадки
            {
                label6.Text += Convert.ToString(elevator2_passengers[i]) + " ";
                if (elevator2_passengers[i] != 1 && checker)
                    checker = false;
                if (i % 4 == 3)
                    label6.Text += "\r\n";
            }

            if (checker && elevators_direction[1] == false)//если всем на 1 то едем вниз
                elevators_direction[1] = true;
        }

        void listFromMaxToMin(List<int> callers)
        {
            for (int i = 0; i < callers.Count - 1; i++)
            {
                for (int j = i + 1; j < callers.Count; j++)
                {
                    if (callers[j] > callers[i])
                    {
                        int x = callers[i];
                        callers[i] = callers[j];
                        callers[j] = x;
                    }
                }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            trackBar1.Enabled = false;
            
        }

        
        async private void button1_Click(object sender, EventArgs e)
        {
            if (!research_ended)
            {
                research_ended = true;
                generation_stopped = true;
                timer1.Stop();
                label1.Text = "Новые пассажиры:\r\nГенерация остановлена";
                bool time_to_stop = false;
                while (!time_to_stop)
                {
                    time_to_stop = true;
                    for (int i = 0; i < floor_button.Count; i++)
                    {
                        if (floor_button[i].State == true)
                        {
                            time_to_stop = false;
                            break;
                        }
                    }
                    await Task.Delay(2000);
                }
                timer2.Stop();
                summ_waiting_time = Convert.ToUInt32(summ_waiting_time / (system_time - system_on_stop_time));
                average_waiting_time[0] = (average_waiting_time[0] + summ_waiting_time) / (average_waiting_time[1] + 1);
                Report.ShowBox(Convert.ToString(system_time), average_waiting_time[0], max_waiting_time);
                this.Close();
            }
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            trackBar2.Enabled = false;
        }
    }

    public class Passenger
        {
            public Passenger(int destination, uint waiting_time)
        {
            Destination = destination;
            Waiting_time = waiting_time;
        }
        public int Destination { get;  set; }
        public uint Waiting_time { get;  set; }
            
        }

    public class Button
    {
        public Button(bool state, uint  state_true_time)
        {
            State = state;
            State_true_time = state_true_time;
        }

        public bool State { get; set; }
        public uint State_true_time { get; set; }   
    }

}
