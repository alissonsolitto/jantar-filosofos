using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Biblioteca
{
    public class Filosofo
    {
        private int num_filosofo { get; set; }
        private int pensa { get; set; }
        private int come { get; set; }
        private int prox_filosofo { get; set; }
        private Mesa mesa { get; set; }
        private Object frm { get; set; }
        

        public Filosofo(int n, int pensa, int come, Mesa mesa, Object frm)
        {
            this.num_filosofo = n;
            this.pensa = pensa;
            this.come = come;
            this.prox_filosofo = (num_filosofo == 4 ? 0 : n + 1);
            this.mesa = mesa;
            this.frm = frm;

            // Cria e inicia a Thread
            new Thread(new ThreadStart(Run)).Start();
        }


        public void Run()
        {

            while(true)
            {
                try
                {
                    Thread.Sleep(pensa); // Pensa
                    mesa.pegarGarfo(num_filosofo, prox_filosofo);

                    Thread.Sleep(come); // Come
                    mesa.devolverGarfo(num_filosofo, prox_filosofo);
                }

                catch
                {
                    return; //para não parar o fluxo de execução
                }
            }
        }

    }
}
