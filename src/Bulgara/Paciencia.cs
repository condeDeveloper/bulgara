namespace Conde.Bulgara;

/// <summary>
/// A regra da paciência búlgara, que cabe numa frase: tire uma carta de cada
/// pilha e junte as cartas tiradas numa pilha nova.
///
/// A pilha que tinha uma carta some. A pilha nova tem tantas cartas quantas
/// pilhas havia na mesa. Nada mais acontece, e não há escolha nenhuma: dada uma
/// mesa, a mesa seguinte está determinada. O jogo é uma função.
/// </summary>
public static class Paciencia
{
    public static Particao Lance(Particao mesa)
    {
        ArgumentNullException.ThrowIfNull(mesa);
        var novas = new List<int>(mesa.Quantidade + 1) { mesa.Quantidade };
        foreach (var pilha in mesa.Pilhas)
        {
            if (pilha > 1)
            {
                novas.Add(pilha - 1);
            }
        }

        return new Particao(novas);
    }

    /// <summary>O caminho a partir de uma mesa, com o número de lances pedido.</summary>
    public static List<Particao> Caminho(Particao inicio, int lances)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(lances);
        var caminho = new List<Particao>(lances + 1) { inicio };
        var atual = inicio;
        for (var i = 0; i < lances; i++)
        {
            atual = Lance(atual);
            caminho.Add(atual);
        }

        return caminho;
    }

    /// <summary>
    /// A escada <c>k, k-1, ..., 1</c>, que é o ponto fixo quando o total de
    /// cartas é um número triangular. Com qualquer outro total ela não existe.
    /// </summary>
    public static Particao? Escada(int total)
    {
        var k = 0;
        var soma = 0;
        while (soma < total)
        {
            k++;
            soma += k;
        }

        if (soma != total)
        {
            return null;
        }

        var pilhas = new List<int>(k);
        for (var i = k; i >= 1; i--)
        {
            pilhas.Add(i);
        }

        return new Particao(pilhas);
    }

    /// <summary>O lado triangular de um total, ou zero se ele não for triangular.</summary>
    public static int LadoTriangular(int total)
    {
        var k = 0;
        var soma = 0;
        while (soma < total)
        {
            k++;
            soma += k;
        }

        return soma == total ? k : 0;
    }
}
