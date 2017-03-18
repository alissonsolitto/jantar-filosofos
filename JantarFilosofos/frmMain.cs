using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JantarFilosofos
{
    public partial class frmMain : Form
    {

        public frmMain()
        {
            InitializeComponent();
        }

        private void btnJantar_Click(object sender, EventArgs e)
        {
            List<Label> lstGarfo = new List<Label>();
            lstGarfo.Add(garfo0);
            lstGarfo.Add(garfo1);
            lstGarfo.Add(garfo2);
            lstGarfo.Add(garfo3);
            lstGarfo.Add(garfo4);

            Mesa mesa = new Mesa(lstGarfo); // Seção critica

            new Filosofo(0, 1000, 2000, mesa, lbl0);
            new Filosofo(1, 1000, 2000, mesa, lbl1);
            new Filosofo(2, 1000, 2000, mesa, lbl2);
            new Filosofo(3, 1000, 2000, mesa, lbl3);
            new Filosofo(4, 1000, 2000, mesa, lbl4);
        }
        
        public class Filosofo
        {
            private int num_filosofo { get; set; }
            private int pensa { get; set; }
            private int come { get; set; }
            private int prox_filosofo { get; set; }
            private Mesa mesa { get; set; }
            private Label lbl { get; set; }

            public Filosofo(int n, int pensa, int come, Mesa mesa, Label lbl)
            {
                this.num_filosofo = n;
                this.pensa = pensa;
                this.come = come;
                this.prox_filosofo = (num_filosofo == 4 ? 0 : n + 1);
                this.mesa = mesa;
                this.lbl = lbl;

                // Cria e inicia a Thread
                new Thread(new ThreadStart(Run)).Start();
            }


            public void Run()
            {
                while (true)
                {
                    Thread.Sleep(pensa); // Pensa
                    mesa.pegarGarfo(num_filosofo, prox_filosofo);
                    lbl.BackColor = Color.Green;
                    Thread.Sleep(come); // Come
                    mesa.devolverGarfo(num_filosofo, prox_filosofo);
                    lbl.BackColor = Color.Yellow;
                }
            }
        }

        public class Mesa
        {
            public bool[] garfos = new bool[5];
            public List<Label> lstGarfos;

            public Mesa(List<Label> lstGarfos)
            {
                this.lstGarfos = lstGarfos;
            }

            public void pegarGarfo(int left, int right)
            {
                // Garante que um thread não entre em uma seção crítica do código enquanto outro thread está na seção crítica
                lock (this) // Seção crítica, bloqueio de exclusão mútua até o fim da instrução
                {
                    while (garfos[left] || garfos[right]) // Enquanto os garfos estão ocupados
                    {
                        Monitor.Wait(this); // Bloqueia o thread atual até que o garfo seja liberado
                    }

                    garfos[left] = true;
                    garfos[right] = true;

                    lstGarfos[left].BackColor = Color.Red;
                    lstGarfos[right].BackColor = Color.Red;
                }

            }

            public void devolverGarfo(int left, int right)
            {
                lock (this)
                {
                    garfos[left] = false;
                    garfos[right] = false;

                    lstGarfos[left].BackColor = Color.Green;
                    lstGarfos[right].BackColor = Color.Green;

                    Monitor.PulseAll(this); // Libero o bloqueio para os outros thread
                }
            }
        }
    }
}
