## Создание сервиса аутентификации и авторизации на .NET

### Создание нового проекта
dotnet new webapp -n KeycloakAuthenticationService

### Добавление библиотеки для OpenID Connect
dotnet add package Microsoft.AspNetCore.Authentication.OpenIdConnect

### Настройка аутентификации
В файле Program.cs, настраиваем middleware для аутентификации с использованием OpenID Connect.

### Настройка keycloak
Keycloak Admin Console - http://localhost:8080/auth/admin/
Создание Realm: Нужно создать пространство, в котором можно будет добавить клиента (приложение).
Настройка клиентов: Создаем клиента для каждого приложения. Access Type = "Confidential", получим SecretKey
Realm settings: По дефолту, только форма входа. Если нужна форма регистрации, включаем User registration

### Форма логина
Пользовательский интерфейс Keycloak
http://localhost:8080/auth/realms/YourRest/protocol/openid-connect/auth?client_id=your_rest_app&redirect_uri=http://localhost:5000&response_type=code&scope=openid

Создаем пользователя через форму. Если все успешно, 

### При разворачивании keycloak в докере указываем конфиг
Конфигурационный файл:
Infrastructure/YourRest.Producer.Infrastructure.Keycloak/Docker/realm-export.json

Можно указывать клиентов, юзеров, группы, роли

### Также можно настраиваеть вручную 

1.Groups. Создаем группу /accommodations/1
2. Users ->Groups -> Join. Привязываем группу к пользователю
3. Clients -> Mappers. Указываем, чтобы в токене приходила группа, фамилия, имя, email, Audience, keyCloakId

Add Group Mapper:

Name: subMapper
Mapper Type: User Property
Property: keyCloakId
Token Claim Name: sub
Add to ID token: ON
Add to access token: ON

Name: firstNameMapper
Mapper Type: User Property
Property: firstName
Token Claim Name: given_name
Add to ID token: ON
Add to access token: ON

Name: lastNameMapper
Mapper Type: User Property
Property: lastName
Token Claim Name: family_name
Add to ID token: ON
Add to access token: ON

Name: emailMapper
Mapper Type: User Property
Property: email
Token Claim Name: email
Add to ID token: ON
Add to access token: ON

Add Group Mapper:
To add the group information:
Name: groupMapper
Mapper Type: Group Membership.
Token Claim Name: "groups".
Full group path: Leave it OFF if you just want the group name, turn it ON if you want the full path.

Name:  "audience-mapper".
Mapper Type: "Audience".
Included Client Audience: the client ID
Add to access token: true so that the audience gets added to the access token.