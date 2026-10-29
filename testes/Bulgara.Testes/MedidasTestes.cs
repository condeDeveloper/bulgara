using Conde.Bulgara;
using Xunit;

namespace Conde.Bulgara.Testes;

/// <summary>
/// Os números do README presos aqui. Nada entra neste arquivo que eu não tenha
/// visto o medidor imprimir.
/// </summary>
public class MedidasTestes
{
    [Theory]
    [InlineData(1, 1, 1, 0, 0)]
    [InlineData(2, 3, 3, 2, 2)]
    [InlineData(3, 6, 11, 6, 6)]
    [InlineData(4, 10, 42, 12, 12)]
    [InlineData(5, 15, 176, 20, 20)]
    [InlineData(6, 21, 792, 30, 30)]
    [InlineData(7, 28, 3718, 42, 42)]
    [InlineData(8, 36, 17977, 56, 56)]
    public void ATabelaDosTriangulares(int lado, int total, long mesas, int maiorCaminho, int limite)
    {
        var l = Medidas.Triangulares([lado])[0];
        Assert.Equal(total, l.Total);
        Assert.Equal(mesas, l.Mesas);
        Assert.Equal(1, l.Ciclos);
        Assert.Equal(maiorCaminho, l.MaiorCaminho);
        Assert.Equal(limite, l.Limite);
    }

    [Fact]
    public void OLimiteClassicoEAtingidoEmTodosOsLadosMedidos()
    {
        foreach (var l in Medidas.Triangulares([1, 2, 3, 4, 5, 6, 7, 8, 9]))
        {
            Assert.Equal(l.Limite, l.MaiorCaminho);
        }
    }

    [Fact]
    public void OLadoDezTambemBateNoLimite()
    {
        var l = Medidas.Triangulares([10])[0];
        Assert.Equal(55, l.Total);
        Assert.Equal(451276, l.Mesas);
        Assert.Equal(90, l.MaiorCaminho);
        Assert.Equal(90, l.Limite);
    }

    [Theory]
    [InlineData(2, 2, 1, 2, 0)]
    [InlineData(4, 5, 1, 3, 2)]
    [InlineData(5, 7, 1, 3, 3)]
    [InlineData(7, 15, 1, 4, 4)]
    [InlineData(8, 22, 2, 4, 5)]
    [InlineData(9, 30, 1, 4, 7)]
    [InlineData(11, 56, 1, 5, 8)]
    [InlineData(12, 77, 2, 5, 8)]
    [InlineData(13, 101, 2, 5, 9)]
    [InlineData(14, 135, 1, 5, 14)]
    [InlineData(16, 231, 1, 6, 15)]
    [InlineData(17, 297, 3, 6, 12)]
    [InlineData(18, 385, 4, 6, 13)]
    [InlineData(19, 490, 3, 6, 16)]
    [InlineData(20, 627, 1, 6, 23)]
    public void ATabelaDosNaoTriangulares(int total, long mesas, int ciclos, int maiorCiclo, int maiorCaminho)
    {
        var l = Medidas.Quaisquer([total])[0];
        Assert.Equal(mesas, l.Mesas);
        Assert.Equal(ciclos, l.Ciclos);
        Assert.Equal(maiorCiclo, l.MaiorCiclo);
        Assert.Equal(maiorCaminho, l.MaiorCaminho);
    }

    [Fact]
    public void OMaiorCicloTemOLadoDaEscadaQueNaoCabe()
    {
        // Entre a escada de lado k-1 e a de lado k, o maior ciclo tem tamanho k.
        foreach (var l in Medidas.Quaisquer([2, 4, 5, 7, 8, 9, 11, 12, 13, 14, 16, 17, 18, 19, 20]))
        {
            var k = 0;
            var soma = 0;
            while (soma < l.Total)
            {
                k++;
                soma += k;
            }

            Assert.Equal(k, l.MaiorCiclo);
        }
    }

    [Fact]
    public void OsTotaisVizinhosDeUmaEscadaTemUmCicloSo()
    {
        foreach (var total in new[] { 2, 4, 5, 7, 9, 11, 14, 16, 20 })
        {
            Assert.Equal(1, Medidas.Quaisquer([total])[0].Ciclos);
        }
    }

    [Theory]
    [InlineData(6, 11, 3, 27.3)]
    [InlineData(10, 42, 14, 33.3)]
    [InlineData(15, 176, 66, 37.5)]
    [InlineData(21, 792, 311, 39.3)]
    [InlineData(28, 3718, 1510, 40.6)]
    [InlineData(36, 17977, 7507, 41.8)]
    [InlineData(45, 89134, 38032, 42.7)]
    public void ATabelaDasOrfas(int total, long mesas, int orfas, double fracao)
    {
        var l = Medidas.Orfas([total])[0];
        Assert.Equal(mesas, l.Mesas);
        Assert.Equal(orfas, l.Orfas);
        Assert.Equal(fracao, l.Fracao);
    }

    [Fact]
    public void AFracaoDeOrfasSobeSempre()
    {
        var linhas = Medidas.Orfas([6, 10, 15, 21, 28, 36, 45]);
        for (var i = 1; i < linhas.Count; i++)
        {
            Assert.True(linhas[i].Fracao > linhas[i - 1].Fracao);
        }
    }

    [Fact]
    public void OsDoisCiclosDeOitoCartasTemTamanhosDiferentes()
    {
        var linhas = Medidas.Tamanhos(8);
        Assert.Equal(2, linhas.Count);
        Assert.Equal(2, linhas[0].Tamanho);
        Assert.Equal(1, linhas[0].Quantos);
        Assert.Equal(4, linhas[1].Tamanho);
        Assert.Equal(1, linhas[1].Quantos);
    }

    [Fact]
    public void OsDoisCiclosDeDozeCartasTemOMesmoTamanho()
    {
        var linhas = Medidas.Tamanhos(12);
        Assert.Single(linhas);
        Assert.Equal(5, linhas[0].Tamanho);
        Assert.Equal(2, linhas[0].Quantos);
    }

    [Fact]
    public void ComVinteCartasHaUmCicloSoDeSeisMesas()
    {
        var linhas = Medidas.Tamanhos(20);
        Assert.Single(linhas);
        Assert.Equal(6, linhas[0].Tamanho);
        Assert.Equal(1, linhas[0].Quantos);
    }
}
