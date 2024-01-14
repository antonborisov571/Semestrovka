# ORM

### ***Как использовать?***

Создаёте сущности которые у вас есть в базе данных и помечаете их аттрибутом ```[Table("Название вашей сущности")]```,
далее вы можете делать некоторые операции

### ***Что можно делать и какие есть возможности?***
1. ```CreateDatabase()``` - создание базы данных 

2. ```CreateTable<TEntity>()``` - создание таблицы

3. ```Insert(TEntity obj)``` - добавление объекта в базу данных

4. ```Update(TEntity obj)``` - обновление объекта в базе данных

5. ```Delete(TEntity obj)``` - удаление объекта из базы данных

6. ```Select<TEntity>()``` - выбрать всю таблицы сущности TEntity
   
7. ```Select<TEntity>(Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> func)``` - выбрать строки которые подходят по условию и некоторые столбцы (запрос выполняется в базе данных)
   
8. ```Select<TEntity>(TEntity entity)``` - выбрать объекты из базы данных, те у которых свойства такие же как у переданного параметра
  
9. ```SelectCrossJoin<TEntity, VEntity>()``` - select с crossjoin (Если ты умеешь делать crossjoin, то ты умеешь делать любой join)

### ***Пример работы***

```cs
MyORM.Init(_settings.DBConnectionString);
var orm = MyORM.Instance;
var topics = await orm.Select<Topic>();
var accounts = await orm.Select<Account>();
var posts = await orm.Select<Post>();
```

## Замечание

```SelectCrossJoin<TEntity, VEntity>()``` - создаёт объект некоторого типа, который **создаётся** во время работы программы!!! Также при использовании лучше приводить полученный результат к ```dynamic```. Таким образом, будет обеспечиваться возможность обращения к его свойствам.
