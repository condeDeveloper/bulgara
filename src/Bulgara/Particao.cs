namespace Conde.Bulgara;

/// <summary>
/// Uma partição de um inteiro: as pilhas de cartas, sempre guardadas da maior
/// para a menor.
///
/// A ordem das pilhas na mesa não é informação — duas mesas com as mesmas
/// pilhas em ordens diferentes são a mesma posição do jogo. Guardar a forma
/// ordenada é o que torna a comparação e o dicionário possíveis, e é também o
/// que impede a varredura de contar a mesma posição duas vezes.
/// </summary>
public sealed class Particao : IEquatable<Particao>, IComparable<Particao>
{
    private readonly int[] _pilhas;
    private readonly int _hash;

    public Particao(IEnumerable<int> pilhas)
    {
        ArgumentNullException.ThrowIfNull(pilhas);
        var lista = pilhas.Where(p => p != 0).ToArray();
        if (Array.Exists(lista, p => p < 0))
        {
            throw new ArgumentOutOfRangeException(nameof(pilhas), "pilha negativa");
        }

        Array.Sort(lista);
        Array.Reverse(lista);
        _pilhas = lista;

        var h = 17;
        foreach (var p in _pilhas)
        {
            h = (h * 31) + p;
        }

        _hash = h;
    }

    public IReadOnlyList<int> Pilhas => _pilhas;

    public int Quantidade => _pilhas.Length;

    public int Total
    {
        get
        {
            var soma = 0;
            foreach (var p in _pilhas)
            {
                soma += p;
            }

            return soma;
        }
    }

    public int Maior => _pilhas.Length == 0 ? 0 : _pilhas[0];

    public bool Equals(Particao? outra)
    {
        if (outra is null)
        {
            return false;
        }

        if (_pilhas.Length != outra._pilhas.Length)
        {
            return false;
        }

        for (var i = 0; i < _pilhas.Length; i++)
        {
            if (_pilhas[i] != outra._pilhas[i])
            {
                return false;
            }
        }

        return true;
    }

    public override bool Equals(object? obj) => Equals(obj as Particao);

    public override int GetHashCode() => _hash;

    public int CompareTo(Particao? outra)
    {
        if (outra is null)
        {
            return 1;
        }

        var n = Math.Min(_pilhas.Length, outra._pilhas.Length);
        for (var i = 0; i < n; i++)
        {
            if (_pilhas[i] != outra._pilhas[i])
            {
                return outra._pilhas[i].CompareTo(_pilhas[i]);
            }
        }

        return outra._pilhas.Length.CompareTo(_pilhas.Length);
    }

    public override string ToString() => _pilhas.Length == 0 ? "()" : string.Join("+", _pilhas);
}
