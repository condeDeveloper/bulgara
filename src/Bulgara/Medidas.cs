namespace Conde.Bulgara;

/// <summary>
/// As tabelas do README. Todo número que aparece lá sai daqui, e cada um está
/// preso num teste.
/// </summary>
public static class Medidas
{
    public readonly record struct LinhaTriangular(int Total, int Lado, long Mesas, int Ciclos, int MaiorCaminho, int Limite);

    public readonly record struct LinhaQualquer(int Total, long Mesas, int Ciclos, int MaiorCiclo, int MaiorCaminho, int NoMaiorCiclo);

    public readonly record struct LinhaOrfas(int Total, long Mesas, int Orfas, double Fracao);

    public readonly record struct LinhaTamanho(int Tamanho, int Quantos);

    /// <summary>
    /// Os totais triangulares, onde o teorema diz que existe um ponto fixo e
    /// toda mesa chega nele. O limite comparado é o <c>k² − k</c> clássico.
    /// </summary>
    public static List<LinhaTriangular> Triangulares(IEnumerable<int> lados)
    {
        var linhas = new List<LinhaTriangular>();
        foreach (var k in lados)
        {
            var total = k * (k + 1) / 2;
            var ciclos = new Ciclos(total);
            linhas.Add(new LinhaTriangular(
                total,
                k,
                Varredura.Quantidade(total),
                ciclos.Encontrados.Count,
                ciclos.MaiorCaminho(),
                (k * k) - k));
        }

        return linhas;
    }

    /// <summary>Os totais que não são triangulares, onde não há ponto fixo.</summary>
    public static List<LinhaQualquer> Quaisquer(IEnumerable<int> totais)
    {
        var linhas = new List<LinhaQualquer>();
        foreach (var total in totais)
        {
            var ciclos = new Ciclos(total);
            var maior = 0;
            var noMaior = 0;
            foreach (var c in ciclos.Encontrados)
            {
                if (c.Count > maior)
                {
                    maior = c.Count;
                }
            }

            foreach (var c in ciclos.Encontrados)
            {
                if (c.Count == maior)
                {
                    noMaior++;
                }
            }

            linhas.Add(new LinhaQualquer(
                total,
                Varredura.Quantidade(total),
                ciclos.Encontrados.Count,
                maior,
                ciclos.MaiorCaminho(),
                noMaior));
        }

        return linhas;
    }

    /// <summary>
    /// As mesas que nenhuma mesa produz. Elas medem o quanto o jogo esquece:
    /// uma vez dado o primeiro lance, essa parte do espaço some para sempre.
    /// </summary>
    public static List<LinhaOrfas> Orfas(IEnumerable<int> totais)
    {
        var linhas = new List<LinhaOrfas>();
        foreach (var total in totais)
        {
            var ciclos = new Ciclos(total);
            var mesas = Varredura.Quantidade(total);
            var orfas = ciclos.SemAntecessor().Count;
            linhas.Add(new LinhaOrfas(total, mesas, orfas, Math.Round(100.0 * orfas / mesas, 1)));
        }

        return linhas;
    }

    /// <summary>Quantos ciclos de cada tamanho existem num total dado.</summary>
    public static List<LinhaTamanho> Tamanhos(int total)
    {
        var ciclos = new Ciclos(total);
        var conta = new Dictionary<int, int>();
        foreach (var c in ciclos.Encontrados)
        {
            conta[c.Count] = conta.GetValueOrDefault(c.Count) + 1;
        }

        var linhas = new List<LinhaTamanho>();
        foreach (var chave in conta.Keys.Order())
        {
            linhas.Add(new LinhaTamanho(chave, conta[chave]));
        }

        return linhas;
    }
}
