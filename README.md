# ASP.NET 8 Clean Architecture design pattern Template

We are excited to introduce a ready-to-use template for  ASP.NET 8, featuring a clean architectural design pattern and a well-organized folder structure. This template is designed to streamline your development process and promote best practices.
Key Features:
1. Generic Repository Design Pattern: Simplifies data access and repository management.
2. CQRS Design Pattern: Enables scalable and maintainable command and query handling.
3. Fluent Validation: Provides a flexible and intuitive validation framework.
4. Global Error Handling: Ensures robust error management and reporting.

This template is perfect for developers seeking a solid foundation for their ASP.NET 8 web api projects.



## Package Installation

You can install this template using [NuGet](https://www.nuget.org/packages/SmartCleanArchitecture):

```powershell
dotnet new -i SmartCleanArchitecture
```

## Dotnet new command

```powershell
dotnet new SmartCleanArchitecture -n YourProjectName
```


## IDE
```powershell
Visual Studio
```
```powershell
Visual Studio Code
```

## Dotnet run application

```powershell
dotnet run YourProjectName.Api
```

## Pack this Template

```powershell
dotnet pack .\nuget.csproj
```

## Install the nuget package
```powershell
dotnet new -i .\YourNugetPackageName.nupkg
```

## Usage

### Controllers
Creating a controller requires that a `Command` and `Handler` are created. The Command is declaration of the request and response payload.  

`Sample Controller`

Path: src/Presentation/SmartCleanArchitecture.Api/Controllers/UsersController.cs
```c#
    [Route("api/v1/user")]
    [ApiController]
    public class UsersController : ApiControllerBase
    {
        [HttpPost, Route("OnboardSS0User")]
        [ServiceFilter(typeof(LanguageFilter))]
        [TypeFilter(typeof(DecryptRequestDataFilter<CreateUserCommand>))]
        public async Task<IActionResult> CreateSSOUser([FromBody] BaseEncryptedRequestDTO encryptedRequestData, [FromQuery] CreateUserCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

    }
```

`Sample Command`

Path: src/Core/SmartCleanArchitecture.Application/Commands/CreateUserCommand.cs
```c#
namespace SmartCleanArchitecture.Application.Commands
{
    public class CreateUserCommand: IRequest<PayloadResponse<UsersDto>>
    {
        public  string FirstName { get; set; }
        public  string LastName { get; set; }
    }
}
```

`Sample Handler`

Path: src/Core/SmartCleanArchitecture.Application/Handler/CreateUserCommandHandler.cs
```c#
namespace SmartCleanArchitecture.Application.Handler
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, PayloadResponse<UsersDto>>
    {
        ...
            
        public async Task<PayloadResponse<UsersDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            ...
            return ResponseStatus<UsersDto>.Create<PayloadResponse<UsersDto>>(ResponseCodes.SUCCESSFUL, _messageProvider.GetMessage(ResponseCodes.SUCCESSFUL), newUser);
        }
    }
```

### Services
Services are registered in `src/Core/SmartCleanArchitecture.Application/ConfigureServices.cs` this will 


### Languages
This template has support for multiple languages, response is default in english `en` except otherwise 
passed in the request header to specify the language of the user e.g to get a French response the header should be as below:

`LanguageCode: fr`

Each controller is also decorated using the LanguageFilter to resolve to the most appropriate language of the user from the responses codes that has been defined.

```c#
[ServiceFilter(typeof(LanguageFilter))]
```

Response codes are stored in `src/Presentation/SmartCleanArchitecture.Api/Resources/` using the following format `response-codes-[LanguageCode].json`

e.g **response-codes-en.json**
```json 
{
  ...
  "E1000": "Unable to complete your process",
  ....
}
```

e.g **response-codes-fr.json**
```json
{
  ...
  "E1000": "Impossible de terminer votre processus",
  ...
}
```


##  Redis Implementation
in `appsettings.json` put your configuration

**Configuration**:

```json
"Redis": {
    "EndPoints": [
      "10.0.10.7:6379"
    ],
    "Password": "xxxxxxxx",
    "UserSentinel": false,
    "AbortOnConnectFail": true,
    "UseRedis": true
},
```

**Usage**



##  Kafka Implementation
in `appsettings.json` put your configuration

**Configuration**:

```json
"Kafka": {
    "bootstrapservers": "localhost:9092",
    "UseKafka": true,
    "FlushProducerInSeconds": 2,
    "ConsumedInSeconds": 2
}
```
`Note that you can use Confluent.Kafka configuration`


 


**Producer Usage**
`Sample Producer DI`

```c#
 public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, PayloadResponse<UsersDto>>
 {
    private readonly IMessageProducers _messageProducers;

    public CreateUserCommandHandler(IMessageProducers messageProducers)
    {
       _messageProducers = messageProducers
    }

    public async Task<PayloadResponse<UsersDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            ...

            await _messageProducers.ProduceAsync<UsersDto>("User", newUser).ConfigureAwait(false);
            
            return ResponseStatus<UsersDto>.Create<PayloadResponse<UsersDto>>(ResponseCodes.SUCCESSFUL, _messageProvider.GetMessage(ResponseCodes.SUCCESSFUL), newUser);
        }

 }

```
TBD

**Consumer Usage**

`Sample Consumer class`

Path: src\Core\SmartCleanArchitecture.Application\kafkaConsumer\Consumer.cs


```c#
    public class Consumer : ConsumerBase
    {

        public Consumer(KafkaConfig configuration, IMessageProducers producers, IMessageAdmin messageAdmin) : base("test1", configuration, messageAdmin)
        {

        }

        public override async Task Invoke()
        {

            await ConsumeAsync<string>("testTopic", (value) =>
            {  
                // put your action here
                Console.WriteLine(value);
            });

            await base.Invoke();
        }
    }
```
Note: - You can create multiple Consumer classes and configure them in the configuration file.
      - The groupId is test1.
      - The topic to be consumed is testTopic.


```c#
services.AddKafkaServices<Consumer>(kafkaConfig);

```

## License

This project is licensed with the [MIT license](LICENSE).