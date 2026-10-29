using Conde.Bulgara;

if (args.Length == 0)
{
    Console.WriteLine("uso: medidor <triangulares|quaisquer|orfas|tamanhos>");
    return 1;
}

switch (args[0])
{
    case "triangulares":
        Triangulares();
        break;
    case "quaisquer":
        Quaisquer();
        break;
    case "orfas":
        Orfas();
        break;
    case "tamanhos":
        Tamanhos();
        break;
    default:
        Console.WriteLine($"comando desconhecido: {args[0]}");
        return 1;
}

return 0;

static void Triangulares()
{
    Console.WriteLine("Totais triangulares: toda mesa cai na escada");
    Console.WriteLine("total  lado  mesas    ciclos  maior caminho  limite k2-k");
    foreach (var l in Medidas.Triangulares([1, 2, 3, 4, 5, 6, 7, 8, 9, 10]))
    {
        Console.WriteLine($"{l.Total,-7}{l.Lado,-6}{l.Mesas,-9}{l.Ciclos,-8}{l.MaiorCaminho,-15}{l.Limite}");
    }
}

static void Quaisquer()
{
    Console.WriteLine("Totais nao triangulares: nao existe escada, so ciclos");
    Console.WriteLine("total  mesas    ciclos  maior ciclo  maior caminho  quantos sao o maior");
    foreach (var l in Medidas.Quaisquer([2, 4, 5, 7, 8, 9, 11, 12, 13, 14, 16, 17, 18, 19, 20]))
    {
        Console.WriteLine($"{l.Total,-7}{l.Mesas,-9}{l.Ciclos,-8}{l.MaiorCiclo,-13}{l.MaiorCaminho,-15}{l.NoMaiorCiclo}");
    }
}

static void Orfas()
{
    Console.WriteLine("As mesas que nenhuma mesa produz");
    Console.WriteLine("total  mesas     orfas    %");
    foreach (var l in Medidas.Orfas([6, 10, 15, 21, 28, 36, 45]))
    {
        Console.WriteLine($"{l.Total,-7}{l.Mesas,-10}{l.Orfas,-9}{l.Fracao}");
    }
}

static void Tamanhos()
{
    foreach (var total in new[] { 8, 12, 20 })
    {
        Console.WriteLine($"Tamanhos dos ciclos com {total} cartas");
        Console.WriteLine("tamanho  quantos");
        foreach (var l in Medidas.Tamanhos(total))
        {
            Console.WriteLine($"{l.Tamanho,-9}{l.Quantos}");
        }

        Console.WriteLine();
    }
}
