namespace Conde.Bulgara;

/// <summary>
/// O grafo do jogo: cada mesa aponta para exatamente uma mesa seguinte, então o
/// desenho inteiro é um bosque de árvores penduradas em ciclos. Toda partida,
/// venha de onde vier, cai num ciclo e fica girando nele para sempre.
///
/// Esta classe acha os ciclos e mede, para cada mesa, quantos lances faltam até
/// ela entrar num deles.
/// </summary>
public sealed class Ciclos
{
    private readonly Dictionary<Particao, int> _indice = [];
    private readonly List<Particao> _mesas;
    private readonly int[] _destino;
    private readonly int[] _passosAteOCiclo;
    private readonly int[] _ciclo;

    public int Total { get; }

    public List<List<Particao>> Encontrados { get; } = [];

    public Ciclos(int total)
    {
        Total = total;
        _mesas = Varredura.Todas(total);
        for (var i = 0; i < _mesas.Count; i++)
        {
            _indice[_mesas[i]] = i;
        }

        _destino = new int[_mesas.Count];
        for (var i = 0; i < _mesas.Count; i++)
        {
            _destino[i] = _indice[Paciencia.Lance(_mesas[i])];
        }

        _ciclo = new int[_mesas.Count];
        Array.Fill(_ciclo, -1);
        _passosAteOCiclo = new int[_mesas.Count];
        Array.Fill(_passosAteOCiclo, -1);

        Marcar();
    }

    private void Marcar()
    {
        // Busca com três estados (nao visto, na pilha, fechado). Quem reencontra
        // um no da propria pilha fechou um ciclo novo; quem reencontra um no
        // fechado so herda a distancia dele.
        var estado = new byte[_mesas.Count];
        var pilha = new List<int>();

        for (var inicio = 0; inicio < _mesas.Count; inicio++)
        {
            if (estado[inicio] != 0)
            {
                continue;
            }

            pilha.Clear();
            var atual = inicio;
            while (estado[atual] == 0)
            {
                estado[atual] = 1;
                pilha.Add(atual);
                atual = _destino[atual];
            }

            if (estado[atual] == 1)
            {
                // Ciclo novo: tudo da pilha a partir de `atual` faz parte dele.
                var corte = pilha.IndexOf(atual);
                var novo = new List<Particao>();
                var id = Encontrados.Count;
                for (var i = corte; i < pilha.Count; i++)
                {
                    _ciclo[pilha[i]] = id;
                    _passosAteOCiclo[pilha[i]] = 0;
                    novo.Add(_mesas[pilha[i]]);

                    // Fechar os nos do ciclo AQUI. Eles saem da pilha logo
                    // abaixo e nao passam pelo laco que fecha o resto: deixa-los
                    // em "na pilha" faz a proxima busca que cair neles achar que
                    // descobriu um ciclo novo, e procurar na pilha errada.
                    estado[pilha[i]] = 2;
                }

                Encontrados.Add(novo);
                pilha.RemoveRange(corte, pilha.Count - corte);
            }

            for (var i = pilha.Count - 1; i >= 0; i--)
            {
                var no = pilha[i];
                var seguinte = _destino[no];
                _ciclo[no] = _ciclo[seguinte];
                _passosAteOCiclo[no] = _passosAteOCiclo[seguinte] + 1;
            }

            foreach (var no in pilha)
            {
                estado[no] = 2;
            }

            estado[atual] = 2;
        }
    }

    public IReadOnlyList<Particao> Mesas => _mesas;

    public int PassosAteOCiclo(Particao mesa) => _passosAteOCiclo[_indice[mesa]];

    public int CicloDe(Particao mesa) => _ciclo[_indice[mesa]];

    public int MaiorCaminho()
    {
        var maior = 0;
        foreach (var p in _passosAteOCiclo)
        {
            maior = Math.Max(maior, p);
        }

        return maior;
    }

    /// <summary>
    /// As mesas que ninguém produz: nenhuma mesa leva a elas. Elas só podem
    /// aparecer como posição inicial, e nunca mais são vistas depois disso.
    /// </summary>
    public List<Particao> SemAntecessor()
    {
        var temAntecessor = new bool[_mesas.Count];
        foreach (var d in _destino)
        {
            temAntecessor[d] = true;
        }

        var orfas = new List<Particao>();
        for (var i = 0; i < _mesas.Count; i++)
        {
            if (!temAntecessor[i])
            {
                orfas.Add(_mesas[i]);
            }
        }

        return orfas;
    }
}
