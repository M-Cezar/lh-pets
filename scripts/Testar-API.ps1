param([string]$BaseUrl = 'http://localhost:5050')
$ErrorActionPreference = 'Stop'
function Verificar($condicao, $mensagem) { if (-not $condicao) { throw $mensagem } }
function Post($rota, $dados) {
    $resposta = Invoke-WebRequest "$BaseUrl$rota" -Method Post -ContentType 'application/json; charset=utf-8' -Body ([Text.Encoding]::UTF8.GetBytes(($dados | ConvertTo-Json)))
    Verificar ($resposta.StatusCode -eq 201) "POST $rota deve retornar 201."
    return $resposta.Content | ConvertFrom-Json
}
function Rejeitar($rota, $dados) {
    try { Post $rota $dados | Out-Null; throw 'Cadastro invalido foi aceito.' }
    catch { if ($null -eq $_.Exception.Response -or [int]$_.Exception.Response.StatusCode -ne 400) { throw } }
}
$swagger = Invoke-WebRequest "$BaseUrl/swagger/index.html"
Verificar ($swagger.StatusCode -eq 200) 'Swagger indisponivel.'
$documento = Invoke-RestMethod "$BaseUrl/swagger/v1/swagger.json"
Verificar ($null -ne $documento.paths.'/api/clientes'.post) 'Swagger sem cadastro de clientes.'
$a = Post '/api/clientes' @{nome='Ana';cpf='12345678901';email='ana@example.com'}
$b = Post '/api/clientes' @{nome='Carlos';cpf='98765432100';email='carlos@example.com'}
Verificar ($a.id -gt 0 -and $b.id -gt $a.id) 'IDs dos clientes invalidos.'
$p = Post '/api/pets' @{nome='Mel';especie='Cachorro';raca='SRD';clienteId=$a.id}
$q = Post '/api/pets' @{nome='Luna';especie='Gato';raca='Siames';clienteId=$b.id}
$clientes = @(Invoke-RestMethod "$BaseUrl/api/clientes" | ForEach-Object { $_ })
$pets = @(Invoke-RestMethod "$BaseUrl/api/pets" | ForEach-Object { $_ })
$filtro = @(Invoke-RestMethod "$BaseUrl/api/pets/cliente/$($a.id)" | ForEach-Object { $_ })
Verificar (@($clientes | Where-Object id -eq $a.id).Count -eq 1) 'Cliente ausente da listagem.'
Verificar (@($pets | Where-Object id -eq $p.id).Count -eq 1) 'Pet ausente da listagem.'
Verificar ($filtro.Count -eq 1 -and $filtro[0].id -eq $p.id) 'Filtro por cliente incorreto.'
Rejeitar '/api/pets' @{nome='Invalido';especie='Gato';raca='SRD';clienteId=2147483647}
Rejeitar '/api/clientes' @{nome='Invalido';cpf='123';email='email-invalido'}
Rejeitar '/api/clientes' @{nome='';cpf='123';email='valido@example.com'}
Write-Output 'APROVADO: cinco endpoints, IDs automaticos, filtro por cliente, Swagger HTML/JSON e rejeicoes HTTP 400.'

