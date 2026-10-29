using Conde.Bulgara;
using Xunit;

namespace Conde.Bulgara.Testes;

public class CiclosTestes
{
    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(6)]
    [InlineData(10)]
    [InlineData(15)]
    [InlineData(21)]
    public void ComTotalTriangularHaUmCicloSoEEleEAEscada(int total)
    {
        var ciclos = new Ciclos(total);
        Assert.Single(ciclos.Encontrados);
        Assert.Single(ciclos.Encontrados[0]);
        Assert.Equal(Paciencia.Escada(total), ciclos.Encontrados[0][0]);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(12)]
    [InlineData(17)]
    [InlineData(18)]
    public void SemTotalTriangularNenhumCicloTemTamanhoUm(int total)
    {
        var ciclos = new Ciclos(total);
        Assert.NotEmpty(ciclos.Encontrados);
        Assert.All(ciclos.Encontrados, c => Assert.True(c.Count > 1));
    }

    [Fact]
    public void TodaMesaChegaNumCicloEFicaNele()
    {
        for (var total = 1; total <= 20; total++)
        {
            var ciclos = new Ciclos(total);
            foreach (var mesa in ciclos.Mesas)
            {
                var passos = ciclos.PassosAteOCiclo(mesa);
                Assert.True(passos >= 0);

                var atual = mesa;
                for (var i = 0; i < passos; i++)
                {
                    atual = Paciencia.Lance(atual);
                }

                Assert.Equal(0, ciclos.PassosAteOCiclo(atual));
                Assert.Equal(ciclos.CicloDe(mesa), ciclos.CicloDe(atual));
            }
        }
    }

    [Fact]
    public void QuemEstaNoCicloVoltaAoProprioLugarNoTamanhoDoCiclo()
    {
        for (var total = 1; total <= 20; total++)
        {
            var ciclos = new Ciclos(total);
            foreach (var c in ciclos.Encontrados)
            {
                foreach (var mesa in c)
                {
                    var atual = mesa;
                    for (var i = 0; i < c.Count; i++)
                    {
                        atual = Paciencia.Lance(atual);
                    }

                    Assert.Equal(mesa, atual);
                }
            }
        }
    }

    [Fact]
    public void OsCiclosNaoCompartilhamMesa()
    {
        for (var total = 1; total <= 22; total++)
        {
            var ciclos = new Ciclos(total);
            var todas = ciclos.Encontrados.SelectMany(c => c).ToList();
            Assert.Equal(todas.Count, todas.Distinct().Count());
        }
    }

    [Fact]
    public void AsOrfasSaoExatamenteAsQueNinguemProduz()
    {
        var ciclos = new Ciclos(15);
        var produzidas = ciclos.Mesas.Select(Paciencia.Lance).ToHashSet();
        foreach (var mesa in ciclos.Mesas)
        {
            Assert.Equal(!produzidas.Contains(mesa), ciclos.SemAntecessor().Contains(mesa));
        }
    }

    [Fact]
    public void NenhumaMesaDeCicloEOrfa()
    {
        for (var total = 1; total <= 20; total++)
        {
            var ciclos = new Ciclos(total);
            var orfas = ciclos.SemAntecessor().ToHashSet();
            foreach (var c in ciclos.Encontrados)
            {
                Assert.All(c, m => Assert.DoesNotContain(m, orfas));
            }
        }
    }

    [Fact]
    public void OMaiorCaminhoEZeroQuandoTudoJaEstaNoCiclo()
    {
        Assert.Equal(0, new Ciclos(1).MaiorCaminho());
        Assert.Equal(0, new Ciclos(2).MaiorCaminho());
    }

    [Theory]
    [InlineData(6, 6)]
    [InlineData(10, 12)]
    [InlineData(15, 20)]
    [InlineData(21, 30)]
    public void OMaiorCaminhoDosTriangularesEExatamenteOLimiteClassico(int total, int esperado)
    {
        var k = Paciencia.LadoTriangular(total);
        Assert.Equal(esperado, (k * k) - k);
        Assert.Equal(esperado, new Ciclos(total).MaiorCaminho());
    }
}
