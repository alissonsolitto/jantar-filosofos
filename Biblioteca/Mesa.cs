using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Biblioteca
{
    public class Mesa
    {
        public bool[] garfos = new bool[5];

        public void pegarGarfo(int left, int right)
        {
            // Garante que um thread não entre em uma seção crítica do código enquanto outro thread está na seção crítica
            lock (this) // Seção crítica, bloqueio de exclusão mútua até o fim da instrução
            {
                while(garfos[left] || garfos[right]) // Enquanto os garfos estão ocupados
                {
                    Monitor.Wait(this); // Bloqueia o thread atual até que o garfo seja liberado
                }

                garfos[left] = true;
                garfos[right] = true;
            }

        }

        public void devolverGarfo(int left, int right)
        {
            lock (this)
            {
                garfos[left] = false;
                garfos[right] = false;

                Monitor.PulseAll(this); // Libero o bloqueio para os outros thread
            }
        }
    }
}
