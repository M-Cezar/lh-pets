# LH-Pets | Web API em ASP.NET Core

Projeto avaliativo de Web API em **C# / ASP.NET Core 10** com arquitetura baseada em **Controllers** e documentação interativa no **Swagger**.

## Objetivo

Gerenciar clientes e seus pets, associando cada pet ao cliente por meio da propriedade `ClienteId`. Os dados são armazenados **em memória**, utilizando coleções `List<Cliente>` e `List<Pet>`, sem SQL Server ou qualquer banco de dados. Ao encerrar a API, os cadastros são perdidos, conforme o escopo da atividade.

## Tecnologias

- .NET SDK 10.0 (projeto `net10.0`);
- C# e ASP.NET Core Web API (Controllers);
- Swashbuckle / Swagger UI;
- Collections `List<T>` e validações com Data Annotations.

## Estrutura do projeto

```text
LH-Pets/
├── Controllers/
│   ├── ClientesController.cs
│   └── PetsController.cs
├── Data/
│   └── CadastroEmMemoria.cs
├── Models/
│   ├── Cliente.cs
│   └── Pet.cs
├── Properties/
│   └── launchSettings.json
├── scripts/
│   └── Testar-API.ps1
├── LH-Pets.csproj
├── LH-Pets.http
├── Program.cs
└── README.md
```

## Como executar

1. Instale o [.NET SDK 10](https://dotnet.microsoft.com/download/dotnet/10.0) (SDK, não apenas Runtime).
2. Abra esta pasta no Visual Studio Code ou em um terminal.
3. Execute:

```powershell
dotnet restore
dotnet build
dotnet run
```

4. Acesse **http://localhost:5050/swagger** no navegador. O projeto define essa porta no arquivo `Properties/launchSettings.json`.
5. Para interromper a API, pressione `Ctrl+C` no terminal.

> Se a porta 5050 estiver em uso, finalize o processo que a utiliza ou execute `dotnet run --urls http://localhost:5051` e abra `http://localhost:5051/swagger`.

## Endpoints

| Método | Rota | Resultado |
| --- | --- | --- |
| GET | `/api/clientes` | Lista todos os clientes cadastrados |
| POST | `/api/clientes` | Cadastra um cliente e atribui ID automático |
| GET | `/api/pets` | Lista todos os pets cadastrados |
| POST | `/api/pets` | Cadastra um pet para um `ClienteId` existente |
| GET | `/api/pets/cliente/{clienteId}` | Lista somente os pets de determinado cliente |

## Teste pelo Swagger (passo a passo)

No Swagger, expanda o endpoint, clique em **Try it out**, preencha o corpo da requisição e clique em **Execute**.

**Etapa 1.** Execute `GET /api/clientes`. Deve retornar `[]` se você ainda não cadastrou ninguém.

**Etapa 2.** Execute `POST /api/clientes` usando este JSON:

```json
{
  "nome": "Ana Silva",
  "cpf": "12345678901",
  "email": "ana@example.com"
}
```

O resultado esperado é **201 Created**, com o campo `id` gerado pela API (por exemplo, `1`).

**Etapa 3.** Execute `GET /api/clientes` e confira se Ana aparece na lista.

**Etapa 4.** Execute `POST /api/pets`:

```json
{
  "nome": "Mel",
  "especie": "Cachorro",
  "raca": "Vira-lata",
  "clienteId": 1
}
```

Use o **ID real** obtido na etapa 2 no campo `clienteId`. O resultado esperado é **201 Created**.

**Etapa 5.** Execute `GET /api/pets` e veja o pet cadastrado.

**Etapa 6.** Execute `GET /api/pets/cliente/1` (ajuste o número se necessário). A resposta deve conter apenas os pets desse cliente.

**Etapa 7, validação extra.** Tente cadastrar um pet com `clienteId: 99999` (sem existir esse cliente). O resultado esperado é **400 Bad Request**. Teste também um cliente com e-mail inválido; a API deve recusar o cadastro.

O arquivo `LH-Pets.http` contém as mesmas requisições para testes com a extensão REST Client do VS Code, se desejado.

## Testes automatizados adicionais (Windows PowerShell)

Com a API em execução, abra **outro** terminal PowerShell na pasta do projeto e execute:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\Testar-API.ps1
```

O script cadastra dois clientes, cadastra dois pets, verifica as duas listas e confirma o filtro de pets por cliente. Ele não substitui o teste no Swagger exigido pela atividade.

## Como publicar no GitHub e entregar

1. Crie no GitHub um repositório **público**, por exemplo `LH-Pets`, sem adicionar arquivos iniciais pelo site.
2. Na pasta do projeto, execute os comandos abaixo, substituindo `SEU_USUARIO` pelo nome da sua conta:

```powershell
git init
git add .
git commit -m "Projeto LH-Pets: Web API com Controllers, List e Swagger"
git branch -M main
git remote add origin https://github.com/SEU_USUARIO/LH-Pets.git
git push -u origin main
```

3. Abra o repositório no navegador e confirme que os arquivos `Program.cs`, `Controllers/`, `Models/` e `README.md` foram publicados.
4. Copie a URL **real do repositório público** e cole na caixa da atividade no AVA antes do vencimento.

**Atenção:** um ZIP baixado ou o código em seu computador **não** concluem a entrega. O enunciado exige a URL de um repositório público no GitHub.

## Observações

- O sistema é um protótipo didático; não inclui autenticação ou persistência em disco.
- Exemplos usam dados fictícios para os testes.
- `[ApiController]` usa as anotações do modelo para rejeitar dados inválidos com HTTP 400.
- A classe `CadastroEmMemoria` é registrada como singleton para compartilhar as listas entre os controladores enquanto a aplicação estiver ativa.

