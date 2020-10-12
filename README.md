# GGTeam/SmartMobileCore
Мини движок мобильных игр    

Свободная лицензия: CC BY Murnik Roman   
Free license: CC BY Murnik Roman    
Tested in Unity 2019.2.X +    

Входит в состав набора инструментов GGTools. Рекомендуемое расположение в проекте:    
*Assets/GGTeam/SmartMobileCore*    
____

## Установка
In order to install package, you need to add acoped registry into your package manifest at
unity_project/Packages/manifest.json
{
  "scopedRegistries": [
    {
      "name": "SmartMobileCore",
      "url": "https://registry.npmjs.org/",
      "scopes": [
        "com.ggteam.smartmobilecore"
      ]
    }
  ],
  "dependencies": {
 
 
 ## В ПЛАНАХ    
 - Сохранять список номеров всех пройденных уровней




 ## Структура проекта (ИДЕТ ПРОЦЕСС ФОРМИРОВАНИЯ)    

- Assets
	- Game
		- Code		*//Пользовательские скрипты*
		- Scenes	*//Сцены*
			- Levels
				- Level_01 : Scene
				- Level_02 : Scene
				- Level_XX : Scene
			- MainScene : Scene
		- MainConfig	*//Файл конфигурации*
		- Data
			- UserDataModel : DataModel	*//Сохраняемые данные*
		- Resources
			- Atlases
			- Materials
			- Models
			- Prefabs
			- Sounds
			- Textures
	- GGTeam	*//Мобильный движок*
	- IronSource	*//Рекламный движок*
	- Plugins
- Packages

 ## Структура BuildSettings scenes
  - 0 Главная сцена
  - 1 Level_1
  - 2 Level_2
  - .....
  - N Level_N


 ## Условности
 - Все сцены должны быть добавлены в BuildSettings
 - Первая сцена - Это сцена главного меню, всегда имеет BuildIndex = 0
 - Весь UI интерфейс расположен на главной сцене (Первой)
 - Каждый новый уровень - это отдельная сцена. BuildIndex равен номеру уровня
 - Не забывать использовать using GGTools.Core когда это необходимо
 
 
 ## Быстрый старт
 - Создать главную сцену SceneMain. Создать пустой объект, например [GameManager] и повесить на него скрипт GameManager.cs.
 - В папке своего проекта создайте файл настроек (правой кнопкой мыши->SmartMobileCore/GameConfig) и назначте эти настройки в соответствующее поле у GameManager.
 - Прочесть раздел `Работа с UI` и создать главное меню с кнопкой [Play]. На событие клика выполнить код: Game.Levels.LoadNext();
 - Добавить эту сцену в BuildSettings на самый верх с id=0.
 - Создать сцену для нового уровня, например Level_01
 - Создать скрипт, где будет обрабатываться основная логика игры, например MyGameplay
 - Отнаследовать его от класса Level и реализовать предложенные абстрактные методы
 - Вешать этот скрипт на пустой обьект каждой созданной сцены с уровнями. Вешаем на сцену Level_01
 API:
    При завершении, провале и т.д. вызывать метод LevelComplete(), LevelFailed()...
 

  ## Работа с UI
  Любой экран (или отдельное окно) интерфейса содержит пользовательский скрипт, наследуемый от UIScreen
  теперь, обратившись к этому пользовательскому скрипту, можно выполнять операции над окном интерфейса.
  Типичная структура:
    Canvas->TestWindow1(тут ваш скрипт)->content->(тут отображаемый интерфейс)
    Canvas->TestWindow2(тут ваш скрипт)->content->(тут отображаемый интерфейс)
  Указав тип интерфейса отличный от UIType=custom вы заставите его работать по нужной логике.
  Например UIType=ScreenLevelComplete будет автоматически отображен при победе на уровне и т.п.

  API:
  - Open(), Close() Отобразить или скрыть окно интерфейса
  - IsOpen Проверить состояние
  
