using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExemploJogoParalelo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Número de personagens no jogo
            int numPersonagens = 5;

            // Inicializa as threads para simular ações das personagens
            Thread[] threads = new Thread[numPersonagens];
            for (int i = 0; i < numPersonagens; i++)
            {
                threads[i] = new Thread(() => RealizarAcoesPersonagem(i));
                threads[i].Start();
            }

            // Aguarda todas as threads terminarem
            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine("Ações de todas as personagens foram concluídas.");
        }

        static void RealizarAcoesPersonagem(int personagemId)
        {
            Console.WriteLine($"Personagem {personagemId} está se movendo.");
            SimularMovimento(personagemId);

            Console.WriteLine($"Personagem {personagemId} está atacando.");
            SimularAtaque(personagemId);

            Console.WriteLine($"Ações do Personagem {personagemId} foram concluídas.");
        }

        static void SimularMovimento(int personagemId)
        {
            // Simulação de movimento da personagem
            Thread.Sleep(1000); // Simula o tempo de movimento
        }

        static void SimularAtaque(int personagemId)
        {
            // Simulação de ataque da personagem
            Thread.Sleep(1500); // Simula o tempo de ataque
        }
    }
}
