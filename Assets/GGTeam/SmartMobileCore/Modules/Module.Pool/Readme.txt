1) Создать скрипт, отнаследовать его от PoolElement
2) Повесить этот скрипт на GameObject и перенести в префаб (prefObj)

// Создаем модуль
PoolModule poolManager = new PoolModule();

// Создаем и заполняем пул 10-ю объектами
var pool = poolManager.PutElement(prefObj, 10);

// Получаем элементы из пула и переносим его в дочерний объект to_transform
var el1 = pool.GetElement(to_transform);
var el2 = pool.GetElement(to_transform);

// Вернуть объект в пул
el2.Return();