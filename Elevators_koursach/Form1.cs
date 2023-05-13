using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Elevators_koursach
{
    public partial class Form1 : Form
    {
        const int floors = 9;

        double lamda = 0.14;
        double lamda1 = 1.12;
        int t = 2880;
        double[] P = new double[4];
        double[] P1 = new double[7];
        double[,] Counters = new double[floors - 1, 4];
        double[] Counters1 = new double[7];
        int[] Intervals = new int[4];
        int[] Intervals1 = new int[7];
        double Summ_counters;
        double Summ_counters1;
        Random rand = new Random();
        List<List<Passenger>> awaitings = new List<List<Passenger>>();
        bool[] elevators_direction = new bool[2];
        bool[] button_state = new bool[floors];
        List<int> elevator1_passengers = new List<int>();
        List<int> elevator2_passengers = new List<int>();
        int elevators_Capasity = 8;
        List<int> calls = new List<int>();
        uint summ_waiting_time = 0;
        uint[] average_waiting_time= new uint[2];

        uint max_waiting_time = 0;
        int system_time = 1;
        int system_on_stop_time;
        bool generation_stopped = false;

        bool checker_2 = true;




        public Form1()
        {
            InitializeComponent();
            double L = Math.Exp(-1 * lamda);
            double L1 = Math.Exp(-1 * lamda1);
            //MessageBox.Show(Convert.ToString(L));
            int factorial = 1;
            textBox1.Text = "Data\r\n";
            for (int i = 0; i < 4; i++)//разная разменость!
            {

                P[i] = (L * Math.Pow(lamda, i)) / factorial;

                Counters[0, i] = Math.Round(t * P[i]);

                //MessageBox.Show(Convert.ToString(Counters[i]));
                Summ_counters += Counters[0, i];

                if (i > 0)
                {

                    Intervals[i] = Intervals[i - 1] + Convert.ToInt32(Counters[0, i]);

                }
                else
                {
                    Intervals[i] = Convert.ToInt32(Counters[0, i]);

                }
                factorial *= (i + 1);
                //textBox1.Text += "P: " + Convert.ToString(P[i]) + " Cnt: " + Convert.ToString(Counters[0,i] + " Int: " + Convert.ToString(Intervals[i]) + "\r\n");


            }
            factorial = 1;
            for (int i = 0; i < 7; i++)
            {

                P1[i] = (L1 * Math.Pow(lamda1, i)) / factorial;
                Counters1[i] = Math.Round(t * P1[i]);
                Summ_counters1 += Counters1[i];
                if (i > 0)
                {
                    Intervals1[i] = Intervals1[i - 1] + Convert.ToInt32(Counters1[i]);
                }
                else
                {
                    Intervals1[i] = Convert.ToInt32(Counters1[i]);
                }
                factorial *= (i + 1);
                textBox1.Text += "P: " + Convert.ToString(P1[i]) + " Cnt: " + Convert.ToString(Counters1[i] + " Int: " + Convert.ToString(Intervals1[i]) + "\r\n");

            }
            //MessageBox.Show(Convert.ToString(t));
            if (Summ_counters < t)
            {
                Counters[0, 1] += (t - Summ_counters);
                //textBox1.Text += "Поправка \r\n" + "Cnt1: " + Convert.ToString(Counters[0, 1]);
                for (int j = 1; j < 4; j++)
                {
                    Intervals[j] += Convert.ToInt32((t - Summ_counters));
                    //textBox1.Text += "Int: " + Convert.ToString(Intervals[j]) + "\r\n";
                }
                Summ_counters += (t - Summ_counters);
            }
            else
            {
                if (Summ_counters > t)
                {
                    Counters[0, 1] -= (Summ_counters - t);
                    //textBox1.Text += "Поправка \r\n" + "Cnt1: " + Convert.ToString(Counters[0, 1]);
                    for (int j = 1; j < 4; j++)
                    {
                        Intervals[j] -= Convert.ToInt32((Summ_counters - t));
                        //textBox1.Text += "Int: " + Convert.ToString(Intervals[j]) + "\r\n";
                    }
                    Summ_counters -= (Summ_counters - t);
                }
            }
            if (Summ_counters1 < t)
            {
                Counters1[1] += (t - Summ_counters1);
                textBox1.Text += "Поправка \r\n" + "Cnt1: " + Convert.ToString(Counters1[1]);
                for (int j = 1; j < 7; j++)
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
                    textBox1.Text += "Поправка \r\n" + "Cnt1: " + Convert.ToString(Counters1[1]);
                    for (int j = 1; j < 7; j++)
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
                for (int u = 0; u < 4; u++)
                    Counters[z, u] = Counters[0, u];
            }

            for (int i = 0; i < floors; i++)
            {
                awaitings.Add(new List<Passenger>());
                button_state[i] = false;
            }
            elevators_direction[0] = false;
            elevators_direction[1] = true;
            trackBar1.Maximum = trackBar2.Maximum = floors - 1;
        }

        private void timer1_Tick(object sender, EventArgs e)

        {
            label1.Text = "Новые пассажиры:\r\n";
            int now = rand.Next(1, t);
            int destination = 0;
            label1.Text += "1: ";



            for (int i = 0; i < 7; i++) //генерация пассажиров на первом 
            {
                if (now <= Intervals1[i] && Counters1[i] > 0)
                {
                    if ((button_state[0] == false) && (i != 0))
                    {
                        button_state[0] = true;
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
                for (int i = 0; i < 4; i++)
                {
                    if (now <= Intervals[i] && Counters[z, i] > 0)
                    {
                        if ((button_state[z + 1] == false) && (i != 0))
                        {
                            button_state[z + 1] = true;
                            calls.Add(z + 1);
                        }

                        label1.Text += Convert.ToString(i) + ": ";
                        Counters[z, i]--;
                        for (int j = 0; j < i; j++)
                        {
                            awaitings[z + 1].Add(new Passenger(destination,0));
                            label1.Text += Convert.ToString(destination) + ", ";
                        }
                        label1.Text += "Осталось: " + Convert.ToString(Counters[z, i]) + "\r\n";
                        break;
                    }
                }

            }

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            label4.Text = "Ожидающие пассажиры\r\n";
            for (int j = 0; j < floors; j++)//вывод ожидающих пассажиров
            {
                //label4.ForeColor = Color.Black;
                label4.Text += Convert.ToString(j + 1) + ": ";
                if ((awaitings[j].Count != 0) && (button_state[j] == false))
                {
                    button_state[j] = true;
                    calls.Add(j);
                }

                for (int i = 0; i < awaitings[j].Count; i++)
                {
                    awaitings[j][i].Waiting_time++;

                    //if (awaitings[j][i].Waiting_time < 15)//идея классная но не реализуется(
                    //    label4.ForeColor=Color.Green;
                    //else
                    //{
                     //   if (awaitings[j][i].Waiting_time > 30)
                     //       label4.ForeColor = Color.Red;
                     //   else
                   //         label4.ForeColor = Color.Yellow;
                   // }
                    //label4.ForeColor = Color.Green;

                    label4.Text += Convert.ToString(awaitings[j][i].Destination) + " ";

                }
                label4.Text += "\r\n";

            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

            system_time++;
            label9.Text = "Время работы:\r\n" + Convert.ToString(system_time) + " сек";
            bool boarding_state_1 = false, boarding_state_2 = false;
            bool landing_1 = false, landing_2 = false;
            if (button_state.Contains(true) || (elevator1_passengers.Count>0) ||(elevator2_passengers.Count>0))
            {
                if (elevator1_passengers.Count == 0 && calls.Count>0)
                {


                    if (calls[0] < trackBar1.Value)
                        elevators_direction[0] = true;
                    else
                    {
                        if (calls[0] > trackBar1.Value)
                            elevators_direction[0] = false;
                        else
                        {
                            boarding_1();
                            boarding_state_1 = true;
                        }
                    }

                }
                else
                {
                    label7.Text = "";
                    if (elevator1_passengers.Contains(trackBar1.Value + 1))
                    {
                        landing_1 = true;
                        for (int i = 0; i < elevator1_passengers.Count; i++)//высадка пассажиров
                        {
                            label7.Text += Convert.ToString(i);
                            if (elevator1_passengers[i] == trackBar1.Value + 1)
                            {
                                elevator1_passengers.RemoveAt(i);
                                i--;
                            }

                        }

                    }
                    int debil = rand.Next(1, 6);
                    //|| (elevators_direction[0]==false && landing_1==true)) пассажиры не садятся в лифт в другую сторону?
                    if (button_state[trackBar1.Value] == true && (elevator1_passengers.Count < elevators_Capasity)
                        && (elevators_direction[0] == true || (elevators_direction[0] == false && landing_1 == true && (debil % 5 == 0 || elevator1_passengers.Count==0))))
                    {
                        boarding_1();
                        boarding_state_1 = true;

                    }
                    else
                    {
                        if (landing_1 == true)
                        {
                            passengers_Update_1(true);
                        }
                    }
                }


                if (boarding_state_1 == false && landing_1 == false)//elevator1 moving
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

                if (elevator2_passengers.Count == 0 && calls.Count > 0)
                {
                    if (elevator1_passengers.Count != 0)
                    {
                        if (calls[0] < trackBar2.Value)
                            elevators_direction[1] = true;
                        else
                        {
                            if (calls[0] > trackBar2.Value)
                                elevators_direction[1] = false;
                            else
                            {
                                boarding_2();
                                boarding_state_2 = true;
                            }
                        }
                    }
                    else
                    {
                        if (calls.Count > 1)
                        {
                            if (calls[1] < trackBar2.Value)
                                elevators_direction[1] = true;
                            else
                            {
                                if (calls[1] > trackBar2.Value)
                                    elevators_direction[1] = false;
                                else
                                {
                                    boarding_2();
                                    boarding_state_2 = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    label8.Text = "";
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
                    if (button_state[trackBar2.Value] == true && (elevator2_passengers.Count < elevators_Capasity)
                        && (elevators_direction[1] == true || (elevators_direction[1] == false && landing_2 == true && (debil % 5 == 0 || elevator2_passengers.Count==0))))
                    {
                        //if((trackBar1.Value == trackBar2.Value) && (landing_1 == true) && (landing_2==false))
                        boarding_2();
                        boarding_state_2 = true;

                    }
                    else
                    {
                        if (landing_2 == true)
                        {
                            passengers_Update_2(true);
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

                if (boarding_state_2 == false && landing_2 == false)//elevator2 moving
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


                label2.Text = Convert.ToString(trackBar1.Value + 1);
                label3.Text = Convert.ToString(trackBar2.Value + 1);
            }
        }

        async void boarding_1()
        {
            await Task.Delay(1000);
            bool checker = true;
            
            //label5.Text = Convert.ToString(awaitings[trackBar1.Value][0]);

            while (awaitings[trackBar1.Value].Count != 0)//посадка на свободные места
            {
                if (elevator1_passengers.Count >= elevators_Capasity) break;
                elevator1_passengers.Add(awaitings[trackBar1.Value][0].Destination);
                
                summ_waiting_time += awaitings[trackBar1.Value][0].Waiting_time;
                
                
                if (awaitings[trackBar1.Value][0].Waiting_time > max_waiting_time)
                    max_waiting_time = awaitings[trackBar1.Value][0].Waiting_time;

                awaitings[trackBar1.Value].RemoveAt(0);
            }
            button_state[trackBar1.Value] = false;
            calls.Remove(trackBar1.Value);

            passengers_Update_1(checker);


        }

        async void boarding_2()
        {
            await Task.Delay(1000);
            bool checker = true;
            //label5.Text = Convert.ToString(awaitings[trackBar1.Value][0]);

            while (awaitings[trackBar2.Value].Count != 0)//посадка на свободные места
            {
                if (elevator2_passengers.Count >= elevators_Capasity) break;
                elevator2_passengers.Add(awaitings[trackBar2.Value][0].Destination);

                
                summ_waiting_time += awaitings[trackBar2.Value][0].Waiting_time;
                    //label9.Text = Convert.ToString(summ_waiting_time);

                if (awaitings[trackBar2.Value][0].Waiting_time > max_waiting_time)
                    max_waiting_time = awaitings[trackBar2.Value][0].Waiting_time;

                awaitings[trackBar2.Value].RemoveAt(0);
            }
            button_state[trackBar2.Value] = false;
            calls.Remove(trackBar2.Value);

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

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        async private void button1_Click(object sender, EventArgs e)
        {
           
            generation_stopped = true;
            timer1.Stop();
            label1.Text = "Новые пассажиры:\r\nГенерация остановлена";
            while (button_state.Contains(true) == true)
            {
                await Task.Delay(2000);
            }
            timer2.Stop();
            summ_waiting_time = Convert.ToUInt32(summ_waiting_time / (system_time - system_on_stop_time));
            average_waiting_time[0] = (average_waiting_time[0]+summ_waiting_time) / (average_waiting_time[1]+1);
            //MessageBox.Show("Время работы программы: "+Convert.ToString(system_time)+"\r\nСреднее время ожидания пассажиром: " + Convert.ToString(average_waiting_time[0]) +
            //    "\r\nМаксимальное время ожидания пассажиром: "+ Convert.ToString(max_waiting_time));
            Report.ShowBox(Convert.ToString(system_time), average_waiting_time[0], max_waiting_time);
            this.Close();
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

    
    
    


}
