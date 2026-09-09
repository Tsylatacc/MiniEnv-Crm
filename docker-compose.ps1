$secrets = dotnet user-secrets list --project .\MiniEnv.Api\MiniEnv.Api.csproj

function Get-UserSecret {
    param (
        [string]$Key
    )

    $line = $secrets | Where-Object { $_ -match "^$([regex]::Escape($Key))\s*=" } | Select-Object -First 1

    if (-not $line) {
        throw "User Secret '$Key' não encontrado."
    }

    return ($line -replace "^$([regex]::Escape($Key))\s*=\s*", "")
}

$env:MINIENV_DB_CONNECTION = Get-UserSecret "ConnectionStrings:db"
$env:MINIENV_RABBITMQ_CONNECTION = Get-UserSecret "ConnectionStrings:rabbitmq"
$env:MINIENV_REDIS_CONNECTION = Get-UserSecret "ConnectionStrings:redis"

$env:JWT_BEARER_KEY = Get-UserSecret "Jwt:Bearer:Key"
$env:JWT_BEARER_EXPIRES_IN_MINUTES = Get-UserSecret "Jwt:Bearer:ExpiresInMinutes"
$env:JWT_BEARER_ISSUER = Get-UserSecret "Jwt:Bearer:Issuer"
$env:JWT_BEARER_AUDIENCE = Get-UserSecret "Jwt:Bearer:Audience"

$env:JWT_SIGNUP_KEY = Get-UserSecret "Jwt:SignUp:Key"
$env:JWT_SIGNUP_EXPIRES_IN_MINUTES = Get-UserSecret "Jwt:SignUp:ExpiresInMinutes"
$env:JWT_SIGNUP_ISSUER = Get-UserSecret "Jwt:SignUp:Issuer"
$env:JWT_SIGNUP_AUDIENCE = Get-UserSecret "Jwt:SignUp:Audience"

$env:JWT_REFRESH_EXPIRES_IN_DAYS = Get-UserSecret "Jwt:Refresh:ExpiresInDays"

$env:FRONTEND_ACTIVEURL = Get-UserSecret "FrontEnd:ActiveUrl"

$env:MAILERSEND_APIKEY = Get-UserSecret "MailerSend:ApiKey"
$env:MAILERSEND_SENDEREMAIL = Get-UserSecret "MailerSend:SenderEmail"
$env:MAILERSEND_SENDERNAME = Get-UserSecret "MailerSend:SenderName"

docker compose up --build