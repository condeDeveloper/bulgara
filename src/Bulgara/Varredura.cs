namespace Conde.Bulgara;

/// <summary>
/// A enumeração de TODAS as partições de um inteiro. É o domínio inteiro do
/// problema, e é finito: com 28 cartas são 3.718 mesas possíveis, e nenhuma
/// mesa de 28 cartas fica de fora.
///
/// Sem isso, qualquer frase sobre "toda mesa chega na escada" seria um palpite
/// a partir de alguns sorteios.
/// </summary>
public static class Varredura
{
    public static List<Particao> Todas(int total)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(total);
        var todas = new List<Particao>();
        var atual = new List<int>();
        Gerar(total, total, atual, todas);
        return todas;
    }

    private static void Gerar(int resto, int maior, List<int> atual, List<Particao> saida)
    {
        if (resto == 0)
        {
            saida.Add(new Particao(atual));
            return;
        }

        var limite = Math.Min(resto, maior);
        for (var p = limite; p >= 1; p--)
        {
            atual.Add(p);
            Gerar(resto - p, p, atual, saida);
            atual.RemoveAt(atual.Count - 1);
        }
    }

    /// <summary>
    /// A quantidade de partições pela recorrência de Euler, que não olha nenhuma
    /// partição. É o juiz da enumeração: se o gerador esquecer uma mesa ou
    /// repetir outra, os dois números deixam de bater.
    /// </summary>
    public static long Quantidade(int total)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(total);
        var p = new long[total + 1];
        p[0] = 1;

        for (var n = 1; n <= total; n++)
        {
            long soma = 0;
            for (var k = 1; ; k++)
            {
                var g1 = (k * ((3 * k) - 1)) / 2;
                var g2 = (k * ((3 * k) + 1)) / 2;
                if (g1 > n && g2 > n)
                {
                    break;
                }

                var sinal = k % 2 == 1 ? 1 : -1;
                if (g1 <= n)
                {
                    soma += sinal * p[n - g1];
                }

                if (g2 <= n)
                {
                    soma += sinal * p[n - g2];
                }
            }

            p[n] = soma;
        }

        return p[total];
    }
}
