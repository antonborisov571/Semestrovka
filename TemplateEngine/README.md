# Шаблонизатор

### ***Как использовать?***

```cs
var template = "{{for number in Model}} {{number}}{{endfor}}";
var result = new TemplateEngine().GetHTML(template, new List<int> {1, 2, 3});
// output: 1 2 3
```

## ***Что можно делать и какие есть возможности?***
1. Писать условные выражения, используя следующие операции:
  - ```&&```
  - ```||```
  - ```==```
  - ```!=```
  - ```<```
  - ```<=```
  - ```>```
  - ```>=```
  - ```+```
  - ```-```
  - ```*```
  - ```/```
2. Писать циклы foreach (пример есть выше)
3. Писать циклы while
```cs
var template = "{{model = 0}}{{while model < 3}} {{model}} {{model = model + 1}}{{endwhile}}";
var result = new TemplateEngine().GetHTML(template);
// output: 0 1 2
```
3. Писать условия
```cs
var template = "{{if Model}} true {{else}} false {{endif}}"
var result = new TemplateEngine().GetHTML(template, true);
// output: true
```
4. Возможность обращаться к свойствам созданной модели или переданной модели
5. Использовать некоторые методы модели (Методы расширения не работают!!!)
6. Писать switch/case
```cs
var template = "{{model = "true"}}{{switch Model}}{{case model}} true {{default}} false {{endswitch}}"
var result = new TemplateEngine().GetHTML(template, "false");
// output: false
```
7. Писать try/catch/finally (Зачем это надо?)
8. Создавать свои переменные (примеры есть выше)
9. Поддерживаются string, int, double для создания переменных

### Замечание

Некоторые методы не работают, методы расширения вообще не работают, т.е. **LINQ** использовать в шаблонах не удастся.

Для каждого if должен быть свой else!
