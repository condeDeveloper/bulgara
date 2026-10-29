using Conde.Bulgara;
using Xunit;

namespace Conde.Bulgara.Testes;

public class PacienciaTestes
{
    [Fact]
    public void OLanceTiraUmaDeCadaPilhaEJuntaOTirado()
    {
        var depois = Paciencia.Lance(new Particao([4, 2, 1]));
        Assert.Equal([3, 3, 1], depois.Pilhas);
    }

    [Fact]
    public void APilhaDeUmaCartaSome()
    {
        var depois = Paciencia.Lance(new Particao([1, 1, 1]));
        Assert.Equal([3], depois.Pilhas);
    }

    [Fact]
    public void APilhaNovaTemTantasCartasQuantasPilhasHavia()
    {
        foreach (var mesa in Varredura.Todas(12))
        {
            var depois = Paciencia.Lance(mesa);
            Assert.Contains(mesa.Quantidade, depois.Pilhas);
        }
    }

    [Fact]
    public void OLanceNaoCriaNemPerdeCarta()
    {
        for (var total = 1; total <= 20; total++)
        {
            foreach (var mesa in Varredura.Todas(total))
            {
                Assert.Equal(total, Paciencia.Lance(mesa).Total);
            }
        }
    }

    [Fact]
    public void AEscadaEUmPontoFixo()
    {
        foreach (var k in new[] { 1, 2, 3, 4, 5, 6, 7 })
        {
            var total = k * (k + 1) / 2;
            var escada = Paciencia.Escada(total);
            Assert.NotNull(escada);
            Assert.Equal(escada, Paciencia.Lance(escada));
        }
    }

    [Theory]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(11)]
    public void SemTotalTriangularNaoExisteEscada(int total)
    {
        Assert.Null(Paciencia.Escada(total));
        Assert.Equal(0, Paciencia.LadoTriangular(total));
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(3, 2)]
    [InlineData(6, 3)]
    [InlineData(10, 4)]
    [InlineData(15, 5)]
    [InlineData(21, 6)]
    [InlineData(28, 7)]
    public void OLadoTriangular(int total, int lado)
    {
        Assert.Equal(lado, Paciencia.LadoTriangular(total));
        var escada = Paciencia.Escada(total);
        Assert.NotNull(escada);
        Assert.Equal(lado, escada.Quantidade);
        Assert.Equal(lado, escada.Maior);
    }

    [Fact]
    public void SemTotalTriangularNenhumaMesaEPontoFixo()
    {
        foreach (var total in new[] { 2, 4, 5, 7, 8, 9, 11, 12, 13, 14 })
        {
            foreach (var mesa in Varredura.Todas(total))
            {
                Assert.NotEqual(mesa, Paciencia.Lance(mesa));
            }
        }
    }

    [Fact]
    public void ComTotalTriangularSoAEscadaEPontoFixo()
    {
        foreach (var total in new[] { 1, 3, 6, 10, 15, 21 })
        {
            var fixos = Varredura.Todas(total).Where(m => Paciencia.Lance(m).Equals(m)).ToList();
            Assert.Single(fixos);
            Assert.Equal(Paciencia.Escada(total), fixos[0]);
        }
    }

    [Fact]
    public void OCaminhoComecaNaMesaDada()
    {
        var inicio = new Particao([5, 1]);
        var caminho = Paciencia.Caminho(inicio, 4);
        Assert.Equal(5, caminho.Count);
        Assert.Equal(inicio, caminho[0]);
        for (var i = 1; i < caminho.Count; i++)
        {
            Assert.Equal(Paciencia.Lance(caminho[i - 1]), caminho[i]);
        }
    }

    [Fact]
    public void LancesNegativosNaoSaoAceitos()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Paciencia.Caminho(new Particao([3]), -1));
        Assert.Throws<ArgumentNullException>(() => Paciencia.Lance(null!));
    }
}
