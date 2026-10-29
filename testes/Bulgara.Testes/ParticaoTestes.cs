using Conde.Bulgara;
using Xunit;

namespace Conde.Bulgara.Testes;

public class ParticaoTestes
{
    [Fact]
    public void AsPilhasFicamDaMaiorParaAMenor()
    {
        var p = new Particao([2, 5, 1, 3]);
        Assert.Equal([5, 3, 2, 1], p.Pilhas);
    }

    [Fact]
    public void AOrdemNaMesaNaoEInformacao()
    {
        Assert.Equal(new Particao([3, 2, 1]), new Particao([1, 3, 2]));
        Assert.Equal(new Particao([3, 2, 1]).GetHashCode(), new Particao([2, 1, 3]).GetHashCode());
    }

    [Fact]
    public void PilhaVaziaNaoExiste()
    {
        var p = new Particao([3, 0, 2, 0]);
        Assert.Equal([3, 2], p.Pilhas);
        Assert.Equal(2, p.Quantidade);
    }

    [Fact]
    public void PilhaNegativaNaoEAceita()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Particao([3, -1]));
    }

    [Theory]
    [InlineData(new[] { 3, 2, 1 }, 6)]
    [InlineData(new[] { 7 }, 7)]
    [InlineData(new int[0], 0)]
    [InlineData(new[] { 1, 1, 1, 1 }, 4)]
    public void OTotalEASomaDasPilhas(int[] pilhas, int esperado)
    {
        Assert.Equal(esperado, new Particao(pilhas).Total);
    }

    [Fact]
    public void AMaiorDeUmaMesaVaziaEZero()
    {
        Assert.Equal(0, new Particao([]).Maior);
        Assert.Equal(0, new Particao([]).Quantidade);
    }

    [Fact]
    public void DuasMesasDiferentesNaoSaoIguais()
    {
        Assert.NotEqual(new Particao([3, 2, 1]), new Particao([4, 2]));
        Assert.NotEqual(new Particao([3, 3]), new Particao([3, 2, 1]));
        Assert.False(new Particao([1]).Equals(null));
        Assert.False(new Particao([1]).Equals("texto"));
    }

    [Fact]
    public void AComparacaoPoeAMesaMaiorPrimeiro()
    {
        var lista = new List<Particao>
        {
            new([1, 1, 1]),
            new([3]),
            new([2, 1]),
        };

        lista.Sort();
        Assert.Equal("3", lista[0].ToString());
        Assert.Equal("2+1", lista[1].ToString());
        Assert.Equal("1+1+1", lista[2].ToString());
    }

    [Fact]
    public void OTextoMostraASomaEAVaziaMostraParenteses()
    {
        Assert.Equal("4+2+1", new Particao([1, 4, 2]).ToString());
        Assert.Equal("()", new Particao([]).ToString());
    }

    [Fact]
    public void NuloNaoEAceito()
    {
        Assert.Throws<ArgumentNullException>(() => new Particao(null!));
    }
}
