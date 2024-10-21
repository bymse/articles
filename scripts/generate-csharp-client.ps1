dotnet build ../Bymse.Articles.Apis -c Debug
dotnet tool restore
dotnet swagger tofile `
--yaml `
--output ../Bymse.Articles.Apis/docs/public-api-openapi.yaml `
../Bymse.Articles.Apis/bin/Debug/net8.0/Bymse.Articles.Apis.dll `
public-api 