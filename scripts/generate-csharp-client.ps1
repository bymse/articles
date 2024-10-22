dotnet build ../Bymse.Articles.Apis -c Debug
dotnet tool restore
dotnet swagger tofile `
--output ../Bymse.Articles.Apis/docs/public-api-openapi.json `
../Bymse.Articles.Apis/bin/Debug/net8.0/Bymse.Articles.Apis.dll `
public-api

dotnet nswag openapi2csclient `
/input:../Bymse.Articles.Apis/docs/public-api-openapi.json `
/namespace:Bymse.Articles.PublicApi.Client `
/output:../Bymse.Articles.PublicApi.Client/PublicApiClient.cs `
/ClassName:PublicApiClient