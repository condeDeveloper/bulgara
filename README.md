# Paciência búlgara

Espalhe um baralho em pilhas de qualquer tamanho. Agora tire **uma carta de cada pilha**
e junte as cartas tiradas numa pilha nova. Repita.

Não há escolha nenhuma: dada a mesa, a mesa seguinte está determinada. O jogo é uma
função, e o resultado é famoso: quando o total de cartas é um número **triangular**
(1, 3, 6, 10, 15, 21, 28...), toda mesa, sem exceção, acaba na escada `k, k-1, ..., 1`
e fica lá para sempre.

Aqui o enunciado não é citado: para cada total, **todas** as mesas possíveis são geradas
e todas são seguidas até o fim.

```
dotnet test testes/Bulgara.Testes/Bulgara.Testes.csproj -c Release
dotnet run  --project ferramentas/Medidor/Medidor.csproj -c Release -- triangulares
dotnet run  --project ferramentas/Medidor/Medidor.csproj -c Release -- quaisquer
dotnet run  --project ferramentas/Medidor/Medidor.csproj -c Release -- orfas
dotnet run  --project ferramentas/Medidor/Medidor.csproj -c Release -- tamanhos
```

## O domínio e o juiz

O conjunto de mesas com `n` cartas é o conjunto das partições de `n`, e é finito: com 28
cartas são 3.718 mesas, com 55 são 451.276. O gerador produz todas elas, e quem confere
se ele não esqueceu nem repetiu nenhuma é a **recorrência de Euler**, que conta as
partições sem olhar partição nenhuma. As duas contagens batem em todos os totais testados,
e a recorrência vai além do que o gerador alcança: `p(100) = 190.569.292`.

Como cada mesa aponta para exatamente uma seguinte, o desenho inteiro é um bosque de
árvores penduradas em ciclos. Achar os ciclos e medir, para cada mesa, quantos lances
faltam até entrar num deles, responde todas as perguntas abaixo de uma vez.

## Os achados

### 1. O limite clássico não é folgado: ele é atingido, nos dez lados medidos

O livro diz que com `k(k+1)/2` cartas toda mesa chega na escada em no máximo `k² − k`
lances. É apresentado como um limite — e limite costuma sobrar.

```
total  lado  mesas    ciclos  maior caminho  limite k2-k
1      1     1        1       0              0
3      2     3        1       2              2
6      3     11       1       6              6
10     4     42       1       12             12
15     5     176      1       20             20
21     6     792      1       30             30
28     7     3718     1       42             42
36     8     17977    1       56             56
45     9     89134    1       72             72
55     10    451276   1       90             90
```

**A coluna medida e a coluna do limite são a mesma coluna.** Em todos os dez lados existe
pelo menos uma mesa que demora exatamente `k² − k` lances, nem um a menos. O número não é
um teto confortável: é o pior caso, e ele acontece.

### 2. O maior ciclo tem o lado da escada que não cabe

Quando o total não é triangular não existe ponto fixo, e o jogo gira para sempre. O
tamanho do maior ciclo não é irregular:

```
total  mesas    ciclos  maior ciclo  maior caminho  quantos sao o maior
2      2        1       2            0              1
4      5        1       3            2              1
5      7        1       3            3              1
7      15       1       4            4              1
8      22       2       4            5              1
9      30       1       4            7              1
11     56       1       5            8              1
12     77       2       5            8              2
13     101      2       5            9              2
14     135      1       5            14             1
16     231      1       6            15             1
17     297      3       6            12             2
18     385      4       6            13             3
19     490      3       6            16             2
20     627      1       6            23             1
```

Entre a escada de lado `k−1` e a de lado `k`, **o maior ciclo tem exatamente `k` mesas**,
nos quinze totais medidos. O jogo não consegue parar porque falta carta para montar a
escada de lado `k`, e o tamanho do giro é justamente esse `k`.

### 3. Os vizinhos de uma escada têm um ciclo só

Uma carta a menos ou uma carta a mais que um total triangular — 2, 4, 5, 7, 9, 11, 14,
16, 20 — e o resultado é sempre **um único ciclo**, para onde tudo converge. No meio do
intervalo a conta se desarruma: com 18 cartas há **quatro** ciclos, três deles do mesmo
tamanho.

E os ciclos não precisam ser parecidos entre si. Com 8 cartas há dois, e eles têm tamanhos
**diferentes**: um de 2 mesas e um de 4. Com 12 cartas há dois, e os dois têm 5.

### 4. O jogo esquece quase metade do espaço no primeiro lance

Há mesas que nenhuma mesa produz. Elas só podem existir como posição inicial: depois do
primeiro lance desaparecem e nunca mais voltam.

```
total  mesas     orfas    %
6      11        3        27.3
10     42        14       33.3
15     176       66       37.5
21     792       311      39.3
28     3718      1510     40.6
36     17977     7507     41.8
45     89134     38032    42.7
```

A fração sobe a cada escada e já passa de 42% com 45 cartas. O jogo é uma função que não
é injetiva nem de longe: **quase metade do baralho de configurações não tem passado**, só
futuro.

## O que as medidas me corrigiram

- **Eu achava que `k² − k` sobrava.** Limite em livro quase sempre sobra, e eu esperava
  medir algo em torno da metade dele. A medida deu o valor exato em **todos** os dez
  lados. Uma tabela em que a coluna medida é igual à coluna prevista é o tipo de coisa que
  eu desconfio de cara, então gastei tempo procurando o erro antes de aceitar: o erro não
  estava lá.

- **Eu não fechei os nós do ciclo e o programa explodiu.** A busca marca cada mesa como
  não vista, na pilha ou fechada. Quando eu achava um ciclo com mais de uma mesa, as mesas
  dele saíam da pilha sem serem marcadas como fechadas, e a próxima busca que caísse nelas
  achava que tinha descoberto um ciclo novo e procurava numa pilha onde elas não estavam.
  O caso de **um** ciclo de uma mesa só — que é exatamente o caso triangular — funcionava
  perfeitamente, e por isso a primeira tabela saiu inteira e certa antes de a segunda
  quebrar. **O caso fácil ter passado foi o que mais me atrasou.**

- **Eu digitei `p(60) = 1.295.971` dentro de um teste.** O valor é 966.467. Foi um número
  que eu "sabia", posto num arquivo que existe justamente para impedir que eu ponha
  números que eu "sei". Já é a segunda vez que escorrego nisso, e a regra agora é curta:
  no arquivo de medidas só entra o que eu vi o programa imprimir.

- **Eu esperava bagunça nos totais não triangulares.** Sem ponto fixo, imaginei muitos
  ciclos pequenos e irregulares. O que existe é um ciclo só na maioria dos casos, com
  tamanho previsível, e a bagunça fica concentrada no meio do intervalo entre duas
  escadas.

## Estrutura

```
src/Bulgara/
  Particao.cs    as pilhas da mesa, sempre da maior para a menor
  Paciencia.cs   a regra, a escada e o lado triangular
  Varredura.cs   todas as mesas de um total, com a recorrencia de Euler como juiz
  Ciclos.cs      o grafo do jogo: ciclos, distancias e mesas sem passado
  Medidas.cs     as tabelas deste README
testes/Bulgara.Testes/
ferramentas/Medidor/
```

115 testes.
