using Conde.Bulgara;
using Xunit;

namespace Conde.Bulgara.Testes;

public class VarreduraTestes
{
    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    [InlineData(4, 5)]
    [InlineData(5, 7)]
    [InlineData(6, 11)]
    [InlineData(10, 42)]
    [InlineData(15, 176)]
    [InlineData(20, 627)]
    [InlineData(28, 3718)]
    public void OGeradorConcordaComARecorrenciaDeEuler(int total, int esperado)
    {
        Assert.Equal(esperado, Varredura.Todas(total).Count);
        Assert.Equal(esperado, Varredura.Quantidade(total));
    }

    [Fact]
    public void ARecorrenciaVaiLongeDemaisParaOGeradorAlcancar()
    {
        Assert.Equal(204226, Varredura.Quantidade(50));
        Assert.Equal(966467, Varredura.Quantidade(60));
        Assert.Equal(190569292, Varredura.Quantidade(100));
    }

    [Fact]
    public void NenhumaMesaAparecemDuasVezes()
    {
        for (var total = 1; total <= 25; total++)
        {
            var todas = Varredura.Todas(total);
            Assert.Equal(todas.Count, todas.Distinct().Count());
        }
    }

    [Fact]
    public void TodaMesaTemOTotalPedido()
    {
        for (var total = 1; total <= 25; total++)
        {
            Assert.All(Varredura.Todas(total), m => Assert.Equal(total, m.Total));
        }
    }

    [Fact]
    public void AMesaDeZeroCartasEUmaSoEVazia()
    {
        var todas = Varredura.Todas(0);
        Assert.Single(todas);
        Assert.Equal(0, todas[0].Quantidade);
    }

    [Fact]
    public void AMesaDeUmaPilhaSoEADeCartasSozinhasEstaoSempreLa()
    {
        var todas = Varredura.Todas(9);
        Assert.Contains(new Particao([9]), todas);
        Assert.Contains(new Particao([1, 1, 1, 1, 1, 1, 1, 1, 1]), todas);
    }

    [Fact]
    public void TotalNegativoNaoEAceito()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Varredura.Todas(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Varredura.Quantidade(-1));
    }
}
