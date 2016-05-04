﻿# Task Simple Query Language

## SimQL1

Тимлид Артур поставил вам задачу — написать вычислялку агрегатных функций по данным из Json-файла.
Вам дан Json такой структуры:

```
{
	"data": {"a":{"x":3.14, "b":[{"c":15}, {"c":9}]}, "z":[2.65, 35]},
	"queries": [
		"sum(a.b.c)",
		"min(z)",
		"max(a.x)"
	]
}
```

Тут в свойстве data лежит произвольный Json. А в queries — запросы к этому Json-у.
Запрос — это одна из трех функций sum, min и max.
Аргумент функции — это путь внутри data.

В результате нужно сформировать по одной строке на каждый запрос в такой форме:
```
<исходный запрос> = <результат>
```

На этом примере вывод должен быть таким:

```
sum(a.b.c) = 24
min(z) = 2.65
max(a.x) = 3.14
```

## SimQL2

**Будет чуть позже...**