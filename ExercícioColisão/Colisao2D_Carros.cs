using System;
using System.Threading;

// Define a classe Carro para representar os carros no jogo
class Carro
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Largura { get; set; }
    public int Altura { get; set; }

    // Implemente o método para verificar colisão entre dois carros
    public bool VerificarColisao(Carro outroCarro)
    {
        /*
        int soma = outroCarro.X + outroCarro.Largura;
        Console.WriteLine("\n(X) " + X + "   <    " + soma + " (outroCarro.X + outroCarro.Largura)");
        soma = X + Largura;
        Console.WriteLine("(X + Largura) " + soma + "   >   " + outroCarro.X + " (outroCarro.X) ");
        soma = outroCarro.Y + outroCarro.Altura;
        Console.WriteLine("(Y) " + Y + "   <   " + soma + " (outroCarro.Y + outroCarro.Altura)");
        soma = Y + Altura;
        Console.WriteLine("(Y + Altura) " + soma + "   >   " + outroCarro.Y + " (outroCarro.Y)");
        Console.WriteLine();
        */

        int xInicial = X;
        int xFinal = X+Largura;
        int yInicial = Y;
        int yFinal = Y+Altura;
        int xInicialOutroCarro = outroCarro.X;
        int xFinalOutroCarro = outroCarro.X + Largura;
        int yInicialOutroCarro = outroCarro.Y;
        int yFinalOutroCarro = outroCarro.Y + Altura;


/*
        int xInicial = X-Largura/2;
        int xFinal = X+Largura/2;
        int yInicial = Y-Largura/2;
        int yFinal = Y+Largura/2;
        int xInicialOutroCarro = outroCarro.X - Largura/2;
        int xFinalOutroCarro = outroCarro.X + Largura/2;
        int yInicialOutroCarro = outroCarro.Y - Altura/2;
        int yFinalOutroCarro = outroCarro.Y + Altura/2;
*/       
        Console.WriteLine("Carro na Area X: ("+xInicial+" → "+xFinal+"), Y: ("+yInicial+" → "+yFinal+")"+" Outro carro na Area X: ("+xInicialOutroCarro+" → "+xFinalOutroCarro+"), Y: ("+yInicialOutroCarro+" → "+yFinalOutroCarro+")");
      

      //https://developer.mozilla.org/pt-BR/docs/Games/Techniques/2D_collision_detection

        if ((X < outroCarro.X + outroCarro.Largura) && 
            (X + Largura > outroCarro.X) && 
            (Y < outroCarro.Y + outroCarro.Altura) && 
            (Y + Altura > outroCarro.Y)){
                //Console.WriteLine("** Colisão **\n");
                return true;
        }
            return false;

    }
}

// Classe principal do jogo
class Program
{
    static readonly object lockObj = new object(); // Objeto de sincronização
    static bool[][] resultado; // Matriz para armazenar os resultados da detecção de colisão
    static Carro[] carros = new Carro[3]; // Lista de carros do jogo

    // Método para ser executado em cada thread
    static void VerificarColisaoThread(object indice)
    {
        int idx = (int)indice;
        //Console.WriteLine("\nThread: " + idx + " verificando Colisão...");
        for (int i = idx + 1; i < carros.Length; i++)
        {
            if (carros[idx].VerificarColisao(carros[i]))
            {
                //Console.WriteLine("Carros "+idx+" e "+i+" Colidiram!");
                //NAO PRECISAMOS de SEMAFOROS, já que se as threads alterarem o valor, alteram para o mesmo: TRUE.
               // lock (lockObj)
               // {
                    resultado[idx][i] = true;
               // }
            }
        }
    }

    // Método principal do jogo
    static void Jogo()
    {
        // Cria uma lista de carros e adicione carros ao jogo

        // Inicializa a matriz de resultados da detecção de colisão
        //Console.WriteLine("\nCriando matriz de resultados de tamanho "+carros.Length+"\n");

        resultado = new bool[carros.Length][];

        for (int i = 0; i < carros.Length; i++)
        {
            resultado[i] = new bool[carros.Length];
        }

        int time = 5;

        while (time > 0)
        {

            // Cria um array para armazenar as threads
            Thread[] threads = new Thread[carros.Length];

            // Inicie as threads para verificar colisão entre os carros. UMA thread para cada CARRO
            for (int i = 0; i < carros.Length; i++)
            {
                //Console.WriteLine("Criando a Thread " + i);
                threads[i] = new Thread(VerificarColisaoThread);
                threads[i].Start(i);
            }

            // Aguarda a conclusão de todas as threads
            for (int i = 0; i < carros.Length; i++)
            {
                threads[i].Join();
            }


            //Console.Clear();
          
            Console.WriteLine("\nQde de carros: "+carros.Length);

            for (int i = 0; i < carros.Length; i++)
            {
                for (int j = 0; j < carros.Length; j++)
                {                                     
                        Console.Write(resultado[i][j]+"   |   ");
                }
                Console.WriteLine();
            }

            //Console.WriteLine("\nConclusão:");
            Console.WriteLine();
            for (int i = 0; i < carros.Length; i++)
            {
                for (int j = 0; j < carros.Length; j++)
                {
                    if (resultado[i][j] == true)
                    {
                        Console.WriteLine("→ → Carros " + i + " e " + j + " Colidiram! ← ←");
                    }
                }               
            }
           

            for (int i = 0; i < carros.Length; i++)
            {
                for (int j = 0; j < carros.Length; j++)
                {
                    resultado[i][j] = false;                    
                }
            }

            //alterando as POSICOES dos CARROS
            Random num = new Random();
            carros[0].X += num.Next(-2,2);
            carros[1].X += num.Next(-2,2);
            carros[2].X += num.Next(-2,2);
            carros[0].Y += num.Next(1,3);
            carros[1].Y += num.Next(1,3);
            carros[2].Y += num.Next(1,3);

            time--;
            Thread.Sleep(700);
            Console.WriteLine();
        }
    }

    // Método de entrada do programa
    static void Main()
    {
        // Criação dos carros
        Carro c1 = new Carro(); //carro 1
        c1.X = 12;
        c1.Y = 10;
        c1.Largura = 6;
        c1.Altura = 6;
        carros[0] = c1;

        Carro c2 = new Carro(); //carro 2
        c2.X = 17;
        c2.Y = 11;
        c2.Largura = 6;
        c2.Altura = 6;
        carros[1] = c2;

        Carro c3 = new Carro(); //carro 3
        c3.X = 8;
        c3.Y = 10;
        c3.Largura = 6;
        c3.Altura = 6;
        carros[2] = c3;

        // Cria um laço para criar os carros de forma simulada com números randômicos
        // ou com dados informados pelo usuário!


        // Inicia o jogo
        Console.WriteLine("☼ ☼ Jogo de Corrida ☼ ☼ \n\nPosições iniciais dos carros:");
        Console.WriteLine("Carro 1 (x,y) = " + carros[0].X + "," + carros[0].Y);
        Console.WriteLine("Carro 2 (x,y) = " + carros[1].X + "," + carros[1].Y);
        Console.WriteLine("Carro 3 (x,y) = " + carros[2].X + "," + carros[2].Y);

        Thread.Sleep(5000);

        Jogo();
    }
}

