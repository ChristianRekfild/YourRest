## Архитектура

### Используемые паттерны

* Микросервисная архитектура
* Клиент-сервер
* Publish/subsribe
* Слои
* Порты и адаптеры

### Порты и адаптеры

#### Краткое описание архитектуры

Сервис использует архитектуру "Порты и адаптеры" (также известную под названием "Шестигранная"). В данной архитектуре приложение
является "черным ящиком", окруженным внешними системами, c которыми она осуществляет ввод/вывод. Например, ввод может
осуществляться посредством REST, интерфейса командной строки или чтения сообщения из внешней системы. В качестве примеров вывода
можно привести синхронный вызов внешних систем, чтение и запись в БД, асинхронную отправку сообщений другим микросервисам.

По-сути, этa архитектура представляет собой симметричный вариант "Слоёной архитектуры", где есть система и "всё остальное" -
внешний мир, с которыми приложение общается через адаптеры, находящиеся на его границе.

Архитектура построена так, что ввод-вывод всегда можно подменить тестовым окружением. Кроме того, она обеспечивает независимость
системы от фреймворков и технологий, которые могут меняться довольно часто из-за устаревания, изменения требований или политики
компании.

#### Порты

Понятие "порт" является абстракцией. Порты представляют из себя интерфейсы, которые реализуют адаптеры.

#### Адаптеры

Aдаптеры находятся на границе приложения и отвечают за взаимодействие с внешним миром. Взаимодействия может быть инициировано
извне (через "driven" адаптер) или может быть инициирована самим приложением (через "driving" адаптер).

Адаптеры на стороне ввода принимают информацию извне и преобразуют её в формат, понятный системе. Например, это может быть GUI,
CLI, REST API, очередь сообщений. В случае с REST API, например, задача адаптера - преобразовать HTTP-запрос в DTO. В случае
Spring эту роль выполняет cам фреймворк и контроллер (адаптер).

Адаптеры на стороне вывода представляют из себя репозитории, специфические для каждой внешней системы.

Архитектура ассимметрична в том смысле, что при вводе адаптер через порт вызывает use case, в то время как при выводе use case
через порт вызывает адаптер.

#### Юзкейсы

Через порт (интерфейс) адаптеры вызывают юзкейсы, которые олицетворяют законченную бизнес-операцию, имеющую смысл
для пользователя системы (изменение бронирования, получение информации об отеле, получение наличия мест в отелях в определенном городе/регионе и т.п.). В их задачу
входит оркестрация всего бизнес-процесса: они, как правило, принимают пользовательский ввод (через "driving" адаптеры),
манипулируют объектами бизнес-слоя для выполнения свой задачи и осуществляют вывод (через "driven" адаптеры).

Например, при подборе отеля в определенном городе с наличием определенных facilities use case принимает ввод от REST-контроллера в виде {"hotel_facilities": ["wifi", "pool", "spa"], "city_id отфильтрованные по городу и facilities, преобразует его в DTO и возвращает контроллеру. Таким образом, use case - оcновное
сосредоточение логики приложения, которое должно тщательно тестироваться.

Сложный use case может состоять из отдельных подопераций. Например, при оформлении заказа в интернет-магазине, необходимо
создать сам заказ, уменьшить количество единиц товара на складе и создать запрос для службы отгрузки товара. В этом случае
use case вызывает несколько подопераций в соответствующих application services, каждый из которых воплощает один bounded context:
shoppingService.createOrder(), inventoryService.decreaseQuantity(), shippingService.createShippingRecord(). Каждая такая операция
не представляет из себя ценности по-отдельности, ей обязательно нужен координатор в виде юзкейса. Во всём же остальном она подобна
юзкейсу: принимает DTO на вход, манипулирует бизнес-объектами, осуществляет взаимодействие с внешними системами через порты.

В микросервисной архитектуре с использованием оркестрации юзкейс становится отдельным микросервисом, принимающим пользовательский ввод
и вызывающим другие микросервисы (в которые преобразуются наши application services). Подоперации, содержащиеся в них, не имеют
самостоятельной ценности (нет никакого смысла создавать заказ, уменьшать количество единиц на складе и создавать запрос об отгрузке
товара - эти подоперации имеют смысл лишь вместе). Именно поэтому в микросервисной архитектуре с использованием оркестрации сервисы,
как правило, не автономны и требуют координации.

### Слои

Архитектура "Порты и адаптеры" не оговаривает то, как устроена сама система, однако в этом проекте используются следующие слои:

* Сервисный слой (представлен use case-классами). Оркестрирует доменные объекты согласно бизнес-сценарию.
* Доменный слой (представлен классами доменных объектов и доменных сервисов).
* Инфраструктурный слой (представлен контроллерами и репозиториями). C помощью него идёт общение со внешним миром.

### Инверсия зависимостей

Архитектура микросервиса использует инверсию зависимостей (DIP) для того, чтобы обеспечить лёгкую изменяемоcть системы. Согласно ей,
высокоуровневые модули не могут зависеть от низкоуровневых. Как следствие:

* DTO никогда не покидают сервисный cлой, где происходит их преобразование в доменные объекты. Благодаря этому доменная логика не
зависит от инфраструктурного слоя
* Все интерфейсы, через которые слой N использует объекты в слое N + 1 (ниже него), находятся на одном уровне со слоем N. Как и в
предыдущем случае, это устраняет зависимость от низкоуровневых модулей, которые могут свободно меняться.


### Операции c доменными объектами

В проекте используется паттерн Data mapper, согласно которому доменные сущности не умеют сохранять/извелекать себя из внешних
систем. Эта работа делегируется инфраструктурному слою, который преобразует объект в DTO, оптимизированный для передачи по сети.
Очень часто объекты этого слоя представляют из себя классы, реализующие паттерн "Репозиторий".

### Репозитории

В проекте используется паттерн "Репозиторий" из Domain Driven Development. Репозитории оперируют "aggregate root" - графом объектов,
операции над которыми должны проиводиться, как с единым целым. Примером может быть `Hotel` и `Room` - последним
бессмысленно оперировать, как отдельной сущностью. Репозиторий гарантирует согласованность данных для операций с сущностями. Как
правило, границы транзакций не выходят за пределы репозитория, внутри же него каждая операция осуществляется транзакционно.
Через репозиторий отель добавляется комната. Отель будет знать о добавлении комнаты, и таким образом данные будут согласованы.
Например, при добавлении комнаты, может обновится площадь отеля или кол-во номеров.

### Сценарий работы микросервиса

Принцип работы архитектуры "порты и адаптеры" легче понять, рассмотрев один из сценариев работы с системой.

Предположим, в систему приходит запрос на изменение бронирования. В системе происходят следующие события:

* Контроллер (адаптер) получает запрос в виде DTO
* Контроллер вызывает соответствующий use case через его интерфейс (порт)
* Use case (application service) преобразует DTO в доменный объект, вызывая cоответствующий метод у DTO
* На доменном объекте вызываются какие-то методы в соответствии с бизнес-логикой
* Use case передаёт доменный объект репозиторию Postgres (адаптер) через его интерфейс (порт)
* Репозиторий конвертирует объект в DTO и изменяет бронирование в базе

## Добавление нового функционала

### Добавление нового адаптера

Предположим, нужно добавить новый endpoint, который будет отдавать пользователю информацию о бронированиях через REST API. Функционал надо
делать с нуля. Новый endpoint должен принимать сложные фильтры в виде JSON. В данной архитектуре нужно сделать следующее:

* Добавить адаптер на стороне ввода (REST-контроллер).
* Cоздать DTO на стороне ввода в который будет десериализован клиентский JSON с фильтрами
* Cоздать DTO на стороне ввода, в который будет сериализован ответ пользователю
* Создать порт (интерфейс) на стороне ввода. Через него адаптер будет вызывать use case
* Создать сам use case (сервис). Здесь будет происходить основная работа. Он должен реализовывать порт, описанный выше
* Создать доменный объект Booking. Предположим, что они будут соответствовать одному DTO на стороне вывода
* Добавить порт (интерфейс) на стороне вывода
* Добавить адаптер на стороне вывода (репозиторий). Он должен реализовывать порт, описанный выше
* Добавить DTO на стороне вывода. Он будет передан библиотеке, работающей с базой (например, EF)

Так будут выглядеть вызовы в одном направлении (от пользователя):
```
User -> Adapter -> Port -> Use case -> Port -> Adapter -> Db
                          \       ^
                           \     /
                            v   /
                            User