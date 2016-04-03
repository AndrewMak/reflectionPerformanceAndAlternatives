# Reflection Performance And Alternatives
Um estudo sobre reflection, performance e alternativas.

Este projeto mostra os tempos gastos em um benchmark org�nico para a instancia��o e preenchimento de propriedades de uma classe usando diversas metodologias, al�m da chamada de um m�todo que realiza um c�lculo simples.

## Simple Reflection

Reflection simples, criando instancias com Activator.CreateInstance() e setando propriedades com GetType(Classe).GetProperty("Propriedade").SetValue(valor).

## Cached Reflection

Reflection que obt�m todos os metadados necess�rios para a implementa��o do Simple Reflection (ou seja, a obten��o das propriedades � feita uma �nica vez e, a partir do PropertyInfo obtido, o SetValue(valor) � executado).

## Dynamic

O mesmo procedimento, mas utilizando-se da palavra-chave dynamic do C#.

## Emit IL

Emiss�o de IL para as opera��es (instancia��o, preenchimento e chamada de m�todo com retorno).

## Original

O teste em si sem nenhum tipo de artif�cio (usando C# comum como escrevemos no dia-a-dia).

# Resultados
```Running 1/3
Running OriginalTest        ... done in 3.392 ms
Running EmitILTest          ... done in 3.582 ms
Running DynamicTest         ... done in 4.489 ms
Running CachedReflectionTest... done in 7.681 ms
Running SimpleReflectionTest... done in 8.774 ms

Running 2/3
Running OriginalTest        ... done in 3.383 ms
Running EmitILTest          ... done in 3.432 ms
Running DynamicTest         ... done in 4.515 ms
Running CachedReflectionTest... done in 7.833 ms
Running SimpleReflectionTest... done in 9.054 ms

Running 3/3
Running OriginalTest        ... done in 3.514 ms
Running EmitILTest          ... done in 3.425 ms
Running DynamicTest         ... done in 4.471 ms
Running CachedReflectionTest... done in 7.938 ms
Running SimpleReflectionTest... done in 8.968 ms


Results
=======
OriginalTest            3.430   (= time)
EmitILTest              3.480   (1,0x slower)
DynamicTest             4.492   (1,3x slower)
CachedReflectionTest    7.817   (2,3x slower)
SimpleReflectionTest    8.932   (2,6x slower)
```